using System.Security.Claims;
using TemplateProject.Providers;
using TemplateProject.Providers.AuthProvider;
using TemplateProject.Providers.EmployeeProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TemplateProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IAuthProvider authProvider,
        IEmployeeProvider employeeProvider,
        JwtProvider jwtProvider,
        ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthProvider _authProvider = authProvider;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly JwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required");
            }

            if (!_authProvider.AuthenticateUser(request.Username, request.Password))
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
                return Unauthorized("Invalid username or password");
            }

            var employee = await _employeeProvider.FindEmployeeByUserName(request.Username);
            if (employee == null)
            {
                _logger.LogWarning("User authenticated but not found in employee database: {Username}", request.Username);
                return Unauthorized("User is not a valid employee");
            }

            var token = await _jwtProvider.GenerateToken(employee);

            return Ok(new
            {
                message = "Login successful",
                token,
                employee = new
                {
                    username = employee.Username,
                    employeeNumber = employee.EmployeeNumber,
                    fullName = employee.FullName,
                    department = employee.Department
                }
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // No need to clear cookies as we're using JWT in Authorization header
            // The client will be responsible for removing the token

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh()
        {
            var employeeNumber = User.FindFirst("employeenumber")?.Value;
            if (string.IsNullOrEmpty(employeeNumber))
            {
                return Unauthorized("Invalid token");
            }

            var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
            if (employee == null)
            {
                return Unauthorized("User is not a valid employee");
            }

            var token = _jwtProvider.GenerateToken(employee);
            return Ok(new { message = "Token refreshed successfully", token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
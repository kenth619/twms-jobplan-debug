using System.Security.Claims;
using TWMSServer.Providers;
using TWMSServer.Providers.AuthProvider;
using TWMSServer.Providers.EmployeeProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWMSServer.Model;
using TWMSServer.Model.Enum;

namespace TWMSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        ILogger<AuthController> logger,
        IAuthProvider authProvider,
        IEmployeeProvider employeeProvider,
        EmployeeRolesProvider employeeRolesProvider,
        JwtProvider jwtProvider) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IAuthProvider _authProvider = authProvider;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly EmployeeRolesProvider _employeeRolesProvider = employeeRolesProvider;
        private readonly JwtProvider _jwtProvider = jwtProvider;

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

            EmployeeWithRoles? employeeWithRoles = await _employeeRolesProvider.GetEmployeeWithRoles(employee.EmployeeNumber);
            if (employeeWithRoles == null)
            {
                _logger.LogWarning("Employee not found in employee database with employee number {EmployeeNumber}", employee.EmployeeNumber);
                return Unauthorized("User does not have access to this system");
            }

            var token = _jwtProvider.GenerateToken(employee, employeeWithRoles.SystemRoles, employeeWithRoles.DepartmentRoleMapping);

            return Ok(new
            {
                message = "Login successful",
                token,
                employee = employeeWithRoles
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

            var systemRoles = new List<SystemRole>();
            var departmentRoles = new List<DepartmentRoleMappingEntry>();
            
            foreach (var claim in User.Claims.Where(c => c.Type == ClaimTypes.Role))
            {
                if (claim.Value.StartsWith("system:"))
                {
                    var roleKey = claim.Value.Substring("system:".Length);
                    systemRoles.Add(SystemRole.FromKey(roleKey));
                }
                else if (claim.Value.StartsWith("department:"))
                {
                    var parts = claim.Value.Substring("department:".Length).Split(':');
                    if (parts.Length == 2)
                    {
                        var departmentCode = parts[0];
                        var roleKey = parts[1];
                        departmentRoles.Add(new DepartmentRoleMappingEntry(departmentCode, DepartmentRole.FromKey(roleKey)));
                    }
                }
            }

            var token = _jwtProvider.GenerateToken(employee, systemRoles, departmentRoles);
            return Ok(new { message = "Token refreshed successfully", token  });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
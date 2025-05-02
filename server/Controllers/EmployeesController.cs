using TWMSServer.Model;
using TWMSServer.Providers.EmployeeProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TWMSServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController(ILogger<EmployeesController> logger, IEmployeeProvider employeeProvider) : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger = logger;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeProvider.AllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<Employee>>> GetActiveEmployees()
        {
            try
            {
                var employees = await _employeeProvider.AllActiveEmployees();
                _logger.LogInformation("Found {Zz} employees", employees.Count);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{employeeNumber}")]
        public async Task<ActionResult<Employee>> GetEmployeeByNumber(string employeeNumber)
        {
            try
            {
                var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                
                if (employee == null)
                {
                    return NotFound($"Employee with number {employeeNumber} not found");
                }
                
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("loggedinuser")]
        public async Task<ActionResult<string>> GetLoggedInUser()
        {
            try
            {
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("User is not logged in");
                }

                return Ok(userName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

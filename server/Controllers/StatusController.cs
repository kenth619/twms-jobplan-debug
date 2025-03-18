using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateProject.Model;
using TemplateProject.Providers;

namespace TemplateProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(
        ILogger<StatusController> logger,
        EmployeeRolesProvider employeeRolesProvider) : ControllerBase
    {
        private readonly ILogger<StatusController> _logger = logger;
        private readonly EmployeeRolesProvider _employeeRolesProvider = employeeRolesProvider;

        [HttpGet]
        [ProducesResponseType<EmployeeWithRoles>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Index()
        {
            var employeeNumber = User.FindFirst("employeenumber")?.Value;
            if (string.IsNullOrEmpty(employeeNumber))
            {
                return Unauthorized("Invalid token");
            }

            var employeeWithRoles = await _employeeRolesProvider.GetEmployeeWithRoles(employeeNumber);
            if (employeeWithRoles == null)
            {
                _logger.LogWarning("Employee not found in employee database with employee number {EmployeeNumber}", employeeNumber);
                return Unauthorized("User is not a valid employee");
            }

            try
            {
                return StatusCode(StatusCodes.Status200OK, employeeWithRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee status for {EmployeeNumber}", employeeNumber);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving employee status");
            }
        }
    }
}
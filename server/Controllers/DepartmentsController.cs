using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWMSServer.Model;
using TWMSServer.Providers.EmployeeProvider;

namespace TWMSServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController(ILogger<DepartmentsController> logger, IEmployeeProvider employeeProvider) : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger = logger;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                var departments = await _employeeProvider.GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving departments");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{departmentCode}")]
        public async Task<ActionResult<Department>> GetDepartmentByCode(string departmentCode)
        {
            try
            {
                var department = await _employeeProvider.FindDepartmentByCode(departmentCode);
                
                if (department == null)
                {
                    return NotFound($"Department with code {departmentCode} not found");
                }
                
                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department with code {DepartmentCode}", departmentCode);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
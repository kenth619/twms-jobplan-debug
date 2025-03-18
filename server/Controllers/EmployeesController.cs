using TemplateProject.Model;
using TemplateProject.Providers.EmployeeProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TemplateProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController(ILogger<EmployeesController> logger, IEmployeeProvider employeeProvider) : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger = logger;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;

        [HttpGet]
        public ActionResult<List<Employee>> GetActiveEmployees()
        {
            try
            {
                var employees = _employeeProvider.AllActiveEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{employeeNumber}")]
        public ActionResult<Employee> GetEmployeeByNumber(string employeeNumber)
        {
            try
            {
                var employee = _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                
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
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWMSServer.Model;
using TWMSServer.Model.Enum;
using TWMSServer.Providers;
using TWMSServer.Providers.EmployeeProvider;

namespace TWMSServer.Controllers
{
    [Authorize(Roles = "system:system-administrator,system:superuser")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeRolesController(ILogger<EmployeeRolesController> logger, IEmployeeProvider employeeProvider, EmployeeRolesProvider employeeRolesProvider) : ControllerBase
    {
        private readonly ILogger<EmployeeRolesController> _logger = logger;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly EmployeeRolesProvider _employeeRolesProvider = employeeRolesProvider;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeProvider.AllActiveEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{employeeNumber}")]
        public async Task<ActionResult<EmployeeWithRoles>> GetEmployeeWithRoles(string employeeNumber)
        {
            try
            {
                var employeeWithRoles = await _employeeRolesProvider.GetEmployeeWithRoles(employeeNumber);
                
                if (employeeWithRoles == null)
                {
                    return NotFound($"Employee with number {employeeNumber} not found");
                }
                
                return Ok(employeeWithRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with roles for {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{employeeNumber}/system-roles")]
        public async Task<ActionResult> AssignSystemRole(string employeeNumber, [FromBody] SystemRoleRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RoleKey))
                {
                    return BadRequest("Role key is required");
                }

                SystemRole? role;
                try
                {
                    role = SystemRole.FromKey(request.RoleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid system role key: {request.RoleKey}");
                }

                var success = await _employeeRolesProvider.AssignSystemRole(employeeNumber, role);
                
                if (!success)
                {
                    return BadRequest("Failed to assign system role");
                }
                
                return Ok(new { message = $"System role {role.Name} assigned to employee {employeeNumber}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning system role to employee {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{employeeNumber}/system-roles/{roleKey}")]
        public async Task<ActionResult> RemoveSystemRole(string employeeNumber, string roleKey)
        {
            try
            {
                SystemRole? role;
                try
                {
                    role = SystemRole.FromKey(roleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid system role key: {roleKey}");
                }

                var success = await _employeeRolesProvider.RemoveSystemRole(employeeNumber, role);
                
                if (!success)
                {
                    return BadRequest("Failed to remove system role");
                }
                
                return Ok(new { message = $"System role {role.Name} removed from employee {employeeNumber}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing system role from employee {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{employeeNumber}/department-roles")]
        public async Task<ActionResult> AssignDepartmentRole(string employeeNumber, [FromBody] DepartmentRoleRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RoleKey))
                {
                    return BadRequest("Role key is required");
                }

                if (string.IsNullOrEmpty(request.DepartmentCode))
                {
                    return BadRequest("Department code is required");
                }

                DepartmentRole? role;
                try
                {
                    role = DepartmentRole.FromKey(request.RoleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid department role key: {request.RoleKey}");
                }

                var success = await _employeeRolesProvider.AssignDepartmentRole(employeeNumber, request.DepartmentCode, role);
                
                if (!success)
                {
                    return BadRequest("Failed to assign department role");
                }
                
                return Ok(new { message = $"Department role {role.Name} for department {request.DepartmentCode} assigned to employee {employeeNumber}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning department role to employee {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{employeeNumber}/department-roles")]
        public async Task<ActionResult> RemoveDepartmentRole(string employeeNumber, [FromQuery] string departmentCode, [FromQuery] string roleKey)
        {
            try
            {
                if (string.IsNullOrEmpty(roleKey))
                {
                    return BadRequest("Role key is required");
                }

                if (string.IsNullOrEmpty(departmentCode))
                {
                    return BadRequest("Department code is required");
                }

                DepartmentRole? role;
                try
                {
                    role = DepartmentRole.FromKey(roleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid department role key: {roleKey}");
                }

                var success = await _employeeRolesProvider.RemoveDepartmentRole(employeeNumber, departmentCode, role);
                
                if (!success)
                {
                    return BadRequest("Failed to remove department role");
                }
                
                return Ok(new { message = $"Department role {role.Name} for department {departmentCode} removed from employee {employeeNumber}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing department role from employee {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("validate")]
        public async Task<ActionResult<bool>> ValidateRoleAssignment([FromQuery] string employeeNumber, [FromQuery] string? roleKey = null, [FromQuery] string? departmentCode = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(roleKey) && !string.IsNullOrEmpty(departmentCode))
                {
                    DepartmentRole? role;
                    try
                    {
                        role = DepartmentRole.FromKey(roleKey);
                    }
                    catch (Exception)
                    {
                        return BadRequest($"Invalid department role key: {roleKey}");
                    }

                    var hasDepartmentRole = await _employeeRolesProvider.HasDepartmentRole(employeeNumber, departmentCode, role);
                    return Ok(hasDepartmentRole);
                }
                else if (!string.IsNullOrEmpty(roleKey))
                {
                    SystemRole? role;
                    try
                    {
                        role = SystemRole.FromKey(roleKey);
                    }
                    catch (Exception)
                    {
                        return BadRequest($"Invalid system role key: {roleKey}");
                    }

                    var hasSystemRole = await _employeeRolesProvider.HasSystemRole(employeeNumber, role);
                    return Ok(hasSystemRole);
                }
                else
                {
                    return BadRequest("Either roleKey or both roleKey and departmentCode must be provided");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating role assignment for employee {EmployeeNumber}", employeeNumber);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public record SystemRoleRequest(string RoleKey);
    public record DepartmentRoleRequest(string RoleKey, string DepartmentCode);
}

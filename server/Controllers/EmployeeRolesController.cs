using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateProject.Model;
using TemplateProject.Model.Enum;
using TemplateProject.Providers;
using TemplateProject.Providers.EmployeeProvider;

namespace TemplateProject.Controllers
{
    [Authorize(Roles = "system:administrator,system:superuser")]
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
                // Check if user has admin or superuser role
                if (!await IsAdminOrSuperuser())
                {
                    return Forbid("Only administrators and superusers can manage roles");
                }

                // Validate request
                if (string.IsNullOrEmpty(request.RoleKey))
                {
                    return BadRequest("Role key is required");
                }

                // Get the role from the key
                SystemRole? role;
                try
                {
                    role = SystemRole.FromKey(request.RoleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid system role key: {request.RoleKey}");
                }

                // Assign the role
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
                // Check if user has admin or superuser role
                if (!await IsAdminOrSuperuser())
                {
                    return Forbid("Only administrators and superusers can manage roles");
                }

                // Get the role from the key
                SystemRole? role;
                try
                {
                    role = SystemRole.FromKey(roleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid system role key: {roleKey}");
                }

                // Remove the role
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
                // Check if user has admin or superuser role
                if (!await IsAdminOrSuperuser())
                {
                    return Forbid("Only administrators and superusers can manage roles");
                }

                // Validate request
                if (string.IsNullOrEmpty(request.RoleKey))
                {
                    return BadRequest("Role key is required");
                }

                if (string.IsNullOrEmpty(request.DepartmentCode))
                {
                    return BadRequest("Department code is required");
                }

                // Get the role from the key
                DepartmentRole? role;
                try
                {
                    role = DepartmentRole.FromKey(request.RoleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid department role key: {request.RoleKey}");
                }

                // Assign the role
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
                // Check if user has admin or superuser role
                if (!await IsAdminOrSuperuser())
                {
                    return Forbid("Only administrators and superusers can manage roles");
                }

                // Validate request
                if (string.IsNullOrEmpty(roleKey))
                {
                    return BadRequest("Role key is required");
                }

                if (string.IsNullOrEmpty(departmentCode))
                {
                    return BadRequest("Department code is required");
                }

                // Get the role from the key
                DepartmentRole? role;
                try
                {
                    role = DepartmentRole.FromKey(roleKey);
                }
                catch (Exception)
                {
                    return BadRequest($"Invalid department role key: {roleKey}");
                }

                // Remove the role
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
                // Check if user has admin or superuser role
                if (!await IsAdminOrSuperuser())
                {
                    return Forbid("Only administrators and superusers can validate role assignments");
                }

                if (!string.IsNullOrEmpty(roleKey) && !string.IsNullOrEmpty(departmentCode))
                {
                    // Validate department role
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
                    // Validate system role
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

        private async Task<bool> IsAdminOrSuperuser()
        {
            var employeeNumber = User.FindFirst("employeeNumber")?.Value;
            if (string.IsNullOrEmpty(employeeNumber))
            {
                return false;
            }

            return await _employeeRolesProvider.HasSystemRole(employeeNumber, SystemRole.Administrator) ||
                   await _employeeRolesProvider.HasSystemRole(employeeNumber, SystemRole.Superuser);
        }
    }

    public class SystemRoleRequest
    {
        public string RoleKey { get; set; } = string.Empty;
    }

    public class DepartmentRoleRequest
    {
        public string RoleKey { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
    }
}

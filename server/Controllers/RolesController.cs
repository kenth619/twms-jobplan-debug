using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWMSServer.Model.Enum;

namespace TWMSServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(ILogger<RolesController> logger) : ControllerBase
    {
        private readonly ILogger<RolesController> _logger = logger;

        [HttpGet("system")]
        public ActionResult<IEnumerable<object>> GetSystemRoles()
        {
            try
            {
                var roles = SystemRole.All().Select(role => new
                {
                    key = role.Key,
                    name = role.Name
                });

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving system roles");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("department")]
        public ActionResult<IEnumerable<object>> GetDepartmentRoles()
        {
            try
            {
                var roles = DepartmentRole.All().Select(role => new
                {
                    key = role.Key,
                    name = role.Name
                });

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department roles");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
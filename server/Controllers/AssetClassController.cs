using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TWMSServer;
using TWMSServer.Model;

namespace YourProjectName.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetClassController : ControllerBase
    {
        private readonly TWMSServerContext _context;

        public AssetClassController(TWMSServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all asset classes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetClass>>> GetAll()
        {
            try
            {
                var assetClasses = await _context.AssetClasses
                    .Where(ac => ac.Status == true)
                    .OrderBy(ac => ac.AssetClassShortDesc)
                    .ToListAsync();

                return Ok(assetClasses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get asset class by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetClass>> GetById(int id)
        {
            try
            {
                var assetClass = await _context.AssetClasses
                    .FirstOrDefaultAsync(ac => ac.AssetClassId == id && ac.Status == true);

                if (assetClass == null)
                {
                    return NotFound($"Asset class with ID {id} not found");
                }

                return Ok(assetClass);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

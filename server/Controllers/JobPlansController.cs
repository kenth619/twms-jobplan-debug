using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TWMSServer.Model;

namespace TWMSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPlansController(
        ILogger<JobPlansController> logger,
        TWMSServerContext context) : ControllerBase
    {
        private readonly ILogger<JobPlansController> _logger = logger;
        private readonly TWMSServerContext _context = context;

        // GET: api/jobplans: List all Job Plans
        [HttpGet]
        [ProducesResponseType<PagedResponse<JobPlansHeader>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobPlans([FromQuery] JobPlanQueryParameters parameters)
        {
            var query = _context.JobPlansHeaders
                .Include(jp => jp.JobPlansLines)
                .AsQueryable();

            // Apply filters
            if (parameters.AssetClassId.HasValue)
                query = query.Where(jp => jp.AssetClassId == parameters.AssetClassId.Value);

            if (parameters.Status.HasValue)
                query = query.Where(jp => jp.JobPlanStatus == parameters.Status.Value);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
                query = query.Where(jp =>
                    jp.JobPlanShortDesc.Contains(parameters.SearchTerm) ||
                    jp.JobPlanLongDesc.Contains(parameters.SearchTerm) ||
                    jp.AssetClassShortDesc.Contains(parameters.SearchTerm));

            // Get total count and apply pagination
            var totalCount = await query.CountAsync();
            var jobPlans = await query
                .OrderByDescending(jp => jp.DateCreated)
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var response = new PagedResponse<JobPlansHeader>
            {
                Data = jobPlans,
                TotalCount = totalCount,
                Page = parameters.Page,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize)
            };

            return Ok(response);
        }

        // GET: api/jobplans/{id}: Get Job Plan by ID
        [HttpGet("{id}")]
        [ProducesResponseType<JobPlansHeader>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobPlan(int id)
        {
            var jobPlan = await _context.JobPlansHeaders
                .Include(jp => jp.JobPlansLines.OrderBy(jpl => jpl.JobPlanLineNo))
                .FirstOrDefaultAsync(jp => jp.JobPlanId == id);

            if (jobPlan == null)
                return NotFound();

            return Ok(jobPlan);
        }
        // GET: api/jobplans/{id}/details
        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(JobPlanWithTasksResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobPlanAndTask(int id)
        {
            var jobPlan = await _context.JobPlansHeaders
                .Include(jp => jp.JobPlansLines.OrderBy(jpl => jpl.JobPlanLineNo)) // Include and order tasks
                .FirstOrDefaultAsync(jp => jp.JobPlanId == id);

            if (jobPlan == null)
                return NotFound();

            // Create response model
            var response = new JobPlanWithTasksResponse
            {
                JobPlanId = jobPlan.JobPlanId,
                JobPlanStatus = jobPlan.JobPlanStatus,
                JobPlanShortDesc = jobPlan.JobPlanShortDesc,
                JobPlanLongDesc = jobPlan.JobPlanLongDesc,
                AssetClassId = jobPlan.AssetClassId,
                AssetClassShortDesc = jobPlan.AssetClassShortDesc,
                DateCreated = jobPlan.DateCreated,
                DateModified = jobPlan.DateModified,
                CreatedBy = jobPlan.CreatedBy,
                ModifiedBy = jobPlan.ModifiedBy,
                Tasks = jobPlan.JobPlansLines.Select(line => new TaskResponse
                {
                    JobPlanLineId = line.JobPlanLineId,
                    JobPlanLineNo = line.JobPlanLineNo,
                    JobPlanLineDesc = line.JobPlanLineDesc
                }).ToList()
            };

            return Ok(response);
        }


        // POST: api/jobplans: Create new Job Plan with tasks
        [HttpPost]
        [ProducesResponseType<JobPlansHeader>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJobPlan([FromBody] CreateJobPlanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentTime = DateTime.UtcNow;
            var currentUser = User?.Identity?.Name ?? "System";

            var jobPlan = new JobPlansHeader
            {
                JobPlanStatus = request.JobPlanStatus,
                JobPlanShortDesc = request.JobPlanShortDesc,
                JobPlanLongDesc = request.JobPlanLongDesc,
                AssetClassId = request.AssetClassId,
                AssetClassShortDesc = request.AssetClassShortDesc,
                DateCreated = currentTime,
                DateModified = currentTime,
                CreatedBy = currentUser,
                ModifiedBy = currentUser
            };

            // Add job plan lines if provided
            if (request.JobPlanLines?.Any() == true)
            {
                foreach (var lineRequest in request.JobPlanLines)
                {
                    jobPlan.JobPlansLines.Add(new JobPlansLine
                    {
                        JobPlanLineNo = lineRequest.JobPlanLineNo,
                        JobPlanLineDesc = lineRequest.JobPlanLineDesc
                    });
                }
            }

            _context.JobPlansHeaders.Add(jobPlan);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created job plan {JobPlanId} by {User}", jobPlan.JobPlanId, currentUser);

            return CreatedAtAction(nameof(GetJobPlan), new { id = jobPlan.JobPlanId }, jobPlan);
        }

        // PUT: api/jobplans/{id}
        [HttpPut("{id}")]
        [ProducesResponseType<JobPlansHeader>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateJobPlan(int id, [FromBody] UpdateJobPlanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jobPlan = await _context.JobPlansHeaders
                .Include(jp => jp.JobPlansLines)
                .FirstOrDefaultAsync(jp => jp.JobPlanId == id);

            if (jobPlan == null)
                return NotFound();

            var currentTime = DateTime.UtcNow;
            var currentUser = User?.Identity?.Name ?? "System";

            // Update header properties
            jobPlan.JobPlanStatus = request.JobPlanStatus;
            jobPlan.JobPlanShortDesc = request.JobPlanShortDesc;
            jobPlan.JobPlanLongDesc = request.JobPlanLongDesc;
            jobPlan.AssetClassId = request.AssetClassId;
            jobPlan.AssetClassShortDesc = request.AssetClassShortDesc;
            jobPlan.DateModified = currentTime;
            jobPlan.ModifiedBy = currentUser;

            // Update job plan lines if provided
            if (request.JobPlanLines != null)
            {
                // Remove existing lines and add new ones
                _context.JobPlansLines.RemoveRange(jobPlan.JobPlansLines);

                foreach (var lineRequest in request.JobPlanLines)
                {
                    jobPlan.JobPlansLines.Add(new JobPlansLine
                    {
                        JobPlanId = jobPlan.JobPlanId,
                        JobPlanLineNo = lineRequest.JobPlanLineNo,
                        JobPlanLineDesc = lineRequest.JobPlanLineDesc
                    });
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated job plan {JobPlanId} by {User}", jobPlan.JobPlanId, currentUser);

            return Ok(jobPlan);
        }

        // PATCH: api/jobplans/{id}/status
        [HttpPut("{id}/status")]
        [ProducesResponseType<JobPlansHeader>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateJobPlanStatus(int id, [FromBody] UpdateJobPlanStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jobPlan = await _context.JobPlansHeaders
                .FirstOrDefaultAsync(jp => jp.JobPlanId == id);

            if (jobPlan == null)
                return NotFound();

            var currentTime = DateTime.UtcNow;
            var currentUser = User?.Identity?.Name ?? "System";

            jobPlan.JobPlanStatus = request.JobPlanStatus;
            jobPlan.DateModified = currentTime;
            jobPlan.ModifiedBy = currentUser;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated job plan {JobPlanId} status to {Status} by {User}",
                jobPlan.JobPlanId, request.JobPlanStatus, currentUser);

            return Ok(jobPlan);
        }

        // DELETE: api/jobplans/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJobPlan(int id)
        {
            var jobPlan = await _context.JobPlansHeaders
                .Include(jp => jp.JobPlansLines)
                .FirstOrDefaultAsync(jp => jp.JobPlanId == id);

            if (jobPlan == null)
                return NotFound();

            var currentUser = User?.Identity?.Name ?? "System";

            _context.JobPlansHeaders.Remove(jobPlan);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted job plan {JobPlanId} by {User}", jobPlan.JobPlanId, currentUser);

            return NoContent();
        }

        // GET: api/jobplans/active
        [HttpGet("active")]
        [ProducesResponseType<IEnumerable<JobPlansHeader>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveJobPlans()
        {
            var activeJobPlans = await _context.JobPlansHeaders
                .Where(jp => jp.JobPlanStatus == true)
                .OrderBy(jp => jp.JobPlanShortDesc)
                .ToListAsync();

            return Ok(activeJobPlans);
        }

        // GET: api/jobplans/by-asset-class/{assetClassId}
        [HttpGet("by-asset-class/{assetClassId}")]
        [ProducesResponseType<IEnumerable<JobPlansHeader>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobPlansByAssetClass(int assetClassId)
        {
            var jobPlans = await _context.JobPlansHeaders
                .Where(jp => jp.AssetClassId == assetClassId)
                .OrderBy(jp => jp.JobPlanShortDesc)
                .ToListAsync();

            return Ok(jobPlans);
        }

        // GET: api/jobplans/inactive
        [HttpGet("inactive")]
        [ProducesResponseType<IEnumerable<JobPlansHeader>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInactiveJobPlans()
        {
            var inactiveJobPlans = await _context.JobPlansHeaders
                .Where(jp => jp.JobPlanStatus == false)
                .OrderBy(jp => jp.JobPlanShortDesc)
                .ToListAsync();

            return Ok(inactiveJobPlans);
        }

        // GET: api/jobplans/search
        [HttpGet("search")]
        [ProducesResponseType<IEnumerable<JobPlansHeader>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchJobPlans([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term is required");

            var jobPlans = await _context.JobPlansHeaders
                .Where(jp =>
                    jp.JobPlanShortDesc.Contains(searchTerm) ||
                    jp.JobPlanLongDesc.Contains(searchTerm) ||
                    jp.AssetClassShortDesc.Contains(searchTerm))
                .OrderBy(jp => jp.JobPlanShortDesc)
                .ToListAsync();

            return Ok(jobPlans);
        }

        // GET: api/jobplans/stats
        [HttpGet("stats")]
        [ProducesResponseType<JobPlanStatsResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobPlanStats()
        {
            var totalCount = await _context.JobPlansHeaders.CountAsync();
            var activeCount = await _context.JobPlansHeaders.CountAsync(jp => jp.JobPlanStatus == true);
            var inactiveCount = totalCount - activeCount;

            var assetClassStats = await _context.JobPlansHeaders
                .GroupBy(jp => new { jp.AssetClassId, jp.AssetClassShortDesc })
                .Select(g => new AssetClassJobPlanCount
                {
                    AssetClassId = g.Key.AssetClassId,
                    AssetClassShortDesc = g.Key.AssetClassShortDesc,
                    JobPlanCount = g.Count()
                })
                .OrderBy(ac => ac.AssetClassShortDesc)
                .ToListAsync();

            var stats = new JobPlanStatsResponse
            {
                TotalJobPlans = totalCount,
                ActiveJobPlans = activeCount,
                InactiveJobPlans = inactiveCount,
                AssetClassBreakdown = assetClassStats
            };

            return Ok(stats);
        }
        [HttpGet("next-id")]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNextJobPlanId()
        {
            try
            {
                // Get max existing ID
                var maxId = await _context.JobPlansHeaders
                    .MaxAsync(jp => (int?)jp.JobPlanId) ?? 0;

                return Ok(maxId + 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching next Job Plan ID");
                return StatusCode(500, "Failed to generate next Job Plan ID");
            }
        }
    }

    // Request Models
    public class CreateJobPlanRequest
    {
        [Required]
        public bool JobPlanStatus { get; set; } = true;

        [Required]
        [StringLength(100)] // Updated to match model MaxLength(100)
        public string JobPlanShortDesc { get; set; } = string.Empty;

        //[Required]
        public string JobPlanLongDesc { get; set; } = string.Empty;

        [Required]
        public int AssetClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssetClassShortDesc { get; set; } = string.Empty;

        public List<CreateJobPlanLineRequest>? JobPlanLines { get; set; }
    }

    public class UpdateJobPlanRequest
    {
        [Required]
        public bool JobPlanStatus { get; set; }

        //[Required]
        //[StringLength(100)] // Updated to match model MaxLength(100)
        public string JobPlanShortDesc { get; set; } = string.Empty;

        //[Required]
        public string JobPlanLongDesc { get; set; } = string.Empty;

        [Required]
        public int AssetClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssetClassShortDesc { get; set; } = string.Empty;

        public List<UpdateJobPlanLineRequest>? JobPlanLines { get; set; }
    }

    public class UpdateJobPlanStatusRequest
    {
        [Required]
        public bool JobPlanStatus { get; set; }
    }

    public class CreateJobPlanLineRequest
    {
        [Required]
        public long JobPlanLineNo { get; set; }

        [Required]
        [StringLength(500)]
        public string JobPlanLineDesc { get; set; } = string.Empty;
    }

    public class UpdateJobPlanLineRequest
    {
        public long JobPlanLineId { get; set; }

        [Required]
        public long JobPlanLineNo { get; set; }

        [Required]
        [StringLength(500)]
        public string JobPlanLineDesc { get; set; } = string.Empty;
    }

    public class JobPlanQueryParameters
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public int? AssetClassId { get; set; }

        public bool? Status { get; set; }

        public string? SearchTerm { get; set; }
    }

    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    // Additional Response Models
    public class JobPlanStatsResponse
    {
        public int TotalJobPlans { get; set; }
        public int ActiveJobPlans { get; set; }
        public int InactiveJobPlans { get; set; }
        public List<AssetClassJobPlanCount> AssetClassBreakdown { get; set; } = new();
    }

    public class AssetClassJobPlanCount
    {
        public int AssetClassId { get; set; }
        public string AssetClassShortDesc { get; set; } = string.Empty;
        public int JobPlanCount { get; set; }
    }
    public class JobPlanWithTasksResponse
    {
        public int JobPlanId { get; set; }
        public bool JobPlanStatus { get; set; }
        public string JobPlanShortDesc { get; set; } = string.Empty;
        public string JobPlanLongDesc { get; set; } = string.Empty;
        public int AssetClassId { get; set; }
        public string AssetClassShortDesc { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
        public List<TaskResponse> Tasks { get; set; } = new();
    }
    public class TaskResponse
    {
        public int JobPlanLineId { get; set; }
        public long JobPlanLineNo { get; set; }
        public string JobPlanLineDesc { get; set; } = string.Empty;
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;

namespace TWMSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPlansLineController : ControllerBase
    {
        private readonly TWMSServerContext _context;

        public JobPlansLineController(TWMSServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all job plan lines
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobPlansLine>>> GetAll()
        {
            try
            {
                var jobPlansLines = await _context.JobPlansLines
                    .OrderBy(jpl => jpl.JobPlanLineNo)
                    .ToListAsync();

                return Ok(jobPlansLines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get job plan line by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<JobPlansLine>> GetById(int id)
        {
            try
            {
                var jobPlansLine = await _context.JobPlansLines
                    .FirstOrDefaultAsync(jpl => jpl.JobPlanLineId == id);

                if (jobPlansLine == null)
                {
                    return NotFound($"Job plan line with ID {id} not found");
                }

                return Ok(jobPlansLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Get all job plan lines by JobPlanId
        /// </summary>
        /// <param name="id">The JobPlanId to filter by</param>
        /// <returns>A list of job plan lines associated with the given JobPlanId</returns>
        [HttpGet("by-jobplan-id/{id}")]
        public async Task<ActionResult<IEnumerable<JobPlansLine>>> GetByJobPlanId(int id)
        {
            try
            {
                // Fetch all job plan lines associated with the given JobPlanId
                var jobPlansLines = await _context.JobPlansLines
                    .Where(jpl => jpl.JobPlanId == id)
                    .ToListAsync();

                // Check if no records were found
                if (jobPlansLines == null || jobPlansLines.Count == 0)
                {
                    return NotFound($"No job plan lines found for JobPlanId {id}");
                }

                return Ok(jobPlansLines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new job plan line
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<JobPlansLine>> Create([FromBody] JobPlansLineDto newJobPlansLineDto)
        {
            try
            {
                if (newJobPlansLineDto == null)
                {
                    return BadRequest("Job plan line is null");
                }

                var newJobPlansLine = new JobPlansLine
                {
                    JobPlanId = newJobPlansLineDto.JobPlanId,
                    JobPlanLineNo = newJobPlansLineDto.JobPlanLineNo,
                    JobPlanLineDesc = newJobPlansLineDto.JobPlanLineDesc
                };

                _context.JobPlansLines.Add(newJobPlansLine);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = newJobPlansLine.JobPlanLineId }, newJobPlansLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        /// Update an existing job plan line
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] JobPlansLineDto updatedJobPlansLineDto)
        {
            try
            {
                if (updatedJobPlansLineDto == null)
                {
                    return BadRequest("Invalid job plan line data");
                }

                var existingJobPlansLine = await _context.JobPlansLines
                    .FirstOrDefaultAsync(jpl => jpl.JobPlanLineId == id);

                if (existingJobPlansLine == null)
                {
                    return NotFound($"Job plan line with ID {id} not found");
                }

                // Update properties
                existingJobPlansLine.JobPlanId = updatedJobPlansLineDto.JobPlanId;
                existingJobPlansLine.JobPlanLineNo = updatedJobPlansLineDto.JobPlanLineNo;
                existingJobPlansLine.JobPlanLineDesc = updatedJobPlansLineDto.JobPlanLineDesc;

                _context.JobPlansLines.Update(existingJobPlansLine);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a job plan line by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var jobPlansLine = await _context.JobPlansLines
                    .FirstOrDefaultAsync(jpl => jpl.JobPlanLineId == id);

                if (jobPlansLine == null)
                {
                    return NotFound($"Job plan line with ID {id} not found");
                }

                _context.JobPlansLines.Remove(jobPlansLine);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    // DTO for creating/updating JobPlansLine
    public class JobPlansLineDto
    {
        public int JobPlanId { get; set; }
        public long JobPlanLineNo { get; set; }
        public string JobPlanLineDesc { get; set; } = string.Empty;
    }
}

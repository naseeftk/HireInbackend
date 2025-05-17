using Application.DTO;
using Application.Interface.IService;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplyController : BaseController
    {
        private readonly IJobApplyService jobApply;
        private readonly ILogger<JobApplyController> _logger;


        public JobApplyController(IJobApplyService _jobApply, ILogger<JobApplyController> logger)
        {
            jobApply = _jobApply;
            _logger = logger;
        }
        [HttpGet("TotalApplication")]
        public async Task<IActionResult> TotalApplication()
        {
            var result = await jobApply.TotalAppliedApplication(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("AllApplications/by an User")]
        public async Task<IActionResult> GetAllApplications()
        {
            try
            {
                var result = await jobApply.GetAllApplicationsAsync(UserId);
                return StatusCode(result.StatusCode, result);

            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "while fetching the applied job");
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
            
        }

        [HttpGet("ApplicationsByStatus")]
        public async Task<IActionResult> GetApplicationsByStatus( [FromQuery] string status)
        {
            var result = await jobApply.GetApplicationsByStatusAsync(UserId, status);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ApplicationsByDate")]
        public async Task<IActionResult> GetApplicationsByDate( [FromQuery] DateTime date)
        {
            var result = await jobApply.GetApplicationsByDateAsync(UserId, date);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("SearchApplications")]
        public async Task<IActionResult> SearchApplications( [FromQuery] string keyword)
        {
            var result = await jobApply.SearchApplicationsAsync(UserId, keyword);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("ApplyToJob")]
    
        public async Task<IActionResult> ApplyToJob([FromForm] ApplyJobDto applyDto, [FromQuery] Guid jobPostId)
        {
            var result = await jobApply.ApplyToJobAsync(applyDto, UserId, jobPostId);
            if (applyDto.Resume == null)
            {
                _logger.LogError("Resume file is null in controller.");
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("RemoveApplication")]
        public async Task<IActionResult> RemoveApplication( Guid applicationId)
        {
            var result = await jobApply.RemoveApplicationAsync(UserId, applicationId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

using Application.DTO;
using Application.DTO.ResponseDTO;
using Application.Interface.IService;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostController : BaseController
    {
        private readonly IJobPostService jobPostService;

        public JobPostController(IJobPostService _jobPostService)
        {
            jobPostService = _jobPostService;
        }

        [HttpPost("postjob")]
        public async Task<IActionResult> AddJobPost([FromBody] JobPostAddDto jobPost)
        {
            var result = await jobPostService.AddJobPost(jobPost, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("RemoveJobPost/employer")]
        public async Task<IActionResult> RemoveJobPost(Guid jobId)
        {
            var result = await jobPostService.RemoveJobPost(jobId, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("employer/GetAllJobpost")]
        public async Task<IActionResult> GetAllJobPostsByEmployer()
        {
            var result = await jobPostService.GetAllJobPostsByEmployer(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("viewJobPost")]
        public async Task<IActionResult> ViewJobPost(Guid jobId)
        {
            var result = await jobPostService.ViewJobPost(jobId,UserId    );
            return StatusCode(result.StatusCode, result);
        }
            [HttpGet("viewCategory")]
        public async Task<IActionResult> ViewJobCategory()
        {
            var result = await jobPostService.GetAllJobcategory();
            return Ok( result);
        }
            [HttpGet("viewJobType")]
        public async Task<IActionResult> ViewJobType()
        {
            var result = await jobPostService.GetAllJobType(  );
            return Ok(result);
        }
        [HttpPost("AddNewCategory")]
        public async Task<IActionResult> AddNewCategory(string category)
        {
          await   jobPostService.AddNewCategory(category);
            return Ok("Added New category");
        }

        [HttpPut("updateJobPost")]
        public async Task<IActionResult> UpdateJobPost([FromQuery]Guid jobId, [FromForm] JobPostAddDto jobPost)
        {
            var result = await jobPostService.UpdateJobPost(jobId, jobPost, UserId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

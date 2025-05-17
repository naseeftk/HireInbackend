using Application.DTO;
using Application.Interface.IService;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListController : BaseController
    {
        private readonly IJobListService jobList;

        public JobListController(IJobListService _jobList)
        {
            jobList = _jobList;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await jobList.GetAllJobPostsAsync();
            return StatusCode(result.StatusCode, result);
        }
               [HttpGet("Location")]
        public async Task<IActionResult> GetAllLocation()
        {
            var result = await jobList.AllJobLocation();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveJobPost( [FromQuery] Guid postId)
        {
            var result = await jobList.SaveJobPostAsync(UserId, postId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Unsave")]
        public async Task<IActionResult> UnsaveJobPost( [FromQuery] Guid postId)
        {
            var result = await jobList.UnsaveJobPostAsync(UserId, postId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Saved/Jobs")]
        public async Task<IActionResult> GetSavedJobs()
        {
            var result = await jobList.GetSavedJobPostsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchJobPosts([FromQuery] string searchText)
        {
            var result = await jobList.SearchJobPostsAsync(searchText);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("SearchSaved")]
        public async Task<IActionResult> SearchSavedJobPosts( [FromQuery] string searchText)
        {
            var result = await jobList.SearchSavedJobPostsAsync(UserId, searchText);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Filter")]
        public async Task<IActionResult> FilterJobPosts([FromBody] FilterJobListDto filter)
        {
            var result = await jobList.FilterJobPostsAsync(filter);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Details/{jobId}")]
        public async Task<IActionResult> GetJobDetails(Guid jobId)
        {
            var result = await jobList.GetJobDetailsAsync(jobId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

using Application.DTO;
using Application.DTO.ResponseDTO;
using Application.Interface.IService;
using Domain.Entities;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterViewController : BaseController
    {
        private readonly IInterviewService interview;

        public InterViewController(IInterviewService _interview)
        {
            interview = _interview;
        }

        [HttpGet("Statuses")]
        public async Task<IActionResult> GetInterviewStatuses()
        {
            var result = await interview.GetInterviewStatusesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Modes")]
        public async Task<IActionResult> GetInterviewModes()
        {
            var result = await interview.GetInterviewModesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Schedule/Interview")]
        public async Task<IActionResult> ScheduleInterview([FromForm] InterviewScheduleDto interviewDto)
        {
            var result = await interview.ScheduleInterviewAsync(interviewDto, UserId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("SelectedEmployees")]
        public async Task<IActionResult> UpdateInterviewResult([FromForm] SelectedInterviewDto employs)
        {

            var result = await interview.InterviewResult( employs,UserId);
            return StatusCode(result.StatusCode, result);
        }
        

        [HttpGet("Scheduled/Employer")]
        public async Task<IActionResult> ViewScheduledInterviews()
        {
            var result = await interview.ViewScheduledInterviewsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("JobSeeker/Interview")]
        public async Task<IActionResult> ViewJobSeekerInterviews()
        {
            var result = await interview.ViewJobSeekerInterviewsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }  
        [HttpGet("Employer/SpecificInterview")]
        public async Task<IActionResult> ViewSpecificInterviews(Guid appId)
        {
            var result = await interview.viewSpecificInterview(appId,UserId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

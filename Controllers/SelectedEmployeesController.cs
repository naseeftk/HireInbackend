using Application.DTO;
using Application.Interface.IRepository;
using Application.Interface.IService;
using Application.Services;
using Domain.Entities;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectedEmployeesController : BaseController
    {
        private readonly ISelectedEmployeesService selectedEmployees;
        public SelectedEmployeesController(ISelectedEmployeesService _selectedEmployees)
        {
               selectedEmployees = _selectedEmployees;
        }

        [HttpGet("JobseekerAllInterviewDetails")]
        public async Task<IActionResult> JobSeekerAllInterview()
        {
            var result = await selectedEmployees.JobSeekerAllInterviews(UserId);
            return StatusCode(result.StatusCode, result);
        }      
        [HttpGet("Employer InterviewDetails")]
        public async Task<IActionResult> EmplouyerAllInterview()
        {
            var result = await selectedEmployees.EmployerSelected(UserId);
            return StatusCode(result.StatusCode, result);
        }  
        [HttpPatch("DeleteInterview")]
        public async Task<IActionResult> DeleteInterviewResult(Guid interviewId)
        {
            var result = await selectedEmployees.DeleteInterviewResult(interviewId);
            return StatusCode(result.StatusCode, result);
        }
    }
}

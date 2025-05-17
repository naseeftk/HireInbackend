using System;
using Application.DTO;
using Application.DTO.ResponseDTO;
using Application.Interface.IService;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : BaseController
    {
        private readonly IApplicantService applicant;

        public ApplicantController(IApplicantService _applicant)
        {
            applicant = _applicant;
        }

        [HttpGet("GetApplicants")]
        public async Task<IActionResult> GetApplicants()
        {
            var result = await applicant.GetApplicantsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetApplicantByApplicationId")]
        public async Task<IActionResult> GetApplicantByAppicationId( Guid appId)
        {
            var result = await applicant.GetApplicantByUserIdAsync(UserId, appId);
            return StatusCode(result.StatusCode, result);
        }
       [HttpGet("getResume")]
       public async Task<IActionResult>getTheResume(Guid appId)
        {
            var result = await applicant.getResume(appId);
            return File(result.Data, "application/pdf", "resume.pdf");
        }
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeApplicantStatus(Guid applicationId, Guid statusId)
        {
            var result = await applicant.ChangeApplicantStatusAsync(applicationId, statusId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Saved/Applicant")]
        public async Task<IActionResult> GetSavedApplicants()
        {
            var result = await applicant.GetSavedApplicantsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("SoftDelete/Delete an application")]
        public async Task<IActionResult> SoftDeleteApplication(Guid applicationId)
        {
            var result = await applicant.SoftDeleteApplicationAsync(applicationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Save/{applicationId}")]
        public async Task<IActionResult> SaveApplicant(Guid applicationId)
        {
            var result = await applicant.SaveApplicantAsync(applicationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Paginated")]
        public async Task<IActionResult> GetApplicantsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await applicant.GetApplicantsPaginatedAsync(UserId, page, pageSize, search);
            return StatusCode(result.StatusCode, result);
        }
    }
}

using Application.Interface.IService;
using Domain.Entities;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : BaseController
    {

        private readonly ISubscriptionService subService;
        public SubscriptionController(ISubscriptionService _subService)
        {
            subService = _subService;
        }
        [HttpGet("companyInfoForAdmin")]
        public async Task<IActionResult> companyDetails()
        {

            var result = await subService.FetchSubscriptionDetailsByAdmin();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("SubInfoForEmployer")]
        public async Task<IActionResult> SubDetailsEmployer()
        {

            var result = await subService.FetchSubscriptionDetails(UserId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("changeSubMode")]
        public async Task<IActionResult> changeSubMode()
        {

            var result = await subService.HandleSubscriptionStatus(UserId);
            return StatusCode(result.StatusCode, result);
        }     
 
        [HttpPatch("upgradeSub")]
        public async Task<IActionResult> upgradeSubscription()
        {

            var result = await subService.UpgradeSubScriptionDate(UserId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
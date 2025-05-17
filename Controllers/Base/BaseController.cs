using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid UserId =>
      Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);


        protected string? Role =>                                                                   
            User.FindFirst(ClaimTypes.Role)?.Value;

    }
}   
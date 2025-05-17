using Application.DTO;
using Application.Interface.IService;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using HireIn.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireIn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("getTheRole")]
        public async Task<string> GetTheRole()
        {
            var str = Role;
            return str;
        }
         [HttpGet("fetchAllUsers")]
        public async Task<IActionResult> fetchUsers()
        {
            var result = await _authService.fetchAllUsers();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> RegisterNewUser([FromForm] UserRegisterDto user)
        {
            var result = await _authService.SignUp(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authService.Login(loginDto);
            var result = response.Data;
            if (result == null)
                return Unauthorized();

            // Set accessToken in a cookie
            Response.Cookies.Append("accessToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Use Lax if your frontend is on same domain
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new
            {
                result.Id,
                result.Username,
                result.IsActive,
                result.RefreshToken,
                result.ProfilePicture// You may also set this in a cookie if needed
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new { message = "Logged out successfully" });
        }


        // ✅ New endpoint to refresh JWT token
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest model)
        {
            var result = await _authService.RefreshToken(model.RefreshToken);
            if (result.StatusCode != 200)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                Token = result.Data.Token,
                RefreshToken = result.Data.RefreshToken
            });
        }

        [HttpPatch("BlockCompany")]
        public async Task<IActionResult> Block_A_Company (Guid organisationId)
        {
            var result = await _authService.BlockCompany(organisationId);
            return StatusCode(result.StatusCode, result);
        }  
        [HttpPatch("BlockUser")]
        public async Task<IActionResult> Block_A_Users (Guid _userId)
        {
            var result = await _authService.BlockUsers(_userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("ApproveCompany")]
        public async Task<IActionResult> ApproveCompany (Guid organisationId)
        {
            var result = await _authService.ApproveCompany(organisationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("UnBlockCompany")]
        public async Task<IActionResult> UnBlock_A_Company(Guid organisationId)
        {
            var result = await _authService.UnBlockCompany(organisationId);
            return StatusCode(result.StatusCode, result);
        }

    }

}
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }

}




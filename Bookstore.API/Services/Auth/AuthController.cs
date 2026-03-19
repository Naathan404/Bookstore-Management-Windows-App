using Microsoft.AspNetCore.Mvc;
using Bookstore.API.Services.Interface;
using Bookstore.Share.DTORequests;
using Bookstore.API.Helpers;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
            {

                return Unauthorized(new { message = "Sai tên tài khoản hoặc mật khẩu!" });
            }

            return Ok(result);
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckMail(string email)
        {
            var exist = _authService.IsEmailExistAsync(email);
            return Ok(new { IsExist = exist });
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] string email)
        {
            var exist = await _authService?.IsEmailExistAsync(email);
            if (!exist) return NotFound();
            string otp = OTPGenerator.GenerateOTPCode(6);
            await 
        }
    }
}
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
            var exist = await _authService.IsEmailExistAsync(email);
            return Ok(new { IsExist = exist });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var result = await _authService.RequestOTPAsync(email);
            if (!result) return NotFound(new { message = "Email chưa được đăng ký thành viên Sahara!" });

            return Ok(new { message = "Đã gửi OTP, check mail đi mẹ!" });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpRequest request)
        {
            var isValid = await _authService.VerifyOTPAsync(request.Email, request.Otp);

            if (!isValid)
            {
                return BadRequest(new { message = "Mã OTP không đúng hoặc đã hết hạn!" });
            }

            return Ok(new { message = "Xác thực thành công!" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request.Email, request.NewPassword);
            if (!result) return BadRequest(new {message = "bad request, khong tim thay user voi email can doi mat khau"} );
            return Ok(new {message = "Doi mat khau thanh cong"});
        }
    }
}
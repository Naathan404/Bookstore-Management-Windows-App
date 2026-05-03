using Bookstore.API.Data;
using Bookstore.API.Services;
using Bookstore.API.Utils;
using Bookstore.Share.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace Bookstore.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMailService _mailService;
        public AuthController(AppDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var hashPw = HashHelper.SHA256_Encode(HashHelper.Base64_Encode(request.Password));
            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u => u.TenDangNhap == request.Username && u.MatKhau == hashPw);

            if (user == null)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không chính xác" });
            }

            // đăng nhập thành công
            var userInfo = new
            {
                Username = user.TenDangNhap,
                TenNguoiDung = user.HoTen
            };

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = userInfo
            });
        }

        /// <summary>
        /// gửi mã otp
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("forgot-pw")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                return BadRequest("Email không tồn tại trong hệ thống");
            }

            string otpCode = OTPGenerator.GenerateOTPCode(6);

            // sinh mã otp
            user.MaOTP = otpCode;
            user.HanOTP = DateTime.Now.AddMinutes(3);
            await _context.SaveChangesAsync();

            // Gửi email
            string subject = "Sahara Bookstore - Mã xác nhận khôi phục mật khẩu";
            string body = $"Chào bạn,\n\nMã xác nhận (OTP) của bạn là: {otpCode}\n\nMã này sẽ hết hạn sau 3 phút. Vui lòng không chia sẻ mã này cho bất kỳ ai.\n\nTrân trọng,\nSahara Bookstore";

            await _mailService.SendEmailAsync(email, subject, body);
            return Ok(new { message = "Gửi mã thành công!" });
        }

        /// <summary>
        /// xác thực otp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpRequest request)
        {
            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || string.IsNullOrEmpty(user.MaOTP))
            {
                return BadRequest("Yêu cầu không hợp lệ!");
            }

            if (user.MaOTP != request.Otp)
            {
                return BadRequest("Mã OTP không chính xác!");
            }

            if (user.HanOTP < DateTime.Now)
            {
                return BadRequest("Mã OTP đã hết hạn!");
            }

            // Mã đúng và còn hạn -> Cho phép qua bước Reset
            return Ok(new { message = "Xác thực thành công!" });
        }

        /// <summary>
        /// Đặt lại mật khẩu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Người dùng không tồn tại!");
            }

            // Băm mật khẩu mới trước khi lưu
            user.MatKhau = HashHelper.SHA256_Encode(HashHelper.Base64_Encode(request.NewPassword));

            // Xóa mã OTP để không dùng lại được nữa
            user.MaOTP = null;
            user.HanOTP = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công!" });
        }
    }
}

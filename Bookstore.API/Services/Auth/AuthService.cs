using Bookstore.API.Helpers;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.API.Services.Interface;
using Bookstore.Share.DTORequests;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookstore.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Account> _repository;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;

        // Tiêm Repository và Config vào Service
        public AuthService(IGenericRepository<Account> repo, IConfiguration config, IMailService mailService)
        {
            _repository = repo;
            _config = config;
            _mailService = mailService;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var accounts = await _repository.GetAllAsync();
            var user = accounts.FirstOrDefault(a => a.Username == request.username);

            if (user == null || user.PasswordHash != request.password)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), //
                signingCredentials: creds
            );

            var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse
            {
                Token = jwtString,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            if(email.IsNullOrEmpty()) return false;
            return await _repository.AnyAsync(x => x.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<bool> RequestOTPAsync(string email)
        {
            if (!await IsEmailExistAsync(email)) return false;

            string otp = OTPGenerator.GenerateOTPCode(6);
            await SaveOTPAsync(email, otp);
            string subject = "🌵 Mã xác thực khôi phục mật khẩu - Nhà sách SAHARA 🌵";
            string body = $"Mã OTP khôi phục mật khẩu của bạn là {otp}. \n Đừng chia sẻ mã này cho bất kì ai.";
            await _mailService.SendEmailAsync(email, subject, body);

            return true;

        }

        public async Task SaveOTPAsync(string email, string otp)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Email.ToLower() == email.Trim().ToLower());
            if (user == null) return;

            user.OTP = otp;
            user.OTPExpire = DateTime.Now.AddMinutes(5);

            _repository.Update(user);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> VerifyOTPAsync(string email, string otp)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp)) return false;
            var searchEmail = email.Trim().ToLower();
            var user = await _repository.FirstOrDefaultAsync(x => x.Email.ToLower() == searchEmail);

            if (user == null) return false; // khong tim được usserr
            if (user.OTP.IsNullOrEmpty()) return false;
            if (user.OTP!.Trim().ToLower() == otp.Trim().ToLower() && user.OTPExpire > DateTime.Now)
            {
                user.OTP = null;
                user.OTPExpire = null;
                _repository.Update(user);
                await _repository.SaveChangesAsync();

                return true; 
            }

            return false;
        }
    }
}

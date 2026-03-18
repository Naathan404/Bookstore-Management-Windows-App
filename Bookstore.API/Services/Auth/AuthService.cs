using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.API.Services.Interface;
using Bookstore.Share.DTORequests;
using Bookstore.Share.DTOResponses;
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

        // Tiêm Repository và Config vào Service
        public AuthService(IGenericRepository<Account> repo, IConfiguration config)
        {
            _repository = repo;
            _config = config;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var accounts = await _repository.GetAllAsync();
            var user = accounts.SingleOrDefault(a => a.Username == request.Username);

            if (user == null || user.PasswordHash != request.Password)
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
    }
}

using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;

namespace Bookstore.API.Services.Interface
{
    public interface IAuthService
    {
        Task<Share.DTOResponses.LoginResponse?> LoginAsync(Share.DTORequests.LoginRequest request);
        Task<bool> IsEmailExistAsync(string email);
        Task SaveOTPAsync(string email, string otp);
        Task<bool> RequestOTPAsync(string email);
        Task<bool> VerifyOTPAsync(string email, string otp);
    }
}

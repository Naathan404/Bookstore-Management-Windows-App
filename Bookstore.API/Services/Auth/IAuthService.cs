using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;

namespace Bookstore.API.Services.Interface
{
    public interface IAuthService
    {
        Task<Share.DTOResponses.LoginResponse?> LoginAsync(Share.DTORequests.LoginRequest request);
    }
}

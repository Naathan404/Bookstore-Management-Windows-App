using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.API.Services.Interface;
using Bookstore.Share.DTORequests;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<User>
    {
        private readonly IAuthService _authService;
        public AccountsController(IGenericRepository<User> repo, IAuthService authService) : base(repo)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu!");
            }

            return Ok(result);
        }
    }
}

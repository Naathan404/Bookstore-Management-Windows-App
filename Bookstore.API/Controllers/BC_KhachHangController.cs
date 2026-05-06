using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BC_KhachHangController : BaseController<BC_KhachHang, int>
    {
        public BC_KhachHangController(IGenericRepository<BC_KhachHang, int> repo) : base(repo)
        {
        }
    }
}

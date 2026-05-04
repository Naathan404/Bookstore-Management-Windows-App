using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : BaseController<HoaDon, int>
    {
        public HoaDonController(IGenericRepository<HoaDon, int> repo) : base(repo)
        {
        }
    }
}

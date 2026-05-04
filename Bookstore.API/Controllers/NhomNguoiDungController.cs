using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomNguoiDungController : BaseController<NhomNguoiDung, int>
    {
        public NhomNguoiDungController(IGenericRepository<NhomNguoiDung, int> repo) : base(repo)
        {
        }
    }
}

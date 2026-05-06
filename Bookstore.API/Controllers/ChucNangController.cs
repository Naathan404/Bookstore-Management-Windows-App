using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucNangController : BaseController<ChucNang, int>
    {
        public ChucNangController(IGenericRepository<ChucNang, int> repo) : base(repo)
        {
        }
    }
}

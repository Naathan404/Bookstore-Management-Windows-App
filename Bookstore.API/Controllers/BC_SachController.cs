using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BC_SachController : BaseController<BC_Sach, int>
    {
        public BC_SachController(IGenericRepository<BC_Sach, int> repo) : base(repo)
        {
        }
    }
}

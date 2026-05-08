using Bookstore.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NhaCungCapController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNhaCungCap()
        {
            var list = await _context.NhaCungCap
                                     .Select(ncc => ncc.TenNhaCungCap)
                                     .ToListAsync();
            return Ok(list);
        }
    }

}

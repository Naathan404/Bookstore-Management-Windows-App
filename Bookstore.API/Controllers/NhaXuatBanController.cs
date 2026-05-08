using Bookstore.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaXuatBanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NhaXuatBanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNhaXuatBan()
        {
            // Lấy danh sách tên NXB từ bảng NhaXuatBan trong DB
            var list = await _context.NhaXuatBan
                                     .Select(nxb => nxb.TenNhaXuatBan)
                                     .ToListAsync();
            return Ok(list);
        }
    }
}

using Bookstore.API.Data;
using Bookstore.API.Models;
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

        [HttpGet("names")]
        public async Task<IActionResult> GetAllNhaXuatBan()
        {
            // Lấy danh sách tên NXB từ bảng NhaXuatBan trong DB
            var list = await _context.NhaXuatBan
                                     .Select(nxb => nxb.TenNhaXuatBan)
                                     .ToListAsync();
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.NhaXuatBan
                .Select(n => new { Id = n.MaNhaXuatBan, Name = n.TenNhaXuatBan })
                .ToListAsync();
            return Ok(list);
        }

        public class NXBCreateDTO { public string TenNhaXuatBan { get; set; } = string.Empty; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NXBCreateDTO request)
        {
            if (await _context.NhaXuatBan.AnyAsync(n => n.TenNhaXuatBan.ToLower() == request.TenNhaXuatBan.ToLower()))
                return BadRequest("Nhà xuất bản đã tồn tại.");

            var newNXB = new NhaXuatBan { TenNhaXuatBan = request.TenNhaXuatBan };
            _context.NhaXuatBan.Add(newNXB);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm thành công" });
        }

    }
}

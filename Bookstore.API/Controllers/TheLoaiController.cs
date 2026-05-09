using Bookstore.API.Data;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TheLoaiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetAllTheLoaiNames()
        {
            try
            {
                var listTheLoai = await _context.TheLoai
                                                .Select(tl => tl.TenTheLoai)
                                                .ToListAsync();

                return Ok(listTheLoai);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.TheLoai
                .Select(t => new { Id = t.MaTheLoai, Name = t.TenTheLoai })
                .ToListAsync();
            return Ok(list);
        }

        public class TheLoaiCreateDTO { public string TenTheLoai { get; set; } }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TheLoaiCreateDTO request)
        {
            if (await _context.TheLoai.AnyAsync(t => t.TenTheLoai.ToLower() == request.TenTheLoai.ToLower()))
                return BadRequest("Thể loại đã tồn tại.");

            var newTheLoai = new TheLoai { TenTheLoai = request.TenTheLoai };
            _context.TheLoai.Add(newTheLoai);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm thành công" });
        }
    }
}
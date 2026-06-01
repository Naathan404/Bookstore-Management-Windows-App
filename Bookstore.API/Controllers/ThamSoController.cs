using Bookstore.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThamSoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThamSoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThamSoDTO>>> GetAllThamSo()
        {
            var thamSos = await _context.ThamSo.ToListAsync();
            return Ok(thamSos.Select(t => new ThamSoDTO
            {
                TenThamSo = t.TenThamSo,
                GiaTri = t.GiaTri
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThamSoDTO>> GetThamSo(string id)
        {
            var thamSo = await _context.ThamSo.FindAsync(id);
            if (thamSo == null)
                return NotFound(new { Message = $"Không tìm thấy tham số: {id}" });

            return Ok(new ThamSoDTO
            {
                TenThamSo = thamSo.TenThamSo,
                GiaTri = thamSo.GiaTri
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateThamSo(string id, [FromBody] ThamSoDTO request)
        {
            // Kiểm tra khớp mã
            if (id != request.TenThamSo)
                return BadRequest(new { Message = "Tên tham số trên URL và Body không khớp nhau" });

            var thamSo = await _context.ThamSo.FindAsync(id);
            if (thamSo == null)
                return NotFound(new { Message = $"Không tìm thấy tham số: {id}" });

            // Cập nhật
            thamSo.GiaTri = request.GiaTri;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Cập nhật tham số {id} thành công" });
        }
    }
}
using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TacGiaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.TacGia
                .Select(t => new TacGiaDTO { Id = t.MaTacGia, TenTacGia = t.TenTacGia })
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TacGiaDTO tacGiaDto)
        {
            if (string.IsNullOrWhiteSpace(tacGiaDto.TenTacGia))
            {
                return BadRequest("Tên tác giả không được để trống.");
            }

            // kiểm tra tác giả tồn tại chưa
            var existingTacGia = await _context.TacGia
                .FirstOrDefaultAsync(t => t.TenTacGia.ToLower() == tacGiaDto.TenTacGia.ToLower());

            if (existingTacGia != null)
            {
                return BadRequest("Tác giả đã tồn tại.");
            }

            // tạo tác giả mới
            var newTacGia = new Models.TacGia
            {
                TenTacGia = tacGiaDto.TenTacGia
            };
            _context.TacGia.Add(newTacGia);
            await _context.SaveChangesAsync();
            tacGiaDto.Id = newTacGia.MaTacGia;
            return CreatedAtAction(nameof(GetAll), new { id = tacGiaDto.Id }, tacGiaDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTacGia(int id)
        {
            try
            {
                var tg = await _context.TacGia.FirstOrDefaultAsync(t => t.MaTacGia == id);
                if (tg == null) return NotFound("Không tìm thấy tác giả");

                bool daBiRangBuoc = await _context.TacGia_Sach.AnyAsync(t => t.MaTacGia == id);
                if (daBiRangBuoc)
                    return BadRequest($"Không thể xóa tác giả {tg.TenTacGia} do đã được ghi nhận là tác giả của ít nhất 01 đầu sách.");

                _context.TacGia.Remove(tg);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

    }
}
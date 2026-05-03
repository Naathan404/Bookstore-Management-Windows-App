using Bookstore.API.Data;
// using Bookstore.API.Data; // Chú ý kiểm tra lại namespace DbContext
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDon_UuDaiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HoaDon_UuDaiController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ DANH SÁCH 
        // GET: api/HoaDon_UuDai
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon_UuDai>>> GetHoaDon_Uudai()
        {
            return await _context.HoaDon_Uudai.ToListAsync();
        }

        // LẤY DANH SÁCH KHUYẾN MÃI CỦA 1 HÓA ĐƠN
        // GET: api/HoaDon_UuDai/HoaDon/5
        [HttpGet("HoaDon/{maHoaDon}")]
        public async Task<ActionResult<IEnumerable<HoaDon_UuDai>>> GetByMaHoaDon(int maHoaDon)
        {
            return await _context.HoaDon_Uudai
                                 .Where(x => x.MaHoaDon == maHoaDon)
                                 .ToListAsync();
        }

        // LẤY CHI TIẾT ĐÚNG 1 DÒNG DỰA VÀO ID
        // GET: api/HoaDon_UuDai/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon_UuDai>> GetHoaDon_UuDai(int id)
        {
            var hoaDon_UuDai = await _context.HoaDon_Uudai.FindAsync(id);

            if (hoaDon_UuDai == null)
            {
                return NotFound(new { message = "Không tìm thấy lịch sử ưu đãi này!" });
            }

            return hoaDon_UuDai;
        }

        // CẬP NHẬT 
        // PUT: api/HoaDon_UuDai/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon_UuDai(int id, HoaDon_UuDai hoaDon_UuDai)
        {
            if (id != hoaDon_UuDai.MaCT_HoaDon_UuDai)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(hoaDon_UuDai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDon_UuDaiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // THÊM MỚI 
        // POST: api/HoaDon_UuDai
        [HttpPost]
        public async Task<ActionResult<HoaDon_UuDai>> PostHoaDon_UuDai(HoaDon_UuDai hoaDon_UuDai)
        {
            _context.HoaDon_Uudai.Add(hoaDon_UuDai);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHoaDon_UuDai), new { id = hoaDon_UuDai.MaCT_HoaDon_UuDai }, hoaDon_UuDai);
        }

        //  XÓA 
        // DELETE: api/HoaDon_UuDai/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon_UuDai(int id)
        {
            var hoaDon_UuDai = await _context.HoaDon_Uudai.FindAsync(id);
            if (hoaDon_UuDai == null)
            {
                return NotFound();
            }

            _context.HoaDon_Uudai.Remove(hoaDon_UuDai);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa lịch sử ưu đãi của hóa đơn!" });
        }

        private bool HoaDon_UuDaiExists(int id)
        {
            return _context.HoaDon_Uudai.Any(e => e.MaCT_HoaDon_UuDai == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.API.Data;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTUD_Sach_QuaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CTUD_Sach_QuaController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ DANH SÁCH
        // GET: api/CTUD_Sach_Qua
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CTUD_Sach_Qua>>> GetCTUD_Sach_Qua()
        {
            return await _context.CTUD_Sach_Qua.ToListAsync();
        }

        // LẤY CHI TIẾT THEO MÃ ƯU ĐÃI
        // GET: api/CTUD_Sach_Qua/UuDai/5
        [HttpGet("UuDai/{maUuDai}")]
        public async Task<ActionResult<IEnumerable<CTUD_Sach_Qua>>> GetByMaUuDai(int maUuDai)
        {
            return await _context.CTUD_Sach_Qua
                                 .Where(x => x.MaUuDai == maUuDai)
                                 .ToListAsync();
        }

        // LẤY ĐÚNG 1 DÒNG DỰA VÀO KHÓA CHÍNH
        // GET: api/CTUD_Sach_Qua/5
        [HttpGet("{maCT}")]
        public async Task<ActionResult<CTUD_Sach_Qua>> GetCTUD_Sach_Qua(int maCT)
        {
            var CTUD_Sach_Qua = await _context.CTUD_Sach_Qua.FindAsync(maCT);

            if (CTUD_Sach_Qua == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết ưu đãi này!" });
            }

            return CTUD_Sach_Qua;
        }

        // CẬP NHẬT
        // PUT: api/CTUD_Sach_Qua/5
        [HttpPut("{maCT}")]
        public async Task<IActionResult> PutCTUD_Sach_Qua(int maCT, CTUD_Sach_Qua CTUD_Sach_Qua)
        {
            if (maCT != CTUD_Sach_Qua.MaCT)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(CTUD_Sach_Qua).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTUD_Sach_QuaExists(maCT))
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
        // POST: api/CTUD_Sach_Qua
        [HttpPost]
        public async Task<ActionResult<CTUD_Sach_Qua>> PostCTUD_Sach_Qua(CTUD_Sach_Qua CTUD_Sach_Qua)
        {
            _context.CTUD_Sach_Qua.Add(CTUD_Sach_Qua);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCTUD_Sach_Qua),
                new { maCT = CTUD_Sach_Qua.MaCT },
                CTUD_Sach_Qua);
        }

        // XÓA
        // DELETE: api/CTUD_Sach_Qua/5
        [HttpDelete("{maCT}")]
        public async Task<IActionResult> DeleteCTUD_Sach_Qua(int maCT)
        {
            var CTUD_Sach_Qua = await _context.CTUD_Sach_Qua.FindAsync(maCT);
            if (CTUD_Sach_Qua == null)
            {
                return NotFound();
            }

            _context.CTUD_Sach_Qua.Remove(CTUD_Sach_Qua);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa chi tiết ưu đãi!" });
        }

        private bool CTUD_Sach_QuaExists(int maCT)
        {
            return _context.CTUD_Sach_Qua.Any(e => e.MaCT == maCT);
        }
    }
}

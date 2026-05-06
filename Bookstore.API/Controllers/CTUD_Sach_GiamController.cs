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
    public class CTUD_Sach_GiamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CTUD_Sach_GiamController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ DANH SÁCH
        // GET: api/CTUD_Sach_Giam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CTUD_Sach_Giam>>> GetCTUD_Sach_Giam()
        {
            return await _context.CTUD_Sach_Giam.ToListAsync();
        }

        // LẤY CHI TIẾT THEO MÃ ƯU ĐÃI
        // GET: api/CTUD_Sach_Giam/UuDai/5
        [HttpGet("UuDai/{maUuDai}")]
        public async Task<ActionResult<IEnumerable<CTUD_Sach_Giam>>> GetByMaUuDai(int maUuDai)
        {
            return await _context.CTUD_Sach_Giam
                                 .Where(x => x.MaUuDai == maUuDai)
                                 .ToListAsync();
        }

        // LẤY ĐÚNG 1 DÒNG DỰA VÀO KHÓA CHÍNH
        // GET: api/CTUD_Sach_Giam/5
        [HttpGet("{maCT}")]
        public async Task<ActionResult<CTUD_Sach_Giam>> GetCTUD_Sach_Giam(int maCT)
        {
            var CTUD_Sach_Giam = await _context.CTUD_Sach_Giam.FindAsync(maCT);

            if (CTUD_Sach_Giam == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết ưu đãi này!" });
            }

            return CTUD_Sach_Giam;
        }

        // CẬP NHẬT
        // PUT: api/CTUD_Sach_Giam/5
        [HttpPut("{maCT}")]
        public async Task<IActionResult> PutCTUD_Sach_Giam(int maCT, CTUD_Sach_Giam CTUD_Sach_Giam)
        {
            if (maCT != CTUD_Sach_Giam.MaCT)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(CTUD_Sach_Giam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTUD_Sach_GiamExists(maCT))
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
        // POST: api/CTUD_Sach_Giam
        [HttpPost]
        public async Task<ActionResult<CTUD_Sach_Giam>> PostCTUD_Sach_Giam(CTUD_Sach_Giam CTUD_Sach_Giam)
        {
            _context.CTUD_Sach_Giam.Add(CTUD_Sach_Giam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCTUD_Sach_Giam),
                new { maCT = CTUD_Sach_Giam.MaCT },
                CTUD_Sach_Giam);
        }

        // XÓA
        // DELETE: api/CTUD_Sach_Giam/5
        [HttpDelete("{maCT}")]
        public async Task<IActionResult> DeleteCTUD_Sach_Giam(int maCT)
        {
            var CTUD_Sach_Giam = await _context.CTUD_Sach_Giam.FindAsync(maCT);
            if (CTUD_Sach_Giam == null)
            {
                return NotFound();
            }

            _context.CTUD_Sach_Giam.Remove(CTUD_Sach_Giam);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa chi tiết ưu đãi!" });
        }

        private bool CTUD_Sach_GiamExists(int maCT)
        {
            return _context.CTUD_Sach_Giam.Any(e => e.MaCT == maCT);
        }
    }
}

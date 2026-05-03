using Bookstore.API.Data;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTUD_HoaDon_GiamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CTUD_HoaDon_GiamController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ DANH SÁCH
        // GET: api/CTUD_HoaDon_Giam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CTUD_HoaDon_Giam>>> GetCTUD_HoaDon_Giam()
        {
            return await _context.CTUD_HoaDon_Giam.ToListAsync();
        }

        // LẤY CHI TIẾT THEO MÃ ƯU ĐÃI
        // GET: api/CTUD_HoaDon_Giam/UuDai/5
        [HttpGet("UuDai/{maUuDai}")]
        public async Task<ActionResult<IEnumerable<CTUD_HoaDon_Giam>>> GetByMaUuDai(int maUuDai)
        {
            return await _context.CTUD_HoaDon_Giam
                                 .Where(x => x.MaUuDai == maUuDai)
                                 .ToListAsync();
        }

        // LẤY ĐÚNG 1 DÒNG DỰA VÀO KHÓA CHÍNH
        // GET: api/CTUD_HoaDon_Giam/5
        [HttpGet("{maCT}")]
        public async Task<ActionResult<CTUD_HoaDon_Giam>> GetCTUD_HoaDon_Giam(int maCT)
        {
            var cTUD_HoaDon_Giam = await _context.CTUD_HoaDon_Giam.FindAsync(maCT);

            if (cTUD_HoaDon_Giam == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết ưu đãi này!" });
            }

            return cTUD_HoaDon_Giam;
        }

        // CẬP NHẬT
        // PUT: api/CTUD_HoaDon_Giam/5
        [HttpPut("{maCT}")]
        public async Task<IActionResult> PutCTUD_HoaDon_Giam(int maCT, CTUD_HoaDon_Giam cTUD_HoaDon_Giam)
        {
            if (maCT != cTUD_HoaDon_Giam.MaCT)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(cTUD_HoaDon_Giam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTUD_HoaDon_GiamExists(maCT))
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
        // POST: api/CTUD_HoaDon_Giam
        [HttpPost]
        public async Task<ActionResult<CTUD_HoaDon_Giam>> PostCTUD_HoaDon_Giam(CTUD_HoaDon_Giam cTUD_HoaDon_Giam)
        {
            _context.CTUD_HoaDon_Giam.Add(cTUD_HoaDon_Giam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCTUD_HoaDon_Giam),
                new { maCT = cTUD_HoaDon_Giam.MaCT },
                cTUD_HoaDon_Giam);
        }

        // XÓA
        // DELETE: api/CTUD_HoaDon_Giam/5
        [HttpDelete("{maCT}")]
        public async Task<IActionResult> DeleteCTUD_HoaDon_Giam(int maCT)
        {
            var cTUD_HoaDon_Giam = await _context.CTUD_HoaDon_Giam.FindAsync(maCT);
            if (cTUD_HoaDon_Giam == null)
            {
                return NotFound();
            }

            _context.CTUD_HoaDon_Giam.Remove(cTUD_HoaDon_Giam);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa chi tiết ưu đãi!" });
        }

        private bool CTUD_HoaDon_GiamExists(int maCT)
        {
            return _context.CTUD_HoaDon_Giam.Any(e => e.MaCT == maCT);
        }
    }
}
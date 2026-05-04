using Bookstore.API.Data;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CT_BC_KhachHangController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CT_BC_KhachHangController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CT_BC_KhachHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CT_BC_KhachHang>>> GetCT_BC_KhachHang()
        {
            return await _context.CT_BC_KhachHang.ToListAsync();
        }

        // lấy chi tiết của 1 báo cáo cụ thể
        // GET: api/CT_BC_KhachHang/BaoCao/5
        [HttpGet("BaoCao/{maBaoCao}")]
        public async Task<ActionResult<IEnumerable<CT_BC_KhachHang>>> GetByMaBaoCao(int maBaoCao)
        {
            return await _context.CT_BC_KhachHang
                                 .Where(x => x.MaBaoCaoKhachHang == maBaoCao)
                                 .ToListAsync();
        }

        // Lấy 1 dòng chi tiết
        // GET: api/CT_BC_KhachHang/5/10
        [HttpGet("{maBaoCao}/{maKhachHang}")]
        public async Task<ActionResult<CT_BC_KhachHang>> GetCT_BC_KhachHang(int maBaoCao, int maKhachHang)
        {
            var cT_BC_KhachHang = await _context.CT_BC_KhachHang.FindAsync(maBaoCao, maKhachHang);

            if (cT_BC_KhachHang == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết báo cáo này!" });
            }

            return cT_BC_KhachHang;
        }

        // cập nhật 1 dòng chi tiết
        // PUT: api/CT_BC_KhachHang/5/10
        [HttpPut("{maBaoCao}/{maKhachHang}")]
        public async Task<IActionResult> PutCT_BC_KhachHang(int maBaoCao, int maKhachHang, CT_BC_KhachHang cT_BC_KhachHang)
        {
            // Phải check khớp cả 2 key
            if (maBaoCao != cT_BC_KhachHang.MaBaoCaoKhachHang || maKhachHang != cT_BC_KhachHang.MaKhachHang)
            {
                return BadRequest(new { message = "Mã URL không khớp với Body dữ liệu!" });
            }

            _context.Entry(cT_BC_KhachHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CT_BC_KhachHangExists(maBaoCao, maKhachHang))
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

        // thêm 1 dòng chi tiết
        // POST: api/CT_BC_KhachHang
        [HttpPost]
        public async Task<ActionResult<CT_BC_KhachHang>> PostCT_BC_KhachHang(CT_BC_KhachHang cT_BC_KhachHang)
        {
            _context.CT_BC_KhachHang.Add(cT_BC_KhachHang);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CT_BC_KhachHangExists(cT_BC_KhachHang.MaBaoCaoKhachHang, cT_BC_KhachHang.MaKhachHang))
                {
                    return Conflict(new { message = "Dữ liệu này đã tồn tại!" });
                }
                else
                {
                    throw;
                }
            }

            // Trả về URL chứa đủ 2 tham số để GET lại cục data vừa tạo
            return CreatedAtAction(nameof(GetCT_BC_KhachHang),
                new { maBaoCao = cT_BC_KhachHang.MaBaoCaoKhachHang, maKhachHang = cT_BC_KhachHang.MaKhachHang },
                cT_BC_KhachHang);
        }

        // xóa 1 dòng chi tiết
        // DELETE: api/CT_BC_KhachHang/5/10
        [HttpDelete("{maBaoCao}/{maKhachHang}")]
        public async Task<IActionResult> DeleteCT_BC_KhachHang(int maBaoCao, int maKhachHang)
        {
            var cT_BC_KhachHang = await _context.CT_BC_KhachHang.FindAsync(maBaoCao, maKhachHang);
            if (cT_BC_KhachHang == null)
            {
                return NotFound();
            }

            _context.CT_BC_KhachHang.Remove(cT_BC_KhachHang);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công!" });
        }

        //  Ceck tồn tại cũng nhận 2 key
        private bool CT_BC_KhachHangExists(int maBaoCao, int maKhachHang)
        {
            return _context.CT_BC_KhachHang.Any(e => e.MaBaoCaoKhachHang == maBaoCao && e.MaKhachHang == maKhachHang);
        }
    }
}
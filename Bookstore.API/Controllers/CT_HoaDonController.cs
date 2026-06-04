using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CT_HoaDonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CT_HoaDonController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ CHI TIẾT HÓA ĐƠN
        // GET: api/CT_HoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CT_HoaDon>>> GetCT_HoaDon()
        {
            return await _context.CT_HoaDon.ToListAsync();
        }

        //  LẤY TOÀN BỘ SÁCH CỦA 1 HÓA ĐƠN CỤ THỂ
        // GET: api/CT_HoaDon/HoaDon/5
        [HttpGet("HoaDon/{maHoaDon}")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailResponse>>> GetByMaHoaDon(int maHoaDon)
        {
            var chiTietHoaDon = await _context.CT_HoaDon
                .Where(x => x.MaHoaDon == maHoaDon)
                .Select(ct => new InvoiceDetailResponse
                {
                    ISBN = ct.ISBN,
                    // Đi xuyên qua Khóa ngoại PhienBanSach sang bảng Sach để lấy tên và ảnh
                    TenSach = ct.PhienBanSach.Sach != null ? ct.PhienBanSach.Sach.TenSach : "Sách đã bị xóa khỏi hệ thống",
                    HinhAnh = ct.PhienBanSach.Sach != null ? ct.PhienBanSach.Sach.ImageUrl : "/Resources/Images/Books/default_book_cover.jpg",
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    GiaNiemYet = ct.PhienBanSach.GiaNiemYet
                })
                .ToListAsync();

            return Ok(chiTietHoaDon);
        }

        // LẤY ĐÚNG 1 DÒNG CHI TIẾT DỰA VÀO 2 KHÓA
        // GET: api/CT_HoaDon/5/978-0132350884
        [HttpGet("{maHoaDon}/{isbn}")]
        public async Task<ActionResult<CT_HoaDon>> GetCT_HoaDon(int maHoaDon, string isbn)
        {
            // Truyền đúng 2 tham số khóa chính vào FindAsync
            var cT_HoaDon = await _context.CT_HoaDon.FindAsync(maHoaDon, isbn);

            if (cT_HoaDon == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết hóa đơn này!" });
            }

            return cT_HoaDon;
        }

        // CẬP NHẬT 1 DÒNG CHI TIẾT
        // PUT: api/CT_HoaDon/5/978-0132350884
        [HttpPut("{maHoaDon}/{isbn}")]
        public async Task<IActionResult> PutCT_HoaDon(int maHoaDon, string isbn, CT_HoaDon cT_HoaDon)
        {
            // Kiểm tra khớp cả 2 mã
            if (maHoaDon != cT_HoaDon.MaHoaDon || isbn != cT_HoaDon.ISBN)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(cT_HoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CT_HoaDonExists(maHoaDon, isbn))
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

        // THÊM MỚI 1 DÒNG CHI TIẾT VÀO HÓA ĐƠN
        // POST: api/CT_HoaDon
        [HttpPost]
        public async Task<ActionResult<CT_HoaDon>> PostCT_HoaDon(CT_HoaDon cT_HoaDon)
        {
            _context.CT_HoaDon.Add(cT_HoaDon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CT_HoaDonExists(cT_HoaDon.MaHoaDon, cT_HoaDon.ISBN))
                {
                    return Conflict(new { message = "Sách này đã tồn tại trong hóa đơn!" });
                }
                else
                {
                    throw;
                }
            }

            // Route trả về phải khớp với hàm GET nhận 2 tham số
            return CreatedAtAction(nameof(GetCT_HoaDon),
                new { maHoaDon = cT_HoaDon.MaHoaDon, isbn = cT_HoaDon.ISBN },
                cT_HoaDon);
        }

        // XÓA 1 SÁCH KHỎI HÓA ĐƠN
        // DELETE: api/CT_HoaDon/5/978-0132350884
        [HttpDelete("{maHoaDon}/{isbn}")]
        public async Task<IActionResult> DeleteCT_HoaDon(int maHoaDon, string isbn)
        {
            var cT_HoaDon = await _context.CT_HoaDon.FindAsync(maHoaDon, isbn);
            if (cT_HoaDon == null)
            {
                return NotFound();
            }

            _context.CT_HoaDon.Remove(cT_HoaDon);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa sách khỏi hóa đơn!" });
        }

        // Hàm kiểm tra tồn tại check bằng 2 khóa
        private bool CT_HoaDonExists(int maHoaDon, string isbn)
        {
            return _context.CT_HoaDon.Any(e => e.MaHoaDon == maHoaDon && e.ISBN == isbn);
        }
    }
}
using Bookstore.API.Data;
// using Bookstore.API.Data; // Chú ý namespace cho chuẩn nhé
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
    public class CT_PhieuNhapSachController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CT_PhieuNhapSachController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ CHI TIẾT PHIẾU NHẬP SÁCH
        // GET: api/CT_PhieuNhapSach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CT_PhieuNhapSach>>> GetCT_PhieuNhapSach()
        {
            return await _context.CT_PhieuNhapSach.ToListAsync();
        }

        // 🌟 LẤY TOÀN BỘ SÁCH CỦA 1 PHIẾU NHẬP CỤ THỂ
        // GET: api/CT_PhieuNhapSach/PhieuNhap/5
        [HttpGet("PhieuNhap/{maPhieuNhap}")]
        public async Task<ActionResult<IEnumerable<CT_PhieuNhapSach>>> GetByMaPhieuNhap(int maPhieuNhap)
        {
            return await _context.CT_PhieuNhapSach
                                 .Where(x => x.MaPhieuNhapSach == maPhieuNhap)
                                 .ToListAsync();
        }

        // LẤY ĐÚNG 1 DÒNG CHI TIẾT DỰA VÀO 2 KHÓA
        // GET: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpGet("{maPhieuNhap}/{isbn}")]
        public async Task<ActionResult<CT_PhieuNhapSach>> GetCT_PhieuNhapSach(int maPhieuNhap, string isbn)
        {
            // Truyền đúng 2 tham số khóa chính vào FindAsync
            var cT_PhieuNhap = await _context.CT_PhieuNhapSach.FindAsync(maPhieuNhap, isbn);

            if (cT_PhieuNhap == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết mã phiếu nhập sách này!" });
            }

            return cT_PhieuNhap;
        }

        // CẬP NHẬT 1 DÒNG CHI TIẾT
        // PUT: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpPut("{maPhieuNhap}/{isbn}")]
        public async Task<IActionResult> PutCT_PhieuNhapSach(int maPhieuNhap, string isbn, CT_PhieuNhapSach ct_PhieuNhap)
        {
            // Kiểm tra khớp cả 2 mã
            if (maPhieuNhap != ct_PhieuNhap.MaPhieuNhapSach || isbn != ct_PhieuNhap.ISBN)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(ct_PhieuNhap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CT_PhieuNhapSachExists(maPhieuNhap, isbn))
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

        // THÊM MỚI 1 DÒNG CHI TIẾT VÀO PHIẾU NHẬP
        // POST: api/CT_PhieuNhapSach
        [HttpPost]
        public async Task<ActionResult<CT_PhieuNhapSach>> PostCT_PhieuNhapSach(CT_PhieuNhapSach cT_PhieuNhapSach)
        {
            _context.CT_PhieuNhapSach.Add(cT_PhieuNhapSach);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CT_PhieuNhapSachExists(cT_PhieuNhapSach.MaPhieuNhapSach, cT_PhieuNhapSach.ISBN))
                {
                    return Conflict(new { message = "Sách này đã tồn tại trong phiếu nhập sách!" });
                }
                else
                {
                    throw;
                }
            }

            // Route trả về khớp với hàm GET nhận 2 tham số (maPhieuNhap và isbn)
            return CreatedAtAction(nameof(GetCT_PhieuNhapSach),
                new { maPhieuNhap = cT_PhieuNhapSach.MaPhieuNhapSach, isbn = cT_PhieuNhapSach.ISBN },
                cT_PhieuNhapSach);
        }

        // XÓA 1 SÁCH KHỎI PHIẾU NHẬP
        // DELETE: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpDelete("{maPhieuNhap}/{isbn}")]
        public async Task<IActionResult> DeleteCT_PhieuNhapSach(int maPhieuNhap, string isbn)
        {
            var cT_PhieuNhap = await _context.CT_PhieuNhapSach.FindAsync(maPhieuNhap, isbn);
            if (cT_PhieuNhap == null)
            {
                return NotFound();
            }

            _context.CT_PhieuNhapSach.Remove(cT_PhieuNhap);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa sách khỏi phiếu nhập!" });
        }

        // Hàm kiểm tra tồn tại check bằng 2 khóa
        private bool CT_PhieuNhapSachExists(int maPhieuNhap, string isbn)
        {
            return _context.CT_PhieuNhapSach.Any(e => e.MaPhieuNhapSach == maPhieuNhap && e.ISBN == isbn);
        }
    }
}

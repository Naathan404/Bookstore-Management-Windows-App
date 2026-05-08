using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
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

        private static System.Linq.Expressions.Expression<Func<CT_PhieuNhapSach, ImportDetailResponse>> MapToDTO()
        {
            return ct => new ImportDetailResponse
            {
                MaPhieuNhap = ct.MaPhieuNhapSach,
                ISBN = ct.ISBN,
                TenSach = (ct.PhienBanSach != null && ct.PhienBanSach.Sach != null)
                    ? ct.PhienBanSach.Sach.TenSach
                    : "Sách không xác định",
                SoLuong = ct.SoLuong,
                DonGiaNhap = ct.DonGiaNhap,
            };
        }

        // LẤY TOÀN BỘ CHI TIẾT PHIẾU NHẬP SÁCH
        // GET: api/CT_PhieuNhapSach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportDetailResponse>>> GetCT_PhieuNhapSach()
        {
            return Ok(
                await _context.CT_PhieuNhapSach.AsQueryable()
                .Select(MapToDTO())
                .OrderBy(c => c.ISBN)
                .ToListAsync()
            );
        }

        // LẤY TOÀN BỘ SÁCH CỦA 1 PHIẾU NHẬP CỤ THỂ
        // GET: api/CT_PhieuNhapSach/PhieuNhap/5
        [HttpGet("PhieuNhap/{maPhieuNhap}")]
        public async Task<ActionResult<IEnumerable<ImportDetailResponse>>> GetByMaPhieuNhap(int maPhieuNhap)
        {
            return Ok(
                await _context.CT_PhieuNhapSach
                    .Where(x => x.MaPhieuNhapSach == maPhieuNhap)
                    .Select(MapToDTO())
                    .OrderBy(x => x.ISBN)
                    .ToListAsync()
            );
        }

        // LẤY ĐÚNG 1 DÒNG CHI TIẾT DỰA VÀO 2 KHÓA
        // GET: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpGet("{maPhieuNhap}/{isbn}")]
        public async Task<ActionResult<ImportDetailResponse>> GetCT_PhieuNhapSach(int maPhieuNhap, string isbn)
        {
            // Truyền đúng 2 tham số khóa chính vào FindAsync
            var chitiet = await _context.CT_PhieuNhapSach
                .Where(ct => ct.MaPhieuNhapSach == maPhieuNhap && ct.ISBN == isbn)
                .Select(MapToDTO())
                .FirstOrDefaultAsync();
            if (chitiet == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết mã phiếu nhập sách này!" });
            }
            return Ok(chitiet);
        }

        // CẬP NHẬT 1 DÒNG CHI TIẾT
        // PUT: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpPut("{maPhieuNhap}/{isbn}")]
        public async Task<IActionResult> UpdateCT_PhieuNhapSach(int maPhieuNhap, string isbn, ImportDetailRequest newChiTiet)
        {
            // Kiểm tra khớp cả 2 mã
            if (maPhieuNhap != newChiTiet.MaPhieuNhap || isbn != newChiTiet.ISBN)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            var tsSoLuongNhapToiThieu = await _context.ThamSo.FindAsync("SoLuongNhapToiThieu");
            int soLuongNhapToiThieu = tsSoLuongNhapToiThieu != null ? tsSoLuongNhapToiThieu.GiaTri : 150;

            if (newChiTiet.SoLuong < soLuongNhapToiThieu)
            {
                return BadRequest(new { message = $"Số lượng nhập tối thiểu là {soLuongNhapToiThieu}" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var oldChiTiet = await _context.CT_PhieuNhapSach.FindAsync(maPhieuNhap, isbn);
                if (oldChiTiet == null)
                {
                    return NotFound(new { message = "Không tìm thấy chi tiết này trong phiếu nhập!" });
                }

                //Xu ly ton kho
                var sach = await _context.PhienBanSach.FindAsync(isbn);
                if (sach == null) throw new Exception("Không tìm thấy sách");

                int chenhLechSoLuong = newChiTiet.SoLuong - oldChiTiet.SoLuong;
                decimal checkLechTien = (newChiTiet.DonGiaNhap * newChiTiet.SoLuong) - (oldChiTiet.DonGiaNhap * oldChiTiet.SoLuong);

                if (sach.TonKho + chenhLechSoLuong < 0)
                {
                    throw new Exception("Không thể giảm số lượng");
                }
                oldChiTiet.SoLuong = newChiTiet.SoLuong;
                oldChiTiet.DonGiaNhap = newChiTiet.DonGiaNhap;

                sach.TonKho += chenhLechSoLuong;
                _context.PhienBanSach.Update(sach);

                //xu ly phieu nhap
                var phieuNhap = await _context.PhieuNhapSach.FindAsync(maPhieuNhap);
                if (phieuNhap == null) throw new Exception("Không tìm thấy phiếu nhập");
                phieuNhap.TongTien += checkLechTien;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Cập nhật chi tiết thành công");

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // CHỈ ĐƯỢC POST BÊN PHIẾU NHẬP

        // XÓA 1 SÁCH KHỎI PHIẾU NHẬP
        // DELETE: api/CT_PhieuNhapSach/5/978-0132350884
        [HttpDelete("{maPhieuNhap}/{isbn}")]
        public async Task<IActionResult> DeleteCT_PhieuNhapSach(int maPhieuNhap, string isbn)
        {
            var cT_PhieuNhap = await _context.CT_PhieuNhapSach.FindAsync(maPhieuNhap, isbn);
            if (cT_PhieuNhap == null)
            {
                return NotFound(new {message = "Không tìm thấy chi tiết phiếu nhập"});
            }

            using var transation = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xu ly ton kho
                var sach = await _context.PhienBanSach.FindAsync(isbn);
                if (sach == null) throw new Exception($"Không tìm thấy sách để xóa {isbn}");

                var tsSoLuongTonToiThieu = await _context.ThamSo.FindAsync("SoLuongTonToiThieu");
                int soLuongTonToiThieu = (tsSoLuongTonToiThieu != null) ? tsSoLuongTonToiThieu.GiaTri : 20;

                if (sach.TonKho - cT_PhieuNhap.SoLuong < soLuongTonToiThieu)
                {
                    throw new Exception("Xóa chi tiết nhập ảnh hưởng số lượng tồn");
                }
                sach.TonKho -= cT_PhieuNhap.SoLuong;

                // Xử lý phiếu nhập
                var phieuNhap = await _context.PhieuNhapSach.FindAsync(maPhieuNhap);
                if (phieuNhap == null) throw new Exception($"Không tim thấy phiếu nhập {maPhieuNhap}");
                phieuNhap.TongTien -= cT_PhieuNhap.DonGiaNhap * cT_PhieuNhap.SoLuong;

                _context.CT_PhieuNhapSach.Remove(cT_PhieuNhap);

                await _context.SaveChangesAsync();
                await transation.CommitAsync();
                return Ok(new { message = "Đã xóa sách khỏi phiếu nhập!" });
            }
            catch (Exception ex)
            {
                await transation.RollbackAsync();
                return BadRequest(new { message = $"Lỗi :  {ex.Message}" });
            }
        }
    }
}

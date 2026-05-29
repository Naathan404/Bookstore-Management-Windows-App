using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controler]")]
    public class PhieuThuController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PhieuThuController(AppDbContext context)
        {
            _context = context;
        }

        private static Expression<Func<PhieuThuTien, ReceiptResponse>> MapToReceipResponse =
          p => new ReceiptResponse
          {
              MaPhieuThuTien = p.MaPhieuThuTien,
              NgayTao = p.NgayTao,
              NguoiTao = p.NguoiTao,
              TenNguoiTao = p.NguoiDung!.HoTen,
              MaKhachHang = p.MaKhachHang,
              TenKhachHang = p.KhachHang!.TenKhachHang,
              SoTienThu = p.SoTienThu,
              LyDoThu = p.LyDoThu,
          };

        [HttpGet]
        public async Task<ActionResult<ReceiptResponse>> GetAllReceipts(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? nguoiTao,
            [FromQuery] int? khachHang)
        {
            var query = _context.PhieuThuTien.AsQueryable();

            if (startDate.HasValue)
            {
                query.Where(p => p.NgayTao >=  startDate.Value.Date);
            }
            if (endDate.HasValue)
            {
                query.Where(p => p.NgayTao  <= endDate.Value.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(nguoiTao))
            {
                query.Where(p => p.NguoiTao.Equals(nguoiTao));
            }
            if (khachHang.HasValue)
            {
                query.Where(p => p.MaKhachHang ==  khachHang.Value);
            }

            var result = await query
                .OrderBy(p => p.NgayTao)
                .Select(MapToReceipResponse)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ReceiptResponse>> GetReceipById(int id)
        {
            var receipt = await _context.PhieuThuTien
                .Where(p => p.MaPhieuThuTien == id)
                .Select(MapToReceipResponse)
                .FirstOrDefaultAsync();
            if (receipt == null)
            {
                return NotFound(new { Message = "Không tìm thấy phiếu thu" });
            }

            return Ok(receipt);
        }

        [HttpPost]
        public async Task<ActionResult<ReceiptResponse>> CreateReceipt(ReceiptRequest request)
        {
            var nguoiDung = await _context.NguoiDung.FindAsync(request.NguoiTao);
            if (nguoiDung == null)
            {
                return NotFound(new { Message = "Người tạo không hợp lệ" });
            }

            var khachHang = await _context.KhachHang.FindAsync(request.MaKhachHang);
            if (khachHang == null)
            {
                return NotFound(new { Message = "Không tồn tại khách hàng" });
            }
            if (khachHang.TienNo <= 0)
            {
                return BadRequest(new { Message = "Khách hàng không có nợ" });
            }

            var tsTienThuLonHonNo = await _context.ThamSo.FindAsync("TienThuLonHonNo");
            bool tienThuLonHonNo = (tsTienThuLonHonNo == null || tsTienThuLonHonNo.GiaTri == 1); // true/false

            if (!tienThuLonHonNo)
            {
                if (request.SoTienThu > khachHang.TienNo)
                {
                    return BadRequest(new { Message = "Tiền thu không được lớn hơn nợ" });
                }
            }

            khachHang.TienNo -= request.SoTienThu;

            PhieuThuTien newPhieuThu = new PhieuThuTien()
            {
                NguoiTao = request.NguoiTao,
                MaKhachHang = request.MaKhachHang,
                SoTienThu = request.SoTienThu,
                LyDoThu = request.LyDoThu
            };

            await _context.PhieuThuTien.AddAsync(newPhieuThu);
            await _context.SaveChangesAsync();

            ReceiptResponse response = MapToReceipResponse.Compile().Invoke(newPhieuThu);
            return Ok(response);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateReceipt(int id, [FromBody] ReceiptRequest request)
        {
            var phieuThu = await _context.PhieuThuTien.FindAsync(id);
            if (phieuThu == null)
            {
                return NotFound(new { Message = "Không tìm thấy phiếu thu" });
            }

            var nguoiDung = await _context.NguoiDung.FindAsync(request.NguoiTao);
            if (nguoiDung == null)
            {
                return NotFound(new { Message = "Người sửa đổi không hợp lệ" });
            }

            var khachHangMoi = await _context.KhachHang.FindAsync(request.MaKhachHang);
            if (khachHangMoi == null)
            {
                return NotFound(new { Message = "Khách hàng không có khách hàng mới" });
            }

            if (khachHangMoi.TienNo < 0)
            {
                return BadRequest(new { Message = "Khách hàng mới không có nợ" }); 
            }

            var tsTienThuLonHonNo = await _context.ThamSo.FindAsync("TienThuLonHonNo");
            bool tienThuLonHonNo = (tsTienThuLonHonNo == null || tsTienThuLonHonNo.GiaTri == 1);

            if (!tienThuLonHonNo && request.SoTienThu > khachHangMoi.TienNo)
            {
                return BadRequest(new { Message = "Tiền thu không được lớn hơn nợ" });
            }

            khachHangMoi.TienNo -= request.SoTienThu;
            if (request.MaKhachHang == phieuThu.MaKhachHang)
            {
                khachHangMoi.TienNo += phieuThu.SoTienThu;
            }
            else
            {
                var khachHangCu = await _context.KhachHang.FindAsync(phieuThu.MaKhachHang);
                if (khachHangCu == null)
                {
                    return NotFound(new {Message = "Không tìm thấy khách hàng cũ"});
                }

                khachHangCu.TienNo += phieuThu.SoTienThu;
            }

            phieuThu.NguoiTao = request.NguoiTao;
            phieuThu.MaKhachHang = request.MaKhachHang;
            phieuThu.SoTienThu = request.SoTienThu;
            phieuThu.LyDoThu = request.LyDoThu;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Lưu và cập nhật tiền dư thành công" });
        }

        [HttpDelete("id")]
        public async Task<IActionResult> deleteReceipt(int id)
        {
            var phieuThu = await _context.PhieuThuTien.FindAsync(id);
            if (phieuThu == null)
            {
                return NotFound(new { Mesage = "Không tìm thấy phiếu thu" });
            }
            string message = string.Empty;
            var khachHang = await _context.KhachHang.FindAsync(phieuThu.MaKhachHang);
            if (khachHang != null)
            {
                khachHang.TienNo += phieuThu.SoTienThu;
                message = $"Đã xóa phiếu thu và cập nhật tiền nợ của khách hàng {khachHang.MaKhachHang}, {khachHang.TenKhachHang}";
            }

            _context.PhieuThuTien.Remove(phieuThu);
            await _context.SaveChangesAsync();

            message = "Đã xóa phiếu thu";

            return Ok(new { Message = message });
        }
    }
}

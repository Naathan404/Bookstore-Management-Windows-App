using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuThuController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhieuThuController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PhieuThu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptResponse>>> GetDanhSachPhieuThu()
        {
            var result = await _context.PhieuThuTien
                .Include(pt => pt.KhachHang)
                // Kết nối với bảng NguoiDung để lấy họ tên nhân viên
                .Join(_context.NguoiDung,
                      pt => pt.NguoiTao,
                      nd => nd.TenDangNhap,
                      (pt, nd) => new ReceiptResponse
                      {
                          MaPhieuThuTien = pt.MaPhieuThuTien,
                          NgayTao = pt.NgayTao,
                          MaKhachHang = pt.MaKhachHang,
                          TenKhachHang = pt.KhachHang.TenKhachHang,
                          NguoiTao = pt.NguoiTao,
                          TenNguoiTao = nd.HoTen, // Hiển thị họ tên nhân viên
                          SoTienThu = pt.SoTienThu,
                          LyDoThu = pt.LyDoThu
                      })
                .OrderByDescending(pt => pt.NgayTao)
                .ToListAsync();

            return Ok(result);
        }

        // POST: api/PhieuThu (THÊM MỚI)
        [HttpPost]
        public async Task<ActionResult> CreatePhieuThu([FromBody] ReceiptRequest request)
        {
            // 1. Kiểm tra khách hàng
            var khachHang = await _context.KhachHang.FindAsync(request.MaKhachHang);
            if (khachHang == null) return NotFound("Không tìm thấy khách hàng!");

            // 2. Kiểm tra tham số: Thu tiền lớn hơn nợ (Nếu quy định không cho phép)
            var thamSoThuTien = await _context.ThamSo.FirstOrDefaultAsync(t => t.TenThamSo == "TienThuLonHonNo");
            if (thamSoThuTien != null && thamSoThuTien.GiaTri == 0) // Giả sử 0 là không cho phép
            {
                if (request.SoTienThu > khachHang.TienNo)
                    return BadRequest("Số tiền thu không được lớn hơn số tiền khách đang nợ!");
            }

            // 3. Tạo phiếu thu mới
            var phieuThuMoi = new PhieuThuTien
            {
                MaKhachHang = request.MaKhachHang,
                NguoiTao = request.NguoiTao,
                NgayTao = DateTime.Now,
                SoTienThu = request.SoTienThu,
                LyDoThu = request.LyDoThu
            };

            // 4. TRỪ NỢ KHÁCH HÀNG
            khachHang.TienNo -= request.SoTienThu;
            // Tránh nợ bị âm nếu có tham số cho phép thu lố
            if (khachHang.TienNo < 0) khachHang.TienNo = 0;

            _context.PhieuThuTien.Add(phieuThuMoi);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tạo phiếu thu thành công!" });
        }

        // DELETE: api/PhieuThu/5 (XÓA PHIẾU THU & HOÀN NỢ)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuThu(int id)
        {
            var phieuThu = await _context.PhieuThuTien.FindAsync(id);
            if (phieuThu == null) return NotFound();

            var khachHang = await _context.KhachHang.FindAsync(phieuThu.MaKhachHang);
            if (khachHang != null)
            {
                // HỦY PHIẾU THU -> CỘNG LẠI NỢ CHO KHÁCH
                khachHang.TienNo += phieuThu.SoTienThu;
            }

            _context.PhieuThuTien.Remove(phieuThu);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đã xóa phiếu thu và hoàn lại công nợ." });
        }
    }
}
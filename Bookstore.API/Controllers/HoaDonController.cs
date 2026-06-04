using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private AppDbContext _context;
        public HoaDonController(AppDbContext context)
        {
            _context = context;
        }

        private static Expression<Func<HoaDon, InvoiceResponse>> MapToResponse =
            h => new InvoiceResponse
            {
                MaHoaDon = h.MaHoaDon,
                NgayTao = h.NgayTao,
                NguoiTao = h.NguoiTao,
                TenNguoiTao = h.NguoiDung != null ? h.NguoiDung.HoTen : string.Empty,
                MaKhachHang = h.MaKhachHang ?? 0,
                TenKhachHang = h.KhachHang != null ? h.KhachHang.TenKhachHang : "Khách vãng lai",
                TongTienTamTinh = h.TongTienTamTinh,
                TongTien = h.TongTien,
                GiamGia = h.GiamGia,
                Thue = h.Thue,
                SoTienTra = h.SoTienTra,
            };

        // GET: api/HoaDon
        [HttpGet]
        public async Task<ActionResult<List<InvoiceResponse>>> GetAllInvoices(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? createdBy,
            [FromQuery] int? customerId,
            [FromQuery] decimal? minValue,
            [FromQuery] decimal? maxValue)
        {
            var query = _context.HoaDon.AsQueryable();
            if (startDate.HasValue)
            {
                query = query.Where(h => h.NgayTao >= startDate.Value.Date);
            }
            if (endDate.HasValue)
            {
                query = query.Where(h => h.NgayTao <= endDate.Value.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(createdBy))
            {
                query = query.Where(h => h.NguoiTao ==  createdBy);
            }
            if (customerId.HasValue)
            {
                query = query.Where(h => h.MaKhachHang == customerId);
            }
            if (minValue.HasValue)
            {
                query = query.Where(h => h.TongTien >= minValue);
            }
            if (maxValue.HasValue)
            {
                query = query.Where(h => h.TongTien <= maxValue);
            }

            var response = await query
                .OrderByDescending(h => h.MaHoaDon)
                .Select(MapToResponse)
                .ToListAsync();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tạo hóa đơn chính
                var hoaDon = new HoaDon
                {
                    NgayTao = DateTime.Now,
                    NguoiTao = request.NguoiTao,
                    MaKhachHang = request.MaKhachHang,
                    TongTienTamTinh = request.TongTienTamTinh,
                    GiamGia = request.GiamGia,
                    Thue = request.Thue,
                    TongTien = request.TongTien,
                    SoTienTra = request.SoTienTra
                };

                _context.HoaDon.Add(hoaDon);
                await _context.SaveChangesAsync(); // Sinh mã MaHoaDon tự động từ DB
                
                // 2. Chi tiết hóa đơn
                foreach (var item in request.ChiTiet)
                {
                    var sach = await _context.PhienBanSach.FirstOrDefaultAsync(s => s.ISBN == item.ISBN);
                    if (sach == null)
                        throw new Exception($"Không tìm thấy sách có mã ISBN: {item.ISBN} trong kho.");

                    if (sach.TonKho < item.SoLuong)
                        throw new Exception($"Sách '{sach.ISBN}' không đủ số lượng tồn kho (Hiện còn: {sach.TonKho}, yêu cầu: {item.SoLuong}).");

                    // Trừ kho hệ thống và tăng số lượng bán
                    sach.TonKho -= item.SoLuong;
                    sach.TongSoDaBan += item.SoLuong;

                    var ct = new CT_HoaDon
                    {
                        MaHoaDon = hoaDon.MaHoaDon,
                        ISBN = item.ISBN,
                        SoLuong = item.SoLuong,
                        GiaBan = item.GiaBan,
                        GiaNiemYet = sach.GiaNiemYet,
                    };
                    _context.CT_HoaDon.Add(ct);
                }

                // 3. Lịch sử ưu đãi áp dụng cho hóa đơn (nếu có)
                if (request.UuDai != null && request.UuDai.Any())
                {
                    foreach (var ud in request.UuDai)
                    {
                        _context.HoaDon_Uudai.Add(new HoaDon_UuDai
                        {
                            MaHoaDon = hoaDon.MaHoaDon,
                            MaUuDai = ud.MaUuDai,
                            SoTienGiam = ud.SoTienGiam
                        });

                        var uuDai = await _context.UuDai.FindAsync(ud.MaUuDai);
                        if (uuDai != null)
                        {
                            uuDai.SoLuongDaDung++;
                        }
                    }
                }

                // 4. Xử lý công nợ nếu khách hàng thanh toán chưa đủ
                if (request.SoTienTra < request.TongTien)
                {
                    if (request.MaKhachHang != 0) // 0 = Khách vãng lai
                    {
                        var khachHang = await _context.KhachHang.FindAsync(request.MaKhachHang);
                        if (khachHang != null)
                        {
                            decimal tienNo = request.TongTien - request.SoTienTra;
                            khachHang.TienNo += tienNo;
                        }
                        else
                        {
                            throw new Exception("Không tìm thấy thông tin thành viên để ghi nhận công nợ.");
                        }
                    }
                    else
                    {
                        throw new Exception("Hệ thống từ chối lệnh: Khách vãng lai bắt buộc thanh toán đủ, không được phép ghi nợ!");
                    }
                }

                // Đẩy toàn bộ dữ liệu sạch xuống SQL Server
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Tạo hóa đơn thành công",
                    MaHoaDon = hoaDon.MaHoaDon
                });
            }
            catch (Exception ex)
            {
                // Khi có bất kỳ lỗi logic nào vi phạm ở trên, hủy bỏ toàn bộ phiên làm việc
                await transaction.RollbackAsync();
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

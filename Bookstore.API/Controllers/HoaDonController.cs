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
                TenNguoiTao = h.NguoiDung!.HoTen,
                MaKhachHang = h.MaKhachHang,
                TenKhachHang = h.KhachHang!.TenKhachHang,
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
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // ==============================================================
                // BƯỚC 1: TẠO HÓA ĐƠN CHÍNH
                // ==============================================================
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
                await _context.SaveChangesAsync(); // Gọi SaveChanges để EF Core sinh ra MaHoaDon

                // ==============================================================
                // BƯỚC 2: THÊM CHI TIẾT HÓA ĐƠN & CẬP NHẬT TỒN KHO SÁCH
                // ==============================================================
                foreach (var item in request.ChiTiet)
                {
                    // Kiểm tra tồn kho
                    var sach = await _context.PhienBanSach.FirstOrDefaultAsync(s => s.ISBN == item.ISBN);
                    if (sach == null)
                        throw new Exception($"Không tìm thấy sách có mã ISBN: {item.ISBN}");

                    if (sach.TonKho < item.SoLuong)
                        throw new Exception($"Sách '{sach.ISBN}' không đủ tồn kho (Chỉ còn {sach.TonKho}).");

                    // Trừ tồn kho và cộng doanh số
                    sach.TonKho -= item.SoLuong;
                    sach.TongSoDaBan += item.SoLuong;

                    // Tạo record chi tiết
                    var ct = new CT_HoaDon
                    {
                        MaHoaDon = hoaDon.MaHoaDon,
                        ISBN = item.ISBN,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia,
                        GiaVon = item.GiaVon
                    };
                    _context.CT_HoaDon.Add(ct);
                }

                // ==============================================================
                // BƯỚC 3: LƯU LỊCH SỬ ƯU ĐÃI (NẾU CÓ)
                // ==============================================================
                if (request.UuDai != null && request.UuDai.Any())
                {
                    foreach (var ud in request.UuDai)
                    {
                        _context.HoaDon_Uudai.Add(new HoaDon_UuDai
                        {
                            MaHoaDon = hoaDon.MaHoaDon,
                            MaUuDai = ud.MaUuDai,
                            ISBN = ud.ISBN, // Cho phép null nếu là voucher giảm trên tổng bill
                            SoTienGiam = ud.SoTienGiam
                        });
                    }
                }

                // ==============================================================
                // BƯỚC 4: XỬ LÝ CÔNG NỢ (KHÁCH TRẢ THIẾU)
                // ==============================================================
                if (request.SoTienTra < request.TongTien)
                {
                    // Nếu không phải khách vãng lai (Giả sử ID khách vãng lai là 1)
                    if (request.MaKhachHang != 1)
                    {
                        var khachHang = await _context.KhachHang.FindAsync(request.MaKhachHang);
                        if (khachHang != null)
                        {
                            decimal tienNo = request.TongTien - request.SoTienTra;
                            khachHang.TienNo += tienNo; 
                        }
                    }
                    else
                    {
                        // Tùy nghiệp vụ: Khách vãng lai có được nợ không? Thường là không.
                        throw new Exception("Khách vãng lai không được phép ghi nợ.");
                    }
                }

                // Lưu tất cả thay đổi từ Bước 2, 3, 4
                await _context.SaveChangesAsync();

                // Xác nhận Commit Transaction (Chính thức ghi vào DB)
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Tạo hóa đơn thành công",
                    MaHoaDon = hoaDon.MaHoaDon
                });
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi nào xảy ra ở các bước trên, Rollback toàn bộ
                await transaction.RollbackAsync();
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

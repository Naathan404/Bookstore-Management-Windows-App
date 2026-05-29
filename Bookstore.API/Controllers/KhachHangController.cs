using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : Controller
    {
        private readonly AppDbContext _context;
        public KhachHangController(AppDbContext context)
        {
            _context = context;
        }

        private static Expression<Func<KhachHang, CustomerResponse>> MapToResponse()
        {
            return k => new CustomerResponse
            {
                MaKhachHang = k.MaKhachHang.ToString(),
                TenKhachHang = k.TenKhachHang,
                SoDienThoai = k.SoDienThoai,
                Email = k.Email,
                DiaChi = k.DiaChi,
                MaSoThue = k.MaSoThue,
                LoaiKhach = (k.LoaiKhachHang != null) ? k.LoaiKhachHang.TenLoaiKhachHang : "Khách vãng lai",
                GioiTinh = (k.GioiTinh == 0) ? "Nam" : "Nữ",
                NgaySinh = k.NgaySinh.HasValue
                   ? (DateTime?)k.NgaySinh.Value.ToDateTime(TimeOnly.MinValue)
                   : null,
                CongNo = (long)k.TienNo,
                NgayTao = k.NgayTao
            };
        }

        private static Expression<Func<CustomerRequest, KhachHang>> MapToEntity()
        {
            return r => new KhachHang
            {
                MaLoaiKhachHang = r.MaLoaiKhachHang,
                TenKhachHang = r.TenKhachHang,
                GioiTinh = r.GioiTinh,
                NgaySinh = r.NgaySinh,
                MaSoThue = r.MaSoThue,
                DiaChi = r.DiaChi,
                SoDienThoai = r.SoDienThoai,
                Email = r.Email,
                NgayTao = DateTime.UtcNow
            };
        }

        private async Task<(bool IsValid, string Message)> CheckUniqueCustomer(CustomerRequest request, int? excludeId = null)
        {
            if (!string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                bool isPhoneExist = await _context.KhachHang
                    .AnyAsync(k => k.SoDienThoai == request.SoDienThoai &&
                                  (!excludeId.HasValue || k.MaKhachHang != excludeId.Value));

                if (isPhoneExist) return (false, "Số điện thoại này đã tồn tại trong hệ thống.");
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                bool isEmailExist = await _context.KhachHang
                    .AnyAsync(k => k.Email == request.Email &&
                                  (!excludeId.HasValue || k.MaKhachHang != excludeId.Value));

                if (isEmailExist) return (false, "Email này đã được sử dụng cho một khách hàng khác.");
            }

            if (!string.IsNullOrWhiteSpace(request.MaSoThue))
            {
                bool isTaxCodeExist = await _context.KhachHang
                    .AnyAsync(k => k.MaSoThue == request.MaSoThue &&
                                  (!excludeId.HasValue || k.MaKhachHang != excludeId.Value));

                if (isTaxCodeExist) return (false, "Mã số thuế này đã tồn tại trong hệ thống.");
            }

            return (true, string.Empty);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetKhachHang(
            [FromQuery] int? limit,
            [FromQuery] string? ten,
            [FromQuery] string? sdt,
            [FromQuery] string? email)
        {
            var query = _context.KhachHang.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ten))
            {
                var search = ten.Trim();
                // Dùng bộ Latin tiêu chuẩn để coi ă/â/ê/ô/ơ/ư/đ là các ký tự có dấu (Accent)
                query = query.Where(k => EF.Functions.Collate(k.TenKhachHang, "SQL_Latin1_General_CP1_CI_AI").Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(sdt))
                query = query.Where(k => k.SoDienThoai.Contains(sdt.Trim()));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(k => k.Email.Contains(email.Trim()));

            IQueryable<KhachHang> finalQuery = query.OrderBy(k => k.MaKhachHang);

            if (limit.HasValue && limit.Value > 0)
            {
                finalQuery = finalQuery.Take(limit.Value);
            }

            var result = await finalQuery
                .OrderByDescending(q => q.NgayTao)
                .Select(MapToResponse())
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetKhachHangById(int id)
        {
            var khachHang = await _context.KhachHang
                .Where(k => k.MaKhachHang == id)
                .Select(MapToResponse())
                .FirstOrDefaultAsync();

            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(khachHang);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhachHang([FromBody] CustomerRequest request)
        {
            request.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email;
            request.DiaChi = string.IsNullOrWhiteSpace(request.DiaChi) ? null : request.DiaChi;
            request.MaSoThue = string.IsNullOrWhiteSpace(request.MaSoThue) ? null : request.MaSoThue;

            var (isValid, message) = await CheckUniqueCustomer(request);

            if (isValid)
            {
                var khachHang = MapToEntity().Compile()(request);
                _context.KhachHang.Add(khachHang);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetKhachHangById), new { id = khachHang.MaKhachHang }, MapToResponse().Compile()(khachHang));
            }
            else
            {
                return BadRequest(new { message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKhachHang(int id, [FromBody] CustomerRequest request)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound($"Không tìm thấy khách hàng {id}");
            }

            var (isValid, message) = await CheckUniqueCustomer(request, id);
            if (isValid)
            {
                khachHang.MaLoaiKhachHang = request.MaLoaiKhachHang;
                khachHang.TenKhachHang = request.TenKhachHang;
                khachHang.GioiTinh = request.GioiTinh;
                khachHang.NgaySinh = request.NgaySinh;
                khachHang.SoDienThoai = request.SoDienThoai;
                khachHang.Email = request.Email;
                khachHang.DiaChi = request.DiaChi;
                khachHang.MaSoThue = request.MaSoThue;

                _context.KhachHang.Update(khachHang);
                await _context.SaveChangesAsync();
                return Ok(MapToResponse().Compile()(khachHang));
            }
            else
            {
                return BadRequest(new { message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound($"Không tìm thấy khách hàng {id}");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hasRelatedInvoices = await _context.HoaDon.AnyAsync(hd => hd.MaKhachHang == id);
                if (hasRelatedInvoices || khachHang.TongDonDaMua != 0)
                {
                    throw new Exception($"Không thể xóa khách hàng vì {id} đã có hóa đơn liên quan.");
                }
                var hasRelatedReceipts = await _context.PhieuThuTien.AnyAsync(pt => pt.MaKhachHang == id);
                if (hasRelatedReceipts)
                {
                    throw new Exception("Không thể xóa khách hàng vì đã có phiếu thu tiền liên quan.");
                }

                if (khachHang.TienNo > 0)
                {
                    throw new Exception("Không thể xóa khách hàng vì còn nợ.");
                }

                _context.KhachHang.Remove(khachHang);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok($"Xóa khách hàng {id} thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Lỗi: {loiThatSu}");
            }
        }
    }
}

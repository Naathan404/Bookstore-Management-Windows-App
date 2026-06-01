using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
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
    }
}

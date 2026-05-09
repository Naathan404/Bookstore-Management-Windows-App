using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
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
                NgaySinh = k.NgaySinh.ToDateTime(TimeOnly.MinValue),
                CongNo = (long)k.TienNo
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetKhachHang()
        {
            var result =  await _context.KhachHang
                .Select(MapToResponse())
                .OrderByDescending(k => k.MaKhachHang)
                .ToListAsync();

            return Ok(result);
        }
    }
}

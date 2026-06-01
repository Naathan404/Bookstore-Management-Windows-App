using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NhaCungCapController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY DANH SÁCH 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            var list = await _context.NhaCungCap
                .Select(n => new SupplierDTO
                {
                    MaNhaCungCap = n.MaNhaCungCap,
                    TenNhaCungCap = n.TenNhaCungCap,
                    DiaChi = n.DiaChi,
                    MaSoThue = n.MaSoThue,
                    SoDienThoai = n.SoDienThoai,
                    Email = n.Email,
                    TenNganHang = n.NganHang, 
                    SoTaiKhoan = n.SoTaiKhoan,
                    NguoiDaiDien = n.NguoiDaiDien,
                    ConHoatDong = n.ConGiaoGich
                })
                .ToListAsync();

            return Ok(list);
        }

        // THÊM MỚI NHÀ CUNG CẤP
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDTO dto)
        {
            var checkUnique = await CheckUnique(0, dto);
            if (!checkUnique.IsValid) return BadRequest(checkUnique.Message);

            var newSupplier = new NhaCungCap
            {
                TenNhaCungCap = dto.TenNhaCungCap,
                DiaChi = dto.DiaChi,
                MaSoThue = dto.MaSoThue,
                SoDienThoai = dto.SoDienThoai,
                Email = dto.Email,
                NganHang = dto.TenNganHang,
                SoTaiKhoan = dto.SoTaiKhoan,
                NguoiDaiDien = dto.NguoiDaiDien,
                ConGiaoGich = dto.ConHoatDong
            };

            try
            {
                _context.NhaCungCap.Add(newSupplier);
                await _context.SaveChangesAsync();
                return Ok(true); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        // CẬP NHẬT NHÀ CUNG CẤP
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO dto)
        {
            if (id != dto.MaNhaCungCap) return BadRequest("ID không hợp lệ");

            var checkUnique = await CheckUnique(id, dto);
            if (!checkUnique.IsValid) return BadRequest(checkUnique.Message);

            var supplier = await _context.NhaCungCap.FindAsync(id);
            if (supplier == null) return NotFound("Không tìm thấy nhà cung cấp");

            supplier.TenNhaCungCap = dto.TenNhaCungCap;
            supplier.DiaChi = dto.DiaChi;
            supplier.MaSoThue = dto.MaSoThue;
            supplier.SoDienThoai = dto.SoDienThoai;
            supplier.Email = dto.Email;
            supplier.NganHang = dto.TenNganHang;
            supplier.SoTaiKhoan = dto.SoTaiKhoan;
            supplier.NguoiDaiDien = dto.NguoiDaiDien;
            supplier.ConGiaoGich = dto.ConHoatDong;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        // XÓA NHÀ CUNG CẤP 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null) return NotFound("Không tìm thấy nhà cung cấp");

            bool hasImport = await _context.PhieuNhapSach.AnyAsync(p => p.MaNhaCungCap == id);
            if (hasImport) return BadRequest("Không thể xóa nhà cung cấp đã tồn tại phiếu nhập");

            try
            {
                _context.NhaCungCap.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        // Hàm hỗ trợ kiểm tra Unique
        private async Task<(bool IsValid, string Message)> CheckUnique(int idToIgnore, SupplierDTO dto)
        {
            if (await _context.NhaCungCap.AnyAsync(n => n.TenNhaCungCap == dto.TenNhaCungCap && n.MaNhaCungCap != idToIgnore))
                return (false, "Tên nhà cung cấp đã tồn tại");

            if (!string.IsNullOrEmpty(dto.SoDienThoai) && await _context.NhaCungCap.AnyAsync(n => n.SoDienThoai == dto.SoDienThoai && n.MaNhaCungCap != idToIgnore))
                return (false, "Số điện thoại đã tồn tại");

            if (!string.IsNullOrEmpty(dto.Email) && await _context.NhaCungCap.AnyAsync(n => n.Email == dto.Email && n.MaNhaCungCap != idToIgnore))
                return (false, "Email đã tồn tại");

            if (!string.IsNullOrEmpty(dto.MaSoThue) && await _context.NhaCungCap.AnyAsync(n => n.MaSoThue == dto.MaSoThue && n.MaNhaCungCap != idToIgnore))
                return (false, "Mã số thuế đã tồn tại");

            return (true, "");
        }
    }
}
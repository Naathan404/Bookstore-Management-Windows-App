using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share;

using Bookstore.Share.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Linq.Expressions;

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

        private static Expression<Func<NhaCungCap, SupplierRequest>> MapToDTO()
        {
            return n => new SupplierRequest
            {
                TenNhaCungCap = n.TenNhaCungCap,
                DiaChi = n.DiaChi,
                MaSoThue = n.MaSoThue,
                SoDienThoai = n.SoDienThoai,
                Email = n.Email,
                NganHang = n.NganHang,
                SoTaiKhoan = n.SoTaiKhoan
            };
        }

        private async Task<(bool IsValid, string Message)> CheckUnique(int idToIgnore, SupplierRequest newSupplier)
        {
            if (await _context.NhaCungCap
                .AnyAsync(x => x.SoDienThoai == newSupplier.SoDienThoai && x.MaNhaCungCap != idToIgnore))
            {
                return (false, "Số điện thoại này đã thuộc về nhà cung cấp khác!");
            }

            if (await _context.NhaCungCap
                .AnyAsync(x => x.MaSoThue == newSupplier.MaSoThue && x.MaNhaCungCap != idToIgnore))
            {
                return (false, "Mã số thuế này đã thuộc về nhà cung cấp khác!");
            }

            if (await _context.NhaCungCap
                .AnyAsync(x => x.Email == newSupplier.Email && x.MaNhaCungCap != idToIgnore))
            {
                return (false, "Email này đã thuộc về nhà cung cấp khác!");
            }

            if (await _context.NhaCungCap
                .AnyAsync(x => x.NganHang == newSupplier.NganHang
                    && x.SoTaiKhoan == newSupplier.SoTaiKhoan
                    && x.MaNhaCungCap != idToIgnore))
            {
                return (false, "Tài khoản ngân hàng này đã thuộc về nhà cung cấp khác!");
            }

            if (await _context.NhaCungCap
                .AnyAsync(x => x.TenNhaCungCap == newSupplier.TenNhaCungCap && x.MaNhaCungCap != idToIgnore))
            {
                return (true, "Cảnh báo trùng tên với nhà cung cấp khác!");
            }

            return (true, "Thông tin hợp lệ!");

        }

        // LẤY TÊN CÁC NHÀ CUNG CẤP
        //GET: api/NhaCungCap/names
        [HttpGet("names")]
        public async Task<IActionResult> GetAllNhaCungCap()
        {
            var list = await _context.NhaCungCap
                                     .Select(ncc => ncc.TenNhaCungCap)
                                     .ToListAsync();
            return Ok(list);
        }

        //TÌM KIẾM NHÀ CUNG CẤP THEO THAM SỐ GẦN ĐÚNG
        //GET: api/NhaCungCap?ten=...&maSoThue=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhaCungCap>>> GetNhaCungCap(
            [FromQuery] string? ten,
            [FromQuery] string? maSoThue,
            [FromQuery] string? soDienThoai,
            [FromQuery] string? email)
        {
            var query = _context.NhaCungCap.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ten))
            {
                query = query.Where(ncc => ncc.TenNhaCungCap.Contains(ten));
            }

            if (!string.IsNullOrWhiteSpace(maSoThue))
            {
                query = query.Where(ncc => ncc.MaSoThue.Contains(maSoThue));
            }

            if (!string.IsNullOrWhiteSpace(soDienThoai))
            {
                query = query.Where(ncc => ncc.SoDienThoai.Contains(soDienThoai));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(ncc => ncc.Email.Contains(email));
            }

            var result = await query.ToListAsync();

            return Ok(result);
        }

        //LẤY THÔNG TIN NHÀ CUNG CẤP
        //GET: api/NhaCungCap/1
        [HttpGet("{id}")]
        public async Task<ActionResult<NhaCungCap>> GetById(int id)
        {
            var nhaCungCap = await _context.NhaCungCap
                .Where(ncc => ncc.MaNhaCungCap == id)
                .FirstOrDefaultAsync();
            if (nhaCungCap == null)
            {
                return NotFound($"Không tìm thấy nhà cung cấp {id}");
            }
            else
            {
                return Ok(nhaCungCap);
            }
        }

        //CẬP NHẬT THÔNG TIN NHÀ CUNG CẤP
        // PUT: api/NhaCungCap/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNhaCungCap(int id, [FromBody] SupplierRequest newNhaCungCap)
        {
            var oldNhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (oldNhaCungCap == null)
            {
                return NotFound($"Không tìm thấy nhà cung cấp {id}");
            }

            // Kiểm tra hợp lệ
            var validation = await CheckUnique(id, newNhaCungCap);
            if (!validation.IsValid)
            {
                return Conflict(new { message = validation.Message });
            }

            try
            {
                oldNhaCungCap.TenNhaCungCap = newNhaCungCap.TenNhaCungCap;
                oldNhaCungCap.DiaChi = newNhaCungCap.DiaChi;
                oldNhaCungCap.MaSoThue = newNhaCungCap.MaSoThue;
                oldNhaCungCap.SoDienThoai = newNhaCungCap.SoDienThoai;
                oldNhaCungCap.Email = newNhaCungCap.Email;
                oldNhaCungCap.NganHang = newNhaCungCap.NganHang;
                oldNhaCungCap.SoTaiKhoan = newNhaCungCap.SoTaiKhoan;

                await _context.SaveChangesAsync();
                return Ok(new { message = $"{validation.Message}, Cập nhật thông tin thành công!" });
            }
            catch (Exception ex)
            {
                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Lỗi: {loiThatSu}");
            }
        }

        // TẠO NHÀ CUNG CẤP MỚI
        // POST: api/NhaCungCap
        [HttpPost]
        public async Task<IActionResult> CreateNewSupplier([FromBody] SupplierRequest supplier)
        {
            var validation = await CheckUnique(-1, supplier); // Không loại trừ
            if (!validation.IsValid)
            {
                return Conflict(new { message = validation.Message });
            }

            try
            {
                var newSupplier = new NhaCungCap
                {
                    TenNhaCungCap = supplier.TenNhaCungCap,
                    DiaChi = supplier.DiaChi,
                    MaSoThue = supplier.MaSoThue,
                    SoDienThoai = supplier.SoDienThoai,
                    Email = supplier.Email,
                    NganHang = supplier.NganHang,
                    SoTaiKhoan = supplier.SoTaiKhoan
                };
                _context.NhaCungCap.Add(newSupplier);

                await _context.SaveChangesAsync();
                return Ok(new { message = $"Tạo thành công nhà cung cấp {newSupplier.MaNhaCungCap}, {validation.Message}" });
            }
            catch (Exception ex)
            {
                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Lỗi: {loiThatSu}");
            }
        }


        // XÓA NHÀ CUNG CẤP KHÔNG CÓ NHẬP SÁCH
        //DELETE: api/NhaCungCap/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null) return NotFound(new { message = $"Không tìm thấy nhà cung cấp {id}" });

            //Kiểm tra có tồn tại phiếu nhập không
            bool hasImport = await _context.PhieuNhapSach
                .AnyAsync(p => p.MaNhaCungCap == id);
            if (hasImport) return BadRequest(new { message = $"Không thể xóa nhà cung cấp đã tồn tại phiếu nhập" });

            try
            {
                _context.NhaCungCap.Remove(nhaCungCap);
                await _context.SaveChangesAsync();

                return Ok($"Xóa thành công nhà cung cấp {id}");
            }
            catch (Exception ex)
            {
                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Lỗi: {loiThatSu}");
            }
        }
    }

}

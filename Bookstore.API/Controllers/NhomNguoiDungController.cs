using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomNguoiDungController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NhomNguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        // ----------------------------------------------------------------
        // GET api/NhomNguoiDung
        // Trả về toàn bộ danh sách nhóm kèm số lượng thành viên.
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.NhomNguoiDung
                .OrderBy(r => r.MaNhomNguoiDung)
                .Select(r => new NhomNguoiDungDTO
                {
                    MaNhomNguoiDung = r.MaNhomNguoiDung,
                    TenNhomNguoiDung = r.TenNhomNguoiDung
                })
                .ToListAsync();

            return Ok(list);
        }

        // ----------------------------------------------------------------
        // POST api/NhomNguoiDung
        // Tạo nhóm mới, trả về object vừa tạo (có MaNhomNguoiDung mới).
        // ----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNhomNguoiDungDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenNhomNguoiDung))
                return BadRequest(new { message = "Tên nhóm không được để trống." });

            bool exists = await _context.NhomNguoiDung
                .AnyAsync(r => r.TenNhomNguoiDung == dto.TenNhomNguoiDung.Trim());

            if (exists)
                return Conflict(new { message = $"Tên nhóm '{dto.TenNhomNguoiDung}' đã tồn tại." });

            var newRole = new NhomNguoiDung
            {
                TenNhomNguoiDung = dto.TenNhomNguoiDung.Trim()
            };

            _context.NhomNguoiDung.Add(newRole);
            await _context.SaveChangesAsync();

            // Trả về object mới để WPF biết MaNhomNguoiDung vừa được DB cấp
            return Ok(new NhomNguoiDungDTO
            {
                MaNhomNguoiDung = newRole.MaNhomNguoiDung,
                TenNhomNguoiDung = newRole.TenNhomNguoiDung
            });
        }

        // ----------------------------------------------------------------
        // DELETE api/NhomNguoiDung/{id}
        // Không cho xóa nếu vẫn còn tài khoản thuộc nhóm này.
        // ----------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.NhomNguoiDung.FindAsync(id);
            if (role == null)
                return NotFound(new { message = "Không tìm thấy nhóm." });

            // Kiểm tra còn tài khoản thuộc nhóm không
            bool hasMembers = await _context.NguoiDung
                .AnyAsync(u => u.MaNhomNguoiDung == id);

            if (hasMembers)
                return BadRequest(new
                {
                    message = $"Không thể xóa nhóm '{role.TenNhomNguoiDung}' vì vẫn còn tài khoản thuộc nhóm này. " +
                              "Vui lòng chuyển các tài khoản đó sang nhóm khác trước."
                });

            // Xóa toàn bộ PhanQuyen của nhóm trước khi xóa nhóm
            var permissions = _context.PhanQuyen.Where(p => p.MaNhomNguoiDung == id);
            _context.PhanQuyen.RemoveRange(permissions);

            _context.NhomNguoiDung.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa nhóm thành công." });
        }
    }
}
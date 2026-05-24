using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanQuyenController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhanQuyenController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ PHÂN QUYỀN
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhanQuyen>>> GetAll()
        {
            return await _context.PhanQuyen.ToListAsync();
        }

        // LẤY DANH SÁCH QUYỀN CỦA 1 NHÓM NGƯỜI DÙNG 
        [HttpGet("Nhom/{maNhom}")]
        public async Task<ActionResult<IEnumerable<PhanQuyen>>> GetByNhom(int maNhom)
        {
            return await _context.PhanQuyen
                                 .Where(x => x.MaNhomNguoiDung == maNhom)
                                 .ToListAsync();
        }

        // THÊM 1 QUYỀN MỚI CHO NHÓM
        [HttpPost]
        public async Task<ActionResult<PhanQuyen>> PostPhanQuyen(PhanQuyen phanQuyen)
        {
            _context.PhanQuyen.Add(phanQuyen);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PhanQuyenExists(phanQuyen.MaNhomNguoiDung, phanQuyen.MaChucNang))
                {
                    return Conflict(new { message = "Nhóm này đã có quyền này rồi!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(phanQuyen);
        }

        // XÓA QUYỀN CỦA NHÓM
        [HttpDelete("{maNhom}/{maChucNang}")]
        public async Task<IActionResult> DeletePhanQuyen(int maNhom, int maChucNang)
        {
            var phanQuyen = await _context.PhanQuyen.FindAsync(maNhom, maChucNang);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            _context.PhanQuyen.Remove(phanQuyen);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã thu hồi quyền thành công!" });
        }

        [HttpGet("chuc-nang/{roleId}")]
        public async Task<IActionResult> GetChucNangForRole(int roleId)
        {
            var allScreens = await _context.ChucNang
                .OrderBy(c => c.MaChucNang)
                .ToListAsync();

            var grantedIds = (await _context.PhanQuyen
                .Where(p => p.MaNhomNguoiDung == roleId)
                .Select(p => p.MaChucNang)
                .ToListAsync())
                .ToHashSet();

            var result = allScreens.Select(c => new ScreenPermissionDTO
            {
                MaChucNang = c.MaChucNang,
                TenChucNang = c.TenChucNang,
                TenManHinh = c.TenManHinh,
                IsGranted = grantedIds.Contains(c.MaChucNang)
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// PUT api/PhanQuyen/{roleId}
        /// Ghi đè toàn bộ phân quyền của nhóm: xóa cũ → insert mới.
        /// </summary>
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdatePermissions(
            int roleId, [FromBody] UpdatePermissionsDTO dto)
        {
            bool roleExists = await _context.NhomNguoiDung
                .AnyAsync(r => r.MaNhomNguoiDung == roleId);

            if (!roleExists)
                return NotFound(new { message = "Không tìm thấy nhóm." });

            bool isAdminGroup = await _context.NhomNguoiDung
                .AnyAsync(r => r.MaNhomNguoiDung == roleId
                && r.TenNhomNguoiDung == "ADMIN");

            if (isAdminGroup && !dto.GrantedChucNangIds.Contains(10))
            {
                return BadRequest(new
                {
                    message = "Không thể tắt quyền 'Tài khoản' của nhóm ADMIN. " +
                              "Hệ thống cần ít nhất 1 nhóm có thể quản lý tài khoản!"
                });
            }

            // Xóa toàn bộ quyền cũ
            var oldPermissions = _context.PhanQuyen
                .Where(p => p.MaNhomNguoiDung == roleId);
            _context.PhanQuyen.RemoveRange(oldPermissions);

            // Insert lại các quyền mới
            var newPermissions = dto.GrantedChucNangIds
                .Distinct()
                .Select(chucNangId => new PhanQuyen
                {
                    MaNhomNguoiDung = roleId,
                    MaChucNang = chucNangId
                });

            _context.PhanQuyen.AddRange(newPermissions);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật phân quyền thành công.",
                grantedCount = dto.GrantedChucNangIds.Distinct().Count()
            });
        }

        // ── Private helper ───────────────────────────────────────────

        private bool PhanQuyenExists(int maNhom, int maChucNang)
        {
            return _context.PhanQuyen.Any(e => e.MaNhomNguoiDung == maNhom && e.MaChucNang == maChucNang);
        }
    }
}
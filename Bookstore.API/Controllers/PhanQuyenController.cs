using Bookstore.API.Data;
using Bookstore.API.Models;
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

        private bool PhanQuyenExists(int maNhom, int maChucNang)
        {
            return _context.PhanQuyen.Any(e => e.MaNhomNguoiDung == maNhom && e.MaChucNang == maChucNang);
        }
    }
}
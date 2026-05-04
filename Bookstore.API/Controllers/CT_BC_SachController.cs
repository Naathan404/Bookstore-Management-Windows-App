using Bookstore.API.Data;
// using Bookstore.API.Data; // Nhớ check lại namespace DbContext
using Bookstore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CT_BC_SachController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CT_BC_SachController(AppDbContext context)
        {
            _context = context;
        }

        // LẤY TOÀN BỘ DANH SÁCH CHI TIẾT
        // GET: api/CT_BC_Sach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CT_BC_Sach>>> GetCT_BC_Sach()
        {
            return await _context.CT_BC_Sach.ToListAsync();
        }

        //LẤY DANH SÁCH CHI TIẾT CỦA 1 BÁO CÁO CỤ THỂ 
        // GET: api/CT_BC_Sach/BaoCao/5
        [HttpGet("BaoCao/{maBaoCao}")]
        public async Task<ActionResult<IEnumerable<CT_BC_Sach>>> GetByMaBaoCao(int maBaoCao)
        {
            return await _context.CT_BC_Sach
                                 .Where(x => x.MaBaoCaoSach == maBaoCao)
                                 .ToListAsync();
        }

        // LẤY ĐÚNG 1 DÒNG CHI TIẾT
        // GET: api/CT_BC_Sach/5/978-0132350884
        [HttpGet("{maBaoCao}/{isbn}")]
        public async Task<ActionResult<CT_BC_Sach>> GetCT_BC_Sach(int maBaoCao, string isbn)
        {
            var cT_BC_Sach = await _context.CT_BC_Sach.FindAsync(maBaoCao, isbn);

            if (cT_BC_Sach == null)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết báo cáo sách này!" });
            }

            return cT_BC_Sach;
        }

        // CẬP NHẬT 1 DÒNG CHI TIẾT
        // PUT: api/CT_BC_Sach/5/978-0132350884
        [HttpPut("{maBaoCao}/{isbn}")]
        public async Task<IActionResult> PutCT_BC_Sach(int maBaoCao, string isbn, CT_BC_Sach cT_BC_Sach)
        {
            // Check khớp cả 2 key
            if (maBaoCao != cT_BC_Sach.MaBaoCaoSach || isbn != cT_BC_Sach.ISBN)
            {
                return BadRequest(new { message = "Mã URL không khớp với Body dữ liệu!" });
            }

            _context.Entry(cT_BC_Sach).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CT_BC_SachExists(maBaoCao, isbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // THÊM MỚI 1 DÒNG CHI TIẾT
        // POST: api/CT_BC_Sach
        [HttpPost]
        public async Task<ActionResult<CT_BC_Sach>> PostCT_BC_Sach(CT_BC_Sach cT_BC_Sach)
        {
            _context.CT_BC_Sach.Add(cT_BC_Sach);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Check tồn tại bằng 2 key
                if (CT_BC_SachExists(cT_BC_Sach.MaBaoCaoSach, cT_BC_Sach.ISBN))
                {
                    return Conflict(new { message = "Chi tiết báo cáo này đã tồn tại!" });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetCT_BC_Sach),
                new { maBaoCao = cT_BC_Sach.MaBaoCaoSach, isbn = cT_BC_Sach.ISBN },
                cT_BC_Sach);
        }

        // xÓA 1 DÒNG CHI TIẾT
        // DELETE: api/CT_BC_Sach/5/978-0132350884
        [HttpDelete("{maBaoCao}/{isbn}")]
        public async Task<IActionResult> DeleteCT_BC_Sach(int maBaoCao, string isbn)
        {
            var cT_BC_Sach = await _context.CT_BC_Sach.FindAsync(maBaoCao, isbn);
            if (cT_BC_Sach == null)
            {
                return NotFound();
            }

            _context.CT_BC_Sach.Remove(cT_BC_Sach);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công!" });
        }

        // Hàm check tồn tại phải nhận cả int và string
        private bool CT_BC_SachExists(int maBaoCao, string isbn)
        {
            return _context.CT_BC_Sach.Any(e => e.MaBaoCaoSach == maBaoCao && e.ISBN == isbn);
        }
    }
}
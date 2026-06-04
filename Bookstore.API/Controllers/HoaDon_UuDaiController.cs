using Bookstore.API.Data;
// using Bookstore.API.Data; // Chú ý kiểm tra lại namespace DbContext
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDon_UuDaiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HoaDon_UuDaiController(AppDbContext context)
        {
            _context = context;
        }

        private static Expression<Func<HoaDon_UuDai, InvoicePromoResponse>> MapToResponse =
            hu => new InvoicePromoResponse
            {
                MaCT_HoaDon_UuDai = hu.MaCT_HoaDon_UuDai,
                MaHoaDon = hu.MaHoaDon,
                MaUuDai = hu.MaUuDai,
                SoTienGiam = hu.SoTienGiam,
                Code = hu.UuDai != null ? hu.UuDai.Code : string.Empty,
                TenUuDai = hu.UuDai != null ? hu.UuDai.TenChuongTrinh : string.Empty,
                SoTienGiamHienThi = hu.SoTienGiam > 0
                    ? "-" + hu.SoTienGiam.ToString("N0") + " đ"
                    : "(Quà tặng) 0 đ"
            };

        // LẤY TOÀN BỘ DANH SÁCH 
        // GET: api/HoaDon_UuDai
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoicePromoResponse>>> GetHoaDon_Uudai()
        {
            return await _context.HoaDon_Uudai.Select(MapToResponse).ToListAsync();
        }

        [HttpGet("HoaDon/{maHoaDon}")]
        public async Task<ActionResult<IEnumerable<InvoicePromoResponse>>> GetByMaHoaDon(int maHoaDon)
        {
            // BƯỚC 1: Kéo dữ liệu thô và THÊM MaLoaiUuDai từ bảng master
            var rawData = await _context.HoaDon_Uudai
                .Where(hu => hu.MaHoaDon == maHoaDon)
                .OrderBy(hu => hu.MaCT_HoaDon_UuDai)
                .Select(hu => new
                {
                    hu.MaCT_HoaDon_UuDai,
                    hu.MaHoaDon,
                    hu.MaUuDai,
                    hu.SoTienGiam,
                    Code = hu.UuDai != null ? hu.UuDai.Code : string.Empty,
                    TenUuDai = hu.UuDai != null ? hu.UuDai.TenChuongTrinh : string.Empty,

                    // THÊM DÒNG NÀY ĐỂ BẮT LOGIC
                    MaLoaiUuDai = hu.UuDai != null ? hu.UuDai.MaLoaiUuDai : PromotionType.HoaDonGiam
                })
                .ToListAsync();

            // BƯỚC 2: Check định dạng hiển thị dựa trên Loại ưu đãi
            var promos = rawData.Select(x => new InvoicePromoResponse
            {
                MaCT_HoaDon_UuDai = x.MaCT_HoaDon_UuDai,
                MaHoaDon = x.MaHoaDon,
                MaUuDai = x.MaUuDai,
                Code = x.Code,
                TenUuDai = x.TenUuDai,
                SoTienGiam = x.SoTienGiam,

                // CHỈ KIỂM TRA LOẠI ƯU ĐÃI: Nếu là HoaDonQua hoặc SachQua -> Ghi chữ (Quà tặng)
                SoTienGiamHienThi = (x.MaLoaiUuDai == PromotionType.HoaDonQua || x.MaLoaiUuDai == PromotionType.SachQua)
                    ? "(Quà tặng)"
                    : $"- {x.SoTienGiam:N0} đ"
            }).ToList();

            return Ok(promos);
        }

        // LẤY CHI TIẾT ĐÚNG 1 DÒNG DỰA VÀO ID
        // GET: api/HoaDon_UuDai/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoicePromoResponse>> GetHoaDon_UuDai(int id)
        {
            var hoaDon_UuDai = await _context.HoaDon_Uudai.FindAsync(id);

            if (hoaDon_UuDai == null)
            {
                return NotFound(new { message = "Không tìm thấy lịch sử ưu đãi này!" });
            }

            return MapToResponse.Compile()(hoaDon_UuDai);
        }

        // CẬP NHẬT 
        // PUT: api/HoaDon_UuDai/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon_UuDai(int id, HoaDon_UuDai hoaDon_UuDai)
        {
            if (id != hoaDon_UuDai.MaCT_HoaDon_UuDai)
            {
                return BadRequest(new { message = "Mã URL không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(hoaDon_UuDai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDon_UuDaiExists(id))
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

        // THÊM MỚI 
        // POST: api/HoaDon_UuDai
        [HttpPost]
        public async Task<ActionResult<HoaDon_UuDai>> PostHoaDon_UuDai(HoaDon_UuDai hoaDon_UuDai)
        {
            _context.HoaDon_Uudai.Add(hoaDon_UuDai);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHoaDon_UuDai), new { id = hoaDon_UuDai.MaCT_HoaDon_UuDai }, hoaDon_UuDai);
        }

        //  XÓA 
        // DELETE: api/HoaDon_UuDai/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon_UuDai(int id)
        {
            var hoaDon_UuDai = await _context.HoaDon_Uudai.FindAsync(id);
            if (hoaDon_UuDai == null)
            {
                return NotFound();
            }

            _context.HoaDon_Uudai.Remove(hoaDon_UuDai);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa lịch sử ưu đãi của hóa đơn!" });
        }

        private bool HoaDon_UuDaiExists(int id)
        {
            return _context.HoaDon_Uudai.Any(e => e.MaCT_HoaDon_UuDai == id);
        }
    }
}
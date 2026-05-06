using Bookstore.API.Data;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SachController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sach
        [HttpGet]
        public async Task<IActionResult> GetAllSach()
        {
            try
            {
                // 1. Join 3 bảng: PhienBanSach, Sach, TheLoai
                var querySach = from pbs in _context.PhienBanSach
                                join s in _context.Sach on pbs.MaSach equals s.MaSach
                                join tl in _context.TheLoai on s.MaTheLoai equals tl.MaTheLoai
                                select new { PBS = pbs, Sach = s, TheLoai = tl };

                var listSachInfo = await querySach.ToListAsync();

                // 2. Lấy danh sách Tác giả (Join bảng TacGia_Sach và TacGia)
                var queryTacGia = await (from tgs in _context.TacGia_Sach
                                         join tg in _context.TacGia on tgs.MaTacGia equals tg.MaTacGia
                                         select new { tgs.MaSach, tg.TenTacGia }).ToListAsync();

                // 3. Map dữ liệu thành DTO trả về cho WPF
                var dtos = listSachInfo.Select(x => new SachDTO
                {
                    ISBN = x.PBS.ISBN, // Mã sách thực tế
                    Id = x.Sach.MaSach,
                    TenSach = x.Sach.TenSach,
                    TheLoai = x.TheLoai.TenTheLoai,
                    MoTa = x.Sach.MoTa,
                    SoLuongTonKho = x.PBS.TonKho,
                    TongDaBan = x.PBS.TongSoDaBan,
                    GiaNiemYet = x.PBS.GiaNiemYet,
                    DonGiaBan = x.PBS.DonGiaBan,
                    HinhAnh = x.Sach.ImageUrl,
                    // Lọc tìm tác giả của mã sách này, gộp lại thành 1 chuỗi cách nhau bằng dấu phẩy
                    TacGia = string.Join(", ", queryTacGia
                                                .Where(t => t.MaSach == x.Sach.MaSach)
                                                .Select(t => t.TenTacGia))
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
}
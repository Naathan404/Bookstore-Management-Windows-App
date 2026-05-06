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
                // Join PhienBanSach, Sach, TheLoai, NXB
                var querySach = from pbs in _context.PhienBanSach
                                join s in _context.Sach on pbs.MaSach equals s.MaSach
                                join tl in _context.TheLoai on s.MaTheLoai equals tl.MaTheLoai
                                join nxb in _context.NhaXuatBan on pbs.MaNhaXuatBan equals nxb.MaNhaXuatBan
                                select new { PBS = pbs, Sach = s, TheLoai = tl, NXB = nxb };

                var listSachInfo = await querySach.ToListAsync();

                //Lấy danh sách Tác giả
                var queryTacGia = await (from tgs in _context.TacGia_Sach
                                         join tg in _context.TacGia on tgs.MaTacGia equals tg.MaTacGia
                                         select new { tgs.MaSach, tg.TenTacGia }).ToListAsync();

                // Map dữ liệu thành DTO trả về cho WPF
                var dtos = listSachInfo.Select(x => new SachDTO
                {
                    ISBN = x.PBS.ISBN,
                    Id = x.Sach.MaSach,
                    TenSach = x.Sach.TenSach,
                    TheLoai = x.TheLoai.TenTheLoai,
                    MoTa = x.Sach.MoTa,
                    SoLuongTonKho = x.PBS.TonKho,
                    TongDaBan = x.PBS.TongSoDaBan,
                    GiaNiemYet = x.PBS.GiaNiemYet,
                    DonGiaBan = x.PBS.DonGiaBan,
                    HinhAnh = x.Sach.ImageUrl,
                    NamXuatBan = x.PBS.NamXuatBan,
                    NhaXuatBan = x.NXB.TenNhaXuatBan,
                    HinhThucBia = x.PBS.HinhThucBia,
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

        // PUT: api/Sach/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSach(int id, [FromBody] SachDTO request)
        {
            try
            {
                // Tìm cuốn sách gốc trong DB
                var sach = await _context.Sach.FirstOrDefaultAsync(s => s.MaSach == id);
                if (sach == null)
                    return NotFound("Không tìm thấy sách!");

                var theLoai = await _context.TheLoai.FirstOrDefaultAsync(t => t.TenTheLoai == request.TheLoai);
                if (theLoai != null)
                {
                    sach.MaTheLoai = theLoai.MaTheLoai;
                }

                //Cập nhật bảng Sach
                sach.TenSach = request.TenSach;
                sach.MoTa = request.MoTa;
                sach.ImageUrl = request.HinhAnh;

                // Tìm và cập nhật bảng PhienBanSach
                var phienBan = await _context.PhienBanSach.FirstOrDefaultAsync(p => p.MaSach == id);
                if (phienBan != null)
                {
                    phienBan.GiaNiemYet = request.GiaNiemYet;
                    phienBan.DonGiaBan = request.DonGiaBan;
                }

                // Lưu xuống Database
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
}
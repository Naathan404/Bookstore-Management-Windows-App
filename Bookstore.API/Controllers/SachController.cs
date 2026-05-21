using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Lấy thông tin từ bảng Sach và join với bảng TheLoai
                var listSach = await _context.Sach
                    .Include(s => s.TheLoai)
                    .ToListAsync();

                // Lấy dữ liệu từ bảng trung gian TacGia_Sach để móc ra Tên Tác Giả
                var listTacGiaSach = await _context.TacGia_Sach // Tương ứng CT_TACGIA
                    .Include(ts => ts.TacGia)
                    .ToListAsync();

                // Map dữ liệu thủ công gộp lại cho WPF dễ đọc
                var result = listSach.Select(s => new DauSachResponseDTO
                {
                    Id = s.MaSach,
                    TenSach = s.TenSach,
                    TenTheLoai = s.TheLoai?.TenTheLoai ?? "Chưa xác định",
                    ImageUrl = s.ImageUrl,
                    MoTa = s.MoTa ?? "",
                    DanhSachTacGia = listTacGiaSach
                        .Where(ts => ts.MaSach == s.MaSach)
                        .Select(ts => new TacGiaDTO
                        {
                            Id = ts.TacGia.MaTacGia,
                            TenTacGia = ts.TacGia.TenTacGia
                        }).ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DauSachRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.TenSach))
                return BadRequest("Tên đầu sách không được để trống.");

            // Dịch Tên Thể Loại -> Mã Thể Loại
            var theLoai = await _context.TheLoai.FirstOrDefaultAsync(tl => tl.TenTheLoai == request.TenTheLoai);
            if (theLoai == null) return BadRequest("Thể loại không tồn tại trong hệ thống!");

            // Thêm vào bảng Sach
            var newSach = new Sach
            {
                TenSach = request.TenSach,
                MaTheLoai = theLoai.MaTheLoai,
                MoTa = request.MoTa ?? "",
                ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? "/Resources/Images/Books/default_book_cover.jpg" : request.ImageUrl
            };

            _context.Sach.Add(newSach);
            await _context.SaveChangesAsync(); // Lưu để lấy được MaSach sinh tự động

            // Thêm vào bảng trung gian TacGia_Sach
            if (request.DanhSachTacGia != null && request.DanhSachTacGia.Any())
            {
                foreach (var maTacGia in request.DanhSachTacGia)
                {
                    _context.TacGia_Sach.Add(new TacGia_Sach
                    {
                        MaSach = newSach.MaSach,
                        MaTacGia = maTacGia
                    });
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Thêm sách thành công!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DauSachRequestDTO request)
        {
            var sach = await _context.Sach.FindAsync(id);
            if (sach == null) return NotFound("Không tìm thấy sách.");

            // Dịch Tên Thể Loại -> Mã Thể Loại
            var theLoai = await _context.TheLoai.FirstOrDefaultAsync(tl => tl.TenTheLoai == request.TenTheLoai);
            if (theLoai == null) return BadRequest("Thể loại không tồn tại!");

            // Cập nhật bảng Sach
            sach.TenSach = request.TenSach;
            sach.MaTheLoai = theLoai.MaTheLoai;
            sach.MoTa = request.MoTa ?? "";

            // Cập nhật bảng TacGia_Sach: Xóa sạch cái cũ của sách này
            var oldTacGiaSachs = _context.TacGia_Sach.Where(ts => ts.MaSach == id);
            _context.TacGia_Sach.RemoveRange(oldTacGiaSachs);

            // Insert lại cái mới
            if (request.DanhSachTacGia != null && request.DanhSachTacGia.Any())
            {
                foreach (var maTacGia in request.DanhSachTacGia)
                {
                    _context.TacGia_Sach.Add(new TacGia_Sach
                    {
                        MaSach = id,
                        MaTacGia = maTacGia
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sach = await _context.Sach.FindAsync(id);
            if (sach == null) return NotFound("Không tìm thấy sách.");

            var oldTacGiaSachs = _context.TacGia_Sach.Where(ts => ts.MaSach == id);
            _context.TacGia_Sach.RemoveRange(oldTacGiaSachs);

            // Xóa Sách
            _context.Sach.Remove(sach);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa sách thành công!" });
        }
    }
}
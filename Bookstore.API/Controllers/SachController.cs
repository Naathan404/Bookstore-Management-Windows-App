using Bookstore.API.Data;
using Bookstore.API.Models;
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
        public async Task<IActionResult> GetAllPhienBanSach()
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateSach(int id, [FromBody] SachDTO request)
        //{
        //    try
        //    {
        //        // Tìm cuốn sách gốc trong DB
        //        var sach = await _context.Sach.FirstOrDefaultAsync(s => s.MaSach == id);
        //        if (sach == null)
        //            return NotFound("Không tìm thấy sách!");

        //        var theLoai = await _context.TheLoai.FirstOrDefaultAsync(t => t.TenTheLoai == request.TheLoai);
        //        if (theLoai != null)
        //        {
        //            sach.MaTheLoai = theLoai.MaTheLoai;
        //        }

        //        //Cập nhật bảng Sach
        //        sach.TenSach = request.TenSach;
        //        sach.MoTa = request.MoTa;
        //        sach.ImageUrl = request.HinhAnh;

        //        // Tìm và cập nhật bảng PhienBanSach
        //        var phienBan = await _context.PhienBanSach.FirstOrDefaultAsync(p => p.MaSach == id);
        //        if (phienBan != null)
        //        {
        //            phienBan.GiaNiemYet = request.GiaNiemYet;
        //            phienBan.DonGiaBan = request.DonGiaBan;
        //        }

        //        // Lưu xuống Database
        //        await _context.SaveChangesAsync();
        //        return Ok(new { message = "Cập nhật thành công!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi server: {ex.Message}");
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSach(string id, [FromBody] SachDTO request)
        {
            try
            {
                // Cập nhật bảng Phiên bản
                var pb = await _context.PhienBanSach.Include(nxb => nxb.NhaXuatBan).FirstOrDefaultAsync(p => p.ISBN == id);
                if (pb == null) return NotFound();

                pb.ISBN = request.ISBN;
                pb.GiaNiemYet = request.GiaNiemYet;
                pb.DonGiaBan = request.DonGiaBan;
                pb.NamXuatBan = request.NamXuatBan;
                pb.HinhThucBia = request.HinhThucBia;

                // Cập nhật bảng Tác phẩm
                var sach = await _context.Sach.FindAsync(pb.MaSach);
                if (sach != null)
                {
                    sach.TenSach = request.TenSach;
                    sach.MoTa = request.MoTa;
                    sach.ImageUrl = request.HinhAnh;

                    var theLoai = await _context.TheLoai.FirstOrDefaultAsync(tl => tl.TenTheLoai == request.TheLoai);
                    if (theLoai != null) sach.MaTheLoai = theLoai.MaTheLoai;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateSach([FromBody] SachDTO request)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // Tạo gốc
        //        var theLoai = await _context.TheLoai.FirstOrDefaultAsync(tl => tl.TenTheLoai == request.TheLoai);
        //        var sachMoi = new Sach
        //        {
        //            TenSach = request.TenSach,
        //            MaTheLoai = theLoai?.MaTheLoai ?? 1,
        //            MoTa = request.MoTa,
        //            ImageUrl = request.HinhAnh
        //        };
        //        _context.Sach.Add(sachMoi);
        //        await _context.SaveChangesAsync();

        //        // Tạo Phiên bản đầu tiên 
        //        var phienBanMoi = new PhienBanSach
        //        {
        //            MaSach = sachMoi.MaSach,
        //            ISBN = request.ISBN,
        //            GiaNiemYet = request.GiaNiemYet,
        //            DonGiaBan = request.DonGiaBan,
        //            TonKho = request.SoLuongTonKho,
        //            NamXuatBan = request.NamXuatBan,
        //            MaNhaXuatBan = request.NhaXuatBan,
        //            HinhThucBia = request.HinhThucBia
        //        };
        //        _context.PhienBanSach.Add(phienBanMoi);
        //        await _context.SaveChangesAsync();

        //        await transaction.CommitAsync();
        //        return Ok(new { message = "Thêm sách thành công!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateSach([FromBody] SachDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // XỬ LÝ NHÀ XUẤT BẢN ĐỂ LẤY MÃ
                int maNXB;
                var nxbTonTai = await _context.NhaXuatBan
                                              .FirstOrDefaultAsync(n => n.TenNhaXuatBan == request.NhaXuatBan);

                if (nxbTonTai == null)
                {
                    // Nếu chưa có NXB này, tạo mới để lấy MaNhaXuatBan tự tăng
                    var nxbMoi = new NhaXuatBan { TenNhaXuatBan = request.NhaXuatBan };
                    _context.NhaXuatBan.Add(nxbMoi);
                    await _context.SaveChangesAsync();
                    maNXB = nxbMoi.MaNhaXuatBan; // Lấy ID vừa sinh ra
                }
                else
                {
                    maNXB = nxbTonTai.MaNhaXuatBan; // Lấy ID đã có
                }

                // --- BƯỚC 2: TẠO TÁC PHẨM (Bảng Sach) ---
                // (Giữ nguyên logic cũ để tạo Sach...)
                var sachMoi = new Sach { TenSach = request.TenSach };
                _context.Sach.Add(sachMoi);
                await _context.SaveChangesAsync();

                // --- BƯỚC 3: TẠO PHIÊN BẢN (Bảng PhienBanSach) ---
                var phienBanMoi = new PhienBanSach
                {
                    MaSach = sachMoi.MaSach,
                    ISBN = request.ISBN,
                    MaNhaXuatBan = maNXB, // GÁN KHÓA NGOẠI VÀO ĐÂY
                                          // ... các trường khác
                };
                _context.PhienBanSach.Add(phienBanMoi);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Thành công!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }
    }
}
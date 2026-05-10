using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhienBanSachController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhienBanSachController(AppDbContext context)
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
                                         select new { tgs.MaSach, tg.MaTacGia, tg.TenTacGia }).ToListAsync();

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
                    LanTaiBan = x.PBS.LanTaiBan,
                    DonGiaBan = x.PBS.DonGiaBan,
                    HinhAnh = x.Sach.ImageUrl,
                    NamXuatBan = x.PBS.NamXuatBan,
                    NhaXuatBan = x.NXB.TenNhaXuatBan,
                    HinhThucBia = x.PBS.HinhThucBia,
                    // Lọc tìm tác giả của mã sách này, gộp lại thành 1 chuỗi cách nhau bằng dấu phẩy
                    //TacGia = string.Join(", ", queryTacGia
                    //                            .Where(t => t.MaSach == x.Sach.MaSach)
                    //                            .Select(t => t.TenTacGia))
                    DanhSachTacGia = queryTacGia
                                        .Where(t => t.MaSach == x.Sach.MaSach)
                                        .Select(t => new TacGiaDTO
                                        {
                                            Id = t.MaTacGia,
                                            TenTacGia = t.TenTacGia
                                        }).ToList()
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        


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
                pb.LanTaiBan = request.LanTaiBan;
                pb.NhaXuatBan = await _context.NhaXuatBan.FirstOrDefaultAsync(nxb => nxb.TenNhaXuatBan == request.NhaXuatBan);

                // Cập nhật bảng Sách
                var sach = await _context.Sach.FindAsync(pb.MaSach);
                if (sach != null)
                {
                    sach.TenSach = request.TenSach;
                    sach.MoTa = request.MoTa;
                    sach.ImageUrl = request.HinhAnh;

                    var oldTacGias = _context.TacGia_Sach.Where(t => t.MaSach == sach.MaSach);
                    _context.TacGia_Sach.RemoveRange(oldTacGias);

                    // Thêm lại danh sách tác giả mới
                    if (request.DanhSachTacGia != null && request.DanhSachTacGia.Any())
                    {
                        foreach (var tg in request.DanhSachTacGia)
                        {
                            _context.TacGia_Sach.Add(new TacGia_Sach
                            {
                                MaSach = sach.MaSach,
                                MaTacGia = tg.Id
                            });
                        }
                    }

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


        [HttpPost]
        public async Task<IActionResult> CreateSach([FromBody] SachDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int maSachThucTe = request.MaSachGoc;

                if (request.IsTacPhamMoi)
                {
                    // Kiểm tra/Tạo Thể loại
                    var theLoai = await _context.TheLoai.FirstOrDefaultAsync(tl => tl.TenTheLoai == request.TheLoai);
                    if (theLoai == null)
                    {
                        theLoai = new TheLoai { TenTheLoai = request.TheLoai };
                        _context.TheLoai.Add(theLoai);
                        await _context.SaveChangesAsync();
                    }

                    var sachMoi = new Sach
                    {
                        TenSach = request.TenSach,
                        MaTheLoai = theLoai.MaTheLoai,
                        MoTa = request.MoTa,
                        ImageUrl = request.HinhAnh
                    };
                    _context.Sach.Add(sachMoi);
                    await _context.SaveChangesAsync();
                    maSachThucTe = sachMoi.MaSach;

                    // Lưu Tác giả cho đầu sách mới
                    foreach (var tg in request.DanhSachTacGia)
                    {
                        _context.TacGia_Sach.Add(new TacGia_Sach { MaSach = maSachThucTe, MaTacGia = tg.Id });
                    }
                }

                var nxb = await _context.NhaXuatBan.FirstOrDefaultAsync(n => n.TenNhaXuatBan == request.NhaXuatBan);
                if (nxb == null)
                {
                    nxb = new NhaXuatBan { TenNhaXuatBan = request.NhaXuatBan };
                    _context.NhaXuatBan.Add(nxb);
                    await _context.SaveChangesAsync();
                }

                var phienBan = new PhienBanSach
                {
                    ISBN = request.ISBN,
                    MaSach = maSachThucTe,
                    MaNhaXuatBan = nxb.MaNhaXuatBan,
                    NamXuatBan = request.NamXuatBan,
                    LanTaiBan = request.LanTaiBan,
                    HinhThucBia = request.HinhThucBia,
                    GiaNiemYet = request.GiaNiemYet,
                    DonGiaBan = request.DonGiaBan,
                    TonKho = request.SoLuongTonKho
                };
                _context.PhienBanSach.Add(phienBan);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Thành công" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteSach(string isbn)
        {
            try
            {
                // Tìm phiên bản sách theo ISBN
                var phienBan = await _context.PhienBanSach.FirstOrDefaultAsync(pb => pb.ISBN == isbn);

                if (phienBan == null) return NotFound("Không tìm thấy sách.");

                // Kiểm tra xem sách này đã có giao dịch chưa (Tránh lỗi ràng buộc dữ liệu)
                bool daCoGiaoDich = await _context.CT_HoaDon.AnyAsync(ct => ct.ISBN == isbn) ||
                                    await _context.CT_PhieuNhapSach.AnyAsync(ct => ct.ISBN == isbn);

                if (daCoGiaoDich)
                    return BadRequest("Sách đã có lịch sử giao dịch, không được phép xóa để đảm bảo thống kê.");

                _context.PhienBanSach.Remove(phienBan);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
}
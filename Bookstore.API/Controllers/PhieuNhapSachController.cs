using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuNhapController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhieuNhapController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportOrderResponse>>> GetAll()
        {
            var data = await _context.PhieuNhapSach
                .Include(p => p.NhaCungCap)
                .Join(_context.NguoiDung, p => p.NguoiTao, n => n.TenDangNhap, (p, n) => new ImportOrderResponse
                {
                    MaPhieuNhap = p.MaPhieuNhapSach,
                    NgayNhap = p.NgayTao,
                    MaNhaCungCap = p.MaNhaCungCap,
                    TenNhaCungCap = p.NhaCungCap.TenNhaCungCap,
                    TenNguoiTao = n.HoTen,
                    TongTien = p.TongTien,
                    GhiChu = p.GhiChu
                })
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImportOrderDetailResponse>> GetDetail(int id)
        {
            var phieuNhap = await _context.PhieuNhapSach
                .Include(p => p.NhaCungCap)
                .FirstOrDefaultAsync(p => p.MaPhieuNhapSach == id);

            if (phieuNhap == null) return NotFound();

            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(n => n.TenDangNhap == phieuNhap.NguoiTao);

            var detail = new ImportOrderDetailResponse
            {
                MaPhieuNhap = phieuNhap.MaPhieuNhapSach,
                NgayNhap = phieuNhap.NgayTao,
                MaNhaCungCap = phieuNhap.MaNhaCungCap,
                TenNhaCungCap = phieuNhap.NhaCungCap.TenNhaCungCap,
                TenNguoiTao = nguoiDung?.HoTen ?? phieuNhap.NguoiTao,
                TongTien = phieuNhap.TongTien,
                ChiTietSach = new List<ImportOrderDetailItem>(),
                GhiChu = phieuNhap.GhiChu
            };

            // Lấy chi tiết sách, join qua PhienBanSach và Sach
            var chiTiets = await _context.CT_PhieuNhapSach
                .Where(ct => ct.MaPhieuNhapSach == id)
                .Include(ct => ct.PhienBanSach).ThenInclude(pb => pb.Sach)
                .ToListAsync();

            int stt = 1;
            foreach (var ct in chiTiets)
            {
                detail.ChiTietSach.Add(new ImportOrderDetailItem
                {
                    STT = stt++,
                    ISBN = ct.ISBN,
                    TenSach = ct.PhienBanSach.Sach.TenSach,
                    TacGia = "Đang cập nhật...", // Lấy tác giả cần join thêm TacGia_Sach
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGiaNhap
                });
            }

            return Ok(detail);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ImportOrderRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var phieuNhap = new PhieuNhapSach
                {
                    NgayTao = DateTime.Now,
                    MaNhaCungCap = request.MaNhaCungCap,
                    NguoiTao = request.NguoiTao,
                    TongTien = request.ChiTiet.Sum(c => c.SoLuong * c.DonGia),
                    GhiChu = request.GhiChu
                };

                _context.PhieuNhapSach.Add(phieuNhap);
                await _context.SaveChangesAsync(); 

                foreach (var item in request.ChiTiet)
                {
                    // Lưu chi tiết
                    _context.CT_PhieuNhapSach.Add(new CT_PhieuNhapSach
                    {
                        MaPhieuNhapSach = phieuNhap.MaPhieuNhapSach,
                        ISBN = item.ISBN,
                        SoLuong = item.SoLuong,
                        DonGiaNhap = item.DonGia
                    });

                    // CỘNG DỒN TỒN KHO
                    var phienBanSach = await _context.PhienBanSach.FirstOrDefaultAsync(p => p.ISBN == item.ISBN);
                    if (phienBanSach != null)
                    {
                        phienBanSach.TonKho += item.SoLuong;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { Message = "Nhập kho thành công!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Lỗi khi nhập kho: {ex.Message}");
            }
        }

        [HttpGet("search-books")]
        public async Task<ActionResult<IEnumerable<BookSearchResponse>>> SearchBooks([FromQuery] string keyword)
        {
            keyword = keyword.ToLower();
            var books = await _context.PhienBanSach
                .Include(p => p.Sach)
                .Where(p => p.ISBN.ToLower().Contains(keyword) || p.Sach.TenSach.ToLower().Contains(keyword))
                .Select(p => new BookSearchResponse
                {
                    ISBN = p.ISBN,
                    TenSach = p.Sach.TenSach,
                    TacGia = "Nhiều tác giả"
                })
                .Take(20)
                .ToListAsync();

            return Ok(books);
        }


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var phieu = await _context.PhieuNhapSach.FindAsync(id);
        //    if (phieu == null) return NotFound();

        //    var chiTiets = await _context.CT_PhieuNhapSach.Where(ct => ct.MaPhieuNhapSach == id).ToListAsync();

        //    // Trừ lại kho
        //    foreach (var ct in chiTiets)
        //    {
        //        var sach = await _context.PhienBanSach.FindAsync(ct.ISBN);
        //        if (sach != null) sach.TonKho -= ct.SoLuong;
        //    }

        //    _context.PhieuNhapSach.Remove(phieu);
        //    await _context.SaveChangesAsync();
        //    return Ok(new { Message = "Đã xóa phiếu nhập và hoàn lại tồn kho." });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phieu = await _context.PhieuNhapSach.FindAsync(id);
            if (phieu == null) return NotFound();

            var chiTiets = await _context.CT_PhieuNhapSach.Where(ct => ct.MaPhieuNhapSach == id).ToListAsync();

            foreach (var ct in chiTiets)
            {
                var sach = await _context.PhienBanSach.FindAsync(ct.ISBN);
                if (sach != null) sach.TonKho -= ct.SoLuong;
            }

            _context.CT_PhieuNhapSach.RemoveRange(chiTiets);

            _context.PhieuNhapSach.Remove(phieu);

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Đã xóa phiếu nhập và hoàn lại tồn kho." });
        }

        public class UpdateGhiChuRequest
        {
            public string GhiChu { get; set; }
        }

        [HttpPut("{id}/ghichu")]
        public async Task<IActionResult> UpdateGhiChu(int id, [FromBody] UpdateGhiChuRequest request)
        {
            var phieu = await _context.PhieuNhapSach.FindAsync(id);
            if (phieu == null) return NotFound("Không tìm thấy phiếu nhập.");

            phieu.GhiChu = request.GhiChu;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật ghi chú thành công!" });
        }
    }
}
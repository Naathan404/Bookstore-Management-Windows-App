using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuNhapSachController : Controller
    {
        private readonly AppDbContext _context;
        public PhieuNhapSachController(AppDbContext context)
        {
            _context = context;
        }

        private static System.Linq.Expressions.Expression<Func<PhieuNhapSach, ImportResponse>> MapToDTO()
        {
            return p => new ImportResponse
            {
                MaPhieuNhap = p.MaPhieuNhapSach,
                NgayTao = p.NgayTao,
                NguoiTao = p.NguoiTao,
                TenNguoiTao = p.NguoiDung != null ? p.NguoiDung.HoTen : "Không xác định",
                TenNhaCungCap = p.NhaCungCap != null ? p.NhaCungCap.TenNhaCungCap : "Không xác định",
                TongTien = p.TongTien
            };
        }

        // LỌC PHIẾU NHẬP THEO THỜI GIAN, TÁC GIẢ, KHÔNG CẦN LỌC THÌ KHÔNG CẦN THAM SỐ
        // GET: api/PhieuNhapSach?maNCC=5&fromDate=2024-01-01&toDate=2024-12-31
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportResponse>>> GetPhieuNhap(
            [FromQuery] int? maNCC,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var query = _context.PhieuNhapSach.AsQueryable();

            if (maNCC.HasValue)
            {
                query = query.Where(p => p.MaNhaCungCap == maNCC.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.NgayTao.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.NgayTao.Date <= toDate.Value.Date);
            }

            var result = await query
                .Select(MapToDTO()) 
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync(); 

            return Ok(result);
        }

        // LẤY PHIẾU NHẬP THEO ID
        //GET: api/PhieuNhapSach/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportResponse>> GetById(int id)
        {
            var phieuNhap = await _context.PhieuNhapSach
                .Where(p => p.MaPhieuNhapSach == id)
                .Select(MapToDTO())
                .FirstOrDefaultAsync();
            if (phieuNhap == null)
            {
                return NotFound(new { message = "Khong tim thay phieu nhap" });
            }
            return Ok(phieuNhap);
        }

        //SỬA THÔNG TIN PHIẾU NHẬP
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhieuNhapSach(int id, [FromBody] ImportRequest newPhieuNhap)
        {
            var phieuNhap = await _context.PhieuNhapSach.FindAsync(id);
            if (phieuNhap == null) throw new Exception($"Không thấy phiếu nhập {id}");

            phieuNhap.MaNhaCungCap = newPhieuNhap.MaNhaCungCap;
            // phieuNhap.NguoiTao = newPhieuNhap.NguoiTao;
            // phieuNhap.NgayTao = DateTime.Now; //Cập nhật thời gian sửa đổi

            return Ok("Cập nhật phiếu nhập thành công");
        }

        // TẠO PHIẾU NHẬP VÀ CÁC CHI TIẾT PHIẾU NHẬP
        //POST: api/PhieuNhapSach
        [HttpPost()]
        public async Task<IActionResult> CreatePhieuNhap([FromBody] ImportRequest request)
        {
            if (request.ChiTietSach == null || !request.ChiTietSach.Any())
            {
                return BadRequest("Nhập ít nhất 1 loại sách");
            }

            var tsSoLuongNhapToiThieu = await _context.ThamSo.FindAsync("SoLuongNhapToiThieu");
            var tsSoLuongToiDa = await _context.ThamSo.FindAsync("SoLuongToiDaCoTheNhap");

            int soLuongNhapToiThieu = tsSoLuongNhapToiThieu != null ? tsSoLuongNhapToiThieu.GiaTri : 150;
            int soLuongToiDaCoTheNhap = tsSoLuongToiDa != null ? tsSoLuongToiDa.GiaTri : 300;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tạo phiếu nhập
                PhieuNhapSach phieuNhap = new PhieuNhapSach()
                {
                    NgayTao = DateTime.Now,
                    NguoiTao = request.NguoiTao,
                    MaNhaCungCap = request.MaNhaCungCap,
                    TongTien = request.ChiTietSach.Sum(x => x.SoLuong * x.DonGiaNhap)
                };
                _context.PhieuNhapSach.Add(phieuNhap);
                await _context.SaveChangesAsync();

                // Tạo các chi tiết con
                foreach (var item in request.ChiTietSach)
                {
                    if (item.SoLuong < soLuongNhapToiThieu)
                    {
                        throw new Exception($"Số lượng nhập tối thiếu là {soLuongNhapToiThieu}");
                    }

                    CT_PhieuNhapSach chiTiet = new CT_PhieuNhapSach
                    {
                        MaPhieuNhapSach = phieuNhap.MaPhieuNhapSach,
                        ISBN = item.ISBN,
                        SoLuong = item.SoLuong,
                        DonGiaNhap = item.DonGiaNhap
                    };
                    _context.CT_PhieuNhapSach.Add(chiTiet);

                    // Cập nhật tồn kho
                    var sach = await _context.PhienBanSach.FindAsync(item.ISBN);
                    if (sach != null)
                    {
                        if (sach.TonKho > soLuongToiDaCoTheNhap)
                        {
                            throw new Exception($"Chỉ nhập sách có số lượng nhỏ hơn {soLuongToiDaCoTheNhap}");
                        }
                        sach.TonKho += item.SoLuong;
                        _context.PhienBanSach.Update(sach);
                    }
                    else
                    {
                        throw new Exception($"Sach {item.ISBN} khong hop le");
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Nhập sách thành công", maPhieu = phieuNhap.MaPhieuNhapSach });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // MẸO BẮT BUG: Gọi thẳng ex.InnerException để xem SQL Server đang chửi cái gì
                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return StatusCode(500, $"Lỗi: {loiThatSu}");
            }
        }

        //XÓA PHIẾU NHẬP CÁC CÁC CHI TIẾT
        // DELETE: api/PhieuNhapSach/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuNhap(int id)
        {
            var phieuNhap = await _context.PhieuNhapSach.FindAsync(id);

            if (phieuNhap == null)
            {
                return NotFound(new { message = "Không tìm thấy phiếu nhập!" });
            }

            var tsSoLuongTonToiThieu = await _context.ThamSo.FindAsync("SoLuongTonToiThieu");
            int soLuongTonToiThieu = (tsSoLuongTonToiThieu != null) ? tsSoLuongTonToiThieu.GiaTri : 20;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Xử lý các phiếu nhập con
                var danhSachChiTiet = await _context.CT_PhieuNhapSach
                    .Where(ct => ct.MaPhieuNhapSach == id)
                    .ToListAsync();

                foreach (var chiTiet in phieuNhap.CT_PhieuNhapSach)
                {
                    var sach = await _context.PhienBanSach.FindAsync(chiTiet.ISBN);
                    if (sach == null) throw new Exception($"Không tìm thấy sách");
                    if (sach.TonKho - chiTiet.SoLuong < soLuongTonToiThieu)
                    {
                        throw new Exception($"Xóa phiếu nhập ảnh hưởng số lượng tồn tối thiểu của {chiTiet.ISBN}");
                    }
                    sach.TonKho -= chiTiet.SoLuong;
                }

                if (danhSachChiTiet.Any())
                {
                    _context.CT_PhieuNhapSach.RemoveRange(danhSachChiTiet);
                    await _context.SaveChangesAsync(); // Chốt nhịp 1: Xóa toàn bộ con và cập nhật kho
                }

                _context.PhieuNhapSach.Remove(phieuNhap);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { message = "Xóa phiếu nhập thành công!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                var loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Lỗi: {loiThatSu}");
            }
        }

    }
}

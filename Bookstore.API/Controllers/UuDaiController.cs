using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
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
    public class UuDaiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UuDaiController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: Lấy danh sách ưu đãi phối hợp đa bảng chuẩn CSDL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetUuDais()
        {
            try
            {
                var uuDais = await _context.UuDai.ToListAsync();
                var resultList = new List<PromotionDTO>();

                foreach (var u in uuDais)
                {
                    var dto = new PromotionDTO
                    {
                        MaUuDai = u.MaUuDai,
                        NgayTao = u.NgayTao,
                        NguoiTao = u.NguoiTao,
                        // TRỪ 1: Biến đổi từ hệ (1,2,3,4) của DB về hệ (0,1,2,3) cho ComboBox XAML
                        MaLoaiUuDai = u.MaLoaiUuDai - 1,
                        Code = u.Code,
                        TenChuongTrinh = u.TenChuongTrinh,
                        MoTa = u.MoTa,
                        NgayBatDau = u.NgayBatDau,
                        NgayKetThuc = u.NgayKetThuc,
                        SoLuongToiDa = u.SoLuongToiDa,
                        SoLuongDaDung = u.SoLuongDaDung,
                        MaLoaiKhachHang = u.MaLoaiKhachHang,
                        CoTheSuDung = u.CoTheSuDung
                    };

                    // Rẽ nhánh nạp dữ liệu dựa trên hệ số gốc (1,2,3,4)
                    if (u.MaLoaiUuDai == 1) // 1. Giảm giá Hóa đơn
                    {
                        var ct = await _context.CTUD_HoaDon_Giam.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null)
                        {
                            dto.SoTienToiThieu = ct.SoTienToiThieu; dto.SoTienToiDa = ct.SoTienToiDa;
                            dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; dto.GiamToiDa = ct.GiamToiDa;
                        }
                    }
                    else if (u.MaLoaiUuDai == 2) // 2. Tặng quà theo Hóa đơn
                    {
                        var ct = await _context.CTUD_HoaDon_Qua.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null) { dto.SoTienToiThieu = ct.SoTienToiThieu; dto.SoTienToiDa = ct.SoTienToiDa; }
                        var q = await _context.UuDai_SachTang.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (q != null) { dto.ISBNTang = q.ISBN; dto.SoLuongTang = q.SoLuongTang; }
                    }
                    else if (u.MaLoaiUuDai == 3) // 3. Giảm giá trực tiếp trên Sách
                    {
                        var ct = await _context.CTUD_Sach_Giam.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null) { dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; dto.GiamToiDa = ct.GiamToiDa; }
                        var dk = await _context.UuDai_SachDieuKien.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (dk != null) { dto.ISBNDieuKien = dk.ISBN; dto.SoLuongMua = dk.SoLuongMua; }
                    }
                    else if (u.MaLoaiUuDai == 4) // 4. Tặng sách khi mua Sách
                    {
                        var dk = await _context.UuDai_SachDieuKien.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (dk != null) { dto.ISBNDieuKien = dk.ISBN; dto.SoLuongMua = dk.SoLuongMua; }
                        var q = await _context.UuDai_SachTang.FirstOrDefaultAsync(x => x.MaUuDai == u.MaUuDai);
                        if (q != null) { dto.ISBNTang = q.ISBN; dto.SoLuongTang = q.SoLuongTang; }
                    }

                    resultList.Add(dto);
                }

                return Ok(resultList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi xử lý Database: {ex.Message}");
            }
        }

        // 2. POST: Thêm mới phiếu ưu đãi rẽ nhánh lưu đa bảng
        [HttpPost]
        public async Task<IActionResult> CreateUuDai([FromBody] PromotionDTO dto)
        {
            if (dto.SoLuongToiDa < 1 || dto.SoLuongToiDa > 200)
                return BadRequest("Số lượng phát hành ưu đãi bắt buộc nằm trong khoảng từ 1 đến 200 theo QĐ6.1.");

            if (await _context.UuDai.AnyAsync(x => x.Code == dto.Code))
                return BadRequest("Mã Voucher Code này đã tồn tại.");

            var master = new UuDai
            {
                NgayTao = DateTime.Now,
                NguoiTao = "WPF Client",
                // CỘNG 1: Đổi từ hệ số (0,1,2,3) của UI sang hệ số (1,2,3,4) để lưu trữ đúng chuẩn CSDL
                MaLoaiUuDai = dto.MaLoaiUuDai + 1,
                Code = dto.Code.ToUpper(),
                TenChuongTrinh = dto.TenChuongTrinh,
                MoTa = dto.MoTa,
                NgayBatDau = dto.NgayBatDau,
                NgayKetThuc = dto.NgayKetThuc,
                SoLuongToiDa = dto.SoLuongToiDa,
                SoLuongDaDung = 0,
                MaLoaiKhachHang = dto.MaLoaiKhachHang,
                CoTheSuDung = true
            };

            _context.UuDai.Add(master);
            await _context.SaveChangesAsync();

            int id = master.MaUuDai;

            // Kiểm tra theo hệ số lưu DB (1 đến 4)
            if (master.MaLoaiUuDai == 1)
            {
                _context.CTUD_HoaDon_Giam.Add(new CTUD_HoaDon_Giam { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
            }
            else if (master.MaLoaiUuDai == 2)
            {
                _context.CTUD_HoaDon_Qua.Add(new CTUD_HoaDon_Qua { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa });
                if (!string.IsNullOrEmpty(dto.ISBNTang))
                    _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = dto.ISBNTang, SoLuongTang = dto.SoLuongTang });
            }
            else if (master.MaLoaiUuDai == 3)
            {
                _context.CTUD_Sach_Giam.Add(new CTUD_Sach_Giam { MaUuDai = id, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
                if (!string.IsNullOrEmpty(dto.ISBNDieuKien))
                    _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = dto.ISBNDieuKien, SoLuongMua = dto.SoLuongMua });
            }
            else if (master.MaLoaiUuDai == 4)
            {
                _context.CTUD_Sach_Qua.Add(new CTUD_Sach_Qua { MaUuDai = id });
                if (!string.IsNullOrEmpty(dto.ISBNDieuKien))
                    _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = dto.ISBNDieuKien, SoLuongMua = dto.SoLuongMua });
                if (!string.IsNullOrEmpty(dto.ISBNTang))
                    _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = dto.ISBNTang, SoLuongTang = dto.SoLuongTang });
            }

            await _context.SaveChangesAsync();
            return Ok(true);
        }

        // 3. PUT: Cập nhật thông tin phiếu và làm sạch bảng chi tiết cũ
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUuDai(int id, [FromBody] PromotionDTO dto)
        {
            var master = await _context.UuDai.FindAsync(id);
            if (master == null) return NotFound();

            master.TenChuongTrinh = dto.TenChuongTrinh;
            master.NgayBatDau = dto.NgayBatDau;
            master.NgayKetThuc = dto.NgayKetThuc;
            master.SoLuongToiDa = dto.SoLuongToiDa;
            master.MoTa = dto.MoTa;
            master.MaLoaiKhachHang = dto.MaLoaiKhachHang;

            // Clear sạch dữ liệu cũ ở các bảng liên kết
            var hGiam = await _context.CTUD_HoaDon_Giam.Where(x => x.MaUuDai == id).ToListAsync(); _context.CTUD_HoaDon_Giam.RemoveRange(hGiam);
            var hQua = await _context.CTUD_HoaDon_Qua.Where(x => x.MaUuDai == id).ToListAsync(); _context.CTUD_HoaDon_Qua.RemoveRange(hQua);
            var sGiam = await _context.CTUD_Sach_Giam.Where(x => x.MaUuDai == id).ToListAsync(); _context.CTUD_Sach_Giam.RemoveRange(sGiam);
            var sQua = await _context.CTUD_Sach_Qua.Where(x => x.MaUuDai == id).ToListAsync(); _context.CTUD_Sach_Qua.RemoveRange(sQua);
            var dk = await _context.UuDai_SachDieuKien.Where(x => x.MaUuDai == id).ToListAsync(); _context.UuDai_SachDieuKien.RemoveRange(dk);
            var t = await _context.UuDai_SachTang.Where(x => x.MaUuDai == id).ToListAsync(); _context.UuDai_SachTang.RemoveRange(t);

            // Gán lại hệ số MaLoaiUuDai mới (UI + 1)
            master.MaLoaiUuDai = dto.MaLoaiUuDai + 1;

            if (master.MaLoaiUuDai == 1) _context.CTUD_HoaDon_Giam.Add(new CTUD_HoaDon_Giam { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
            else if (master.MaLoaiUuDai == 2)
            {
                _context.CTUD_HoaDon_Qua.Add(new CTUD_HoaDon_Qua { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa });
                if (!string.IsNullOrEmpty(dto.ISBNTang)) _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = dto.ISBNTang, SoLuongTang = dto.SoLuongTang });
            }
            else if (master.MaLoaiUuDai == 3)
            {
                _context.CTUD_Sach_Giam.Add(new CTUD_Sach_Giam { MaUuDai = id, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
                if (!string.IsNullOrEmpty(dto.ISBNDieuKien)) _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = dto.ISBNDieuKien, SoLuongMua = dto.SoLuongMua });
            }
            else if (master.MaLoaiUuDai == 4)
            {
                _context.CTUD_Sach_Qua.Add(new CTUD_Sach_Qua { MaUuDai = id });
                if (!string.IsNullOrEmpty(dto.ISBNDieuKien)) _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = dto.ISBNDieuKien, SoLuongMua = dto.SoLuongMua });
                if (!string.IsNullOrEmpty(dto.ISBNTang)) _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = dto.ISBNTang, SoLuongTang = dto.SoLuongTang });
            }

            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPut("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var u = await _context.UuDai.FindAsync(id);
            if (u == null) return NotFound();
            u.CoTheSuDung = !u.CoTheSuDung;
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var u = await _context.UuDai.FindAsync(id);
            if (u == null) return NotFound();

            if (u.SoLuongDaDung > 0)
                return BadRequest("Ưu đãi này đã phát sinh lịch sử sử dụng trên hóa đơn thực tế. Không thể thực hiện xóa cứng!");

            _context.UuDai.Remove(u);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
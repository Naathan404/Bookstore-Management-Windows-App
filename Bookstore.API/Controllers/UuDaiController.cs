using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTO;
using Bookstore.Share.DTO.Bookstore.Share.DTO;
using Bookstore.Share.Enums;
using Bookstore.WPF.Utils;
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
                var uuDais = await _context.UuDai.Include(u => u.LoaiUuDai).ToListAsync();
                if (!uuDais.Any()) return Ok(new List<PromotionDTO>());

                var uIds = uuDais.Select(u => u.MaUuDai).ToList();

                var hGiamList = await _context.CTUD_HoaDon_Giam.Where(x => uIds.Contains(x.MaUuDai)).ToListAsync();
                var hQuaList = await _context.CTUD_HoaDon_Qua.Where(x => uIds.Contains(x.MaUuDai)).ToListAsync();
                var sGiamList = await _context.CTUD_Sach_Giam.Where(x => uIds.Contains(x.MaUuDai)).ToListAsync();
                var sQuaList = await _context.CTUD_Sach_Qua.Where(x => uIds.Contains(x.MaUuDai)).ToListAsync();

                // ==============================================================
                // MÓC TÊN SÁCH (JOIN VỚI BẢNG PHIENBANSACH VÀ BẢNG SACH GỐC)
                // ==============================================================

                // 1. Sách Điều Kiện
                var dkSachList = await (from dk in _context.UuDai_SachDieuKien
                                        join pb in _context.PhienBanSach on dk.ISBN equals pb.ISBN
                                        join s in _context.Sach on pb.MaSach equals s.MaSach
                                        where uIds.Contains(dk.MaUuDai)
                                        select new
                                        {
                                            dk.MaUuDai,
                                            dk.ISBN,
                                            dk.SoLuongMua,
                                            TenSach = s.TenSach // Móc tên sách ra đây
                                        }).ToListAsync();

                // 2. Sách Tặng
                var tangSachList = await (from tang in _context.UuDai_SachTang
                                          join pb in _context.PhienBanSach on tang.ISBN equals pb.ISBN
                                          join s in _context.Sach on pb.MaSach equals s.MaSach
                                          where uIds.Contains(tang.MaUuDai)
                                          select new
                                          {
                                              tang.MaUuDai,
                                              tang.ISBN,
                                              tang.SoLuongTang,
                                              TenSach = s.TenSach // Móc tên sách ra đây
                                          }).ToListAsync();

                var resultList = new List<PromotionDTO>();

                foreach (var u in uuDais)
                {
                    var dto = new PromotionDTO
                    {
                        MaUuDai = u.MaUuDai,
                        NgayTao = u.NgayTao,
                        NguoiTao = u.NguoiTao,
                        Code = u.Code,
                        TenChuongTrinh = u.TenChuongTrinh,
                        MoTa = u.MoTa,
                        NgayBatDau = u.NgayBatDau,
                        NgayKetThuc = u.NgayKetThuc,
                        SoLuongToiDa = u.SoLuongToiDa,
                        SoLuongDaDung = u.SoLuongDaDung,
                        MaLoaiKhachHang = u.MaLoaiKhachHang,
                        LoaiKhachHangApDung = u.LoaiKhachHang?.TenLoaiKhachHang ?? "Tất cả khách hàng",
                        CoTheSuDung = u.CoTheSuDung,
                        MaLoaiUuDai = u.MaLoaiUuDai,
                        LoaiUuDai = u.LoaiUuDai?.TenLoaiUuDai ?? "Chưa xác định",
                        DanhSachSachDieuKien = new List<SachDieuKienDTO>(),
                        DanhSachSachTang = new List<SachTangDTO>()
                    };

                    // Nạp danh sách Sách Điều Kiện kèm Tên Sách
                    var dks = dkSachList.Where(x => x.MaUuDai == u.MaUuDai).ToList();
                    if (dks.Any())
                        dto.DanhSachSachDieuKien = dks.Select(d => new SachDieuKienDTO
                        {
                            ISBN = d.ISBN,
                            TenSach = d.TenSach, // Map Tên Sách vào DTO
                            SoLuongMua = d.SoLuongMua
                        }).ToList();

                    // Nạp danh sách Sách Tặng kèm Tên Sách
                    var tangs = tangSachList.Where(x => x.MaUuDai == u.MaUuDai).ToList();
                    if (tangs.Any())
                        dto.DanhSachSachTang = tangs.Select(t => new SachTangDTO
                        {
                            ISBN = t.ISBN,
                            TenSach = t.TenSach, // Map Tên Sách vào DTO
                            SoLuongTang = t.SoLuongTang
                        }).ToList();

                    // Nạp thông tin cấu hình giá (SoTien, TiLe...)
                    if (u.MaLoaiUuDai == PromotionType.HoaDonGiam)
                    {
                        var ct = hGiamList.FirstOrDefault(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null) { dto.SoTienToiThieu = ct.SoTienToiThieu; dto.SoTienToiDa = ct.SoTienToiDa; dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; dto.GiamToiDa = ct.GiamToiDa; }
                    }
                    else if (u.MaLoaiUuDai == PromotionType.HoaDonQua)
                    {
                        var ct = hQuaList.FirstOrDefault(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null) { dto.SoTienToiThieu = ct.SoTienToiThieu; dto.SoTienToiDa = ct.SoTienToiDa; }
                    }
                    else if (u.MaLoaiUuDai == PromotionType.SachGiam)
                    {
                        var ct = sGiamList.FirstOrDefault(x => x.MaUuDai == u.MaUuDai);
                        if (ct != null) { dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; dto.GiamToiDa = ct.GiamToiDa; }
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

        /// <summary>
        /// Danh sách ưu đãi khả dụng khi lập hóa đơn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("khadung")]
        public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetUuDaiKhaDung([FromBody] CheckPromotionRequest request)
        {
            var now = DateTime.Now;

            // LỚP LỌC 1 (DƯỚI DATABASE)
            var activePromos = await _context.UuDai
                .Where(u => u.CoTheSuDung
                         && u.NgayBatDau <= now
                         && u.NgayKetThuc >= now
                         && u.SoLuongDaDung < u.SoLuongToiDa
                         && (u.MaLoaiKhachHang == null || u.MaLoaiKhachHang == request.MaLoaiKhachHang))
                .ToListAsync();

            if (!activePromos.Any())
                return Ok(new List<PromotionDTO>());

            var promoIds = activePromos.Select(p => p.MaUuDai).ToList();

            var hdGiamList = await _context.CTUD_HoaDon_Giam.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();
            var hdQuaList = await _context.CTUD_HoaDon_Qua.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();
            var sachGiamList = await _context.CTUD_Sach_Giam.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();
            var sachQuaList = await _context.CTUD_Sach_Qua.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();
            var dkSachList = await _context.UuDai_SachDieuKien.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();
            var tangSachList = await _context.UuDai_SachTang.Where(x => promoIds.Contains(x.MaUuDai)).ToListAsync();

            // LỚP LỌC 2 (TRÊN RAM)
            var eligiblePromos = new List<PromotionDTO>();

            // SỬA TẠI ĐÂY: Ép Trim() cho toàn bộ ISBN từ giỏ hàng gửi lên
            var cartDict = request.CartItems.ToDictionary(c => c.ISBN.Trim(), c => c.SoLuong);

            foreach (var promo in activePromos)
            {
                var dto = new PromotionDTO
                {
                    MaUuDai = promo.MaUuDai,
                    NgayTao = promo.NgayTao,
                    NguoiTao = promo.NguoiTao,
                    Code = promo.Code,
                    TenChuongTrinh = promo.TenChuongTrinh,
                    MoTa = promo.MoTa,
                    NgayBatDau = promo.NgayBatDau,
                    NgayKetThuc = promo.NgayKetThuc,
                    SoLuongToiDa = promo.SoLuongToiDa,
                    SoLuongDaDung = promo.SoLuongDaDung,
                    MaLoaiKhachHang = promo.MaLoaiKhachHang,
                    LoaiKhachHangApDung = promo.LoaiKhachHang?.TenLoaiKhachHang ?? "Tất cả khách hàng",
                    CoTheSuDung = promo.CoTheSuDung,
                    MaLoaiUuDai = promo.MaLoaiUuDai,
                    DanhSachSachDieuKien = new List<SachDieuKienDTO>(),
                    DanhSachSachTang = new List<SachTangDTO>()
                };

                bool isEligible = false;

                if (promo.MaLoaiUuDai == PromotionType.HoaDonGiam)
                {
                    var ct = hdGiamList.FirstOrDefault(x => x.MaUuDai == promo.MaUuDai);
                    if (ct != null && request.TamTinh >= ct.SoTienToiThieu)
                    {
                        isEligible = true;
                        dto.SoTienToiThieu = ct.SoTienToiThieu; dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; //... map phần còn lại
                    }
                }
                else if (promo.MaLoaiUuDai == PromotionType.HoaDonQua)
                {
                    var ct = hdQuaList.FirstOrDefault(x => x.MaUuDai == promo.MaUuDai);
                    var tangs = tangSachList.Where(x => x.MaUuDai == promo.MaUuDai).ToList();

                    if (ct != null && tangs.Any() && request.TamTinh >= ct.SoTienToiThieu)
                    {
                        isEligible = true;
                        dto.SoTienToiThieu = ct.SoTienToiThieu;
                        dto.DanhSachSachTang = tangs.Select(t => new SachTangDTO { ISBN = t.ISBN.Trim(), SoLuongTang = t.SoLuongTang }).ToList();
                    }
                }
                else if (promo.MaLoaiUuDai == PromotionType.SachGiam)
                {
                    var ct = sachGiamList.FirstOrDefault(x => x.MaUuDai == promo.MaUuDai);
                    var dks = dkSachList.Where(x => x.MaUuDai == promo.MaUuDai).ToList();

                    if (ct != null && dks.Any())
                    {
                        // KIỂM TRA ĐIỀU KIỆN COMBO: Phải thỏa mãn TẤT CẢ các sách yêu cầu
                        bool isMatchAllConditions = dks.All(dk => cartDict.TryGetValue(dk.ISBN.Trim(), out int qty) && qty >= dk.SoLuongMua);

                        if (isMatchAllConditions)
                        {
                            isEligible = true;
                            dto.SoTienGiam = ct.SoTienGiam; dto.TiLeGiam = ct.TiLeGiam; dto.GiamToiDa = ct.GiamToiDa;
                            dto.DanhSachSachDieuKien = dks.Select(d => new SachDieuKienDTO { ISBN = d.ISBN.Trim(), SoLuongMua = d.SoLuongMua }).ToList();
                        }
                    }
                }
                else if (promo.MaLoaiUuDai == PromotionType.SachQua)
                {
                    var ct = sachQuaList.FirstOrDefault(x => x.MaUuDai == promo.MaUuDai);
                    var dks = dkSachList.Where(x => x.MaUuDai == promo.MaUuDai).ToList();
                    var tangs = tangSachList.Where(x => x.MaUuDai == promo.MaUuDai).ToList();

                    if (ct != null && dks.Any() && tangs.Any())
                    {
                        // KIỂM TRA ĐIỀU KIỆN COMBO TẶNG
                        bool isMatchAllConditions = dks.All(dk => cartDict.TryGetValue(dk.ISBN.Trim(), out int qty) && qty >= dk.SoLuongMua);

                        if (isMatchAllConditions)
                        {
                            isEligible = true;
                            dto.DanhSachSachDieuKien = dks.Select(d => new SachDieuKienDTO { ISBN = d.ISBN.Trim(), SoLuongMua = d.SoLuongMua }).ToList();
                            dto.DanhSachSachTang = tangs.Select(t => new SachTangDTO { ISBN = t.ISBN.Trim(), SoLuongTang = t.SoLuongTang }).ToList();
                        }
                    }
                }

                if (isEligible) eligiblePromos.Add(dto);
            }

            return Ok(eligiblePromos);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUuDai([FromBody] PromotionDTO dto)
        {
            if (dto.SoLuongToiDa < 1 || dto.SoLuongToiDa > 200)
                return BadRequest("Số lượng phát hành ưu đãi bắt buộc nằm trong khoảng từ 1 đến 200 theo QĐ6.1.");

            if (await _context.UuDai.AnyAsync(x => x.Code == dto.Code))
                return BadRequest("Mã Voucher Code này đã tồn tại.");

            // Bắt đầu Transaction (Khuyên dùng vì insert vào nhiều bảng)
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var master = new UuDai
                {
                    NgayTao = DateTime.Now,
                    NguoiTao = dto.NguoiTao,
                    MaLoaiUuDai = dto.MaLoaiUuDai,
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

                // XỬ LÝ THEO TỪNG LOẠI ƯU ĐÃI
                if (master.MaLoaiUuDai == PromotionType.HoaDonGiam)
                {
                    _context.CTUD_HoaDon_Giam.Add(new CTUD_HoaDon_Giam { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
                }
                else if (master.MaLoaiUuDai == PromotionType.HoaDonQua)
                {
                    _context.CTUD_HoaDon_Qua.Add(new CTUD_HoaDon_Qua { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa });

                    // Xử lý List Sách Tặng
                    if (dto.DanhSachSachTang != null && dto.DanhSachSachTang.Any())
                    {
                        foreach (var item in dto.DanhSachSachTang)
                        {
                            _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = item.ISBN, SoLuongTang = item.SoLuongTang });
                        }
                    }
                }
                else if (master.MaLoaiUuDai == PromotionType.SachGiam)
                {
                    _context.CTUD_Sach_Giam.Add(new CTUD_Sach_Giam { MaUuDai = id, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });

                    // Xử lý List Sách Điều Kiện
                    if (dto.DanhSachSachDieuKien != null && dto.DanhSachSachDieuKien.Any())
                    {
                        foreach (var item in dto.DanhSachSachDieuKien)
                        {
                            _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = item.ISBN, SoLuongMua = item.SoLuongMua });
                        }
                    }
                }
                else if (master.MaLoaiUuDai == PromotionType.SachQua)
                {
                    _context.CTUD_Sach_Qua.Add(new CTUD_Sach_Qua { MaUuDai = id });

                    // Xử lý List Sách Điều Kiện
                    if (dto.DanhSachSachDieuKien != null && dto.DanhSachSachDieuKien.Any())
                    {
                        foreach (var item in dto.DanhSachSachDieuKien)
                        {
                            _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = item.ISBN, SoLuongMua = item.SoLuongMua });
                        }
                    }

                    // Xử lý List Sách Tặng
                    if (dto.DanhSachSachTang != null && dto.DanhSachSachTang.Any())
                    {
                        foreach (var item in dto.DanhSachSachTang)
                        {
                            _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = item.ISBN, SoLuongTang = item.SoLuongTang });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // 3. PUT: Cập nhật thông tin phiếu và làm sạch bảng chi tiết cũ
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUuDai(int id, [FromBody] PromotionDTO dto)
        {
            // Vì update ảnh hưởng nhiều bảng, ta NÊN bọc trong Transaction để an toàn
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                master.MaLoaiUuDai = dto.MaLoaiUuDai;

                // CHÈN LẠI DỮ LIỆU MỚI BẰNG LIST
                if (master.MaLoaiUuDai == PromotionType.HoaDonGiam)
                {
                    _context.CTUD_HoaDon_Giam.Add(new CTUD_HoaDon_Giam { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
                }
                else if (master.MaLoaiUuDai == PromotionType.HoaDonQua)
                {
                    _context.CTUD_HoaDon_Qua.Add(new CTUD_HoaDon_Qua { MaUuDai = id, SoTienToiThieu = dto.SoTienToiThieu, SoTienToiDa = dto.SoTienToiDa });
                    if (dto.DanhSachSachTang != null)
                        foreach (var item in dto.DanhSachSachTang)
                            _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = item.ISBN, SoLuongTang = item.SoLuongTang });
                }
                else if (master.MaLoaiUuDai == PromotionType.SachGiam)
                {
                    _context.CTUD_Sach_Giam.Add(new CTUD_Sach_Giam { MaUuDai = id, SoTienGiam = dto.SoTienGiam, TiLeGiam = dto.TiLeGiam, GiamToiDa = dto.GiamToiDa });
                    if (dto.DanhSachSachDieuKien != null)
                        foreach (var item in dto.DanhSachSachDieuKien)
                            _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = item.ISBN, SoLuongMua = item.SoLuongMua });
                }
                else if (master.MaLoaiUuDai == PromotionType.SachQua)
                {
                    _context.CTUD_Sach_Qua.Add(new CTUD_Sach_Qua { MaUuDai = id });
                    if (dto.DanhSachSachDieuKien != null)
                        foreach (var item in dto.DanhSachSachDieuKien)
                            _context.UuDai_SachDieuKien.Add(new UuDai_SachDieuKien { MaUuDai = id, ISBN = item.ISBN, SoLuongMua = item.SoLuongMua });
                    if (dto.DanhSachSachTang != null)
                        foreach (var item in dto.DanhSachSachTang)
                            _context.UuDai_SachTang.Add(new UuDai_SachTang { MaUuDai = id, ISBN = item.ISBN, SoLuongTang = item.SoLuongTang });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi server khi cập nhật: {ex.Message}");
            }
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
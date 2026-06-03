using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.API.Utils;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly AppDbContext _context;

        private string DefaultPasswordHash =
            "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676";

        public NguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/NguoiDung
        // Trả về toàn bộ danh sách tài khoản kèm tên nhóm.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.NguoiDung
                .Include(u => u.NhomNguoiDung)
                .OrderBy(u => u.MaNhomNguoiDung)
                .ThenBy(u => u.HoTen)
                .Select(u => new AccountDTO
                {
                    Username = u.TenDangNhap,
                    MaNhomNguoiDung = u.MaNhomNguoiDung,
                    HoTen = u.HoTen,
                    GioiTinh = u.GioiTinh,
                    ChucVu = u.ChucVu,
                    Email = u.Email,
                    DangLamViec = u.DangLamViec,
                    NgaySinh = u.NgaySinh,
                    NgayVaoLam = u.NgayVaoLam,
                    RoleName = u.NhomNguoiDung.TenNhomNguoiDung
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET api/NguoiDung/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var u = await _context.NguoiDung
                .Include(x => x.NhomNguoiDung)
                .FirstOrDefaultAsync(x => x.TenDangNhap == username);

            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản." });

            return Ok(new AccountDTO
            {
                Username = u.TenDangNhap,
                MaNhomNguoiDung = u.MaNhomNguoiDung,
                HoTen = u.HoTen,
                GioiTinh = u.GioiTinh,
                ChucVu = u.ChucVu,
                Email = u.Email,
                DangLamViec = u.DangLamViec,
                NgaySinh = u.NgaySinh,
                NgayVaoLam = u.NgayVaoLam,
                RoleName = u.NhomNguoiDung.TenNhomNguoiDung
            });
        }

        // POST api/NguoiDung
        // Tạo tài khoản mới, mật khẩu mặc định = "123456"
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest(new { message = "Tên đăng nhập không được để trống." });

            // Kiểm tra username đã tồn tại chưa
            bool exists = await _context.NguoiDung
                .AnyAsync(u => u.TenDangNhap == dto.Username);

            if (exists)
                return Conflict(new { message = $"Tên đăng nhập '{dto.Username}' đã tồn tại." });

            // Kiểm tra nhóm hợp lệ
            bool roleExists = await _context.NhomNguoiDung
                .AnyAsync(r => r.MaNhomNguoiDung == dto.MaNhomNguoiDung);

            if (!roleExists)
                return BadRequest(new { message = "Nhóm người dùng không hợp lệ." });

            var newUser = new NguoiDung
            {
                TenDangNhap = dto.Username.Trim(),
                MatKhau = DefaultPasswordHash,
                MaNhomNguoiDung = dto.MaNhomNguoiDung,
                HoTen = dto.HoTen.Trim(),
                GioiTinh = dto.GioiTinh,
                ChucVu = dto.ChucVu,
                Email = dto.Email,
                DangLamViec = dto.DangLamViec,
                NgaySinh = dto.NgaySinh,
                NgayVaoLam = dto.NgayVaoLam
            };

            _context.NguoiDung.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tạo tài khoản thành công." });
        }

        // PUT api/NguoiDung/{username}
        // Cập nhật thông tin cá nhân + nhóm. Không đổi mật khẩu ở đây.
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, [FromBody] UpdateAccountDTO dto)
        {
            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            // Kiểm tra nhóm hợp lệ
            bool roleExists = await _context.NhomNguoiDung
                .AnyAsync(r => r.MaNhomNguoiDung == dto.MaNhomNguoiDung);

            if (!roleExists)
                return BadRequest(new { message = "Nhóm người dùng không hợp lệ." });

            user.MaNhomNguoiDung = dto.MaNhomNguoiDung;
            user.HoTen = dto.HoTen.Trim();
            user.GioiTinh = dto.GioiTinh;
            user.ChucVu = dto.ChucVu;
            user.Email = dto.Email;
            user.DangLamViec = dto.DangLamViec;
            user.NgaySinh = dto.NgaySinh;
            user.NgayVaoLam = dto.NgayVaoLam;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật tài khoản thành công." });
        }

        // POST api/NguoiDung/{username}/reset-password
        // Reset về mật khẩu mặc định "123456"
        [HttpPost("{username}/reset-password")]
        public async Task<IActionResult> ResetPassword(string username)
        {
            var newPass = await _context.ThamSo.Where(x => x.TenThamSo == "MatKhauMacDinh").Select(x => x.GiaTri).FirstOrDefaultAsync();
            string newPassString = newPass > - 0 ? newPass.ToString() : "123456";
            DefaultPasswordHash = HashHelper.SHA256_Encode(HashHelper.Base64_Encode(newPassString));
            
            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            user.MatKhau = DefaultPasswordHash;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt lại mật khẩu thành công." });
        }

        // DELETE api/NguoiDung/{username}
        // Xóa tài khoản — chặn nếu là tài khoản duy nhất của nhóm ADMIN
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản." });

            // Không cho xóa nếu đây là admin cuối cùng
            bool isLastAdmin = user.MaNhomNguoiDung == 1 &&
                               await _context.NguoiDung.CountAsync(u => u.MaNhomNguoiDung == 1) == 1;

            if (isLastAdmin)
                return BadRequest(new { message = "Không thể xóa tài khoản ADMIN duy nhất của hệ thống!" });

            _context.NguoiDung.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa tài khoản thành công." });
        }
    }
}
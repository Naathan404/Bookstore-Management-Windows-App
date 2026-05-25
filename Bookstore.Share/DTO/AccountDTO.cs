namespace Bookstore.Share.DTOs
{
    /// <summary>
    /// Dùng cho GET danh sách + form Thêm/Sửa tài khoản ở WPF
    /// </summary>
    public class AccountDTO
    {
        public int STT { get; set; }

        // Thông tin đăng nhập
        public string Username { get; set; } = "";
        public int MaNhomNguoiDung { get; set; }

        // Thông tin cá nhân
        public string HoTen { get; set; } = "";
        public string GioiTinh { get; set; } = "Nam";
        public string ChucVu { get; set; } = "";
        public string Email { get; set; } = "";
        public bool DangLamViec { get; set; } = true;
        public DateOnly NgaySinh { get; set; } = new DateOnly(2000, 1, 1);
        public DateOnly NgayVaoLam { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string RoleName { get; set; } = "";

        public string TrangThaiText => DangLamViec ? "Đang làm" : "Đã nghỉ";
        public string TrangThaiColor => DangLamViec ? "#05CD99" : "#EE5D50";
    }

    /// <summary> tạo tài khoản mới</summary>
    public class CreateAccountDTO
    {
        public string Username { get; set; } = "";
        public int MaNhomNguoiDung { get; set; }
        public string HoTen { get; set; } = "";
        public string GioiTinh { get; set; } = "Nam";
        public string ChucVu { get; set; } = "";
        public string Email { get; set; } = "";
        public bool DangLamViec { get; set; } = true;
        public DateOnly NgaySinh { get; set; } = new DateOnly(2000, 1, 1);
        public DateOnly NgayVaoLam { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }

    /// <summary>Payload khi sửa thông tin tài khoản</summary>
    public class UpdateAccountDTO
    {
        public int MaNhomNguoiDung { get; set; }
        public string HoTen { get; set; } = "";
        public string GioiTinh { get; set; } = "Nam";
        public string ChucVu { get; set; } = "";
        public string Email { get; set; } = "";
        public bool DangLamViec { get; set; } = true;
        public DateOnly NgaySinh { get; set; }
        public DateOnly NgayVaoLam { get; set; }
    }

    // ================================================================
    // NHOM NGUOI DUNG (Nhóm / Role)

    public class NhomNguoiDungDTO
    {
        public int MaNhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; } = "";
    }

    public class CreateNhomNguoiDungDTO
    {
        public string TenNhomNguoiDung { get; set; } = "";
    }

    // ================================================================
    // PHAN QUYEN (Phân quyền chức năng)

    /// <summary>
    /// /// IsGranted = true nghĩa là nhóm này được phép truy cập màn hình đó.
    /// </summary>
    public class ScreenPermissionDTO
    {
        public int MaChucNang { get; set; }
        public string TenChucNang { get; set; } = "";
        public string TenManHinh { get; set; } = "";
        public bool IsGranted { get; set; }
    }

    /// <summary>Payload gửi lên khi bấm Lưu phân quyền cho 1 nhóm.</summary>
    public class UpdatePermissionsDTO
    {
        public int MaNhomNguoiDung { get; set; }

        /// Danh sách MaChucNang được cấp quyền (checked). 
        public List<int> GrantedChucNangIds { get; set; } = new();
    }
}
using System;

namespace Bookstore.Share.DTO
{
    using global::Bookstore.Share.Enums;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    namespace Bookstore.Share.DTO
    {
        public class PromotionDTO : INotifyPropertyChanged
        {
            public int MaUuDai { get; set; }
            public DateTime NgayTao { get; set; } = DateTime.Now;
            public string NguoiTao { get; set; } = string.Empty;
            public PromotionType MaLoaiUuDai { get; set; }
            public string LoaiUuDai { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public string TenChuongTrinh { get; set; } = string.Empty;
            public string MoTa { get; set; } = string.Empty;
            public DateTime NgayBatDau { get; set; } = DateTime.Today;
            public DateTime NgayKetThuc { get; set; } = DateTime.Today.AddDays(30);
            public int SoLuongToiDa { get; set; } = 1;
            public int SoLuongDaDung { get; set; }
            public int? MaLoaiKhachHang { get; set; }
            public string LoaiKhachHangApDung { get; set; } = string.Empty;
            public bool CoTheSuDung { get; set; } = true;

            public List<SachDieuKienDTO> DanhSachSachDieuKien { get; set; } = new();
            public List<SachTangDTO> DanhSachSachTang { get; set; } = new();

            public decimal SoTienToiThieu { get; set; } = 0;
            public decimal SoTienToiDa { get; set; } = 10000000;

            public decimal SoTienGiam { get; set; }
            public double TiLeGiam { get; set; }
            public decimal GiamToiDa { get; set; }

            public decimal SoTienGiamThucTe { get; set; }

            // ========================================================================
            // UI BÁN HÀNG
            // ========================================================================
            private string _mucGiamDisplay = string.Empty;
            public string MucGiamDisplay
            {
                get => _mucGiamDisplay;
                set
                {
                    if (_mucGiamDisplay != value)
                    {
                        _mucGiamDisplay = value;
                        OnPropertyChanged(); // Phát tín hiệu để UI WPF tự động vẽ lại chữ
                    }
                }
            }

            // ========================================================================
            // CÁC THUỘC TÍNH READ-ONLY CHO GIAO DIỆN QUẢN LÝ
            // ========================================================================
            public int STT { get; set; }
            public DateTime ThoiGianBatDau => NgayBatDau;
            public DateTime ThoiGianKetThuc => NgayKetThuc;
            public string SoLuongToiDaDisplay => SoLuongToiDa == 0 ? "Vô hạn" : SoLuongToiDa.ToString();
          
            public string TrangThai
            {
                get
                {
                    if (!CoTheSuDung) return "Tạm dừng";
                    if (DateTime.Now.Date > NgayKetThuc.Date) return "Hết hạn";
                    if (DateTime.Now.Date < NgayBatDau.Date) return "Chưa áp dụng";
                    return "Đang áp dụng";
                }
            }

            public string TrangThaiBackground
            {
                get
                {
                    if (!CoTheSuDung) return "#FEF3C7";
                    if (DateTime.Now.Date > NgayKetThuc.Date) return "#FEE2E2";
                    if (DateTime.Now.Date < NgayBatDau.Date) return "#DBEAFE";
                    return "#D1FAE5";
                }
            }

            public string TrangThaiForeground
            {
                get
                {
                    if (!CoTheSuDung) return "#B45309";
                    if (DateTime.Now.Date > NgayKetThuc.Date) return "#B91C1C";
                    if (DateTime.Now.Date < NgayBatDau.Date) return "#1E40AF";
                    return "#047857";
                }
            }

            public string ToggleButtonBackground => CoTheSuDung ? "#FEF2F2" : "#ECFDF5";
            public string ToggleButtonForeground => CoTheSuDung ? "#EF4444" : "#10B981";
            public string ToggleButtonIcon => CoTheSuDung ? "Pause" : "Play";

            // ========================================================================
            // CƠ CHẾ NOTIFY PROPERTY CHANGED (MVVM)
            // ========================================================================
            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CheckPromotionRequest
    {
        public decimal TamTinh { get; set; }
        public int? MaLoaiKhachHang { get; set; } // 1: Vãng lai, 2: Thành viên (Tùy bạn quy ước)
        public List<CartItemRequest> CartItems { get; set; } = new();
    }

    public class CartItemRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public int SoLuong { get; set; }
    }

    public class PromotionTypeResponse
    {
        public PromotionType MaLoaiUuDai { get; set; }
        public string TenLoaiUuDai { get; set; } = string.Empty;
        public int ApDungToiDa { get; set; }
    }

    public class SachDieuKienDTO
    {
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public int SoLuongMua { get; set; }
    }

    public class SachTangDTO
    {
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public int SoLuongTang { get; set; }
    }
}
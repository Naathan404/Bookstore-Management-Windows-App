using System;

namespace Bookstore.Share.DTO
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    namespace Bookstore.Share.DTO
    {
        // Bổ sung giao diện INotifyPropertyChanged để UI tự cập nhật khi dữ liệu thay đổi
        public class PromotionDTO : INotifyPropertyChanged
        {
            public int MaUuDai { get; set; }
            public DateTime NgayTao { get; set; } = DateTime.Now;
            public string NguoiTao { get; set; } = string.Empty;
            public int MaLoaiUuDai { get; set; } // 0: HD_Giam, 1: HD_Qua, 2: Sach_Giam, 3: Sach_Qua 
            public string Code { get; set; } = string.Empty;
            public string TenChuongTrinh { get; set; } = string.Empty;
            public string MoTa { get; set; } = string.Empty;
            public DateTime NgayBatDau { get; set; } = DateTime.Today;
            public DateTime NgayKetThuc { get; set; } = DateTime.Today.AddDays(30);
            public int SoLuongToiDa { get; set; } = 1;
            public int SoLuongDaDung { get; set; }
            public int? MaLoaiKhachHang { get; set; }
            public bool CoTheSuDung { get; set; } = true;

            public decimal SoTienToiThieu { get; set; } = 0;
            public decimal SoTienToiDa { get; set; } = 10000000;

            public decimal SoTienGiam { get; set; }
            public double TiLeGiam { get; set; }
            public decimal GiamToiDa { get; set; }

            public string? ISBNDieuKien { get; set; }
            public int SoLuongMua { get; set; }

            public string? ISBNTang { get; set; }
            public int SoLuongTang { get; set; }
            public int STT { get; set; }

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
            public DateTime ThoiGianBatDau => NgayBatDau;
            public DateTime ThoiGianKetThuc => NgayKetThuc;
            public string SoLuongToiDaDisplay => SoLuongToiDa == 0 ? "Vô hạn" : SoLuongToiDa.ToString();

            public string LoaiUuDai => MaLoaiUuDai switch
            {
                0 => "Giảm giá / Tổng hóa đơn",
                1 => "Tặng quà / Tổng hóa đơn",
                2 => "Giảm giá / Đầu sách",
                3 => "Tặng quà / Đầu sách",
                _ => "Không xác định"
            };

            // Đã đồng bộ lại khớp với logic Code
            public string LoaiKhachHangApDung => MaLoaiKhachHang switch
            {
                0 => "Tất cả khách hàng",
                1 => "Khách vãng lai",
                2 => "Thành viên",
                _ => "Không xác định"
            };

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
}
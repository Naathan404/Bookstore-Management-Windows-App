using System;

namespace Bookstore.Share.DTO
{
    public class PromotionDTO
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
        public int MaLoaiKhachHang { get; set; }
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
        public DateTime ThoiGianBatDau => NgayBatDau;
        public DateTime ThoiGianKetThuc => NgayKetThuc;
        public string SoLuongToiDaDisplay => SoLuongToiDa == 0 ? "Vô hạn" : SoLuongToiDa.ToString();

        public string LoaiUuDai => MaLoaiUuDai switch
        {
            0 => "Giảm giá/ Tổng hóa đơn",
            1 => "Tặng quà / Tổng hóa đơn",
            2 => "Giảm giá / Đầu sách",
            3 => "Tặng quà / Đầu sách",
        };

        public string LoaiKhachHangApDung => MaLoaiKhachHang switch
        {
            1 => "Cá nhân",
            2 => "Doanh nghiệp",
        };

        // Logic hiển thị trạng thái động
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
                if (!CoTheSuDung) return "#FEF3C7"; // Vàng nhạt (Tạm dừng)
                if (DateTime.Now.Date > NgayKetThuc.Date) return "#FEE2E2"; // Đỏ nhạt (Hết hạn)
                if (DateTime.Now.Date < NgayBatDau.Date) return "#DBEAFE"; // Xanh dương nhạt (Chưa áp dụng)
                return "#D1FAE5"; // Xanh lá nhạt (Đang áp dụng)
            }
        }

        public string TrangThaiForeground
        {
            get
            {
                if (!CoTheSuDung) return "#B45309"; // Vàng cam đậm
                if (DateTime.Now.Date > NgayKetThuc.Date) return "#B91C1C"; // Đỏ đậm
                if (DateTime.Now.Date < NgayBatDau.Date) return "#1E40AF"; // Xanh dương đậm
                return "#047857"; // Xanh lá đậm
            }
        }

        public string ToggleButtonBackground => CoTheSuDung ? "#FEF2F2" : "#ECFDF5";
        public string ToggleButtonForeground => CoTheSuDung ? "#EF4444" : "#10B981";
        public string ToggleButtonIcon => CoTheSuDung ? "Pause" : "Play";
    }
}
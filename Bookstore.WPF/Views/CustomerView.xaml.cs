using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views
{
    public partial class CustomerView : UserControl
    {
        private ObservableCollection<KhachHangItem> DanhSachKhachHang = new();
        private ObservableCollection<KhachHangItem> DanhSachKhachHangGoc = new();
        private KhachHangItem? KhachHangDangChon;
        private bool DangSua = false;

        private int TrangHienTai = 1;
        private int SoDongTrenTrang = 10;
        private int TongSoTrang = 1;

        private string KieuTimKiemHienTai = "Tên khách hàng";
        private string LocLoaiKhachHienTai = "Tất cả";
        private string LocCongNoHienTai = "Tất cả";

        public CustomerView()
        {
            InitializeComponent();
            Loaded += (s, e) => KhoiTaoDuLieu();
        }

        private void KhoiTaoDuLieu()
        {
            DanhSachKhachHangGoc = new ObservableCollection<KhachHangItem>
            {
                new() { MaKhachHang = "KH001", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0912345678", Email = "vana@gmail.com", DiaChi = "Hà Nội", CongNo = 1500000, LoaiKhach = "Cá nhân", GioiTinh = "Nam", NgaySinh = new DateTime(1990, 5, 15) },
                new() { MaKhachHang = "KH002", TenKhachHang = "Trần Thị B", SoDienThoai = "0987654321", Email = "thib@gmail.com", DiaChi = "TP.HCM", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1995, 8, 20) },
                new() { MaKhachHang = "KH003", TenKhachHang = "Công ty ABC", SoDienThoai = "0977777777", Email = "abc@gmail.com", DiaChi = "Đà Nẵng", CongNo = 3500000, LoaiKhach = "Doanh nghiệp", GioiTinh = "Khác", NgaySinh = new DateTime(2015, 1, 1) },
                new() { MaKhachHang = "KH004", TenKhachHang = "Phạm Thị D", SoDienThoai = "0966666666", Email = "thid@gmail.com", DiaChi = "Huế", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1992, 3, 25) },
                new() { MaKhachHang = "KH005", TenKhachHang = "Hoàng Văn E", SoDienThoai = "0955555555", Email = "vane@gmail.com", DiaChi = "Cần Thơ", CongNo = 500000, LoaiKhach = "Cá nhân", GioiTinh = "Nam", NgaySinh = new DateTime(1998, 7, 12) },
                new() { MaKhachHang = "KH006", TenKhachHang = "Ngô Thị F", SoDienThoai = "0944444444", Email = "thif@gmail.com", DiaChi = "Hải Phòng", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1993, 11, 8) },
                new() { MaKhachHang = "KH007", TenKhachHang = "Đỗ Văn G", SoDienThoai = "0933333333", Email = "vang@gmail.com", DiaChi = "Nha Trang", CongNo = 2000000, LoaiKhach = "Cá nhân", GioiTinh = "Nam", NgaySinh = new DateTime(1985, 1, 30) },
                new() { MaKhachHang = "KH008", TenKhachHang = "Vũ Thị H", SoDienThoai = "0922222222", Email = "thih@gmail.com", DiaChi = "Vũng Tàu", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1997, 6, 18) },
                new() { MaKhachHang = "KH009", TenKhachHang = "Bùi Văn I", SoDienThoai = "0911111111", Email = "vani@gmail.com", DiaChi = "Quảng Ninh", CongNo = 800000, LoaiKhach = "Doanh nghiệp", GioiTinh = "Nam", NgaySinh = new DateTime(1991, 9, 22) },
                new() { MaKhachHang = "KH010", TenKhachHang = "Đặng Thị K", SoDienThoai = "0900000000", Email = "thik@gmail.com", DiaChi = "Bình Dương", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1996, 4, 5) },
            };
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var searchText = txtTimKiem?.Text?.ToLower().Trim() ?? "";
            var filtered = DanhSachKhachHangGoc.AsEnumerable();

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(kh => KieuTimKiemHienTai switch
                {
                    "Số điện thoại" => kh.SoDienThoai?.Contains(searchText) ?? false,
                    "Email" => kh.Email?.ToLower().Contains(searchText) ?? false,
                    _ => kh.TenKhachHang?.ToLower().Contains(searchText) ?? false
                });
            }

            if (LocLoaiKhachHienTai != "Tất cả")
                filtered = filtered.Where(kh => kh.LoaiKhach == LocLoaiKhachHienTai);

            if (LocCongNoHienTai != "Tất cả")
            {
                filtered = filtered.Where(kh => LocCongNoHienTai switch
                {
                    "Không nợ" => kh.CongNo == 0,
                    "Dưới 1 triệu" => kh.CongNo > 0 && kh.CongNo < 1000000,
                    "1 - 2 triệu" => kh.CongNo >= 1000000 && kh.CongNo < 2000000,
                    "2 - 3 triệu" => kh.CongNo >= 2000000 && kh.CongNo < 3000000,
                    "3 - 4 triệu" => kh.CongNo >= 3000000 && kh.CongNo < 4000000,
                    "4 - 5 triệu" => kh.CongNo >= 4000000 && kh.CongNo < 5000000,
                    "Trên 5 triệu" => kh.CongNo >= 5000000,
                    _ => true
                });
            }

            DanhSachKhachHang = new ObservableCollection<KhachHangItem>(filtered);
            TongSoTrang = Math.Max(1, (int)Math.Ceiling(DanhSachKhachHang.Count / (double)SoDongTrenTrang));
            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            HienThiTrangHienTai();
            CapNhatPhanTrang();
            txtTongBanGhi.Text = DanhSachKhachHangGoc.Count.ToString();
        }

        private void HienThiTrangHienTai()
        {
            dgDanhSachKhach.ItemsSource = DanhSachKhachHang
                .Skip((TrangHienTai - 1) * SoDongTrenTrang)
                .Take(SoDongTrenTrang)
                .ToList();
        }

        private void CapNhatPhanTrang()
        {
            btnTrang1.Visibility = Visibility.Collapsed;
            btnTrang2.Visibility = Visibility.Collapsed;
            btnTrang3.Visibility = Visibility.Collapsed;
            txtEllipsis.Visibility = Visibility.Collapsed;
            btnTrangCuoiSo.Visibility = Visibility.Collapsed;

            if (DanhSachKhachHang.Count == 0)
            {
                if (txtPhanTrangInfo != null) txtPhanTrangInfo.Text = "Không có dữ liệu";
                return;
            }

            if (txtPhanTrangInfo != null)
                txtPhanTrangInfo.Text = $"Trang {TrangHienTai}/{TongSoTrang} ({DanhSachKhachHang.Count} khách hàng)";

            btnTrang1.Visibility = Visibility.Visible;
            btnTrang1.Content = "1";
            btnTrang1.Tag = 1;
            btnTrang1.Style = TrangHienTai == 1 ?
                FindResource("ActivePageButtonStyle") as Style :
                FindResource("PageButtonStyle") as Style;

            if (TongSoTrang >= 2)
            {
                btnTrang2.Visibility = Visibility.Visible;
                btnTrang2.Content = "2";
                btnTrang2.Tag = 2;
                btnTrang2.Style = TrangHienTai == 2 ?
                    FindResource("ActivePageButtonStyle") as Style :
                    FindResource("PageButtonStyle") as Style;
            }

            if (TongSoTrang >= 3)
            {
                btnTrang3.Visibility = Visibility.Visible;
                btnTrang3.Content = "3";
                btnTrang3.Tag = 3;
                btnTrang3.Style = TrangHienTai == 3 ?
                    FindResource("ActivePageButtonStyle") as Style :
                    FindResource("PageButtonStyle") as Style;
            }

            if (TongSoTrang > 3)
            {
                txtEllipsis.Visibility = Visibility.Visible;
                btnTrangCuoiSo.Visibility = Visibility.Visible;
                btnTrangCuoiSo.Content = TongSoTrang.ToString();
                btnTrangCuoiSo.Tag = TongSoTrang;
                btnTrangCuoiSo.Style = TrangHienTai == TongSoTrang ?
                    FindResource("ActivePageButtonStyle") as Style :
                    FindResource("PageButtonStyle") as Style;
            }
        }

        private void XoaForm()
        {
            txtPopupTenKH.Text = "";
            txtPopupSDT.Text = "";
            txtPopupEmail.Text = "";
            txtPopupDiaChi.Text = "";
            txtPopupMaSoThue.Text = "";
            dpPopupNgaySinh.SelectedDate = DateTime.Now;
            cboPopupGioiTinh.SelectedIndex = 0;
            cboPopupLoaiKH.SelectedIndex = 0;
        }

        // ==================== CRUD ====================
        private void btnThemKhach_Click(object sender, RoutedEventArgs e)
        {
            DangSua = false;
            iconPopupKhachHang.Kind = MaterialDesignThemes.Wpf.PackIconKind.AccountPlus;
            txtPopupTitle.Text = "THÊM KHÁCH HÀNG MỚI";
            pnlCongNo.Visibility = Visibility.Collapsed;
            txtPopupMaKH.Text = $"KH{DanhSachKhachHangGoc.Count + 1:D3}";
            XoaForm();
            popupKhachHang.Visibility = Visibility.Visible;
        }

        private void btnSuaKhach_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is KhachHangItem kh)
            {
                DangSua = true;
                KhachHangDangChon = kh;
                iconPopupKhachHang.Kind = MaterialDesignThemes.Wpf.PackIconKind.AccountEdit;
                txtPopupTitle.Text = "CHỈNH SỬA KHÁCH HÀNG";
                pnlCongNo.Visibility = Visibility.Visible;
                txtPopupMaKH.Text = kh.MaKhachHang;
                txtPopupTenKH.Text = kh.TenKhachHang;
                txtPopupSDT.Text = kh.SoDienThoai;
                txtPopupEmail.Text = kh.Email;
                txtPopupDiaChi.Text = kh.DiaChi;
                txtPopupMaSoThue.Text = kh.MaSoThue;
                dpPopupNgaySinh.SelectedDate = kh.NgaySinh;
                txtPopupCongNo.Text = $"{kh.CongNo:N0} VNĐ";
                cboPopupGioiTinh.SelectedIndex = kh.GioiTinh switch { "Nữ" => 1, "Khác" => 2, _ => 0 };
                cboPopupLoaiKH.SelectedIndex = kh.LoaiKhach == "Doanh nghiệp" ? 1 : 0;
                popupKhachHang.Visibility = Visibility.Visible;
            }
        }

        private void btnXoaKhach_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is KhachHangItem kh)
            {
                if (MessageBox.Show($"Xóa '{kh.TenKhachHang}'?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DanhSachKhachHangGoc.Remove(kh);
                    ApplyFilter();
                }
            }
        }

        private void btnPopupQuayLai_Click(object sender, RoutedEventArgs e)
        {
            popupKhachHang.Visibility = Visibility.Collapsed;
        }

        private void btnPopupLuu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPopupTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPopupSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var loai = (cboPopupLoaiKH.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Cá nhân";
            var gt = (cboPopupGioiTinh.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Nam";

            if (DangSua && KhachHangDangChon != null)
            {
                KhachHangDangChon.TenKhachHang = txtPopupTenKH.Text.Trim();
                KhachHangDangChon.SoDienThoai = txtPopupSDT.Text.Trim();
                KhachHangDangChon.Email = txtPopupEmail.Text.Trim();
                KhachHangDangChon.DiaChi = txtPopupDiaChi.Text.Trim();
                KhachHangDangChon.MaSoThue = txtPopupMaSoThue.Text.Trim();
                KhachHangDangChon.NgaySinh = dpPopupNgaySinh.SelectedDate ?? DateTime.Now;
                KhachHangDangChon.GioiTinh = gt;
                KhachHangDangChon.LoaiKhach = loai;
            }
            else
            {
                DanhSachKhachHangGoc.Add(new KhachHangItem
                {
                    MaKhachHang = txtPopupMaKH.Text,
                    TenKhachHang = txtPopupTenKH.Text.Trim(),
                    SoDienThoai = txtPopupSDT.Text.Trim(),
                    Email = txtPopupEmail.Text.Trim(),
                    DiaChi = txtPopupDiaChi.Text.Trim(),
                    MaSoThue = txtPopupMaSoThue.Text.Trim(),
                    NgaySinh = dpPopupNgaySinh.SelectedDate ?? DateTime.Now,
                    GioiTinh = gt,
                    LoaiKhach = loai,
                    CongNo = 0
                });
            }

            popupKhachHang.Visibility = Visibility.Collapsed;
            ApplyFilter();
        }

        // ==================== THU TIỀN ====================
        private void btnLapPhieuThu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is KhachHangItem kh)
            {
                if (kh.CongNo <= 0) { MessageBox.Show("Không có công nợ!"); return; }
                KhachHangDangChon = kh;
                txtPopupMaPhieu.Text = $"PT{DateTime.Now:yyyyMMddHHmmss}";
                dtpPopupNgayLap.SelectedDate = DateTime.Now;
                txtPopupKhachHang.Text = kh.TenKhachHang;
                txtPopupSoTienThu.Text = kh.CongNo.ToString();
                txtPopupConNoSauKhiThu.Text = "0 VNĐ";
                popupThuTien.Visibility = Visibility.Visible;
            }
        }

        private void txtPopupSoTienThu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (KhachHangDangChon != null && long.TryParse(txtPopupSoTienThu.Text, out long soTien))
            {
                long conNo = Math.Max(0, KhachHangDangChon.CongNo - soTien);
                txtPopupConNoSauKhiThu.Text = $"{conNo:N0} VNĐ";
            }
        }

        private void btnPopupDong_Click(object sender, RoutedEventArgs e)
        {
            popupThuTien.Visibility = Visibility.Collapsed;
        }

        private void btnPopupXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(txtPopupSoTienThu.Text, out long soTien) || soTien <= 0)
            { MessageBox.Show("Số tiền không hợp lệ!"); return; }
            if (soTien > KhachHangDangChon!.CongNo)
            { MessageBox.Show("Số tiền thu > công nợ!"); return; }

            KhachHangDangChon.CongNo -= soTien;
            popupThuTien.Visibility = Visibility.Collapsed;
            ApplyFilter();
            MessageBox.Show($"Thu thành công! Còn nợ: {KhachHangDangChon.CongNo:N0} VNĐ");
        }

        // ==================== FILTER HANDLERS ====================
        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e) { if (IsLoaded) ApplyFilter(); }
        private void cboKieuTimKiem_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (IsLoaded) { KieuTimKiemHienTai = (cboKieuTimKiem.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tên khách hàng"; ApplyFilter(); } }
        private void cboLocLoaiKhach_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (IsLoaded) { LocLoaiKhachHienTai = (cboLocLoaiKhach.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tất cả"; ApplyFilter(); } }
        private void cboLocCongNo_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (IsLoaded) { LocCongNoHienTai = (cboLocCongNo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tất cả"; ApplyFilter(); } }
        private void btnXoaLoc_Click(object sender, RoutedEventArgs e) { txtTimKiem.Text = ""; cboKieuTimKiem.SelectedIndex = 0; cboLocLoaiKhach.SelectedIndex = 0; cboLocCongNo.SelectedIndex = 0; KieuTimKiemHienTai = "Tên khách hàng"; LocLoaiKhachHienTai = "Tất cả"; LocCongNoHienTai = "Tất cả"; ApplyFilter(); }

        // ==================== PAGINATION ====================
        private void btnTrangDau_Click(object sender, RoutedEventArgs e) { TrangHienTai = 1; HienThiTrangHienTai(); CapNhatPhanTrang(); }
        private void btnTrangTruoc_Click(object sender, RoutedEventArgs e) { if (TrangHienTai > 1) { TrangHienTai--; HienThiTrangHienTai(); CapNhatPhanTrang(); } }
        private void btnTrangSau_Click(object sender, RoutedEventArgs e) { if (TrangHienTai < TongSoTrang) { TrangHienTai++; HienThiTrangHienTai(); CapNhatPhanTrang(); } }
        private void btnTrangCuoi_Click(object sender, RoutedEventArgs e) { TrangHienTai = TongSoTrang; HienThiTrangHienTai(); CapNhatPhanTrang(); }
        private void btnTrangSo_Click(object sender, RoutedEventArgs e) { if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int t)) { TrangHienTai = t; HienThiTrangHienTai(); CapNhatPhanTrang(); } }
    }

    public class KhachHangItem
    {
        public string? MaKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? MaSoThue { get; set; }
        public string? LoaiKhach { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public long CongNo { get; set; }
    }
}
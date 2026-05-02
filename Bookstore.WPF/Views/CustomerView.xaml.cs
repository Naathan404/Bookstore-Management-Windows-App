using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views
{
    public partial class CustomerView : UserControl
    {
        private ObservableCollection<KhachHangItem> DanhSachKhachHang = null!;
        private ObservableCollection<KhachHangItem> DanhSachKhachHangGoc = null!;
        private KhachHangItem? KhachHangDangChon;

        public CustomerView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DanhSachKhachHang = new ObservableCollection<KhachHangItem>();
            DanhSachKhachHangGoc = new ObservableCollection<KhachHangItem>
            {
                new KhachHangItem { MaKhachHang = "KH001", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0912345678", Email = "vana@gmail.com", DiaChi = "Hà Nội", CongNo = 1500000, LoaiKhach = "VIP", GioiTinh = "Nam", NgaySinh = new DateTime(1990, 5, 15) },
                new KhachHangItem { MaKhachHang = "KH002", TenKhachHang = "Trần Thị B", SoDienThoai = "0987654321", Email = "thib@gmail.com", DiaChi = "Hồ Chí Minh", CongNo = 0, LoaiKhach = "Thường", GioiTinh = "Nữ", NgaySinh = new DateTime(1995, 8, 20) },
                new KhachHangItem { MaKhachHang = "KH003", TenKhachHang = "Lê Văn C", SoDienThoai = "0977777777", Email = "vanc@gmail.com", DiaChi = "Đà Nẵng", CongNo = 3500000, LoaiKhach = "Doanh nghiệp", GioiTinh = "Nam", NgaySinh = new DateTime(1988, 12, 10) }
            };

            foreach (var item in DanhSachKhachHangGoc)
                DanhSachKhachHang.Add(item);

            dgDanhSachKhach.ItemsSource = DanhSachKhachHang;
            UpdateThongKe();
            popupThuTien.Visibility = Visibility.Collapsed;
        }

        private void UpdateThongKe()
        {
            txtTongKhach.Text = DanhSachKhachHang.Count.ToString();
            int soKhachConNo = DanhSachKhachHang.Count(x => x.CongNo > 0);
            txtKhachNo.Text = soKhachConNo.ToString();
        }

        private void ApplyFilter()
        {
            // Kiểm tra null trước khi sử dụng
            if (DanhSachKhachHangGoc == null || txtTimKiem == null || cboLocNhanh == null)
                return;

            string searchText = txtTimKiem.Text?.ToLower() ?? "";
            var selectedItem = cboLocNhanh.SelectedItem as ComboBoxItem;
            string selectedFilter = selectedItem?.Content?.ToString() ?? "Tất cả";

            var filtered = DanhSachKhachHangGoc.Where(kh =>
                (string.IsNullOrEmpty(searchText) ||
                 kh.TenKhachHang?.ToLower().Contains(searchText) == true ||
                 kh.SoDienThoai?.Contains(searchText) == true ||
                 kh.Email?.ToLower().Contains(searchText) == true)
            ).ToList();

            switch (selectedFilter)
            {
                case "Còn nợ":
                    filtered = filtered.Where(kh => kh.CongNo > 0).ToList();
                    break;
                case "Khách VIP":
                    filtered = filtered.Where(kh => kh.LoaiKhach == "VIP").ToList();
                    break;
                case "Khách mới":
                    filtered = filtered.Where(kh => kh.NgaySinh > DateTime.Now.AddYears(-1)).ToList();
                    break;
            }

            DanhSachKhachHang?.Clear();
            if (DanhSachKhachHang != null)
            {
                foreach (var item in filtered)
                    DanhSachKhachHang.Add(item);
            }

            UpdateThongKe();
        }

        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void cboLocNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void dgDanhSachKhach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDanhSachKhach.SelectedItem != null)
            {
                KhachHangDangChon = dgDanhSachKhach.SelectedItem as KhachHangItem;
                if (KhachHangDangChon != null)
                {
                    txtTenKhachHang.Text = KhachHangDangChon.TenKhachHang;
                    txtSoDienThoai.Text = KhachHangDangChon.SoDienThoai;
                    txtEmail.Text = KhachHangDangChon.Email;
                    txtDiaChi.Text = KhachHangDangChon.DiaChi;
                    txtMaSoThue.Text = KhachHangDangChon.MaSoThue;
                    dpNgaySinh.SelectedDate = KhachHangDangChon.NgaySinh;

                    if (KhachHangDangChon.GioiTinh == "Nam")
                        cboGioiTinh.SelectedIndex = 0;
                    else if (KhachHangDangChon.GioiTinh == "Nữ")
                        cboGioiTinh.SelectedIndex = 1;
                    else
                        cboGioiTinh.SelectedIndex = 2;

                    switch (KhachHangDangChon.LoaiKhach)
                    {
                        case "VIP":
                            cboLoaiKhach.SelectedIndex = 1;
                            break;
                        case "Doanh nghiệp":
                            cboLoaiKhach.SelectedIndex = 2;
                            break;
                        case "Thân thiết":
                            cboLoaiKhach.SelectedIndex = 3;
                            break;
                        default:
                            cboLoaiKhach.SelectedIndex = 0;
                            break;
                    }
                }
            }
        }

        private void btnThemKhach_Click(object sender, RoutedEventArgs e)
        {
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            txtMaSoThue.Text = "";
            dpNgaySinh.SelectedDate = DateTime.Now;
            cboGioiTinh.SelectedIndex = 0;
            cboLoaiKhach.SelectedIndex = 0;
            KhachHangDangChon = null;
        }

        private void btnSuaKhach_Click(object sender, RoutedEventArgs e)
        {
            if (KhachHangDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!");
                return;
            }

            KhachHangDangChon.TenKhachHang = txtTenKhachHang.Text;
            KhachHangDangChon.SoDienThoai = txtSoDienThoai.Text;
            KhachHangDangChon.Email = txtEmail.Text;
            KhachHangDangChon.DiaChi = txtDiaChi.Text;
            KhachHangDangChon.MaSoThue = txtMaSoThue.Text;
            KhachHangDangChon.NgaySinh = dpNgaySinh.SelectedDate ?? DateTime.Now;

            var selectedGioiTinh = (cboGioiTinh.SelectedItem as ComboBoxItem)?.Content?.ToString();
            KhachHangDangChon.GioiTinh = selectedGioiTinh ?? "Nam";

            var selectedLoaiKhach = (cboLoaiKhach.SelectedItem as ComboBoxItem)?.Content?.ToString();
            KhachHangDangChon.LoaiKhach = selectedLoaiKhach ?? "Thường";

            dgDanhSachKhach.Items.Refresh();
            MessageBox.Show("Cập nhật thành công!");
        }

        private void btnXoaKhach_Click(object sender, RoutedEventArgs e)
        {
            if (KhachHangDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DanhSachKhachHangGoc.Remove(KhachHangDangChon);
                ApplyFilter();
                btnThemKhach_Click(null, null);
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void btnLuuKhach_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            var selectedLoaiKhach = (cboLoaiKhach.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Thường";
            var selectedGioiTinh = (cboGioiTinh.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Nam";

            KhachHangItem newKhach = new KhachHangItem
            {
                MaKhachHang = "KH" + (DanhSachKhachHangGoc.Count + 1).ToString("D3"),
                TenKhachHang = txtTenKhachHang.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                DiaChi = txtDiaChi.Text,
                MaSoThue = txtMaSoThue.Text,
                CongNo = 0,
                LoaiKhach = selectedLoaiKhach,
                GioiTinh = selectedGioiTinh,
                NgaySinh = dpNgaySinh.SelectedDate ?? DateTime.Now
            };

            DanhSachKhachHangGoc.Add(newKhach);
            ApplyFilter();
            btnThemKhach_Click(null, null);
            MessageBox.Show("Thêm khách hàng thành công!");
        }

        private void btnHuyForm_Click(object sender, RoutedEventArgs e)
        {
            btnThemKhach_Click(null, null);
        }

        private void btnLapPhieuThu_Click(object sender, RoutedEventArgs e)
        {
            if (KhachHangDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!");
                return;
            }

            if (KhachHangDangChon.CongNo <= 0)
            {
                MessageBox.Show("Khách hàng không có công nợ!");
                return;
            }

            txtPopupMaPhieu.Text = "PT" + DateTime.Now.ToString("yyyyMMddHHmmss");
            dtpPopupNgayLap.SelectedDate = DateTime.Now;
            txtPopupNguoiLap.Text = "Admin";
            txtPopupKhachHang.Text = KhachHangDangChon.TenKhachHang;
            txtPopupSoTienThu.Text = KhachHangDangChon.CongNo.ToString();

            popupThuTien.Visibility = Visibility.Visible;
        }

        private void txtPopupSoTienThu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (KhachHangDangChon != null && txtPopupSoTienThu != null)
            {
                if (long.TryParse(txtPopupSoTienThu.Text, out long soTienThu))
                {
                    long conNo = KhachHangDangChon.CongNo - soTienThu;
                    if (conNo < 0) conNo = 0;
                    txtPopupConNoSauKhiThu.Text = $"{conNo:N0} VNĐ";
                }
            }
        }

        private void btnPopupDong_Click(object sender, RoutedEventArgs e)
        {
            popupThuTien.Visibility = Visibility.Collapsed;
        }

        private void btnPopupXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(txtPopupSoTienThu.Text, out long soTienThu) || soTienThu <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ!");
                return;
            }

            if (soTienThu > KhachHangDangChon!.CongNo)
            {
                MessageBox.Show("Số tiền thu lớn hơn công nợ!");
                return;
            }

            KhachHangDangChon.CongNo -= soTienThu;
            dgDanhSachKhach.Items.Refresh();
            UpdateThongKe();
            popupThuTien.Visibility = Visibility.Collapsed;

            MessageBox.Show($"THU TIỀN THÀNH CÔNG!\n\n" +
                $"Mã phiếu: {txtPopupMaPhieu.Text}\n" +
                $"Ngày lập: {dtpPopupNgayLap.SelectedDate:dd/MM/yyyy}\n" +
                $"Người lập: {txtPopupNguoiLap.Text}\n" +
                $"Khách hàng: {txtPopupKhachHang.Text}\n" +
                $"Số tiền thu: {soTienThu:N0} VNĐ\n" +
                $"Còn nợ: {KhachHangDangChon.CongNo:N0} VNĐ",
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void popupThuTien_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            popupThuTien.Visibility = Visibility.Collapsed;
        }
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
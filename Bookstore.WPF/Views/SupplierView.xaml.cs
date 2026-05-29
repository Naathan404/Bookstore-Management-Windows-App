using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views
{
    public partial class SupplierView : UserControl
    {
        // Danh sách hiển thị và danh sách gốc để lọc
        //private ObservableCollection<SupplierItem> DanhSachNhaCungCap = new();
        //private ObservableCollection<SupplierItem> DanhSachNhaCungCapGoc = new();
        //private SupplierItem? NhaCungCapDangChon;
        //private bool DangSua = false;

        //// Các thuộc tính phân trang
        //private int TrangHienTai = 1;
        //private int SoDongTrenTrang = 10;
        //private int TongSoTrang = 1;
        //private string KieuTimKiemHienTai = "Tên nhà cung cấp";

        public SupplierView()
        {
            InitializeComponent();
            //Loaded += (s, e) => KhoiTaoDuLieu();
        }

        //    private void KhoiTaoDuLieu()
        //    {
        //        DanhSachNhaCungCapGoc = new ObservableCollection<SupplierItem>
        //{
        //    new() { MaNhaCungCap = "NCC001", TenNhaCungCap = "NXB Trẻ", SoDienThoai = "02839316211", Email = "info@nxbtre.com.vn", DiaChi = "161 Lý Chính Thắng, Q3, HCM", MaSoThue = "0300446243", SoTaiKhoan = "123456789", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC002", TenNhaCungCap = "NXB Kim Đồng", SoDienThoai = "02439434730", Email = "info@nxbkimdong.com.vn", DiaChi = "55 Quang Trung, Hai Bà Trưng, HN", MaSoThue = "0100110363", SoTaiKhoan = "987654321", TenNganHang = "BIDV" },
        //    new() { MaNhaCungCap = "NCC003", TenNhaCungCap = "Công ty CP Văn hóa Phương Nam", SoDienThoai = "02838222464", Email = "pnc@phuongnam.com.vn", DiaChi = "940 Đường 3/2, Q11, HCM", MaSoThue = "0302511452", SoTaiKhoan = "555666777", TenNganHang = "Agribank" },
        //    new() { MaNhaCungCap = "NCC004", TenNhaCungCap = "NXB Giáo Dục Việt Nam", SoDienThoai = "02438220801", Email = "lienhe@nxbgd.vn", DiaChi = "81 Trần Hưng Đạo, Hoàn Kiếm, HN", MaSoThue = "0100110483", SoTaiKhoan = "1020100001", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC005", TenNhaCungCap = "NXB Tổng hợp TP.HCM", SoDienThoai = "02838225340", Email = "tonghop@nxbhcm.com.vn", DiaChi = "62 Nguyễn Thị Minh Khai, Q1, HCM", MaSoThue = "0300461245", SoTaiKhoan = "200014849", TenNganHang = "Eximbank" },
        //    new() { MaNhaCungCap = "NCC006", TenNhaCungCap = "NXB Phụ Nữ Việt Nam", SoDienThoai = "02439420748", Email = "nxbphunu@vnn.vn", DiaChi = "39 Hàng Chuối, Hai Bà Trưng, HN", MaSoThue = "0100110564", SoTaiKhoan = "1180000002", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC007", TenNhaCungCap = "NXB Lao Động", SoDienThoai = "02438515124", Email = "nxblaodong@yahoo.com", DiaChi = "175 Giảng Võ, Đống Đa, HN", MaSoThue = "0100110689", SoTaiKhoan = "0011000123", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC008", TenNhaCungCap = "Công ty Sách Alpha (Alpha Books)", SoDienThoai = "02437226234", Email = "info@alphabooks.vn", DiaChi = "11A ngõ 282 Nguyễn Huy Tưởng, Thanh Xuân, HN", MaSoThue = "0101614741", SoTaiKhoan = "0451000222", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC009", TenNhaCungCap = "Công ty Văn hóa Đông A", SoDienThoai = "02437333037", Email = "dongabooks@vnn.vn", DiaChi = "113 Đông Các, Ô Chợ Dừa, HN", MaSoThue = "0101534053", SoTaiKhoan = "1902050123", TenNganHang = "Techcombank" },
        //    new() { MaNhaCungCap = "NCC010", TenNhaCungCap = "Công ty CP Sách Thái Hà", SoDienThoai = "02437930487", Email = "sales@thaihabooks.com", DiaChi = "119 C3 Nghĩa Tân, Cầu Giấy, HN", MaSoThue = "0102288000", SoTaiKhoan = "0491000034", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC011", TenNhaCungCap = "NXB Chính trị Quốc gia Sự thật", SoDienThoai = "02438221581", Email = "nxbctqg@gmail.com", DiaChi = "6/86 Duy Tân, Cầu Giấy, HN", MaSoThue = "0100110412", SoTaiKhoan = "1100000004", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC012", TenNhaCungCap = "Công ty Văn hóa Huy Hoàng", SoDienThoai = "02839250550", Email = "huyhoang@book.vn", DiaChi = "357 Lê Văn Sỹ, Tân Bình, HCM", MaSoThue = "0303845921", SoTaiKhoan = "0071001245", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC013", TenNhaCungCap = "NXB Khoa học và Kỹ thuật", SoDienThoai = "02438222371", Email = "nxbkhkt@hn.vnn.vn", DiaChi = "70 Trần Hưng Đạo, Hoàn Kiếm, HN", MaSoThue = "0100110596", SoTaiKhoan = "1020100055", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC014", TenNhaCungCap = "NXB Văn học", SoDienThoai = "02438253139", Email = "vanhoc@nxbvanhoc.com.vn", DiaChi = "18 Nguyễn Trường Tộ, Ba Đình, HN", MaSoThue = "0100110518", SoTaiKhoan = "0011000999", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC015", TenNhaCungCap = "Công ty Sách Nhã Nam", SoDienThoai = "02435146205", Email = "book@nhanam.vn", DiaChi = "59 Đỗ Quang, Trung Hòa, Cầu Giấy, HN", MaSoThue = "0101665489", SoTaiKhoan = "0611001888", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC016", TenNhaCungCap = "Công ty CP Tiền Phong", SoDienThoai = "02435371214", Email = "tienphong@jsc.vn", DiaChi = "15 Hồ Xuân Hương, Hai Bà Trưng, HN", MaSoThue = "0101423456", SoTaiKhoan = "1902008888", TenNganHang = "Techcombank" },
        //    new() { MaNhaCungCap = "NCC017", TenNhaCungCap = "NXB Đà Nẵng", SoDienThoai = "02363821012", Email = "nxbdanang@vnn.vn", DiaChi = "03 Lê Thánh Tôn, Đà Nẵng", MaSoThue = "0400100256", SoTaiKhoan = "5611000111", TenNganHang = "BIDV" },
        //    new() { MaNhaCungCap = "NCC018", TenNhaCungCap = "Văn phòng phẩm Hồng Hà", SoDienThoai = "02438521129", Email = "sales@vpphongha.com.vn", DiaChi = "25 Lý Thường Kiệt, Hoàn Kiếm, HN", MaSoThue = "0101237890", SoTaiKhoan = "1100123456", TenNganHang = "MB Bank" },
        //    new() { MaNhaCungCap = "NCC019", TenNhaCungCap = "Văn phòng phẩm Thiên Long", SoDienThoai = "02837505525", Email = "info@thienlonggroup.com", DiaChi = "Lô 6-8-10 Đường số 2, Tân Tạo, HCM", MaSoThue = "0301464830", SoTaiKhoan = "0071006666", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC020", TenNhaCungCap = "Công ty Sách Bách Khoa", SoDienThoai = "02438694112", Email = "nxb@hust.edu.vn", DiaChi = "Số 1 Đại Cồ Việt, Hai Bà Trưng, HN", MaSoThue = "0100412356", SoTaiKhoan = "0211000555", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC021", TenNhaCungCap = "NXB Y học", SoDienThoai = "02438232341", Email = "nxbyhoc@fpt.vn", DiaChi = "68 Giải Phóng, Đống Đa, HN", MaSoThue = "0100110755", SoTaiKhoan = "1020100999", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC022", TenNhaCungCap = "Công ty First News - Trí Việt", SoDienThoai = "02838227979", Email = "triviet@firstnews.com.vn", DiaChi = "11H Nguyễn Thị Minh Khai, Q1, HCM", MaSoThue = "0301452243", SoTaiKhoan = "0071002233", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC023", TenNhaCungCap = "NXB Mỹ Thuật", SoDienThoai = "02439423522", Email = "nxbmythuat@vnn.vn", DiaChi = "44B Hàm Long, Hoàn Kiếm, HN", MaSoThue = "0100110777", SoTaiKhoan = "1130000005", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC024", TenNhaCungCap = "NXB Thế Giới", SoDienThoai = "02438253843", Email = "thegioi@thegioipublishers.com.vn", DiaChi = "46 Trần Hưng Đạo, Hoàn Kiếm, HN", MaSoThue = "0100110784", SoTaiKhoan = "0011002244", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC025", TenNhaCungCap = "NXB Thanh Niên", SoDienThoai = "02439434057", Email = "nxbthanhnien@vnn.vn", DiaChi = "64 Bà Triệu, Hoàn Kiếm, HN", MaSoThue = "0100110791", SoTaiKhoan = "1020100123", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC026", TenNhaCungCap = "Nhà sách Fahasa", SoDienThoai = "02838225796", Email = "info@fahasa.com.vn", DiaChi = "60-62 Lê Lợi, Quận 1, HCM", MaSoThue = "0304132047", SoTaiKhoan = "0071003344", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC027", TenNhaCungCap = "Sách MCBooks", SoDienThoai = "02437921466", Email = "contact@mcbooks.vn", DiaChi = "Lô 34 Cầu Diễn, Bắc Từ Liêm, HN", MaSoThue = "0105872145", SoTaiKhoan = "1902568899", TenNganHang = "Techcombank" },
        //    new() { MaNhaCungCap = "NCC028", TenNhaCungCap = "NXB Công an nhân dân", SoDienThoai = "02439420045", Email = "nxbcand@fpt.vn", DiaChi = "92 Nguyễn Du, Hai Bà Trưng, HN", MaSoThue = "0100110811", SoTaiKhoan = "1150000007", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC029", TenNhaCungCap = "NXB Văn hóa - Văn nghệ", SoDienThoai = "02838225211", Email = "nxbvhvn@hcm.vnn.vn", DiaChi = "88-90 Ký Con, Quận 1, HCM", MaSoThue = "0300461247", SoTaiKhoan = "0071004455", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC030", TenNhaCungCap = "Đồ dùng học tập Deli", SoDienThoai = "02435624567", Email = "info@delivietnam.vn", DiaChi = "Tòa nhà 789 Mỹ Đình, Nam Từ Liêm, HN", MaSoThue = "0106543210", SoTaiKhoan = "1903334445", TenNganHang = "Techcombank" },
        //    new() { MaNhaCungCap = "NCC031", TenNhaCungCap = "NXB Thông tin và Truyền thông", SoDienThoai = "02435773223", Email = "nxb.tttt@mic.gov.vn", DiaChi = "115 Trần Duy Hưng, Cầu Giấy, HN", MaSoThue = "0100110822", SoTaiKhoan = "1160000008", TenNganHang = "VietinBank" },
        //    new() { MaNhaCungCap = "NCC032", TenNhaCungCap = "Công ty CP Văn hóa Tân Việt", SoDienThoai = "02437574747", Email = "tanviet@vnn.vn", DiaChi = "449 Bạch Mai, Hai Bà Trưng, HN", MaSoThue = "0102145632", SoTaiKhoan = "0451000888", TenNganHang = "Vietcombank" },
        //    new() { MaNhaCungCap = "NCC033", TenNhaCungCap = "NXB Đại học Quốc gia Hà Nội", SoDienThoai = "02437547736", Email = "nxb@vnu.edu.vn", DiaChi = "16 Hàng Chuối, Hai Bà Trưng, HN", MaSoThue = "0100110833", SoTaiKhoan = "0011004466", TenNganHang = "Vietcombank" }
        //};
        //        ApplyFilter();
        //    }

        //    private void ApplyFilter()
        //    {
        //        var searchText = txtTimKiem?.Text?.ToLower().Trim() ?? "";
        //        var filtered = DanhSachNhaCungCapGoc.AsEnumerable();

        //        // Lọc theo từ khóa tìm kiếm và kiểu tìm kiếm (Tên, SĐT, Email)
        //        if (!string.IsNullOrEmpty(searchText))
        //        {
        //            filtered = filtered.Where(ncc => KieuTimKiemHienTai switch
        //            {
        //                "Số điện thoại" => ncc.SoDienThoai?.Contains(searchText) ?? false,
        //                "Email" => ncc.Email?.ToLower().Contains(searchText) ?? false,
        //                "Địa chỉ" => ncc.DiaChi?.ToLower().Contains(searchText) ?? false,
        //                "Mã số thuế" => ncc.MaSoThue?.Contains(searchText) ?? false,
        //                _ => ncc.TenNhaCungCap?.ToLower().Contains(searchText) ?? false
        //            });
        //        }

        //        DanhSachNhaCungCap = new ObservableCollection<SupplierItem>(filtered);

        //        // Tính toán phân trang
        //        TongSoTrang = Math.Max(1, (int)Math.Ceiling(DanhSachNhaCungCap.Count / (double)SoDongTrenTrang));
        //        if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;

        //        HienThiTrangHienTai();
        //        CapNhatPhanTrang();

        //        // Hiển thị tổng số bản ghi
        //        txtTongBanGhi.Text = DanhSachNhaCungCap.Count.ToString();
        //    }

        //    private void HienThiTrangHienTai()
        //    {
        //        dgDanhSachKhach.ItemsSource = DanhSachNhaCungCap
        //            .Skip((TrangHienTai - 1) * SoDongTrenTrang)
        //            .Take(SoDongTrenTrang)
        //            .ToList();
        //    }

        //    private void CapNhatPhanTrang()
        //    {
        //        // Reset hiển thị các nút phân trang
        //        btnTrang1.Visibility = Visibility.Collapsed;
        //        btnTrang2.Visibility = Visibility.Collapsed;
        //        btnTrang3.Visibility = Visibility.Collapsed;
        //        txtEllipsis.Visibility = Visibility.Collapsed;
        //        btnTrangCuoiSo.Visibility = Visibility.Collapsed;

        //        if (DanhSachNhaCungCap.Count == 0)
        //        {
        //            txtPhanTrangInfo.Text = "Không có dữ liệu";
        //            return;
        //        }

        //        txtPhanTrangInfo.Text = $"Trang {TrangHienTai}/{TongSoTrang} ({DanhSachNhaCungCap.Count} nhà cung cấp)";

        //        // Logic hiển thị các nút số trang đơn giản
        //        btnTrang1.Visibility = Visibility.Visible;
        //        btnTrang1.Style = TrangHienTai == 1 ? FindResource("ActivePageButtonStyle") as Style : FindResource("PageButtonStyle") as Style;

        //        if (TongSoTrang >= 2)
        //        {
        //            btnTrang2.Visibility = Visibility.Visible;
        //            btnTrang2.Style = TrangHienTai == 2 ? FindResource("ActivePageButtonStyle") as Style : FindResource("PageButtonStyle") as Style;
        //        }

        //        if (TongSoTrang >= 3)
        //        {
        //            btnTrang3.Visibility = Visibility.Visible;
        //            btnTrang3.Style = TrangHienTai == 3 ? FindResource("ActivePageButtonStyle") as Style : FindResource("PageButtonStyle") as Style;
        //        }

        //        if (TongSoTrang > 3)
        //        {
        //            txtEllipsis.Visibility = Visibility.Visible;
        //            btnTrangCuoiSo.Visibility = Visibility.Visible;
        //            btnTrangCuoiSo.Content = TongSoTrang.ToString();
        //            btnTrangCuoiSo.Tag = TongSoTrang;
        //            btnTrangCuoiSo.Style = TrangHienTai == TongSoTrang ? FindResource("ActivePageButtonStyle") as Style : FindResource("PageButtonStyle") as Style;
        //        }
        //    }

        //    private void XoaForm()
        //    {
        //        txtPopupTenNCC.Text = "";
        //        txtPopupSDT.Text = "";
        //        txtPopupEmail.Text = "";
        //        txtPopupDiaChi.Text = "";
        //        txtPopupMaSoThue.Text = "";
        //        txtPopupSoTai.Text = "";
        //        txtPopupTenNganHang.Text = "";
        //    }

        //    // ==================== CRUD EVENTS ====================

        //    private void btnThemKhach_Click(object sender, RoutedEventArgs e)
        //    {
        //        DangSua = false;
        //        iconPopupNhaCungCap.Kind = MaterialDesignThemes.Wpf.PackIconKind.ShopPlus;
        //        txtPopupTitle.Text = "THÊM NHÀ CUNG CẤP MỚI";
        //        txtPopupMaNCC.Text = $"NCC{DanhSachNhaCungCapGoc.Count + 1:D3}";
        //        XoaForm();
        //        popupNhaCungCap.Visibility = Visibility.Visible;
        //    }

        //    private void btnSuaKhach_Click(object sender, RoutedEventArgs e)
        //    {
        //        if (sender is Button btn && btn.Tag is SupplierItem ncc)
        //        {
        //            DangSua = true;
        //            NhaCungCapDangChon = ncc;
        //            iconPopupNhaCungCap.Kind = MaterialDesignThemes.Wpf.PackIconKind.TruckAdd;
        //            txtPopupTitle.Text = "CHỈNH SỬA NHÀ CUNG CẤP";

        //            // Đổ dữ liệu vào popup
        //            txtPopupMaNCC.Text = ncc.MaNhaCungCap;
        //            txtPopupTenNCC.Text = ncc.TenNhaCungCap;
        //            txtPopupSDT.Text = ncc.SoDienThoai;
        //            txtPopupEmail.Text = ncc.Email;
        //            txtPopupDiaChi.Text = ncc.DiaChi;
        //            txtPopupMaSoThue.Text = ncc.MaSoThue;
        //            txtPopupSoTai.Text = ncc.SoTaiKhoan;
        //            txtPopupTenNganHang.Text = ncc.TenNganHang;

        //            popupNhaCungCap.Visibility = Visibility.Visible;
        //        }
        //    }

        //    private void btnXoaKhach_Click(object sender, RoutedEventArgs e)
        //    {
        //        if (sender is Button btn && btn.Tag is SupplierItem ncc)
        //        {
        //            if (MessageBox.Show($"Bạn có chắc muốn xóa nhà cung cấp '{ncc.TenNhaCungCap}'?", "Xác nhận xóa",
        //                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //            {
        //                DanhSachNhaCungCapGoc.Remove(ncc);
        //                ApplyFilter();
        //            }
        //        }
        //    }

        //    private void btnPopupLuu_Click(object sender, RoutedEventArgs e)
        //    {
        //        // Validate sơ bộ
        //        if (string.IsNullOrWhiteSpace(txtPopupTenNCC.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        if (string.IsNullOrWhiteSpace(txtPopupSDT.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        else
        //        {
        //            // Kiểm tra định dạng số điện thoại (có thể là 10-15 chữ số)
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPopupSDT.Text.Trim(), @"^\d{10,15}$"))
        //            {
        //                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(txtPopupEmail.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập email!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        else
        //        {
        //            // Kiểm tra định dạng email cơ bản
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPopupEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        //            {
        //                MessageBox.Show("Email không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(txtPopupDiaChi.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        else
        //        {
        //            // Kiểm tra độ dài địa chỉ (tối thiểu 5 ký tự)
        //            if (txtPopupDiaChi.Text.Trim().Length < 5)
        //            {
        //                MessageBox.Show("Địa chỉ quá ngắn! Vui lòng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(txtPopupMaSoThue.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập mã số thuế!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        else
        //        {
        //            // Kiểm tra định dạng mã số thuế (có thể là 10-15 chữ số)
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPopupMaSoThue.Text.Trim(), @"^\d{10,15}$"))
        //            {
        //                MessageBox.Show("Mã số thuế không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(txtPopupSoTai.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập số tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }
        //        else
        //        {
        //            // Kiểm tra định dạng số tài khoản (có thể là 10-20 chữ số)
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPopupSoTai.Text.Trim(), @"^\d{10,20}$"))
        //            {
        //                MessageBox.Show("Số tài khoản không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(txtPopupTenNganHang.Text))
        //        {
        //            MessageBox.Show("Vui lòng nhập tên ngân hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }


        //        if (DangSua && NhaCungCapDangChon != null)
        //        {
        //            // Cập nhật
        //            NhaCungCapDangChon.TenNhaCungCap = txtPopupTenNCC.Text.Trim();
        //            NhaCungCapDangChon.SoDienThoai = txtPopupSDT.Text.Trim();
        //            NhaCungCapDangChon.Email = txtPopupEmail.Text.Trim();
        //            NhaCungCapDangChon.DiaChi = txtPopupDiaChi.Text.Trim();
        //            NhaCungCapDangChon.MaSoThue = txtPopupMaSoThue.Text.Trim();
        //            NhaCungCapDangChon.SoTaiKhoan = txtPopupSoTai.Text.Trim();
        //            NhaCungCapDangChon.TenNganHang = txtPopupTenNganHang.Text.Trim();
        //        }
        //        else
        //        {
        //            // Thêm mới
        //            DanhSachNhaCungCapGoc.Add(new SupplierItem
        //            {
        //                MaNhaCungCap = txtPopupMaNCC.Text,
        //                TenNhaCungCap = txtPopupTenNCC.Text.Trim(),
        //                SoDienThoai = txtPopupSDT.Text.Trim(),
        //                Email = txtPopupEmail.Text.Trim(),
        //                DiaChi = txtPopupDiaChi.Text.Trim(),
        //                MaSoThue = txtPopupMaSoThue.Text.Trim(),
        //                SoTaiKhoan = txtPopupSoTai.Text.Trim(),
        //                TenNganHang = txtPopupTenNganHang.Text.Trim()
        //            });
        //        }

        //        popupNhaCungCap.Visibility = Visibility.Collapsed;
        //        ApplyFilter();
        //    }

        //    private void btnPopupQuayLai_Click(object sender, RoutedEventArgs e)
        //    {
        //        popupNhaCungCap.Visibility = Visibility.Hidden;
        //    }

        //    // ==================== FILTER & PAGINATION HANDLERS ====================

        //    private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        //    private void cboKieuTimKiem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        if (IsLoaded)
        //        {
        //            KieuTimKiemHienTai = (cboKieuTimKiem.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tên nhà cung cấp";
        //            ApplyFilter();
        //        }
        //    }

        //    private void btnXoaLoc_Click(object sender, RoutedEventArgs e)
        //    {
        //        txtTimKiem.Text = "";
        //        cboKieuTimKiem.SelectedIndex = 0;
        //        ApplyFilter();
        //    }

        //    private void btnTrangDau_Click(object sender, RoutedEventArgs e) { TrangHienTai = 1; ApplyFilter(); }
        //    private void btnTrangTruoc_Click(object sender, RoutedEventArgs e) { if (TrangHienTai > 1) { TrangHienTai--; ApplyFilter(); } }
        //    private void btnTrangSau_Click(object sender, RoutedEventArgs e) { if (TrangHienTai < TongSoTrang) { TrangHienTai++; ApplyFilter(); } }
        //    private void btnTrangCuoi_Click(object sender, RoutedEventArgs e) { TrangHienTai = TongSoTrang; ApplyFilter(); }
        //    private void btnTrangSo_Click(object sender, RoutedEventArgs e) { if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int t)) { TrangHienTai = t; ApplyFilter(); } }
        //}

        //// Model cho Nhà cung cấp
        //public class SupplierItem
        //{
        //    public string? MaNhaCungCap { get; set; }
        //    public string? TenNhaCungCap { get; set; }
        //    public string? SoDienThoai { get; set; }
        //    public string? Email { get; set; }
        //    public string? DiaChi { get; set; }
        //    public string? MaSoThue { get; set; }
        //    public string? SoTaiKhoan { get; set; }
        //    public string? TenNganHang { get; set; }
        //}
    }
}
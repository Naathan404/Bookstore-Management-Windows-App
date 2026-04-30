using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views
{
    public partial class TraCuuSach : UserControl
    {
        private ObservableCollection<BookItem> _allBooks;
        private ObservableCollection<BookItem> _filteredBooks;

        public TraCuuSach()
        {
            InitializeComponent();
            LoadSampleData();
            LoadComboBoxData();
        }

        private void LoadSampleData()
        {
            _allBooks = new ObservableCollection<BookItem>
            {
                new BookItem {
                    STT = 1,
                    TenSach = "Lập trình C# và .NET Core",
                    TacGia = "Nguyễn Văn A",
                    TheLoai = "Công nghệ",
                    MoTa = "Sách hướng dẫn lập trình C# từ cơ bản đến nâng cao với .NET Core",
                    SoLuongTonKho = 15,
                    TongDaBan = 45,
                    GiaNiemYet = 250000,
                    DonGiaBan = 225000
                },
                new BookItem {
                    STT = 2,
                    TenSach = "WPF Master - Xây dựng ứng dụng Desktop",
                    TacGia = "Trần Thị B",
                    TheLoai = "Công nghệ",
                    MoTa = "Xây dựng ứng dụng desktop chuyên nghiệp với WPF và MVVM",
                    SoLuongTonKho = 8,
                    TongDaBan = 32,
                    GiaNiemYet = 320000,
                    DonGiaBan = 288000
                },
                new BookItem {
                    STT = 3,
                    TenSach = "SQL Server từ cơ bản đến nâng cao",
                    TacGia = "Lê Văn C",
                    TheLoai = "Cơ sở dữ liệu",
                    MoTa = "Tìm hiểu SQL Server, viết query tối ưu và quản trị database",
                    SoLuongTonKho = 0,
                    TongDaBan = 78,
                    GiaNiemYet = 280000,
                    DonGiaBan = 252000
                },
                new BookItem {
                    STT = 4,
                    TenSach = "Design Patterns trong C#",
                    TacGia = "Phạm Thị D",
                    TheLoai = "Công nghệ",
                    MoTa = "24 mẫu thiết kế phổ biến và cách áp dụng trong C#",
                    SoLuongTonKho = 5,
                    TongDaBan = 28,
                    GiaNiemYet = 350000,
                    DonGiaBan = 315000
                },
                new BookItem {
                    STT = 5,
                    TenSach = "Clean Code - Nguyên tắc viết mã sạch",
                    TacGia = "Robert Martin",
                    TheLoai = "Lập trình",
                    MoTa = "Những nguyên tắc và thực hành tốt nhất để viết code dễ bảo trì",
                    SoLuongTonKho = 3,
                    TongDaBan = 56,
                    GiaNiemYet = 290000,
                    DonGiaBan = 261000
                }
            };

            _filteredBooks = new ObservableCollection<BookItem>(_allBooks);
            dgSach.ItemsSource = _filteredBooks;
            UpdateResultCount();
        }

        private void LoadComboBoxData()
        {
            // Chỉ còn ComboBox Thể loại
            cboTheLoai.Items.Clear();
            cboTheLoai.Items.Add("Tất cả");
            cboTheLoai.Items.Add("Công nghệ");
            cboTheLoai.Items.Add("Cơ sở dữ liệu");
            cboTheLoai.Items.Add("Lập trình");
            cboTheLoai.Items.Add("Mobile App");
            cboTheLoai.Items.Add("Web Development");
            cboTheLoai.SelectedIndex = 0;
        }

        private void txtTenSach_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void btnNhapLai_Click(object sender, RoutedEventArgs e)
        {
            // Reset tất cả field lọc
            txtTenSach.Text = "";
            txtTacGia.Text = "";
            txtGiaNiemYet.Text = "";
            txtDonGiaBan.Text = "";
            txtSoLuongTon.Text = "";
            cboTheLoai.SelectedIndex = 0;

            PerformSearch();
        }

        private void btnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            // Xuất danh sách đang hiển thị ra Excel
            if (_filteredBooks.Count == 0)
            {
                MessageBox.Show("⚠️ Không có dữ liệu để xuất!", "Thông báo",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"📊 Đang xuất {_filteredBooks.Count} sách ra file Excel...", "Thông báo",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PerformSearch()
        {
            _filteredBooks.Clear();

            // Lấy giá trị tìm kiếm
            string searchTen = txtTenSach.Text?.Trim().ToLower() ?? "";
            string searchTacGia = txtTacGia.Text?.Trim().ToLower() ?? "";
            string selectedTheLoai = cboTheLoai.SelectedItem?.ToString();

            // Lọc theo giá niêm yết (hỗ trợ nhập khoảng: "100000-300000" hoặc số cụ thể)
            string giaNiemYetText = txtGiaNiemYet.Text?.Trim() ?? "";
            decimal? giaNiemYetMin = null;
            decimal? giaNiemYetMax = null;
            ParseRangeInput(giaNiemYetText, out giaNiemYetMin, out giaNiemYetMax);

            // Lọc theo đơn giá bán
            string donGiaBanText = txtDonGiaBan.Text?.Trim() ?? "";
            decimal? donGiaBanMin = null;
            decimal? donGiaBanMax = null;
            ParseRangeInput(donGiaBanText, out donGiaBanMin, out donGiaBanMax);

            // Lọc theo số lượng tồn kho
            string soLuongTonText = txtSoLuongTon.Text?.Trim() ?? "";
            int? soLuongTonMin = null;
            int? soLuongTonMax = null;
            ParseRangeInputInt(soLuongTonText, out soLuongTonMin, out soLuongTonMax);

            int stt = 1;
            foreach (var book in _allBooks)
            {
                bool match = true;

                // Lọc theo tên sách
                if (!string.IsNullOrEmpty(searchTen) && !book.TenSach.ToLower().Contains(searchTen))
                    match = false;

                // Lọc theo tác giả
                if (match && !string.IsNullOrEmpty(searchTacGia) && !book.TacGia.ToLower().Contains(searchTacGia))
                    match = false;

                // Lọc theo thể loại
                if (match && selectedTheLoai != null && selectedTheLoai != "Tất cả" && book.TheLoai != selectedTheLoai)
                    match = false;

                // Lọc theo giá niêm yết
                if (match && giaNiemYetMin.HasValue && book.GiaNiemYet < giaNiemYetMin.Value)
                    match = false;
                if (match && giaNiemYetMax.HasValue && book.GiaNiemYet > giaNiemYetMax.Value)
                    match = false;

                // Lọc theo đơn giá bán
                if (match && donGiaBanMin.HasValue && book.DonGiaBan < donGiaBanMin.Value)
                    match = false;
                if (match && donGiaBanMax.HasValue && book.DonGiaBan > donGiaBanMax.Value)
                    match = false;

                // Lọc theo số lượng tồn kho
                if (match && soLuongTonMin.HasValue && book.SoLuongTonKho < soLuongTonMin.Value)
                    match = false;
                if (match && soLuongTonMax.HasValue && book.SoLuongTonKho > soLuongTonMax.Value)
                    match = false;

                if (match)
                {
                    book.STT = stt++;
                    _filteredBooks.Add(book);
                }
            }

            UpdateResultCount();
        }

        /// <summary>
        /// Hỗ trợ nhập khoảng giá: "100000-300000", ">=100000", "<=300000", hoặc số cụ thể
        /// </summary>
        private void ParseRangeInput(string input, out decimal? min, out decimal? max)
        {
            min = null;
            max = null;

            if (string.IsNullOrEmpty(input))
                return;

            // Dạng ">=100000"
            if (input.StartsWith(">="))
            {
                if (decimal.TryParse(input.Substring(2), out decimal val))
                    min = val;
                return;
            }

            // Dạng "<=300000"
            if (input.StartsWith("<="))
            {
                if (decimal.TryParse(input.Substring(2), out decimal val))
                    max = val;
                return;
            }

            // Dạng ">100000"
            if (input.StartsWith(">"))
            {
                if (decimal.TryParse(input.Substring(1), out decimal val))
                    min = val + 1; // Lớn hơn (không bao gồm)
                return;
            }

            // Dạng "<300000"
            if (input.StartsWith("<"))
            {
                if (decimal.TryParse(input.Substring(1), out decimal val))
                    max = val - 1; // Nhỏ hơn (không bao gồm)
                return;
            }

            // Dạng "100000-300000"
            if (input.Contains("-"))
            {
                string[] parts = input.Split('-');
                if (parts.Length == 2)
                {
                    decimal.TryParse(parts[0].Trim(), out decimal minVal);
                    decimal.TryParse(parts[1].Trim(), out decimal maxVal);
                    min = minVal;
                    max = maxVal;
                }
                return;
            }

            // Dạng số cụ thể
            if (decimal.TryParse(input, out decimal exactVal))
            {
                min = exactVal;
                max = exactVal;
            }
        }

        /// <summary>
        /// Parse khoảng số nguyên (dùng cho số lượng tồn kho)
        /// </summary>
        private void ParseRangeInputInt(string input, out int? min, out int? max)
        {
            min = null;
            max = null;

            if (string.IsNullOrEmpty(input))
                return;

            // Parse dạng decimal trước rồi convert sang int
            decimal? decMin, decMax;
            ParseRangeInput(input, out decMin, out decMax);

            if (decMin.HasValue)
                min = (int)decMin.Value;
            if (decMax.HasValue)
                max = (int)decMax.Value;
        }

        private void UpdateResultCount()
        {
            if (_filteredBooks.Count == 0)
                txtKetQua.Text = "🔍 Không tìm thấy kết quả nào";
            else if (_filteredBooks.Count == 1)
                txtKetQua.Text = $"✨ Hiển thị 1 kết quả";
            else
                txtKetQua.Text = $"✨ Hiển thị {_filteredBooks.Count} kết quả";
        }
    }

    public class BookItem
    {
        public int STT { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }
    }
}
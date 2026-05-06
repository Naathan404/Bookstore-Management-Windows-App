using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Bookstore.WPF.Views
{
    public partial class ProductView : UserControl
    {
        private ObservableCollection<BookItem> _allBooks;
        private ObservableCollection<BookItem> _filteredBooks;
        private ObservableCollection<BookItem> _currentPageBooks;

        // Phân trang
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        // Biến lưu trữ khi SỬA
        private int? _editingBookId;
        private string _selectedImagePath;

        // Đường dẫn ảnh mặc định
        private readonly string DefaultImagePath = "/Resources/Images/Books/default_book_cover.jpg";

        public ProductView()
        {
            InitializeComponent();
            LoadSampleData();
            LoadComboBoxData();
        }

        private void LoadSampleData()
        {
            _allBooks = new ObservableCollection<BookItem>();
            _filteredBooks = new ObservableCollection<BookItem>();
            _currentPageBooks = new ObservableCollection<BookItem>();

            string[] categories = { "Công nghệ", "Lập trình", "CSDL", "Web Dev", "Mobile App" };
            string[] authors = { "Nguyễn Văn A", "Trần Thị B", "Lê Văn C", "Phạm Thị D", "Hoàng Văn E" };
            string[] bookNames = {
                "Lập trình C# và .NET Core",
                "WPF Master - Building Desktop Apps",
                "SQL Server từ cơ bản đến nâng cao",
                "Design Patterns trong C#",
                "Clean Code - Viết mã sạch",
                "React Native cho người mới",
                "ASP.NET Core MVC Toàn tập",
                "Entity Framework Core Pro",
                "Docker và Kubernetes",
                "Microservices Architecture",
                "Machine Learning với Python",
                "DevOps cho Developer",
                "Flutter từ A đến Z",
                "Node.js Backend Development",
                "Angular Framework Chuyên sâu",
                "Vue.js 3 cho dự án thực tế",
                "Java Spring Boot Master",
                "PHP Laravel Framework",
                "TypeScript Nâng cao",
                "Python cho Data Science",
                "Blockchain và Ứng dụng",
                "Cloud Computing với AWS",
                "Git và Quản lý mã nguồn",
                "Agile Scrum thực hành",
                "UI/UX Design cơ bản"
            };

            for (int i = 1; i <= 25; i++)
            {
                _allBooks.Add(new BookItem
                {
                    Id = i,
                    STT = i,
                    TenSach = $"{bookNames[(i - 1) % bookNames.Length]} #{i}",
                    TacGia = authors[i % 5],
                    TheLoai = categories[i % 5],
                    MoTa = $"Hướng dẫn chi tiết về {categories[i % 5]} - Phiên bản {i}.",
                    SoLuongTonKho = (i * 7) % 20,
                    TongDaBan = i * 15 + (i % 3) * 10,
                    GiaNiemYet = 200000 + (i * 30000),
                    DonGiaBan = 180000 + (i * 27000),
                    HinhAnh = DefaultImagePath
                });
            }

            PerformSearch();
        }

        private void LoadComboBoxData()
        {
            cboTheLoai.Items.Clear();
            cboTheLoai.Items.Add("Tất cả");
            cboTheLoai.Items.Add("Công nghệ");
            cboTheLoai.Items.Add("Lập trình");
            cboTheLoai.Items.Add("CSDL");
            cboTheLoai.Items.Add("Web Dev");
            cboTheLoai.Items.Add("Mobile App");
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

        // ==================== POPUP XỬ LÝ ====================

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            _editingBookId = null;
            _selectedImagePath = DefaultImagePath;

            txtPopupTitle.Text = "THÊM SÁCH MỚI";
            iconPopupSach.Kind = PackIconKind.BookPlus;
            txtPopupMaSach.Visibility = Visibility.Collapsed;

            ClearPopupFields();
            LoadDefaultImage();
            popupSach.Visibility = Visibility.Visible;
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is BookItem book)
            {
                var bookInAll = _allBooks.FirstOrDefault(b => b.Id == book.Id);
                if (bookInAll != null)
                {
                    _editingBookId = bookInAll.Id;
                    _selectedImagePath = bookInAll.HinhAnh;

                    txtPopupTitle.Text = "SỬA THÔNG TIN SÁCH";
                    iconPopupSach.Kind = PackIconKind.BookEdit;
                    txtPopupMaSach.Visibility = Visibility.Visible;
                    txtPopupMaSach.Text = $"SACH-{bookInAll.Id:D4}";

                    txtPopupTenSach.Text = bookInAll.TenSach;
                    txtPopupTacGia.Text = bookInAll.TacGia;
                    txtPopupMoTa.Text = bookInAll.MoTa;
                    txtPopupSoLuongTon.Text = bookInAll.SoLuongTonKho.ToString();
                    txtPopupTongDaBan.Text = bookInAll.TongDaBan.ToString();
                    txtPopupGiaNiemYet.Text = bookInAll.GiaNiemYet.ToString();
                    txtPopupDonGiaBan.Text = bookInAll.DonGiaBan.ToString();

                    foreach (ComboBoxItem item in cboPopupTheLoai.Items)
                    {
                        if (item.Content.ToString() == bookInAll.TheLoai)
                        {
                            item.IsSelected = true;
                            break;
                        }
                    }
                    if (cboPopupTheLoai.SelectedIndex == -1 && cboPopupTheLoai.Items.Count > 0)
                        cboPopupTheLoai.SelectedIndex = 0;

                    LoadImage(bookInAll.HinhAnh);
                    popupSach.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnPopupQuayLai_Click(object sender, RoutedEventArgs e)
        {
            popupSach.Visibility = Visibility.Collapsed;
            ClearPopupFields();
        }

        private void btnPopupLuu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPopupTenSach.Text))
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên sách!", "Lỗi",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPopupTenSach.Focus();
                return;
            }

            if (!decimal.TryParse(txtPopupGiaNiemYet.Text, out decimal giaNiemYet) || giaNiemYet < 0)
            {
                MessageBox.Show("⚠️ Giá Niêm Yết không hợp lệ!", "Lỗi",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPopupGiaNiemYet.Focus();
                return;
            }

            if (_editingBookId.HasValue)
            {
                var book = _allBooks.FirstOrDefault(b => b.Id == _editingBookId.Value);
                if (book != null)
                {
                    book.TenSach = txtPopupTenSach.Text.Trim();
                    book.TacGia = txtPopupTacGia.Text.Trim();
                    book.TheLoai = (cboPopupTheLoai.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Công nghệ";
                    book.MoTa = txtPopupMoTa.Text.Trim();

                    int.TryParse(txtPopupSoLuongTon.Text, out int ton);
                    book.SoLuongTonKho = ton;

                    int.TryParse(txtPopupTongDaBan.Text, out int ban);
                    book.TongDaBan = ban;

                    book.GiaNiemYet = giaNiemYet;

                    decimal.TryParse(txtPopupDonGiaBan.Text, out decimal donGia);
                    book.DonGiaBan = donGia;

                    if (!string.IsNullOrEmpty(_selectedImagePath))
                        book.HinhAnh = _selectedImagePath;

                    PerformSearch();
                    MessageBox.Show($"✏️ Đã cập nhật sách: {book.TenSach}", "Thành công",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                int newId = _allBooks.Count > 0 ? _allBooks.Max(b => b.Id) + 1 : 1;

                var newBook = new BookItem
                {
                    Id = newId,
                    TenSach = txtPopupTenSach.Text.Trim(),
                    TacGia = txtPopupTacGia.Text.Trim(),
                    TheLoai = (cboPopupTheLoai.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Công nghệ",
                    MoTa = txtPopupMoTa.Text.Trim(),
                    GiaNiemYet = giaNiemYet,
                    HinhAnh = _selectedImagePath ?? DefaultImagePath
                };

                int.TryParse(txtPopupSoLuongTon.Text, out int ton);
                newBook.SoLuongTonKho = ton;

                int.TryParse(txtPopupTongDaBan.Text, out int ban);
                newBook.TongDaBan = ban;

                decimal.TryParse(txtPopupDonGiaBan.Text, out decimal donGia);
                newBook.DonGiaBan = donGia;

                _allBooks.Add(newBook);
                PerformSearch();
                MessageBox.Show($"✅ Đã thêm sách: {newBook.TenSach}", "Thành công",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }

            popupSach.Visibility = Visibility.Collapsed;
            ClearPopupFields();
        }

        // ==================== HÌNH ẢNH ====================

        private void ImageBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh bìa sách",
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                LoadImage(_selectedImagePath);
            }
        }

        private void ImageBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            overlayBorder.Visibility = Visibility.Visible;
            changeImagePanel.Visibility = Visibility.Visible;
        }

        private void ImageBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            overlayBorder.Visibility = Visibility.Collapsed;
            changeImagePanel.Visibility = Visibility.Collapsed;
        }

        private void LoadImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    BitmapImage bitmap = null;

                    if (imagePath.StartsWith("/"))
                    {
                        try
                        {
                            bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri("pack://application:,,," + imagePath);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                        }
                        catch
                        {
                            string fullPath = AppDomain.CurrentDomain.BaseDirectory +
                                imagePath.TrimStart('/').Replace('/', '\\');
                            if (File.Exists(fullPath))
                            {
                                bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(fullPath);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                            }
                        }
                    }
                    else if (Path.IsPathRooted(imagePath) && File.Exists(imagePath))
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                    }

                    if (bitmap != null)
                    {
                        imgBookCover.Source = bitmap;
                        txtImagePath.Text = Path.GetFileName(imagePath);
                        return;
                    }
                }
                LoadDefaultImage();
            }
            catch
            {
                LoadDefaultImage();
            }
        }

        private void LoadDefaultImage()
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Images/Books/default_book_cover.jpg");
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgBookCover.Source = bitmap;
                txtImagePath.Text = "default_book_cover.jpg";
            }
            catch
            {
                try
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Resources", "Images", "Books", "default_book_cover.jpg");

                    if (File.Exists(fullPath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(fullPath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        imgBookCover.Source = bitmap;
                        txtImagePath.Text = "default_book_cover.jpg";
                    }
                    else
                    {
                        imgBookCover.Source = null;
                        txtImagePath.Text = "Không tìm thấy ảnh";
                    }
                }
                catch
                {
                    imgBookCover.Source = null;
                    txtImagePath.Text = "Lỗi tải ảnh";
                }
            }
        }

        private void ClearPopupFields()
        {
            txtPopupTenSach.Text = "";
            txtPopupTacGia.Text = "";
            txtPopupMoTa.Text = "";
            txtPopupSoLuongTon.Text = "";    
            txtPopupTongDaBan.Text = "";     
            txtPopupGiaNiemYet.Text = "";   
            txtPopupDonGiaBan.Text = "";     
            txtPopupMaSach.Text = "";
            cboPopupTheLoai.SelectedIndex = 0;
            _selectedImagePath = DefaultImagePath;
            LoadDefaultImage();
        }

        // ==================== XÓA SÁCH ====================

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is BookItem book)
            {
                var result = MessageBox.Show(
                    $"❌ Bạn có chắc muốn xóa sách?\n\n📖 Tên: {book.TenSach}\n👤 Tác giả: {book.TacGia}\n💰 Giá Niêm Yết: {book.GiaNiemYet:N0} đ",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var bookToRemove = _allBooks.FirstOrDefault(b => b.Id == book.Id);
                    if (bookToRemove != null)
                    {
                        _allBooks.Remove(bookToRemove);
                    }

                    PerformSearch();
                    MessageBox.Show($"🗑️ Đã xóa sách: {book.TenSach}", "Xóa sách thành công",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredBooks.Count == 0)
            {
                MessageBox.Show("⚠️ Không có dữ liệu để xuất!", "Thông báo",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"📊 Đang xuất {_filteredBooks.Count} sách ra file Excel...", "Xuất Excel",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ==================== TÌM KIẾM & PHÂN TRANG ====================

        private void PerformSearch()
        {
            _filteredBooks.Clear();

            string searchTen = txtTenSach.Text?.Trim().ToLower() ?? "";
            string searchTacGia = txtTacGia.Text?.Trim().ToLower() ?? "";
            string selectedTheLoai = cboTheLoai.SelectedItem?.ToString();

            ParseRangeInput(txtGiaNiemYet.Text?.Trim() ?? "", out decimal? giaNiemYetMin, out decimal? giaNiemYetMax);
            ParseRangeInput(txtDonGiaBan.Text?.Trim() ?? "", out decimal? donGiaBanMin, out decimal? donGiaBanMax);
            ParseRangeInputInt(txtSoLuongTon.Text?.Trim() ?? "", out int? soLuongTonMin, out int? soLuongTonMax);

            int stt = 1;
            foreach (var book in _allBooks)
            {
                if (!MatchesSearch(book, searchTen, searchTacGia, selectedTheLoai,
                                   giaNiemYetMin, giaNiemYetMax,
                                   donGiaBanMin, donGiaBanMax,
                                   soLuongTonMin, soLuongTonMax))
                    continue;

                book.STT = stt++;
                _filteredBooks.Add(book);
            }

            _currentPage = 1;
            UpdatePaginationAndDisplay();
        }

        private bool MatchesSearch(BookItem book, string searchTen, string searchTacGia,
                                   string selectedTheLoai,
                                   decimal? giaNiemYetMin, decimal? giaNiemYetMax,
                                   decimal? donGiaBanMin, decimal? donGiaBanMax,
                                   int? soLuongTonMin, int? soLuongTonMax)
        {
            if (!string.IsNullOrEmpty(searchTen) && !book.TenSach.ToLower().Contains(searchTen))
                return false;

            if (!string.IsNullOrEmpty(searchTacGia) && !book.TacGia.ToLower().Contains(searchTacGia))
                return false;

            if (selectedTheLoai != null && selectedTheLoai != "Tất cả" && book.TheLoai != selectedTheLoai)
                return false;

            if (giaNiemYetMin.HasValue && book.GiaNiemYet < giaNiemYetMin.Value)
                return false;
            if (giaNiemYetMax.HasValue && book.GiaNiemYet > giaNiemYetMax.Value)
                return false;

            if (donGiaBanMin.HasValue && book.DonGiaBan < donGiaBanMin.Value)
                return false;
            if (donGiaBanMax.HasValue && book.DonGiaBan > donGiaBanMax.Value)
                return false;

            if (soLuongTonMin.HasValue && book.SoLuongTonKho < soLuongTonMin.Value)
                return false;
            if (soLuongTonMax.HasValue && book.SoLuongTonKho > soLuongTonMax.Value)
                return false;

            return true;
        }

        private void UpdatePaginationAndDisplay()
        {
            _totalPages = (int)Math.Ceiling((double)_filteredBooks.Count / _pageSize);
            if (_totalPages < 1) _totalPages = 1;

            if (_currentPage > _totalPages) _currentPage = _totalPages;
            if (_currentPage < 1) _currentPage = 1;

            DisplayCurrentPage();
            BuildPaginationButtons();
        }

        private void DisplayCurrentPage()
        {
            _currentPageBooks.Clear();

            var pagedData = _filteredBooks
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            int startSTT = (_currentPage - 1) * _pageSize + 1;
            foreach (var book in pagedData)
            {
                book.STT = startSTT++;
                _currentPageBooks.Add(book);
            }

            dgSach.ItemsSource = null;
            dgSach.ItemsSource = _currentPageBooks;
        }

        private void BuildPaginationButtons()
        {
            paginationPanel.Children.Clear();

            var btnFirst = CreateNavigationButton(PackIconKind.ChevronDoubleLeft, () => GoToPage(1));
            btnFirst.Margin = new Thickness(0, 0, 4, 0);
            paginationPanel.Children.Add(btnFirst);

            var btnPrev = CreateNavigationButton(PackIconKind.ChevronLeft, () => GoToPage(_currentPage - 1));
            btnPrev.Margin = new Thickness(0, 0, 8, 0);
            paginationPanel.Children.Add(btnPrev);

            int startPage = Math.Max(1, _currentPage - 2);
            int endPage = Math.Min(_totalPages, startPage + 4);
            if (endPage - startPage < 4)
                startPage = Math.Max(1, endPage - 4);

            for (int i = startPage; i <= endPage; i++)
            {
                var pageButton = CreatePageButton(i);
                paginationPanel.Children.Add(pageButton);
            }

            var btnNext = CreateNavigationButton(PackIconKind.ChevronRight, () => GoToPage(_currentPage + 1));
            btnNext.Margin = new Thickness(8, 0, 0, 0);
            paginationPanel.Children.Add(btnNext);

            var btnLast = CreateNavigationButton(PackIconKind.ChevronDoubleRight, () => GoToPage(_totalPages));
            btnLast.Margin = new Thickness(4, 0, 0, 0);
            paginationPanel.Children.Add(btnLast);

            var pageInfo = new TextBlock
            {
                Text = $"  {_currentPage}/{_totalPages} trang",
                FontSize = 12,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A3AED0")),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, 0, 0, 0)
            };
            paginationPanel.Children.Add(pageInfo);
        }

        private Button CreateNavigationButton(PackIconKind iconKind, Action clickAction)
        {
            var button = new Button
            {
                Style = FindResource("PaginationButtonStyle") as Style,
                Content = new PackIcon
                {
                    Kind = iconKind,
                    Width = 16,
                    Height = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A3AED0"))
                }
            };
            button.Click += (s, e) => clickAction();
            return button;
        }

        private Button CreatePageButton(int pageNumber)
        {
            var isActive = pageNumber == _currentPage;
            var button = new Button
            {
                Style = isActive
                    ? FindResource("ActivePaginationButtonStyle") as Style
                    : FindResource("PaginationButtonStyle") as Style,
                Content = new TextBlock { Text = pageNumber.ToString(), FontSize = 12 },
                Tag = pageNumber,
                Margin = new Thickness(0, 0, 4, 0)
            };
            button.Click += (s, e) =>
            {
                if (s is Button btn && btn.Tag is int page)
                    GoToPage(page);
            };
            return button;
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > _totalPages || page == _currentPage)
                return;

            _currentPage = page;
            UpdatePaginationAndDisplay();
        }

        private void ParseRangeInput(string input, out decimal? min, out decimal? max)
        {
            min = null;
            max = null;

            if (string.IsNullOrEmpty(input)) return;

            if (input.StartsWith(">="))
            {
                if (decimal.TryParse(input.Substring(2), out decimal val)) min = val;
            }
            else if (input.StartsWith("<="))
            {
                if (decimal.TryParse(input.Substring(2), out decimal val)) max = val;
            }
            else if (input.StartsWith(">"))
            {
                if (decimal.TryParse(input.Substring(1), out decimal val)) min = val + 1;
            }
            else if (input.StartsWith("<"))
            {
                if (decimal.TryParse(input.Substring(1), out decimal val)) max = val - 1;
            }
            else if (input.Contains("-"))
            {
                string[] parts = input.Split('-');
                if (parts.Length == 2)
                {
                    decimal.TryParse(parts[0].Trim(), out decimal minVal);
                    decimal.TryParse(parts[1].Trim(), out decimal maxVal);
                    min = minVal;
                    max = maxVal;
                }
            }
            else if (decimal.TryParse(input, out decimal exactVal))
            {
                min = exactVal;
                max = exactVal;
            }
        }

        private void ParseRangeInputInt(string input, out int? min, out int? max)
        {
            min = null;
            max = null;

            if (string.IsNullOrEmpty(input)) return;

            ParseRangeInput(input, out decimal? decMin, out decimal? decMax);

            if (decMin.HasValue) min = (int)decMin.Value;
            if (decMax.HasValue) max = (int)decMax.Value;
        }
    }

    public class BookItem
    {
        public int Id { get; set; }
        public int STT { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }
        public string HinhAnh { get; set; }
    }
}
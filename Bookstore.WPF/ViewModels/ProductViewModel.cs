using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        #region Collections
        private ObservableCollection<BookItem> _allBooks;
        private ObservableCollection<BookItem> _filteredBooks;

        // Danh sách sách hiển thị trên 1 trang
        public ObservableCollection<BookItem> PagedBooks { get; set; }

        // Danh sách Thể loại cho ComboBox
        public ObservableCollection<string> ListTheLoai { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
        #endregion

        #region Properties - Tìm Kiếm
        private string _searchTenSach;
        public string SearchTenSach { get => _searchTenSach; set { _searchTenSach = value; OnPropertyChanged(); PerformSearch(); } }

        private string _searchTacGia;
        public string SearchTacGia { get => _searchTacGia; set { _searchTacGia = value; OnPropertyChanged(); PerformSearch(); } }

        private string _selectedTheLoai;
        public string SelectedTheLoai { get => _selectedTheLoai; set { _selectedTheLoai = value; OnPropertyChanged(); PerformSearch(); } }

        // Thêm các property cho Giá và Số lượng tương tự nếu cần...
        #endregion

        #region Properties - Phân Trang
        private int _currentPage = 1;
        public int CurrentPage { get => _currentPage; set { _currentPage = value; OnPropertyChanged(); } }

        private int _totalPages = 1;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        private int _pageSize = 10;
        #endregion

        #region Properties - Popup Thêm/Sửa
        private Visibility _isPopupVisible = Visibility.Collapsed;
        public Visibility IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        private string _popupTitle;
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private PackIconKind _popupIcon;
        public PackIconKind PopupIcon { get => _popupIcon; set { _popupIcon = value; OnPropertyChanged(); } }

        // Biến chứa dữ liệu sách đang được thêm hoặc sửa
        private BookItem _editingBook;
        public BookItem EditingBook { get => _editingBook; set { _editingBook = value; OnPropertyChanged(); } }

        private bool _isAddingNew;
        #endregion

        #region Commands
        public ICommand OpenAddPopupCommand { get; set; }
        public ICommand OpenEditPopupCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand SaveBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }

        // Phân trang Commands
        public ICommand FirstPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        #endregion

        public ProductViewModel()
        {
            _allBooks = new ObservableCollection<BookItem>();
            _filteredBooks = new ObservableCollection<BookItem>();
            PagedBooks = new ObservableCollection<BookItem>();
            PageNumbers = new ObservableCollection<int>();
            ListTheLoai = new ObservableCollection<string>();

            //LoadSampleData();
            InitCommands();

            _ = LoadTheLoaiAsync();
            _ = LoadDataAsync();
        }

        private void InitCommands()
        {
            // Popup Commands
            OpenAddPopupCommand = new RelayCommand<object>((p) =>
            {
                PopupTitle = "THÊM SÁCH MỚI";
                PopupIcon = PackIconKind.BookPlus;
                _isAddingNew = true;
                EditingBook = new BookItem { HinhAnh = "/Resources/Images/Books/default_book_cover.jpg", GiaNiemYet = 0, DonGiaBan = 0, SoLuongTonKho = 0, TongDaBan = 0 };
                IsPopupVisible = Visibility.Visible;
            });

            //OpenEditPopupCommand = new RelayCommand<BookItem>((book) =>
            //{
            //    if (book == null) return;
            //    PopupTitle = "SỬA THÔNG TIN SÁCH";
            //    PopupIcon = PackIconKind.BookEdit;
            //    _isAddingNew = false;

            //    // Clone ra object mới để sửa, lỡ bấm Hủy thì không bị lưu đè
            //    EditingBook = new BookItem
            //    {
            //        Id = book.Id,
            //        TenSach = book.TenSach,
            //        TacGia = book.TacGia,
            //        TheLoai = book.TheLoai,
            //        MoTa = book.MoTa,
            //        GiaNiemYet = book.GiaNiemYet,
            //        DonGiaBan = book.DonGiaBan,
            //        SoLuongTonKho = book.SoLuongTonKho,
            //        TongDaBan = book.TongDaBan,
            //        HinhAnh = book.HinhAnh
            //    };
            //    IsPopupVisible = Visibility.Visible;
            //});

            OpenEditPopupCommand = new RelayCommand<BookItem>((book) =>
            {
                if (book == null) return;

                PopupTitle = "SỬA THÔNG TIN SÁCH";
                PopupIcon = PackIconKind.BookEdit;
                _isAddingNew = false;

                // PHÉP MÀU LÀ Ở ĐÂY:
                // Tui tạo một object EditingBook mới và copy data từ dòng DataGrid sang.
                EditingBook = new BookItem
                {
                    Id = book.Id,
                    TenSach = book.TenSach,
                    TacGia = book.TacGia,
                    TheLoai = book.TheLoai,
                    MoTa = book.MoTa,
                    GiaNiemYet = book.GiaNiemYet,
                    DonGiaBan = book.DonGiaBan,
                    SoLuongTonKho = book.SoLuongTonKho,
                    TongDaBan = book.TongDaBan,
                    HinhAnh = book.HinhAnh
                };

                // Mở popup lên
                IsPopupVisible = Visibility.Visible;
            });

            ClosePopupCommand = new RelayCommand<object>((p) => IsPopupVisible = Visibility.Collapsed);

            SaveBookCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrWhiteSpace(EditingBook.TenSach))
                {
                    MessageBox.Show("Vui lòng nhập tên sách!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_isAddingNew)
                {
                    EditingBook.Id = _allBooks.Count > 0 ? _allBooks.Max(b => b.Id) + 1 : 1;
                    _allBooks.Insert(0, EditingBook); // Thêm lên đầu
                }
                else
                {
                    var bookInDb = _allBooks.FirstOrDefault(b => b.Id == EditingBook.Id);
                    if (bookInDb != null)
                    {
                        bookInDb.TenSach = EditingBook.TenSach;
                        bookInDb.TacGia = EditingBook.TacGia;
                        bookInDb.TheLoai = EditingBook.TheLoai;
                        bookInDb.MoTa = EditingBook.MoTa;
                        bookInDb.GiaNiemYet = EditingBook.GiaNiemYet;
                        bookInDb.DonGiaBan = EditingBook.DonGiaBan;
                        bookInDb.HinhAnh = EditingBook.HinhAnh;
                    }
                }

                IsPopupVisible = Visibility.Collapsed;
                PerformSearch();
            });

            DeleteBookCommand = new RelayCommand<BookItem>((book) =>
            {
                if (book == null) return;
                var result = MessageBox.Show($"Bạn có chắc muốn xóa sách {book.TenSach}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var bookToRemove = _allBooks.FirstOrDefault(b => b.Id == book.Id);
                    if (bookToRemove != null) _allBooks.Remove(bookToRemove);
                    PerformSearch();
                }
            });

            ChangeImageCommand = new RelayCommand<object>((p) =>
            {
                var openFileDialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpg)|*.png;*.jpg" };
                if (openFileDialog.ShowDialog() == true)
                {
                    EditingBook.HinhAnh = openFileDialog.FileName;
                }
            });

            // Phân trang commands
            FirstPageCommand = new RelayCommand<object>((p) => GoToPage(1));
            PrevPageCommand = new RelayCommand<object>((p) => GoToPage(CurrentPage - 1));
            NextPageCommand = new RelayCommand<object>((p) => GoToPage(CurrentPage + 1));
            LastPageCommand = new RelayCommand<object>((p) => GoToPage(TotalPages));
            GoToPageCommand = new RelayCommand<int>((page) => GoToPage(page));
        }

        private void PerformSearch()
        {
            var result = _allBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTenSach))
                result = result.Where(b => b.TenSach.ToLower().Contains(SearchTenSach.ToLower()));

            if (!string.IsNullOrWhiteSpace(SearchTacGia))
                result = result.Where(b => b.TacGia.ToLower().Contains(SearchTacGia.ToLower()));

            if (SelectedTheLoai != "Tất cả" && !string.IsNullOrEmpty(SelectedTheLoai))
                result = result.Where(b => b.TheLoai == SelectedTheLoai);

            _filteredBooks.Clear();
            int stt = 1;
            foreach (var b in result)
            {
                b.STT = stt++;
                _filteredBooks.Add(b);
            }

            CurrentPage = 1;
            UpdatePagination();
        }

        private void UpdatePagination()
        {
            TotalPages = (int)Math.Ceiling((double)_filteredBooks.Count / _pageSize);
            if (TotalPages < 1) TotalPages = 1;

            PagedBooks.Clear();
            var pagedData = _filteredBooks.Skip((CurrentPage - 1) * _pageSize).Take(_pageSize);
            foreach (var b in pagedData) PagedBooks.Add(b);

            PageNumbers.Clear();
            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, startPage + 4);
            for (int i = startPage; i <= endPage; i++) PageNumbers.Add(i);
        }

        private void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                UpdatePagination();
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // cal api
                using (var client = new HttpClient())
                {
                    // ⚠️ QUAN TRỌNG: Sửa lại cái cổng (port) 7123 này cho khớp với port của Backend ông đang chạy
                    client.BaseAddress = new Uri("https://localhost:7001/");

                    // Gọi API GET: api/Sach (Hoặc api/Sach/method-syntax nếu ông xài hàm dưới)
                    var danhSachTuApi = await client.GetFromJsonAsync<List<SachDTO>>("api/Sach");

                    if (danhSachTuApi != null)
                    {
                        // Vì ObservableCollection thay đổi giao diện, đôi khi tải ngầm cần đưa về luồng chính (UI Thread)
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _allBooks.Clear(); // Dọn dẹp rác cũ

                            int stt = 1;
                            foreach (var item in danhSachTuApi)
                            {
                                _allBooks.Add(new BookItem
                                {
                                    Id = item.Id,
                                    STT = stt++,
                                    TenSach = item.TenSach,
                                    TacGia = item.TacGia,
                                    TheLoai = item.TheLoai,
                                    MoTa = item.MoTa,
                                    SoLuongTonKho = item.SoLuongTonKho,
                                    TongDaBan = item.TongDaBan,
                                    GiaNiemYet = item.GiaNiemYet,
                                    DonGiaBan = item.DonGiaBan,
                                    // Nếu API không có hình, fallback về hình mặc định để UI không bị trống
                                    HinhAnh = string.IsNullOrEmpty(item.HinhAnh) ? "/Resources/Images/Books/default_book_cover.jpg" : item.HinhAnh
                                });
                            }

                            // Kéo data xong thì gọi hàm này để chia trang và render lên UI
                            PerformSearch();
                        });
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Không thể kết nối đến Backend. Ông đã bật project API chưa?\nChi tiết: {httpEx.Message}",
                                "Lỗi kết nối", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu sách: {ex.Message}",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadTheLoaiAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // ⚠️ Nhớ đổi port 7123 này giống với port API của ông nha
                    client.BaseAddress = new Uri("https://localhost:7001/");

                    // Gọi API lấy mảng string các tên thể loại
                    var danhSachTheLoai = await client.GetFromJsonAsync<List<string>>("api/TheLoai");

                    if (danhSachTheLoai != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ListTheLoai.Clear();

                            // Thêm phần tử "Tất cả" lên đầu tiên để dùng cho bộ lọc Tìm kiếm
                            ListTheLoai.Add("Tất cả");

                            // Đổ dữ liệu từ DB vào
                            foreach (var tl in danhSachTheLoai)
                            {
                                ListTheLoai.Add(tl);
                            }

                            // Reset lại giá trị hiển thị mặc định
                            SelectedTheLoai = "Tất cả";
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Có thể in ra Console hoặc để trống để app không bị crash nếu API lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi tải thể loại: {ex.Message}");
            }
        }
    }

    // Class Model: Kế thừa BaseViewModel để tự động update UI khi đổi hình ảnh, chữ,...
    public class BookItem : BaseViewModel
    {
        public int Id { get; set; }
        public int STT { get; set; }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }

        private string _hinhAnh;
        public string HinhAnh { get => _hinhAnh; set { _hinhAnh = value; OnPropertyChanged(); } }
    }
}
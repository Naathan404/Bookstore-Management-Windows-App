using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace Bookstore.WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        #region Collections
        private ObservableCollection<BookItem> _allBooks;
        private ObservableCollection<BookItem> _filteredBooks;
        public ObservableCollection<string> ListNhaCungCap { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ListNhaXuatBan { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> ListHinhThucBia { get; set; }

        // Danh sách sách hiển thị trên 1 trang
        public ObservableCollection<BookItem> PagedBooks { get; set; }

        // Danh sách Thể loại cho ComboBox
        public ObservableCollection<string> ListTheLoai { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
        #endregion

        #region Properties - Tìm Kiếm
        private string _searchTenSach = string.Empty;
        public string SearchTenSach { get => _searchTenSach; set { _searchTenSach = value; OnPropertyChanged(); PerformSearch(); } }

        private string _searchTacGia = string.Empty;
        public string SearchTacGia { get => _searchTacGia; set { _searchTacGia = value; OnPropertyChanged(); PerformSearch(); } }

        private string _selectedTheLoai = string.Empty;
        public string SelectedTheLoai { get => _selectedTheLoai; set { _selectedTheLoai = value; OnPropertyChanged(); PerformSearch(); } }

        // NOTE Để bổ sung các properties còn thíu

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
        public bool IsAddingNew
        {
            get { return _isAddingNew; }
            set
            {
                _isAddingNew = value;
                OnPropertyChanged(nameof(IsAddingNew));
            }
        }

        private bool _isViewByVersion = true;
        public bool IsViewByVersion
        {
            get => _isViewByVersion;
            set
            {
                _isViewByVersion = value;
                OnPropertyChanged();
                _ = LoadDataAsync();
            }
        }
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

            ListHinhThucBia = new ObservableCollection<string>
            {
                "Bìa mềm",
                "Bìa cứng",
                "Bìa gập",
                "Bìa rời",
                "Bìa da",
                "Khác"
            };

            //LoadSampleData();
            InitCommands();

            _ = LoadTheLoaiAsync();
            _ = LoadNhaXuatBanAsync();
            //_ = LoadNhaCungCapAsync();
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
                IsAddingNew = true;
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
                    HinhAnh = book.HinhAnh,
                    ISBN = book.ISBN,
                    NamXuatBan = book.NamXuatBan,
                    NhaXuatBan = book.NhaXuatBan,
                    HinhThucBia = book.HinhThucBia
                };

                // Mở popup lên
                IsPopupVisible = Visibility.Visible;
                IsAddingNew = false;
            });

            ClosePopupCommand = new RelayCommand<object>((p) => IsPopupVisible = Visibility.Collapsed);

            SaveBookCommand = new RelayCommand<object>(async (p) =>
            {
                if (string.IsNullOrWhiteSpace(EditingBook.TenSach))
                {
                    MessageBox.Show("Vui lòng nhập tên sách!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    bool isSuccess = false;

                    if (_isAddingNew)
                    {
                        // API POST 
                        isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/Sach", EditingBook);
                    }
                    else
                    {
                        // API PUT 
                        isSuccess = await ApiClient.PutAndCheckSuccessAsync($"api/Sach/{EditingBook.Id}", EditingBook);
                    }

                    if (isSuccess)
                    {
                        MessageBox.Show("Lưu thông tin sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsPopupVisible = Visibility.Collapsed;
                        _ = LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Lưu thất bại! Hãy kiểm tra lại API hoặc kết nối.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                // Nhờ ApiClient, gọi API giờ chỉ còn đúng 1 dòng này!
                var danhSachTuApi = await ApiClient.GetAsync<List<SachDTO>>("api/Sach");

                if (danhSachTuApi != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _allBooks.Clear();
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
                                HinhAnh = string.IsNullOrEmpty(item.HinhAnh) ? "/Resources/Images/Books/default_book_cover.jpg" : item.HinhAnh,
                                ISBN = item.ISBN,
                                NamXuatBan = item.NamXuatBan,
                                NhaXuatBan = item.NhaXuatBan,
                                HinhThucBia = item.HinhThucBia
                            });
                        }
                        PerformSearch();
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadTheLoaiAsync()
        {
            try
            {
                var danhSachTheLoai = await ApiClient.GetAsync<List<string>>("api/TheLoai");

                if (danhSachTheLoai != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListTheLoai.Clear();
                        ListTheLoai.Add("Tất cả");

                        foreach (var tl in danhSachTheLoai)
                        {
                            ListTheLoai.Add(tl);
                        }
                        SelectedTheLoai = "Tất cả";
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải thể loại: {ex.Message}");
            }
        }

        private async Task LoadNhaXuatBanAsync()
        {
            var data = await ApiClient.GetAsync<List<string>>("api/NhaXuatBan");
            if (data != null)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    ListNhaXuatBan.Clear();
                    foreach (var item in data) ListNhaXuatBan.Add(item);
                });
            }
        }

        private async Task LoadNhaCungCapAsync()
        {
            var data = await ApiClient.GetAsync<List<string>>("api/NhaCungCap");
            if (data != null)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    ListNhaCungCap.Clear();
                    foreach (var item in data) ListNhaCungCap.Add(item);
                });
            }
        }
    }

    public class BookItem : BaseViewModel
    {
        public int Id { get; set; }
        public int STT { get; set; }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        public string ISBN { get; set; }
        public int NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public string HinhThucBia { get; set; }
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
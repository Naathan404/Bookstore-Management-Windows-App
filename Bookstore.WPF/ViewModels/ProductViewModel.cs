using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections;
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
        public ObservableCollection<BookItem> ListTacPhamGoc { get; set; } = new ObservableCollection<BookItem>();

        public ObservableCollection<string> ListHinhThucBia { get; set; }

        // Danh sách sách hiển thị trên 1 trang
        public ObservableCollection<BookItem> PagedBooks { get; set; }

        // Danh sách Thể loại cho ComboBox
        public ObservableCollection<string> ListTheLoai { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
        #endregion

        #region Properties - Tìm Kiếm
        private string _searchTenSach = string.Empty;
        public string SearchTenSach 
        { 
            get => _searchTenSach; 
            set 
            { 
                _searchTenSach = value; 
                OnPropertyChanged(); 
                PerformSearch(); 
            } 
        }

        private string _searchTacGia = string.Empty;
        public string SearchTacGia 
        { 
            get => _searchTacGia; 
            set 
            { _searchTacGia = value; 
                OnPropertyChanged(); 
                PerformSearch(); 
            } 
        }

        private string _selectedTheLoai = string.Empty;
        public string SelectedTheLoai 
        { 
            get => _selectedTheLoai; 
            set 
            { 
                _selectedTheLoai = value; 
                OnPropertyChanged();
                PerformSearch(); 
            } 
        }

        private string _searchTonKho;
        public string SearchTonKho
        {
            get => _searchTonKho;
            set 
            { 
                _searchTonKho = value; 
                OnPropertyChanged();
                PerformSearch();
            }
        }

        // filter lọc theo mức giá
        private int _selectedPriceRangeIndex = 0; // Mặc định là 0 (Tất cả mức giá)
        public int SelectedPriceRangeIndex
        {
            get => _selectedPriceRangeIndex;
            set
            {
                _selectedPriceRangeIndex = value;
                OnPropertyChanged();
                PerformSearch();
            }
        }

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

        private bool _isNewProduct = true;
        public bool IsNewProduct
        {
            get => _isNewProduct;
            set
            {
                _isNewProduct = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOldProduct));

                if (value)
                {
                    SelectedTacPhamGoc = null;
                }
            }
        }

        public bool IsOldProduct
        {
            get => !_isNewProduct;
            set
            {
                IsNewProduct = !value;
            }
        }
        private BookItem _selectedTacPhamGoc;
        public BookItem SelectedTacPhamGoc
        {
            get => _selectedTacPhamGoc;
            set
            {
                _selectedTacPhamGoc = value;
                OnPropertyChanged();

                if (value != null && EditingBook != null)
                {
                    EditingBook.TenSach = value.TenSach;
                    EditingBook.TacGia = value.TacGia;
                    EditingBook.TheLoai = value.TheLoai;
                    EditingBook.MoTa = value.MoTa;
                    EditingBook.HinhAnh = value.HinhAnh;
                }
            }
        }

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
        public ICommand ClearFilterCommand { get; set; }

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

                IsNewProduct = true;
                SelectedTacPhamGoc = null;

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
                if (IsOldProduct && SelectedTacPhamGoc == null)
                {
                    MessageBox.Show("Vui lòng CHỌN đúng một tác phẩm có sẵn từ danh sách, hoặc chuyển sang chế độ 'Tạo tác phẩm mới'!",
                                    "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
            

            ClearFilterCommand = new RelayCommand<object>((p) => {
                SearchTenSach = string.Empty;
                SearchTacGia = string.Empty;
                SelectedTheLoai = null;
                SelectedPriceRangeIndex = 0;
                SearchTonKho = string.Empty;

                PerformSearch();
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
            
            // lọc tên sách
            if (!string.IsNullOrWhiteSpace(SearchTenSach))
                result = result.Where(b => b.TenSach.ToLower().Contains(SearchTenSach.ToLower()));

            // lọc tên tác giả
            if (!string.IsNullOrWhiteSpace(SearchTacGia))
                result = result.Where(b => b.TacGia.ToLower().Contains(SearchTacGia.ToLower()));

            // lọc theo thể loại
            if (SelectedTheLoai != "Tất cả" && !string.IsNullOrEmpty(SelectedTheLoai))
                result = result.Where(b => b.TheLoai == SelectedTheLoai);

            // lọc theo khoảng giá
            switch (SelectedPriceRangeIndex)
            {
                case 1: // Dưới 50.000đ
                    result = result.Where(x => x.DonGiaBan < 50000);
                    break;
                case 2: // 50.000đ - 100.000đ
                    result = result.Where(x => x.DonGiaBan >= 50000 && x.DonGiaBan <= 100000);
                    break;
                case 3: // 100.000đ - 200.000đ
                    result = result.Where(x => x.DonGiaBan > 100000 && x.DonGiaBan <= 200000);
                    break;
                case 4: // Trên 200.000đ
                    result = result.Where(x => x.DonGiaBan > 200000);
                    break;
                case 0: // Tất cả mức giá -> Không làm gì cả
                default:
                    break;
            }

            // lọc theo tồn kho
            if (!string.IsNullOrWhiteSpace(SearchTonKho) && int.TryParse(SearchTonKho, out int tonKho))
            {
                result = result.Where(x => x.SoLuongTonKho <= tonKho);
            }

            _filteredBooks.Clear();
            int stt = 1;
            foreach (var b in result)
            {
                b.STT = stt++;
                _filteredBooks.Add(b);
            }

            //_filteredBooks = new ObservableCollection<BookItem>(result);

            //CurrentPage = 1;
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

                        ListTacPhamGoc.Clear();
                        var uniqueBooks = _allBooks.GroupBy(x => x.TenSach).Select(g => g.First()).ToList();
                        foreach (var b in uniqueBooks) ListTacPhamGoc.Add(b);
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

        private string _tacGia;
        public string TacGia { get => _tacGia; set { _tacGia = value; OnPropertyChanged(); } }

        private string _theLoai;
        public string TheLoai { get => _theLoai; set { _theLoai = value; OnPropertyChanged(); } }

        private string _moTa;
        public string MoTa { get => _moTa; set { _moTa = value; OnPropertyChanged(); } }

        private string _hinhAnh;
        public string HinhAnh { get => _hinhAnh; set { _hinhAnh = value; OnPropertyChanged(); } }

        public string ISBN { get; set; }
        public int NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public string HinhThucBia { get; set; }
        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }
    }
}
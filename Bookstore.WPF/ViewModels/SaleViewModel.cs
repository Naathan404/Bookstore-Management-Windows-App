using Bookstore.Share.DTOResponses;
using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    // Lớp trợ giúp định nghĩa Thể loại sách phục vụ hiển thị combobox lọc
    public class CategoryModel
    {
        public string TenTheLoai { get; set; }
    }

    // ==========================================================================================
    // CÁCH 2: TẠO LỚP BỌC (WRAPPER) ĐỂ BỔ SUNG ISSELECTED MÀ KHÔNG CAN THIỆP VÀO CLASS BOOKITEM GỐC
    // ==========================================================================================
    public class BookSaleModel : BaseViewModel
    {
        // Giữ đối tượng gốc phục vụ cho việc gửi nhận dữ liệu sau này nếu cần
        public BookItem BookData { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(); // Thông báo giao diện WPF cập nhật CheckBox lập tức
                }
            }
        }

        // Các thuộc tính bắc cầu (Proxy Properties) giúp XAML Binding giữ nguyên không bị lỗi
        public string ISBN => BookData?.ISBN;
        public string TenSach => BookData?.TenSach;
        public decimal DonGiaBan => BookData?.DonGiaBan ?? 0;
        public int SoLuongTonKho => BookData?.SoLuongTonKho ?? 0;
        public string TheLoai => BookData?.TheLoai;
        public string HinhAnh => BookData?.HinhAnh; // Ánh xạ đường dẫn ảnh nếu có hiển thị trên giao diện
    }

    public class CartItemModel : BaseViewModel
    {
        public string ISBN { get; set; }
        public string TenSach { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTonKho { get; set; }

        private int _soLuongMua;
        public int SoLuongMua
        {
            get => _soLuongMua;
            set
            {
                if (_soLuongMua != value)
                {
                    if (value > SoLuongTonKho) _soLuongMua = SoLuongTonKho;
                    else if (value < 1) _soLuongMua = 1;
                    else _soLuongMua = value;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public decimal ThanhTien => GiaBan * _soLuongMua;
    }

    public class SaleViewModel : BaseViewModel
    {
        // ĐỊNH NGHĨA SỐ LƯỢNG THẺ HIỂN THỊ TRÊN MỖI TRANG
        private const int ItemsPerPage = 30;

        #region PROPERTIES
        private bool _isBookDetailOpen;
        public bool IsBookDetailOpen
        {
            get => _isBookDetailOpen;
            set { _isBookDetailOpen = value; OnPropertyChanged(); }
        }

        private bool _isConfirmPaymentOpen;
        public bool IsConfirmPaymentOpen
        {
            get => _isConfirmPaymentOpen;
            set { _isConfirmPaymentOpen = value; OnPropertyChanged(); }
        }

        private BookSaleModel _sachDuocChonXemChiTiet;
        public BookSaleModel SachDuocChonXemChiTiet
        {
            get => _sachDuocChonXemChiTiet;
            set { _sachDuocChonXemChiTiet = value; OnPropertyChanged(); }
        }

        private List<BookSaleModel> _allBooks = new List<BookSaleModel>();
        public ObservableCollection<BookSaleModel> DanhSachSachHienThi { get; set; } = new ObservableCollection<BookSaleModel>();

        public ObservableCollection<CartItemModel> DanhSachGioHang { get; set; } = new ObservableCollection<CartItemModel>();

        private int _tongSoSach;
        public int TongSoSach
        {
            get => _tongSoSach;
            set { _tongSoSach = value; OnPropertyChanged(); }
        }

        private bool _isKhachVangLai;
        public bool IsKhachVangLai
        {
            get => _isKhachVangLai;
            set
            {
                _isKhachVangLai = value;
                OnPropertyChanged();
                if (_isKhachVangLai)
                {
                    _keywordKhachHang = string.Empty;
                    OnPropertyChanged(nameof(MaKhachHang));
                    TenKhachHang = "Khách vãng lai";
                    KhachHangDuocChon = null;
                }
                else
                {
                    TenKhachHang = "Chưa chọn khách hàng";
                }
            }
        }

        public string MaKhachHang
        {
            get => _keywordKhachHang;
            set
            {
                _keywordKhachHang = value;
                OnPropertyChanged();
                ThucHienTimKiemKhachHangAmTham();
            }
        }

        private string _keywordKhachHang = string.Empty;

        private string _tenKhachHang = "Chưa chọn khách hàng";
        public string TenKhachHang
        {
            get => _tenKhachHang;
            set { _tenKhachHang = value; OnPropertyChanged(); }
        }

        private CustomerResponse _khachHangDuocChon;
        public CustomerResponse KhachHangDuocChon
        {
            get => _khachHangDuocChon;
            set
            {
                _khachHangDuocChon = value;
                OnPropertyChanged();
                if (_khachHangDuocChon != null)
                {
                    TenKhachHang = _khachHangDuocChon.TenKhachHang;
                }
            }
        }

        private List<CustomerResponse> _allKhachHangs = new List<CustomerResponse>();

        public decimal TamTinh => DanhSachGioHang.Sum(item => item.ThanhTien);

        private int _phanTramGiam = 0;
        public int PhanTramGiam
        {
            get => _phanTramGiam;
            set
            {
                if (value < 0) _phanTramGiam = 0;
                else if (value > 100) _phanTramGiam = 100;
                else _phanTramGiam = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(TongTienThanhToan));
            }
        }

        public decimal TongTienThanhToan
        {
            get
            {
                decimal giamGia = TamTinh * ((decimal)PhanTramGiam / 100);
                return TamTinh - giamGia;
            }
        }

        private string _searchTextSach = string.Empty;
        public string SearchTextSach
        {
            get => _searchTextSach;
            set { _searchTextSach = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); }
        }

        private object _kieuTimKiemSach;
        public object KieuTimKiemSach
        {
            get => _kieuTimKiemSach;
            set { _kieuTimKiemSach = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); }
        }

        public ObservableCollection<CategoryModel> DanhSachTheLoai { get; set; } = new ObservableCollection<CategoryModel>();

        private CategoryModel _theLoaiDuocChon;
        public CategoryModel TheLoaiDuocChon
        {
            get => _theLoaiDuocChon;
            set { _theLoaiDuocChon = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); }
        }

        private object _locGiaSach;
        public object LocGiaSach
        {
            get => _locGiaSach;
            set { _locGiaSach = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> PageNumbers { get; set; } = new ObservableCollection<int>();

        // --- THUỘC TÍNH ĐIỀU KHIỂN TRẠNG THÁI POP-UP ---
        private bool _isConfirmingOrder;
        public bool IsConfirmingOrder
        {
            get => _isConfirmingOrder;
            set { _isConfirmingOrder = value; OnPropertyChanged(); }
        }

        private bool _isThanhToanTienMat = true; // Mặc định chọn tiền mặt
        public bool IsThanhToanTienMat
        {
            get => _isThanhToanTienMat;
            set { _isThanhToanTienMat = value; OnPropertyChanged(); }
        }

        private bool _isThanhToanChuyenKhoan;
        public bool IsThanhToanChuyenKhoan
        {
            get => _isThanhToanChuyenKhoan;
            set { _isThanhToanChuyenKhoan = value; OnPropertyChanged(); }
        }

        #endregion

        #region COMMANDS
        public ICommand MoPopupThanhToanCommand { get; set; }
        public ICommand XemChiTietSachCommand { get; set; }
        public ICommand ChonSachCommand { get; set; }
        public ICommand ThemVaoGioHangCommand { get; set; }
        public ICommand ThemVaoGioHangTuPopupCommand { get; set; }
        public ICommand TangSoLuongCommand { get; set; }
        public ICommand GiamSoLuongCommand { get; set; }
        public ICommand XoaKhoiGioHangCommand { get; set; }
        public ICommand XacNhanTaoDonCommand { get; set; }
        public ICommand HuyBoGiaoDichCommand { get; set; }
        public ICommand XoaBoLocCommand { get; set; }
        public ICommand FirstPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }

        #endregion

        #region CONSTRUCTOR

        public SaleViewModel()
        {
            LoadMockBookData();
            _ = LoadDanhSachKhachHangAsync();

            DanhSachGioHang.CollectionChanged += (s, e) => { CapNhatGiaTriHoaDon(); };

            #region CHỨC NĂNG BÁN HÀNG & GIỎ HÀNG KHỞI TẠO

            XemChiTietSachCommand = new RelayCommand<BookSaleModel>((selectedBook) => {
                if (selectedBook != null)
                {
                    SachDuocChonXemChiTiet = selectedBook;
                    DialogHost.OpenDialogCommand.Execute(null, null);
                }
            });

            ChonSachCommand = new RelayCommand<BookSaleModel>((book) => {
                if (book == null) return;

                var itemTrongGio = DanhSachGioHang.FirstOrDefault(i => i.ISBN == book.ISBN);

                if (book.IsSelected)
                {
                    if (itemTrongGio == null)
                    {
                        ThemSachVaoGioHangLogic(book);
                    }
                }
                else
                {
                    if (itemTrongGio != null)
                    {
                        DanhSachGioHang.Remove(itemTrongGio);
                    }
                }
                CapNhatGiaTriHoaDon();
            });

            ThemVaoGioHangCommand = new RelayCommand<BookSaleModel>((book) => { ThemSachVaoGioHangLogic(book); });

            ThemVaoGioHangTuPopupCommand = new RelayCommand<BookSaleModel>((book) => {
                if (book != null)
                {
                    ThemSachVaoGioHangLogic(book);
                    book.IsSelected = true;
                    DialogHost.CloseDialogCommand.Execute(null, null);
                }
            });

            TangSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua++; CapNhatGiaTriHoaDon(); }
            });

            GiamSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua--; CapNhatGiaTriHoaDon(); }
            });

            XoaKhoiGioHangCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null)
                {
                    DanhSachGioHang.Remove(item);

                    var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == item.ISBN);
                    if (originalBook != null)
                    {
                        originalBook.IsSelected = false;
                    }
                }
            });

            // Gán logic xử lý khi ấn nút Thanh Toán (Mở popup xác nhận)
            MoPopupThanhToanCommand = new RelayCommand<object>((param) => {
                if (DanhSachGioHang.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!IsKhachVangLai && KhachHangDuocChon == null)
                {
                    MessageBox.Show("Vui lòng nhập thông tin khách hàng hợp lệ hoặc tích chọn 'Khách vãng lai'!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                IsConfirmingOrder = true; // Chuyển giao diện popup sang Xác nhận đơn
                IsBookDetailOpen = true;   // Mở hộp thoại DialogHost
            });

            // Gán logic khi ấn HỦY BỎ GIAO DỊCH
            HuyBoGiaoDichCommand = new RelayCommand<object>((param) => {
                IsConfirmingOrder = false;
                IsBookDetailOpen = false;
            });

            // Gán logic khi ấn XÁC NHẬN TẠO ĐƠN
            XacNhanTaoDonCommand = new RelayCommand<object>((param) => {
                string phuongThuc = IsThanhToanTienMat ? "Tiền mặt" : "Chuyển khoản";
                MessageBox.Show($"Tạo đơn hàng thành công!\nPhương thức: {phuongThuc}\nTổng tiền: {TongTienThanhToan:N0} đ", "Thông báo");

                // Reset đơn hàng sau khi tạo thành công
                DanhSachGioHang.Clear();
                IsConfirmingOrder = false;
                IsBookDetailOpen = false;
            });
            #endregion

            #region CHỨC NĂNG LỌC & PHÂN TRANG KHỞI TẠO
            XoaBoLocCommand = new RelayCommand<object>((param) => {
                SearchTextSach = string.Empty;
                TheLoaiDuocChon = DanhSachTheLoai.FirstOrDefault(t => t.TenTheLoai == "Tất cả");
                CurrentPage = 1;
                ApplyFilterAndPagination();
            });

            FirstPageCommand = new RelayCommand<object>((p) => { CurrentPage = 1; ApplyFilterAndPagination(); });

            PrevPageCommand = new RelayCommand<object>((p) => {
                if (CurrentPage > 1) { CurrentPage--; ApplyFilterAndPagination(); }
            });

            NextPageCommand = new RelayCommand<object>((p) => {
                int totalPages = (int)Math.Ceiling((double)TongSoSach / ItemsPerPage);
                if (CurrentPage < totalPages) { CurrentPage++; ApplyFilterAndPagination(); }
            });

            LastPageCommand = new RelayCommand<object>((p) => {
                int totalPages = (int)Math.Ceiling((double)TongSoSach / ItemsPerPage);
                CurrentPage = totalPages > 0 ? totalPages : 1;
                ApplyFilterAndPagination();
            });

            GoToPageCommand = new RelayCommand<int>((page) => {
                CurrentPage = page;
                ApplyFilterAndPagination();
            });
            #endregion
        }
        #endregion

        #region HELPER METHODS

        private void CapNhatGiaTriHoaDon()
        {
            OnPropertyChanged(nameof(TamTinh));
            OnPropertyChanged(nameof(TongTienThanhToan));
        }

        private void ThemSachVaoGioHangLogic(BookSaleModel book)
        {
            if (book == null || book.SoLuongTonKho <= 0) return;

            var itemTrongGio = DanhSachGioHang.FirstOrDefault(i => i.ISBN == book.ISBN);
            if (itemTrongGio != null)
            {
                itemTrongGio.SoLuongMua++;
            }
            else
            {
                var newCartItem = new CartItemModel
                {
                    ISBN = book.ISBN,
                    TenSach = book.TenSach,
                    GiaBan = book.DonGiaBan,
                    SoLuongTonKho = book.SoLuongTonKho,
                    SoLuongMua = 1
                };
                newCartItem.PropertyChanged += (s, e) => {
                    if (e.PropertyName == nameof(CartItemModel.ThanhTien)) CapNhatGiaTriHoaDon();
                };
                DanhSachGioHang.Add(newCartItem);
            }

            book.IsSelected = true;
        }

        private void ThucHienTimKiemKhachHangAmTham()
        {
            if (string.IsNullOrWhiteSpace(MaKhachHang))
            {
                KhachHangDuocChon = null;
                TenKhachHang = IsKhachVangLai ? "Khách vãng lai" : "Chưa chọn khách hàng";
                return;
            }

            var query = MaKhachHang.ToLower().Trim();
            var khachHangFound = _allKhachHangs.FirstOrDefault(k =>
                (!string.IsNullOrEmpty(k.MaKhachHang) && k.MaKhachHang.ToLower() == query) ||
                (!string.IsNullOrEmpty(k.SoDienThoai) && k.SoDienThoai.Trim() == query)
            );

            if (khachHangFound != null)
            {
                KhachHangDuocChon = khachHangFound;
            }
            else
            {
                KhachHangDuocChon = null;
                TenKhachHang = "Không tìm thấy khách hàng!";
            }
        }

        private string GetComboBoxValue(object obj)
        {
            if (obj == null) return string.Empty;
            if (obj is System.Windows.Controls.ComboBoxItem item)
            {
                return item.Content?.ToString() ?? string.Empty;
            }
            return obj.ToString();
        }

        private void ApplyFilterAndPagination()
        {
            if (_allBooks == null) return;

            var filtered = _allBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTextSach))
            {
                string query = SearchTextSach.ToLower().Trim();
                string kieuTimKiem = GetComboBoxValue(KieuTimKiemSach);

                if (kieuTimKiem == "Mã ISBN")
                {
                    filtered = filtered.Where(b => b.ISBN != null && b.ISBN.ToLower().Contains(query));
                }
                else
                {
                    filtered = filtered.Where(b => b.TenSach != null && b.TenSach.ToLower().Contains(query));
                }
            }

            if (TheLoaiDuocChon != null && TheLoaiDuocChon.TenTheLoai != "Tất cả")
            {
                filtered = filtered.Where(b => b.TheLoai == TheLoaiDuocChon.TenTheLoai);
            }

            string locGia = GetComboBoxValue(LocGiaSach);
            if (!string.IsNullOrEmpty(locGia) && locGia != "Tất cả")
            {
                if (locGia == "Dưới 50.000 đ")
                    filtered = filtered.Where(b => b.DonGiaBan < 50000);
                else if (locGia == "50.000 - 150.000 đ")
                    filtered = filtered.Where(b => b.DonGiaBan >= 50000 && b.DonGiaBan <= 150000);
                else if (locGia == "150.000 - 300.000 đ")
                    filtered = filtered.Where(b => b.DonGiaBan >= 150000 && b.DonGiaBan <= 300000);
                else if (locGia == "Trên 300.000 đ")
                    filtered = filtered.Where(b => b.DonGiaBan > 300000);
            }

            var filteredList = filtered.ToList();
            TongSoSach = filteredList.Count;

            int totalPages = (int)Math.Ceiling((double)TongSoSach / ItemsPerPage);
            if (totalPages < 1) totalPages = 1;

            if (_currentPage > totalPages) _currentPage = totalPages;
            if (_currentPage < 1) _currentPage = 1;

            var pageItems = filteredList.Skip((_currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

            DanhSachSachHienThi.Clear();
            foreach (var item in pageItems)
            {
                item.IsSelected = DanhSachGioHang.Any(g => g.ISBN == item.ISBN);
                DanhSachSachHienThi.Add(item);
            }

            PageNumbers.Clear();
            for (int i = 1; i <= totalPages; i++)
            {
                PageNumbers.Add(i);
            }

            OnPropertyChanged(nameof(CurrentPage));
        }

        private async Task LoadDanhSachKhachHangAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");
                if (result != null && result.Count > 0)
                {
                    _allKhachHangs = result;
                    return;
                }
            }
            catch { }

            LoadMockCustomerData();
        }

        private void LoadMockBookData()
        {
            DanhSachTheLoai.Clear();
            DanhSachTheLoai.Add(new CategoryModel { TenTheLoai = "Tất cả" });
            DanhSachTheLoai.Add(new CategoryModel { TenTheLoai = "Tâm lý - Kỹ năng" });
            DanhSachTheLoai.Add(new CategoryModel { TenTheLoai = "Văn học" });
            DanhSachTheLoai.Add(new CategoryModel { TenTheLoai = "Khoa học" });
            TheLoaiDuocChon = DanhSachTheLoai[0];

            var mockApiData = new List<BookItem>
            {
                new BookItem { ISBN = "9786041183214", TenSach = "Đắc Nhân Tâm", DonGiaBan = 86000, SoLuongTonKho = 15, TheLoai = "Tâm lý - Kỹ năng" },
                new BookItem { ISBN = "9786045662142", TenSach = "Nhà Giả Kim", DonGiaBan = 79000, SoLuongTonKho = 5, TheLoai = "Văn học" },
                new BookItem { ISBN = "9786043351026", TenSach = "Sapiens: Lược Sử Loài Người", DonGiaBan = 165000, SoLuongTonKho = 20, TheLoai = "Khoa học" },
                new BookItem { ISBN = "9786041022353", TenSach = "Hạt Giống Tâm Hồn", DonGiaBan = 45000, SoLuongTonKho = 50, TheLoai = "Tâm lý - Kỹ năng" },
                new BookItem { ISBN = "9786049221151", TenSach = "Kỷ Luật Tự Giác", DonGiaBan = 99000, SoLuongTonKho = 2, TheLoai = "Tâm lý - Kỹ năng" },
                new BookItem { ISBN = "9786041022354", TenSach = "Cha Giàu Cha Nghèo", DonGiaBan = 120000, SoLuongTonKho = 12, TheLoai = "Tâm lý - Kỹ năng" }
            };

            _allBooks.Clear();
            foreach (var originalItem in mockApiData)
            {
                _allBooks.Add(new BookSaleModel
                {
                    BookData = originalItem,
                    IsSelected = false
                });
            }

            ApplyFilterAndPagination();
        }

        private void LoadMockCustomerData()
        {
            _allKhachHangs = new List<CustomerResponse>
            {
                new CustomerResponse { MaKhachHang = "KH001", TenKhachHang = "Lê Hoàng Quân", SoDienThoai = "0912345678" },
                new CustomerResponse { MaKhachHang = "KH002", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0987654321" },
                new CustomerResponse { MaKhachHang = "KH003", TenKhachHang = "Trần Thị B", SoDienThoai = "0905111222" }
            };
        }

        #endregion
    }
}
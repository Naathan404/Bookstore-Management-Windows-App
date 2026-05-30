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
        public decimal PromotionPrice { get; set; }
        private bool _hasPromotion;
        public bool HasPromotion
        {
            get => _hasPromotion;
            set { _hasPromotion = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); }
        }

        private bool _isGift;
        public bool IsGift
        {
            get => _isGift;
            set { _isGift = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); }
        }

        public bool IsNormalPrice => !HasPromotion && !IsGift;
    }

    public class CartItemModel : BaseViewModel
    {
        public string ISBN { get; set; }
        public string TenSach { get; set; }
        private decimal _giaBan;
        public decimal GiaBan { get => _giaBan; set { _giaBan = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThanhTien)); } }

        private decimal _originalGiaBan;
        public decimal OriginalGiaBan { get => _originalGiaBan; set { _originalGiaBan = value; OnPropertyChanged(); } }

        public int SoLuongTonKho { get; set; }

        private bool _isGift;
        public bool IsGift { get => _isGift; set { _isGift = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); } }

        private bool _isPromotionApplied;
        public bool IsPromotionApplied { get => _isPromotionApplied; set { _isPromotionApplied = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); } }

        public bool IsNormalPrice => !IsGift && !IsPromotionApplied;

        private int _soLuongMua;
        public int SoLuongMua
        {
            get => _soLuongMua;
            set
            {
                if (_soLuongMua != value)
                {
                    if (value > SoLuongTonKho) _soLuongMua = SoLuongTonKho;
                    else if (value < 0) _soLuongMua = 0; // allow 0 so parent VM can remove item from cart
                    else _soLuongMua = value;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public decimal ThanhTien => GiaBan * _soLuongMua;
    }

    // Simple promotion representation used by SaleViewModel
    public class Promotion
    {
        public string Code { get; set; }
        // Applicable customer type: "Tất cả", "VIP", "Thành viên", "Khách vãng lai"
        public string ApplicableCustomerType { get; set; } = "Tất cả";
        // If AppliesToISBN is set, promotion targets a specific book
        public string AppliesToISBN { get; set; }
        public bool IsGift { get; set; }
        public int DiscountPercent { get; set; }
        // Optional: a promotion can be triggered when buying a specific ISBN and gift a different ISBN
        public string TriggerISBN { get; set; }
        public string GiftISBN { get; set; }

        // Optional time window for the promotion
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
                    _sdtKhachHang = string.Empty;
                    OnPropertyChanged(nameof(SdtKhachHang));
                    TenKhachHang = "Khách vãng lai";
                    KhachHangDuocChon = null;
                }
                else
                {
                    TenKhachHang = "Chưa chọn khách hàng";
                }
                CommandManager.InvalidateRequerySuggested(); // Cập nhật lại nút thanh toán
                // Apply promotions that depend on customer type
                if (!string.IsNullOrEmpty(MaUuDai)) ApplyPromotionByCode(MaUuDai);
            }
        }

        // Thay thế thuộc tính MaKhachHang cũ bằng thuộc tính này để khớp hoàn toàn với Binding XAML
        private string _sdtKhachHang = string.Empty;
        public string SdtKhachHang
        {
            get => _sdtKhachHang;
            set
            {
                _sdtKhachHang = value;
                OnPropertyChanged();
                ThucHienTimKiemKhachHangAmTham();
            }
        }

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
                CommandManager.InvalidateRequerySuggested(); // Cập nhật lại nút thanh toán
            }
        }

        private List<CustomerResponse> _allKhachHangs = new List<CustomerResponse>();

        // Promotions
        private List<Promotion> _availablePromotions = new List<Promotion>();
        private Promotion _appliedPromotion = null;

        private string _maUuDai = string.Empty;
        public string MaUuDai
        {
            get => _maUuDai;
            set
            {
                _maUuDai = value?.Trim() ?? string.Empty;
                OnPropertyChanged();
                ApplyPromotionByCode(_maUuDai);
            }
        }

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
                OnPropertyChanged(nameof(GiamTien));
            }
        }

        public decimal GiamTien => TamTinh * ((decimal)PhanTramGiam / 100);

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
        public ObservableCollection<string> DanhSachKhoangGia { get; set; } = new ObservableCollection<string>
        {
            "Tất cả",
            "Dưới 50.000 đ",
            "50.000 - 150.000 đ",
            "150.000 - 300.000 đ",
            "Trên 300.000 đ"
        };
        private object _locGiaSach = "Tất cả";
        public object LocGiaSach
        {
            get => _locGiaSach;
            set { if (_locGiaSach != value) { _locGiaSach = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); } }
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
        public ICommand CloseDialogCommand { get; set; }
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

            DanhSachGioHang.CollectionChanged += (s, e) => { CapNhatGiaTriHoaDon(); ApplyAutoPromotions(); CommandManager.InvalidateRequerySuggested(); };

            #region CHỨC NĂNG BÁN HÀNG & GIỎ HÀNG KHỞI TẠO

            // 1. Sửa chức năng xem chi tiết: gán trực tiếp thuộc tính kích hoạt bảng phủ overlay
            XemChiTietSachCommand = new RelayCommand<BookSaleModel>((selectedBook) => {
                if (selectedBook != null)
                {
                    SachDuocChonXemChiTiet = selectedBook;
                    IsBookDetailOpen = true; 
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
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
                else
                {
                    if (itemTrongGio != null)
                    {
                        DanhSachGioHang.Remove(itemTrongGio);
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
                CapNhatGiaTriHoaDon();
            });

            ThemVaoGioHangCommand = new RelayCommand<BookSaleModel>((book) => { ThemSachVaoGioHangLogic(book); CommandManager.InvalidateRequerySuggested(); });

            ThemVaoGioHangTuPopupCommand = new RelayCommand<BookSaleModel>((book) => {
                if (book != null)
                {
                    ThemSachVaoGioHangLogic(book);
                    book.IsSelected = true;
                    IsBookDetailOpen = false; // Đóng bảng phủ chi tiết
                    CommandManager.InvalidateRequerySuggested();
                }
            });

            TangSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua++; CapNhatGiaTriHoaDon(); CommandManager.InvalidateRequerySuggested(); }
            });

            GiamSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua--; CapNhatGiaTriHoaDon(); CommandManager.InvalidateRequerySuggested(); }
            });

            XoaKhoiGioHangCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null)
                {
                    DanhSachGioHang.Remove(item);
                    var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == item.ISBN);
                    if (originalBook != null) originalBook.IsSelected = false;
                    CommandManager.InvalidateRequerySuggested();
                }
            });

            // 2. SỬA TẠI ĐÂY: Chuyển sang RelayCommand thường (không generic) để loại bỏ lỗi chặn nút bấm do Null parameter
            MoPopupThanhToanCommand = new RelayCommand(
                () => // Execute
                {
                    if (DanhSachGioHang == null || DanhSachGioHang.Count == 0)
                    {
                        MessageBox.Show("Giỏ hàng đang trống!", "Thông báo");
                        return;
                    }

                    bool hopLeKhachHang = IsKhachVangLai || KhachHangDuocChon != null;
                    if (!hopLeKhachHang)
                    {
                        MessageBox.Show("Vui lòng chọn thông tin khách hàng!", "Thông báo");
                        return;
                    }

                    IsConfirmingOrder = true; // Kích hoạt mở popup xác nhận thanh toán công nghệ mới
                },
                () => // CanExecute
                {
                    return DanhSachGioHang != null && DanhSachGioHang.Count > 0 && (IsKhachVangLai || KhachHangDuocChon != null);
                }
            );

            // Sửa các nút điều khiển popup đóng mở về RelayCommand thường không tham số
            CloseDialogCommand = new RelayCommand(() => {
                IsBookDetailOpen = false;
                IsConfirmingOrder = false;
            });

            HuyBoGiaoDichCommand = new RelayCommand(() => {
                IsConfirmingOrder = false;
            });

            XacNhanTaoDonCommand = new RelayCommand(() => {
                string phuongThuc = IsThanhToanTienMat ? "Tiền mặt" : "Chuyển khoản";
                MessageBox.Show($"Tạo đơn hàng thành công!\nPhương thức: {phuongThuc}\nTổng tiền: {TongTienThanhToan:N0} đ", "Thông báo");

                DanhSachGioHang.Clear();
                IsConfirmingOrder = false;
                IsBookDetailOpen = false;
                CommandManager.InvalidateRequerySuggested();
            });
            #endregion

            #region CHỨC NĂNG LỌC & PHÂN TRANG KHỞI TẠO (Chuyển về RelayCommand thường)
            XoaBoLocCommand = new RelayCommand(() => {
                SearchTextSach = string.Empty;
                TheLoaiDuocChon = DanhSachTheLoai.FirstOrDefault(t => t.TenTheLoai == "Tất cả");
                LocGiaSach = "Tất cả";
                CurrentPage = 1;
                ApplyFilterAndPagination();
            });

            FirstPageCommand = new RelayCommand(() => { CurrentPage = 1; ApplyFilterAndPagination(); });

            PrevPageCommand = new RelayCommand(() => {
                if (CurrentPage > 1) { CurrentPage--; ApplyFilterAndPagination(); }
            });

            NextPageCommand = new RelayCommand(() => {
                int totalPages = (int)Math.Ceiling((double)TongSoSach / ItemsPerPage);
                if (CurrentPage < totalPages) { CurrentPage++; ApplyFilterAndPagination(); }
            });

            LastPageCommand = new RelayCommand(() => {
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
            // Also update enable state of payment-related UI
            OnPropertyChanged(nameof(IsThanhToanEnabled));
            OnPropertyChanged(nameof(GiamTien));
        }
        // Thêm thuộc tính này vào SaleViewModel
        public bool IsThanhToanEnabled
        {
            get => DanhSachGioHang != null && DanhSachGioHang.Count > 0 && (IsKhachVangLai || KhachHangDuocChon != null);
        }

        // Quan trọng: Mỗi khi bạn thêm hoặc xóa sách khỏi DanhSachGioHang, 
        // bạn phải thông báo cho giao diện cập nhật thuộc tính này:
        public void CapNhatTrangThaiNut()
        {
            OnPropertyChanged(nameof(IsThanhToanEnabled));
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
                    OriginalGiaBan = book.DonGiaBan,
                    IsGift = false,
                    IsPromotionApplied = false,
                    SoLuongTonKho = book.SoLuongTonKho,
                    SoLuongMua = 1
                };
                newCartItem.PropertyChanged += (s, e) => {
                    // Update totals when line item total changes
                    if (e.PropertyName == nameof(CartItemModel.ThanhTien))
                    {
                        CapNhatGiaTriHoaDon();
                    }

                    // If quantity becomes less than 1, remove the item from cart
                    if (e.PropertyName == nameof(CartItemModel.SoLuongMua))
                    {
                        var ci = s as CartItemModel;
                        if (ci != null && ci.SoLuongMua < 1)
                        {
                            // Remove and update selection state
                            DanhSachGioHang.Remove(ci);
                            var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == ci.ISBN);
                            if (originalBook != null) originalBook.IsSelected = false;
                            CapNhatGiaTriHoaDon();
                            CommandManager.InvalidateRequerySuggested();
                        }
                    }
                };
                // If there's a promotion that applies and it's a gift for this ISBN, adjust
                var promoForBook = _availablePromotions.FirstOrDefault(p => !string.IsNullOrEmpty(p.AppliesToISBN) && p.AppliesToISBN == book.ISBN);
                if (promoForBook != null && promoForBook.IsGift)
                {
                    newCartItem.IsGift = true;
                    newCartItem.GiaBan = 0;
                    newCartItem.TenSach += " (HÀNG TẶNG)";
                }
                DanhSachGioHang.Add(newCartItem);
            }

            book.IsSelected = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void ApplyPromotionByCode(string code)
        {
            _appliedPromotion = null;
            if (string.IsNullOrWhiteSpace(code))
            {
                // Reset any promotion pricing
                foreach (var c in DanhSachGioHang)
                {
                    if (c.IsPromotionApplied || c.IsGift)
                    {
                        c.GiaBan = c.OriginalGiaBan;
                        c.IsPromotionApplied = false;
                        c.IsGift = false;
                        if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)"))
                            c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");
                    }
                }
                CapNhatGiaTriHoaDon();
                return;
            }

            var promo = _availablePromotions.FirstOrDefault(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            if (promo == null) return;

            // Check time window if defined
            if (promo.StartDate.HasValue && promo.EndDate.HasValue)
            {
                var now = DateTime.Now.Date;
                if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date) return;
            }

            _appliedPromotion = promo;
            // If promotion defines a global percent discount (no AppliesToISBN), apply as PhanTramGiam
            if (promo.DiscountPercent > 0 && string.IsNullOrEmpty(promo.AppliesToISBN))
            {
                PhanTramGiam = promo.DiscountPercent;
            }
            else if (promo.DiscountPercent == 0)
            {
                // reset global discount if none
                PhanTramGiam = 0;
            }
            // Apply promotion to displayed books and cart based on promotion definition and customer type
            ApplyPromotionToDisplayedBooks(promo);

            foreach (var c in DanhSachGioHang)
            {
                // Reset before applying
                c.GiaBan = c.OriginalGiaBan;
                c.IsPromotionApplied = false;
                c.IsGift = false;
                if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");

                // Check customer applicability
                bool customerMatches = promo.ApplicableCustomerType == "Tất cả" ||
                    (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                    (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                    (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

                if (!customerMatches) continue;

                // If the promo is triggered by buying a particular ISBN, ensure the trigger exists in cart
                if (!string.IsNullOrEmpty(promo.TriggerISBN) && !DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN)) continue;

                if (!string.IsNullOrEmpty(promo.AppliesToISBN) && promo.AppliesToISBN == c.ISBN)
                {
                    if (promo.IsGift)
                    {
                        c.IsGift = true;
                        c.GiaBan = 0;
                        c.TenSach += " (HÀNG TẶNG)";
                    }
                    else if (promo.DiscountPercent > 0)
                    {
                        c.IsPromotionApplied = true;
                        c.GiaBan = Math.Round(c.OriginalGiaBan * (100 - promo.DiscountPercent) / 100);
                    }
                }
                // If promotion specifies GiftISBN and a TriggerISBN is present in cart (and gift differs),
                // add the gift item to the cart if it's not already present.
                if (!string.IsNullOrEmpty(promo.GiftISBN) && !string.IsNullOrEmpty(promo.TriggerISBN) && promo.TriggerISBN != promo.GiftISBN)
                {
                    var triggerExists = DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN);
                    var giftExists = DanhSachGioHang.Any(x => x.ISBN == promo.GiftISBN);
                    if (triggerExists && !giftExists)
                    {
                        var book = _allBooks.FirstOrDefault(b => b.ISBN == promo.GiftISBN);
                        if (book != null)
                        {
                            var newCartItem = new CartItemModel
                            {
                                ISBN = book.ISBN,
                                TenSach = book.TenSach + " (HÀNG TẶNG)",
                                GiaBan = 0,
                                OriginalGiaBan = book.DonGiaBan,
                                SoLuongTonKho = book.SoLuongTonKho,
                                SoLuongMua = 1,
                                IsGift = true
                            };
                            DanhSachGioHang.Add(newCartItem);
                            var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == newCartItem.ISBN);
                            if (originalBook != null) originalBook.IsSelected = true;
                        }
                    }
                }
            }
            CapNhatGiaTriHoaDon();
        }

        private void ApplyPromotionToDisplayedBooks(Promotion promo)
        {
            if (promo == null)
            {
                // reset
                foreach (var b in _allBooks)
                {
                    b.HasPromotion = false;
                    b.IsGift = false;
                    b.PromotionPrice = 0;
                }
                return;
            }

            bool customerMatches = promo.ApplicableCustomerType == "Tất cả" ||
                (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

            foreach (var b in _allBooks)
            {
                b.HasPromotion = false;
                b.IsGift = false;
                b.PromotionPrice = 0;
                if (customerMatches && !string.IsNullOrEmpty(promo.AppliesToISBN) && promo.AppliesToISBN == b.ISBN)
                {
                    if (promo.IsGift)
                    {
                        b.IsGift = true;
                    }
                    else if (promo.DiscountPercent > 0)
                    {
                        b.HasPromotion = true;
                        b.PromotionPrice = Math.Round(b.DonGiaBan * (100 - promo.DiscountPercent) / 100);
                    }
                }
            }

            // If promotion is gift for a particular ISBN, ensure it's present in cart as gift
            if (promo.IsGift && !string.IsNullOrEmpty(promo.AppliesToISBN))
            {
                var existing = DanhSachGioHang.FirstOrDefault(c => c.ISBN == promo.AppliesToISBN);
                if (existing == null)
                {
                    var book = _allBooks.FirstOrDefault(b => b.ISBN == promo.AppliesToISBN);
                    if (book != null)
                    {
                        var newCartItem = new CartItemModel
                        {
                            ISBN = book.ISBN,
                            TenSach = book.TenSach + " (HÀNG TẶNG)",
                            GiaBan = 0,
                            OriginalGiaBan = book.DonGiaBan,
                            SoLuongTonKho = book.SoLuongTonKho,
                            SoLuongMua = 1,
                            IsGift = true
                        };
                        DanhSachGioHang.Add(newCartItem);
                        var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == newCartItem.ISBN);
                        if (originalBook != null) originalBook.IsSelected = true;
                    }
                }
            }

            // refresh visible list
            ApplyFilterAndPagination();
            CapNhatGiaTriHoaDon();
        }

        // Check and apply any promotions that should trigger automatically when cart changes
        private void ApplyAutoPromotions()
        {
            if (_availablePromotions == null || !_availablePromotions.Any()) return;

            // For each promotion that has a TriggerISBN or is a gift, try to apply its effects
            foreach (var promo in _availablePromotions)
            {
                // time window check
                if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                {
                    var now = DateTime.Now.Date;
                    if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date) continue;
                }

                // If promo requires a trigger ISBN, ensure trigger exists in cart
                if (!string.IsNullOrEmpty(promo.TriggerISBN))
                {
                    var triggerInCart = DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN);
                    if (!triggerInCart) continue;

                    // If promo has a GiftISBN, ensure gift is added
                    if (!string.IsNullOrEmpty(promo.GiftISBN))
                    {
                        var giftExists = DanhSachGioHang.Any(x => x.ISBN == promo.GiftISBN);
                        if (!giftExists)
                        {
                            var book = _allBooks.FirstOrDefault(b => b.ISBN == promo.GiftISBN);
                            if (book != null)
                            {
                                var newCartItem = new CartItemModel
                                {
                                    ISBN = book.ISBN,
                                    TenSach = book.TenSach + " (HÀNG TẶNG)",
                                    GiaBan = 0,
                                    OriginalGiaBan = book.DonGiaBan,
                                    SoLuongTonKho = book.SoLuongTonKho,
                                    SoLuongMua = 1,
                                    IsGift = true
                                };
                                DanhSachGioHang.Add(newCartItem);
                                var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == newCartItem.ISBN);
                                if (originalBook != null) originalBook.IsSelected = true;
                            }
                        }
                    }
                }
            }
            // Re-apply any currently entered code-based promotion
            if (!string.IsNullOrEmpty(MaUuDai)) ApplyPromotionByCode(MaUuDai);
            else
            {
                // If no code, re-evaluate display promotions
                foreach (var p in _availablePromotions) ApplyPromotionToDisplayedBooks(p);
            }
        }

        private void ThucHienTimKiemKhachHangAmTham()
        {
            if (string.IsNullOrWhiteSpace(SdtKhachHang))
            {
                KhachHangDuocChon = null;
                TenKhachHang = IsKhachVangLai ? "Khách vãng lai" : "Chưa chọn khách hàng";
                CommandManager.InvalidateRequerySuggested();
                return;
            }

            var query = SdtKhachHang.ToLower().Trim();
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
            CommandManager.InvalidateRequerySuggested();
            // Apply promotions that depend on customer type or auto-apply code
            if (!string.IsNullOrEmpty(MaUuDai)) ApplyPromotionByCode(MaUuDai);
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
            LoadMockPromotions();
        }

        private void LoadMockPromotions()
        {
            // Sample promotions demonstrating: gift, time-bound discount, and customer-type discount
            _availablePromotions = new List<Promotion>
            {
                // Buy Sapiens (9786043351026) get a small book free (mock ISBN 9786000000001)
                new Promotion { Code = "GIFT-SAPIENS", TriggerISBN = "9786043351026", GiftISBN = "9786000000001", IsGift = true, ApplicableCustomerType = "Tất cả" },

                // VIP customers get 10% off Sapiens
                new Promotion { Code = "VIP10", DiscountPercent = 10, AppliesToISBN = "9786043351026", ApplicableCustomerType = "VIP" },

                // Discount on "Nhà Giả Kim" (9786045662142) - 20% off for all customers
                new Promotion { Code = "BOOK-NGK20", DiscountPercent = 20, AppliesToISBN = "9786045662142", ApplicableCustomerType = "Tất cả" },

                // Promotion for guest customers: 5% off total bill
                new Promotion { Code = "GUEST5", DiscountPercent = 5, ApplicableCustomerType = "Khách vãng lai" },

                // Sitewide 15% off during a promo window
                new Promotion { Code = "SUMMER15", DiscountPercent = 15, ApplicableCustomerType = "Tất cả", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(7) }
            };
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
                new CustomerResponse { MaKhachHang = "KH001", TenKhachHang = "Lê Hoàng Quân", SoDienThoai = "0912345678", LoaiKhach = "VIP" },
                new CustomerResponse { MaKhachHang = "KH002", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0987654321", LoaiKhach = "Thành viên" },
                new CustomerResponse { MaKhachHang = "KH003", TenKhachHang = "Trần Thị B", SoDienThoai = "0905111222", LoaiKhach = "Khách thường" }
            };
        }

        #endregion
    }
}
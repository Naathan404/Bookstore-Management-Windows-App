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
    public class CategoryModel
    {
        public string TenTheLoai { get; set; }
    }

 
    public class BookSaleModel : BaseViewModel
    {
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
        private decimal _promotionPrice;
        public decimal PromotionPrice { 
            get => _promotionPrice;
            set
            {
                if (_promotionPrice != value)
                {
                    _promotionPrice = value;
                    OnPropertyChanged(); // Thông báo giao diện WPF cập nhật ngay lập tức
                }
            }
        }
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
        public decimal GiaBan 
        { 
            get => _giaBan; 
            set 
            { 
                if (_giaBan != value)
                {
                    _giaBan = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThanhTien));
                }
            } 
        }

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
                    else if (value < 0) _soLuongMua = 0; 
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

        // Biến cờ hiệu bảo vệ chống vòng lặp đệ quy vô hạn ngầm khi tự động cập nhật giỏ hàng
        private bool _isUpdatingCart = false;

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

                    // Auto-apply any running promotion for guest customers
                    var promo = _availablePromotions.FirstOrDefault(p =>
                        p.ApplicableCustomerType == "Khách vãng lai" &&
                        (!p.StartDate.HasValue || !p.EndDate.HasValue || (DateTime.Now.Date >= p.StartDate.Value.Date && DateTime.Now.Date <= p.EndDate.Value.Date)));
                    MaUuDai = promo?.Code ?? string.Empty;
                }
                else
                {
                    TenKhachHang = "Chưa chọn khách hàng";
                    // If turning off guest, clear guest-only promo if it was set
                    if (!string.IsNullOrEmpty(MaUuDai))
                    {
                        var currentPromo = _availablePromotions.FirstOrDefault(p => p.Code.Equals(MaUuDai, StringComparison.OrdinalIgnoreCase));
                        if (currentPromo != null && currentPromo.ApplicableCustomerType == "Khách vãng lai") MaUuDai = string.Empty;
                    }
                }
                CommandManager.InvalidateRequerySuggested();
                // MaUuDai setter will call ApplyPromotionByCode for us
            }
        }

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
                    // Auto-apply promotion for this customer type if available
                    var promo = _availablePromotions.FirstOrDefault(p =>
                        p.ApplicableCustomerType != null &&
                        (p.ApplicableCustomerType == "Tất cả" || p.ApplicableCustomerType == _khachHangDuocChon.LoaiKhach) &&
                        (!p.StartDate.HasValue || !p.EndDate.HasValue || (DateTime.Now.Date >= p.StartDate.Value.Date && DateTime.Now.Date <= p.EndDate.Value.Date)));
                    if (promo != null) MaUuDai = promo.Code;
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
                var newVal = value?.Trim() ?? string.Empty;

                // If not a guest and no customer selected, disallow entering a promo code
                if (!IsKhachVangLai && KhachHangDuocChon == null && !string.IsNullOrEmpty(newVal))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng trước khi nhập mã ưu đãi.", "Thiếu thông tin khách hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _maUuDai = string.Empty;
                    OnPropertyChanged();
                    return;
                }

                _maUuDai = newVal;
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

        public List<string> ListKieuTimKiem { get; set; } = new List<string>
        {
            "Tên sách",
            "Mã ISBN",
            "Tác giả"
        };
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
            // Ensure promotions are loaded first so books/customers can be adjusted based on active promos
            _ = InitializeAsync();

            // SỬA: Thêm cờ bảo vệ _isUpdatingCart để chặn lặp đệ quy gây đóng băng luồng dữ liệu
            DanhSachGioHang.CollectionChanged += (s, e) => {
                // Defer handling to the dispatcher to avoid modifying the collection while it's raising CollectionChanged
                var dispatcher = Application.Current?.Dispatcher ?? System.Windows.Threading.Dispatcher.CurrentDispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_isUpdatingCart) return;
                // After collection changed, ensure aggregated promotions are (re)applied
                ApplyPromotionsAggregate();
                CapNhatGiaTriHoaDon();
                    try { ApplyAutoPromotions(); } catch { }
                    CommandManager.InvalidateRequerySuggested();
                }), System.Windows.Threading.DispatcherPriority.Background);
            };

            #region CHỨC NĂNG BÁN HÀNG & GIỎ HÀNG KHỞI TẠO

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
                        // Remove the selected item
                        DanhSachGioHang.Remove(itemTrongGio);
                        // Also remove any gift items that were added because of this trigger ISBN
                        var promosTriggered = _availablePromotions.Where(p => !string.IsNullOrEmpty(p.TriggerISBN) && p.TriggerISBN == itemTrongGio.ISBN && !string.IsNullOrEmpty(p.GiftISBN)).ToList();
                        if (promosTriggered.Any())
                        {
                            _isUpdatingCart = true;
                            try
                            {
                                foreach (var promo in promosTriggered)
                                {
                                    var giftItem = DanhSachGioHang.FirstOrDefault(x => x.ISBN == promo.GiftISBN && x.IsGift);
                                    if (giftItem != null) DanhSachGioHang.Remove(giftItem);
                                    var giftBook = _allBooks.FirstOrDefault(b => b.ISBN == promo.GiftISBN);
                                    if (giftBook != null) giftBook.IsSelected = false;
                                }
                            }
                            finally { _isUpdatingCart = false; }
                        }
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
                    IsBookDetailOpen = false;
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
                if (item == null) return;

                // 1. Xóa sản phẩm chính được chọn khỏi giỏ hàng
                DanhSachGioHang.Remove(item);

                // Khôi phục trạng thái chọn trên danh sách hiển thị sách của sản phẩm vừa xóa
                var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == item.ISBN);
                if (originalBook != null) originalBook.IsSelected = false;

                // 2. Tìm các chương trình khuyến mãi tặng quà phụ thuộc vào quyển sách vừa bị xóa này
                var promosTriggered = _availablePromotions
                    .Where(p => !string.IsNullOrEmpty(p.TriggerISBN) && p.TriggerISBN == item.ISBN && !string.IsNullOrEmpty(p.GiftISBN))
                    .ToList();

                if (promosTriggered.Any())
                {
                    // Sử dụng cờ bảo vệ để tránh việc CollectionChanged kích hoạt tính toán gộp đệ quy trong lúc đang xóa
                    _isUpdatingCart = true;
                    try
                    {
                        foreach (var promo in promosTriggered)
                        {
                            // KIỂM TRA CHÉO: Xem trong giỏ hàng hiện tại còn quyển sách nào khác cũng kích hoạt món quà này không
                            bool stillHasOtherTrigger = _availablePromotions.Any(p =>
                                p.GiftISBN == promo.GiftISBN &&
                                p.TriggerISBN != item.ISBN &&
                                DanhSachGioHang.Any(x => x.ISBN == p.TriggerISBN && x.SoLuongMua > 0));

                            // Nếu không còn bất cứ sách điều kiện nào khác giữ món quà này, tiến hành gỡ bỏ quà
                            if (!stillHasOtherTrigger)
                            {
                                // Tìm món quà thực tế đang nằm trong giỏ hàng để xóa
                                var giftItem = DanhSachGioHang.FirstOrDefault(x => x.ISBN == promo.GiftISBN && x.IsGift);
                                if (giftItem != null)
                                {
                                    DanhSachGioHang.Remove(giftItem);
                                }

                                // KHẮC PHỤC LỖI HIỂN THỊ UI: Khôi phục hoàn toàn trạng thái sách ngoài danh sách hiển thị chính
                                var giftBook = _allBooks.FirstOrDefault(b => b.ISBN == promo.GiftISBN);
                                if (giftBook != null)
                                {
                                    giftBook.IsSelected = false;
                                    giftBook.IsGift = false;                         // RESET: Bỏ nhãn quà tặng
                                    giftBook.HasPromotion = false;                    // RESET: Bỏ trạng thái giảm giá
                                    giftBook.PromotionPrice = giftBook.DonGiaBan;     // RESET: Trả lại giá gốc ban đầu
                                }
                            }
                        }
                    }
                    finally
                    {
                        _isUpdatingCart = false;
                    }
                }

                CommandManager.InvalidateRequerySuggested();
            });

            MoPopupThanhToanCommand = new RelayCommand(
                () =>
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

                    // If a promo code is present, validate it against date and customer type before allowing payment
                    if (!string.IsNullOrEmpty(MaUuDai))
                    {
                        var promo = _availablePromotions.FirstOrDefault(p => p.Code.Equals(MaUuDai, StringComparison.OrdinalIgnoreCase));
                        if (promo == null)
                        {
                            MessageBox.Show("Mã ưu đãi không hợp lệ.", "Lỗi mã ưu đãi", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                        {
                            var now = DateTime.Now.Date;
                            if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date)
                            {
                                MessageBox.Show("Mã ưu đãi chưa trong thời gian áp dụng.", "Lỗi mã ưu đãi", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }

                        bool customerMatches = promo.ApplicableCustomerType == "Tất cả" ||
                            (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                            (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                            (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

                        if (!customerMatches)
                        {
                            MessageBox.Show("Mã ưu đãi chưa đúng với loại khách hàng.", "Lỗi mã ưu đãi", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    IsConfirmingOrder = true;
                },
                () =>
                {
                    return DanhSachGioHang != null && DanhSachGioHang.Count > 0 && (IsKhachVangLai || KhachHangDuocChon != null);
                }
            );

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

            #region CHỨC NĂNG LỌC & PHÂN TRANG KHỞI TẠO
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
            OnPropertyChanged(nameof(IsThanhToanEnabled));
            OnPropertyChanged(nameof(GiamTien));
        }

        public bool IsThanhToanEnabled
        {
            get => DanhSachGioHang != null && DanhSachGioHang.Count > 0 && (IsKhachVangLai || KhachHangDuocChon != null);
        }

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
                    OriginalGiaBan = book.DonGiaBan,
                    SoLuongTonKho = book.SoLuongTonKho,
                    SoLuongMua = 1
                };

                // If displayed book has a promotion or is a gift, set initial GiaBan accordingly
                if (book.IsGift)
                {
                    newCartItem.IsGift = true;
                    newCartItem.GiaBan = 0;
                    newCartItem.TenSach += " (HÀNG TẶNG)";
                }
                else if (book.HasPromotion && book.PromotionPrice > 0)
                {
                    newCartItem.IsPromotionApplied = true;
                    newCartItem.GiaBan = book.PromotionPrice;
                }
                else
                {
                    newCartItem.GiaBan = book.DonGiaBan;
                }
                newCartItem.PropertyChanged += (s, e) => {
                    if (e.PropertyName == nameof(CartItemModel.ThanhTien))
                    {
                        CapNhatGiaTriHoaDon();
                    }

                    if (e.PropertyName == nameof(CartItemModel.SoLuongMua))
                    {
                        var ci = s as CartItemModel;
                        if (ci != null && ci.SoLuongMua < 1)
                        {
                            DanhSachGioHang.Remove(ci);
                            var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == ci.ISBN);
                            if (originalBook != null) originalBook.IsSelected = false;
                            CapNhatGiaTriHoaDon();
                            CommandManager.InvalidateRequerySuggested();
                        }
                    }
                };

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
            // After adding the book, check for promotions that use this book as a trigger and add gift items if needed
            var triggerPromos = _availablePromotions.Where(p => !string.IsNullOrEmpty(p.TriggerISBN) && p.TriggerISBN == book.ISBN && !string.IsNullOrEmpty(p.GiftISBN)).ToList();
            if (triggerPromos.Any())
            {
                _isUpdatingCart = true;
                try
                {
                    foreach (var promo in triggerPromos)
                    {
                        // ensure promo is currently active
                        if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                        {
                            var now = DateTime.Now.Date;
                            if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date) continue;
                        }

                        var giftExists = DanhSachGioHang.Any(x => x.ISBN == promo.GiftISBN);
                        if (giftExists) continue;

                        var giftBook = _allBooks.FirstOrDefault(b => b.ISBN == promo.GiftISBN);
                        if (giftBook == null) continue;

                        var giftCartItem = new CartItemModel
                        {
                            ISBN = giftBook.ISBN,
                            TenSach = giftBook.TenSach + " (HÀNG TẶNG)",
                            GiaBan = 0,
                            OriginalGiaBan = giftBook.DonGiaBan,
                            SoLuongTonKho = giftBook.SoLuongTonKho,
                            SoLuongMua = 1,
                            IsGift = true
                        };
                        DanhSachGioHang.Add(giftCartItem);
                        giftBook.IsSelected = true;
                    }
                }
                finally { _isUpdatingCart = false; }
            }
            CommandManager.InvalidateRequerySuggested();
        }

        // Apply a set of promotions as an aggregate to displayed books and cart items
        private void ApplyPromotionsAggregate(IEnumerable<Promotion> promos = null)
        {
            // Đồng bộ lại mã ưu đãi nhập tay nếu biến quản lý trạng thái _appliedPromotion đang bị null
            if (!string.IsNullOrEmpty(MaUuDai) && _appliedPromotion == null)
            {
                _appliedPromotion = (_availablePromotions ?? new List<Promotion>())
                    .FirstOrDefault(p => p.Code.Equals(MaUuDai, StringComparison.OrdinalIgnoreCase));
            }

            // Xác định nguồn chương trình khuyến mãi cần quét
            var sourcePromos = promos ?? _availablePromotions ?? Enumerable.Empty<Promotion>();
            if (_appliedPromotion != null)
            {
                // Nếu đang áp dụng mã giảm giá cụ thể, ưu tiên sử dụng duy nhất mã đó
                sourcePromos = new List<Promotion> { _appliedPromotion };
            }

            // Lọc danh sách các chương trình khuyến mãi đang còn hạn và phù hợp với loại khách hàng
            var activePromos = sourcePromos
                .Where(p =>
                {
                    if (p == null) return false;
                    if (p.StartDate.HasValue && p.EndDate.HasValue)
                    {
                        var now = DateTime.Now.Date;
                        if (now < p.StartDate.Value.Date || now > p.EndDate.Value.Date) return false;
                    }
                    // Kiểm tra điều kiện nhóm khách hàng hợp lệ
                    bool custMatch = p.ApplicableCustomerType == "Tất cả" ||
                        (p.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                        (p.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                        (p.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");
                    return custMatch;
                }).ToList();

            // KHẮC PHỤC LỖI MẤT MÃ GIẢM GIÁ TOÀN HÓA ĐƠN:
            // Kiểm tra xem có chương trình giảm giá trực tiếp trên tổng đơn hàng (Global Voucher) nào không
            var globalPromo = activePromos.FirstOrDefault(p => string.IsNullOrEmpty(p.AppliesToISBN) && string.IsNullOrEmpty(p.TriggerISBN) && p.DiscountPercent > 0);
            PhanTramGiam = globalPromo != null ? globalPromo.DiscountPercent : 0;

            // Reset danh sách sách hiển thị ngoài ô chọn
            foreach (var b in _allBooks)
            {
                b.HasPromotion = false;
                b.IsGift = false;
                b.PromotionPrice = b.DonGiaBan; // SỬA LỖI 1: Reset về DonGiaBan gốc thay vì bằng 0
            }

            // Duyệt qua từng đầu sách để tìm ra chương trình ưu đãi tốt nhất (Quà tặng > Giảm giá sâu nhất)
            foreach (var b in _allBooks)
            {
                Promotion bestDiscountPromo = null;
                foreach (var p in activePromos)
                {
                    if (!string.IsNullOrEmpty(p.AppliesToISBN) && p.AppliesToISBN == b.ISBN)
                    {
                        if (p.IsGift)
                        {
                            b.IsGift = true;
                            bestDiscountPromo = null; // Quà tặng đi kèm sách thì giá trị sẽ về hẳn 0, bỏ qua tính % giảm giá
                            break;
                        }
                        else if (p.DiscountPercent > 0)
                        {
                            if (bestDiscountPromo == null || p.DiscountPercent > bestDiscountPromo.DiscountPercent)
                                bestDiscountPromo = p;
                        }
                    }
                }

                // Cập nhật giá bán sau khi áp dụng khuyến mãi lên danh sách hiển thị sách
                if (b.IsGift)
                {
                    b.PromotionPrice = 0;
                }
                else if (bestDiscountPromo != null)
                {
                    b.HasPromotion = true;
                    b.PromotionPrice = Math.Round(b.DonGiaBan * (100 - bestDiscountPromo.DiscountPercent) / 100);
                }
            }

            // Cập nhật lại giá tiền từng sản phẩm đang nằm trong giỏ hàng dựa trên bộ lọc trạng thái sách ở trên
            foreach (var c in DanhSachGioHang)
            {
                var book = _allBooks.FirstOrDefault(b => b.ISBN == c.ISBN);

                // Nếu sản phẩm trong giỏ là hàng tặng nhưng cấu hình khuyến mãi hiện tại không còn ghi nhận nó là quà
                if (c.IsGift && book != null && !book.IsGift && !activePromos.Any(p => p.GiftISBN == c.ISBN && DanhSachGioHang.Any(x => x.ISBN == p.TriggerISBN && x.SoLuongMua > 0)))
                {
                    c.IsGift = false;
                }

                if (c.IsGift)
                {
                    c.GiaBan = 0;
                    if (c.TenSach != null && !c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach += " (HÀNG TẶNG)";
                    continue;
                }

                // Khôi phục thuộc tính về mặc định trước khi tính toán áp giá mới
                c.GiaBan = c.OriginalGiaBan;
                c.IsPromotionApplied = false;
                if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");

                if (book == null) continue;

                if (book.IsGift)
                {
                    c.IsGift = true;
                    c.GiaBan = 0;
                    if (!c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach += " (HÀNG TẶNG)";
                }
                else if (book.HasPromotion && book.PromotionPrice > 0)
                {
                    c.IsPromotionApplied = true;
                    c.GiaBan = book.PromotionPrice; // Áp dụng giá đã giảm chuẩn xác từ danh sách cấu hình sách
                }
            }

            // Xử lý tự động thêm sách tặng vào giỏ hàng khi thỏa mãn điều kiện mua (Mua sách A tặng sách B)
            var giftsToAdd = new List<CartItemModel>();
            foreach (var p in activePromos)
            {
                if (!string.IsNullOrEmpty(p.TriggerISBN) && !string.IsNullOrEmpty(p.GiftISBN))
                {
                    // KHẮC PHỤC LỖI SÁCH TẶNG: Kiểm tra sách điều kiện phải nằm trong giỏ và có số lượng mua lớn hơn 0
                    var triggerExists = DanhSachGioHang.Any(x => x.ISBN == p.TriggerISBN && x.SoLuongMua > 0);
                    var giftExists = DanhSachGioHang.Any(x => x.ISBN == p.GiftISBN) || giftsToAdd.Any(x => x.ISBN == p.GiftISBN);

                    if (triggerExists && !giftExists)
                    {
                        var gb = _allBooks.FirstOrDefault(b => b.ISBN == p.GiftISBN);
                        if (gb != null)
                        {
                            giftsToAdd.Add(new CartItemModel
                            {
                                ISBN = gb.ISBN,
                                TenSach = gb.TenSach + " (HÀNG TẶNG)",
                                GiaBan = 0,
                                OriginalGiaBan = gb.DonGiaBan,
                                SoLuongTonKho = gb.SoLuongTonKho,
                                SoLuongMua = 1,
                                IsGift = true
                            });
                        }
                    }
                }
            }

            // Đẩy danh sách quà tặng hợp lệ vào giỏ hàng (nếu có) mà không gây lặp đệ quy vô hạn
            if (giftsToAdd.Any())
            {
                _isUpdatingCart = true;
                try
                {
                    foreach (var g in giftsToAdd)
                    {
                        if (!DanhSachGioHang.Any(x => x.ISBN == g.ISBN))
                        {
                            DanhSachGioHang.Add(g);
                            var ob = _allBooks.FirstOrDefault(b => b.ISBN == g.ISBN);
                            if (ob != null) ob.IsSelected = true;
                        }
                    }
                }
                finally { _isUpdatingCart = false; }
            }

            // Đồng bộ lại bộ lọc tìm kiếm giao diện và tính toán lại tổng tiền hóa đơn cuối cùng
            ApplyFilterAndPagination();
            CapNhatGiaTriHoaDon();
        }

        // SỬA: Hàm xử lý áp mã ưu đãi an toàn, sửa đổi qua list trung gian và xóa sạch bộ đệm cũ trước khi kiểm tra điều kiện thoát hàm
        private void ApplyPromotionByCode(string code)
        {
            // LUÔN LUÔN đưa tỷ lệ giảm giá tổng và trạng thái sách trong giỏ về nguyên bản trước khi tính toán mã mới
            PhanTramGiam = 0;
            _appliedPromotion = null;

            foreach (var c in DanhSachGioHang)
            {
                c.GiaBan = c.OriginalGiaBan;
                c.IsPromotionApplied = false;
                c.IsGift = false;
                if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)"))
                    c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                ApplyPromotionToDisplayedBooks(null);
                CapNhatGiaTriHoaDon();
                return;
            }

            var promo = _availablePromotions.FirstOrDefault(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            if (promo == null)
            {
                ApplyPromotionToDisplayedBooks(null);
                CapNhatGiaTriHoaDon();
                return;
            }

            if (promo.StartDate.HasValue && promo.EndDate.HasValue)
            {
                var now = DateTime.Now.Date;
                if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date)
                {
                    ApplyPromotionToDisplayedBooks(null);
                    CapNhatGiaTriHoaDon();
                    return;
                }
            }

            _appliedPromotion = promo;

            // Determine if the promotion applies to the current customer context
            bool promoAppliesToCustomer = promo.ApplicableCustomerType == "Tất cả" ||
                (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

            // Xác định giảm giá tổng hóa đơn (chỉ khi khuyến mãi áp dụng cho toàn bộ hóa đơn và phù hợp với loại khách)
            if (promoAppliesToCustomer && promo.DiscountPercent > 0 && string.IsNullOrEmpty(promo.AppliesToISBN))
            {
                PhanTramGiam = promo.DiscountPercent;
            }

            ApplyPromotionToDisplayedBooks(promo);

            // Sử dụng danh sách đệm tránh lỗi InvalidOperationException (Collection was modified)
            List<CartItemModel> giftsToAdd = new List<CartItemModel>();

            foreach (var c in DanhSachGioHang)
            {
                bool customerMatches = promo.ApplicableCustomerType == "Tất cả" ||
                    (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                    (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                    (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

                if (!customerMatches) continue;

                if (!string.IsNullOrEmpty(promo.TriggerISBN) && !DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN)) continue;

                if (!string.IsNullOrEmpty(promo.AppliesToISBN) && promo.AppliesToISBN == c.ISBN)
                {
                    if (promo.IsGift)
                    {
                        c.IsGift = true;
                        c.GiaBan = 0;
                        if (!c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach += " (HÀNG TẶNG)";
                    }
                    else if (promo.DiscountPercent > 0)
                    {
                        c.IsPromotionApplied = true;
                        c.GiaBan = Math.Round(c.OriginalGiaBan * (100 - promo.DiscountPercent) / 100);
                    }
                }

                if (!string.IsNullOrEmpty(promo.GiftISBN) && !string.IsNullOrEmpty(promo.TriggerISBN) && promo.TriggerISBN != promo.GiftISBN)
                {
                    var triggerExists = DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN);
                    var giftExists = DanhSachGioHang.Any(x => x.ISBN == promo.GiftISBN) || giftsToAdd.Any(x => x.ISBN == promo.GiftISBN);
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
                            giftsToAdd.Add(newCartItem);
                        }
                    }
                }
            }

            // Thực hiện thêm quà tặng ra ngoài vòng lặp foreach bằng cơ chế cờ hiệu bảo vệ
            if (giftsToAdd.Any())
            {
                _isUpdatingCart = true;
                try
                {
                    foreach (var gift in giftsToAdd)
                    {
                        DanhSachGioHang.Add(gift);
                        var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == gift.ISBN);
                        if (originalBook != null) originalBook.IsSelected = true;
                    }
                }
                finally
                {
                    _isUpdatingCart = false;
                }
            }

            CapNhatGiaTriHoaDon();
        }

        private void ApplyPromotionToDisplayedBooks(Promotion promo)
        {
            // If promo is null, reset displayed books and cart items back to original state
            if (promo == null)
            {
                foreach (var b in _allBooks)
                {
                    b.HasPromotion = false;
                    b.IsGift = false;
                    b.PromotionPrice = 0;
                }

                foreach (var c in DanhSachGioHang)
                {
                    c.GiaBan = c.OriginalGiaBan;
                    c.IsPromotionApplied = false;
                    c.IsGift = false;
                    if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)"))
                        c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");
                }

                ApplyFilterAndPagination();
                CapNhatGiaTriHoaDon();
                return;
            }

            bool customerMatches = promo.ApplicableCustomerType == "Tất cả" ||
                (promo.ApplicableCustomerType == "Khách vãng lai" && IsKhachVangLai) ||
                (promo.ApplicableCustomerType == "VIP" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "VIP") ||
                (promo.ApplicableCustomerType == "Thành viên" && KhachHangDuocChon != null && KhachHangDuocChon.LoaiKhach == "Thành viên");

            // Update displayed books
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

            // Update cart items to reflect displayed promotion as well
            foreach (var c in DanhSachGioHang)
            {
                // Reset per-item state first
                c.GiaBan = c.OriginalGiaBan;
                c.IsPromotionApplied = false;
                c.IsGift = false;
                if (c.TenSach != null && c.TenSach.Contains(" (HÀNG TẶNG)"))
                    c.TenSach = c.TenSach.Replace(" (HÀNG TẶNG)", "");

                if (!customerMatches) continue;

                if (!string.IsNullOrEmpty(promo.AppliesToISBN) && promo.AppliesToISBN == c.ISBN)
                {
                    if (promo.IsGift)
                    {
                        c.IsGift = true;
                        c.GiaBan = 0;
                        if (!c.TenSach.Contains(" (HÀNG TẶNG)")) c.TenSach += " (HÀNG TẶNG)";
                    }
                    else if (promo.DiscountPercent > 0)
                    {
                        c.IsPromotionApplied = true;
                        c.GiaBan = Math.Round(c.OriginalGiaBan * (100 - promo.DiscountPercent) / 100);
                    }
                }
            }

            // If promotion is a gift that applies to a book (and customer matches), ensure the gifted item is added to cart
            if (promo.IsGift && !string.IsNullOrEmpty(promo.AppliesToISBN) && customerMatches)
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

                        _isUpdatingCart = true;
                        try { DanhSachGioHang.Add(newCartItem); }
                        finally { _isUpdatingCart = false; }

                        var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == newCartItem.ISBN);
                        if (originalBook != null) originalBook.IsSelected = true;
                    }
                }
            }

            ApplyFilterAndPagination();
            CapNhatGiaTriHoaDon();
        }

        // SỬA: Bảo vệ hàm quét ưu đãi tự động bằng cờ bảo vệ chống đệ quy lặp vô tận khi đồng bộ giỏ hàng
        private void ApplyAutoPromotions()
        {
            if (_availablePromotions == null || !_availablePromotions.Any() || _isUpdatingCart) return;

            _isUpdatingCart = true;
            try
            {
                List<CartItemModel> itemsToAdd = new List<CartItemModel>();

                foreach (var promo in _availablePromotions)
                {
                    if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                    {
                        var now = DateTime.Now.Date;
                        if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date) continue;
                    }

                    if (!string.IsNullOrEmpty(promo.TriggerISBN))
                    {
                        var triggerInCart = DanhSachGioHang.Any(x => x.ISBN == promo.TriggerISBN);
                        if (!triggerInCart) continue;

                        if (!string.IsNullOrEmpty(promo.GiftISBN))
                        {
                            var giftExists = DanhSachGioHang.Any(x => x.ISBN == promo.GiftISBN) || itemsToAdd.Any(x => x.ISBN == promo.GiftISBN);
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
                                    itemsToAdd.Add(newCartItem);
                                }
                            }
                        }
                    }
                }

                foreach (var item in itemsToAdd)
                {
                    DanhSachGioHang.Add(item);
                    var originalBook = _allBooks.FirstOrDefault(b => b.ISBN == item.ISBN);
                    if (originalBook != null) originalBook.IsSelected = true;
                }
            }
            finally
            {
                _isUpdatingCart = false; // Đảm bảo hạ cờ an toàn
            }

            if (!string.IsNullOrEmpty(MaUuDai)) ApplyPromotionByCode(MaUuDai);
            else
            {
                foreach (var p in _availablePromotions) ApplyPromotionToDisplayedBooks(p);
            }
        }

        // SỬA: Gọi hàm ApplyPromotionByCode() kể cả khi ô SĐT bị xóa rỗng để làm sạch ưu đãi VIP (tránh lưu ảnh voucher VIP)
        private void ThucHienTimKiemKhachHangAmTham()
        {
            if (string.IsNullOrWhiteSpace(SdtKhachHang))
            {
                KhachHangDuocChon = null;
                TenKhachHang = IsKhachVangLai ? "Khách vãng lai" : "Chưa chọn khách hàng";
                CommandManager.InvalidateRequerySuggested();

                ApplyPromotionByCode(MaUuDai); // Chạy lại logic để hủy bỏ ưu đãi phân loại khách cũ
                return;
            }

            var query = SdtKhachHang?.Trim();
            bool isId = int.TryParse(query, out int parsedId);
            var khachHangFound = _allKhachHangs.FirstOrDefault(k =>
                (isId && k.MaKhachHang == parsedId) ||
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

            ApplyPromotionByCode(MaUuDai);
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

        private async Task LoadAvailablePromotionsAsync()
        {
            try
            {
                var response = await ApiClient.GetAsync<List<Bookstore.Share.DTO.PromotionDTO>>("api/UuDai");
                if (response != null && response.Count > 0)
                {
                    _availablePromotions = response.Select(d => new Promotion
                    {
                        Code = d.Code,
                        ApplicableCustomerType = "Tất cả",
                        AppliesToISBN = string.IsNullOrEmpty(d.ISBNDieuKien) ? null : d.ISBNDieuKien,
                        IsGift = d.MaLoaiUuDai == 1 || d.MaLoaiUuDai == 3,
                        DiscountPercent = (int)Math.Round(d.TiLeGiam),
                        TriggerISBN = d.ISBNDieuKien,
                        GiftISBN = d.ISBNTang,
                        StartDate = d.NgayBatDau,
                        EndDate = d.NgayKetThuc
                    }).ToList();
                    return;
                }
            }
            catch { }

            // Fallback to mock promotions only if none could be loaded from API
            if (_availablePromotions == null || !_availablePromotions.Any())
            {
                LoadMockPromotions();
            }
        }

        private async Task InitializeAsync()
        {
            await LoadAvailablePromotionsAsync();
            await LoadDanhSachKhachHangAsync();
            // Load books after promotions so initial book list reflects active promos
            LoadMockBookData();
            // Ensure promotions aggregate applied to displayed books/cart after initial load
            ApplyPromotionsAggregate();
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
            // Only load mock promotions here if none are present (avoid overwriting API-loaded promos)
            if (_availablePromotions == null || !_availablePromotions.Any())
                LoadMockPromotions();
        }

        private void LoadMockPromotions()
        {
            _availablePromotions = new List<Promotion>
            {
                new Promotion { Code = "GIFT-SAPIENS", TriggerISBN = "9786043351026", GiftISBN = "9786000000001", IsGift = true, ApplicableCustomerType = "Tất cả", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(7) },
                new Promotion { Code = "VIP10", DiscountPercent = 10, AppliesToISBN = "9786043351026", ApplicableCustomerType = "VIP", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(7) },
                new Promotion { Code = "BOOK-NGK20", DiscountPercent = 20, AppliesToISBN = "9786045662142", ApplicableCustomerType = "Tất cả", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(7) },
                new Promotion { Code = "GUEST5", DiscountPercent = 5, ApplicableCustomerType = "Khách vãng lai", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(7) },
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
                new BookItem { ISBN = "9786041022354", TenSach = "Cha Giàu Cha Nghèo", DonGiaBan = 120000, SoLuongTonKho = 12, TheLoai = "Tâm lý - Kỹ năng" },
                new BookItem { ISBN = "9786000000001", TenSach = "Sách Tặng (Mock)", DonGiaBan = 50000, SoLuongTonKho = 100, TheLoai = "Văn học"}
            };

            _allBooks.Clear();
            // Kiểm tra các sách được giảm giá trong các chương trình ưu đãi đang chạy
            foreach (var originalItem in mockApiData)
            {
                bool added = false;
                for (var promoIndex = 0; promoIndex < _availablePromotions.Count; promoIndex++)
                {
                    var promo = _availablePromotions[promoIndex];
                    if (promo.StartDate.HasValue && promo.EndDate.HasValue)
                    {
                        var now = DateTime.Now.Date;
                        if (now < promo.StartDate.Value.Date || now > promo.EndDate.Value.Date) continue;
                    }
                    if (!string.IsNullOrEmpty(promo.AppliesToISBN) && (promo.AppliesToISBN == originalItem.ISBN ))
                    {
                       if (promo.DiscountPercent > 0)
                        {
                           _allBooks.Add(new BookSaleModel
                           {
                               BookData = new BookItem
                               {
                                   ISBN = originalItem.ISBN,
                                   TenSach = originalItem.TenSach,
                                   DonGiaBan = Math.Round(originalItem.DonGiaBan * (100 - promo.DiscountPercent) / 100),
                                   SoLuongTonKho = originalItem.SoLuongTonKho,
                                   TheLoai = originalItem.TheLoai,
                                   HinhAnh = originalItem.HinhAnh
                               },
                               IsSelected = false,
                               HasPromotion = true,
                               PromotionPrice = Math.Round(originalItem.DonGiaBan * (100 - promo.DiscountPercent) / 100)
                           });
                            added = true;
                            break; // Nếu đã áp dụng giảm giá, không cần kiểm tra các chương trình khác
                        }
                    }
                }
                if (!added)
                {
                    _allBooks.Add(new BookSaleModel
                    {
                        BookData = originalItem,
                        IsSelected = false
                    });
                }
            }

            ApplyFilterAndPagination();
        }

        private void LoadMockCustomerData()
        {
            _allKhachHangs = new List<CustomerResponse>
            {
                new CustomerResponse { MaKhachHang = 1, TenKhachHang = "Lê Hoàng Quân", SoDienThoai = "0912345678", LoaiKhach = "VIP" },
                new CustomerResponse { MaKhachHang = 2, TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0987654321", LoaiKhach = "Thành viên" },
                new CustomerResponse { MaKhachHang = 3, TenKhachHang = "Trần Thị B", SoDienThoai = "0905111222", LoaiKhach = "Khách thường" }
            };
        }

        #endregion
    }
}
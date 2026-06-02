using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.Share.DTOs;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using Bookstore.WPF.Views.Components;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml.Sorting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{

    public class SaleViewModel : BaseListViewModel
    {
        private bool _isCalculating = false;

        #region 1. UI STATES (Trạng thái giao diện & Các Popup)

        private bool _isConfirmPaymentOpen;
        public bool IsConfirmPaymentOpen
        {
            get => _isConfirmPaymentOpen;
            set { _isConfirmPaymentOpen = value; OnPropertyChanged(); }
        }

        private bool _isSelectCustomerOpen;
        public bool IsSelectCustomerOpen
        {
            get => _isSelectCustomerOpen;
            set { _isSelectCustomerOpen = value; OnPropertyChanged(); }
        }

        private bool _isBookDetailOpen;
        public bool IsBookDetailOpen
        {
            get => _isBookDetailOpen;
            set { _isBookDetailOpen = value; OnPropertyChanged(); }
        }

        private bool _isThanhToanTienMat = true;
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

        #region 2. THÔNG TIN KHÁCH HÀNG (Hiển thị ở màn hình chính, chọn qua Popup)

        private bool _isKhachVangLai = true; // Mặc định ban đầu luôn là khách vãng lai
        public bool IsKhachVangLai
        {
            get => _isKhachVangLai;
            set
            {
                _isKhachVangLai = value;
                OnPropertyChanged();
                if (_isKhachVangLai)
                {
                    KhachHangDuocChon = null;
                    TenKhachHang = "Khách vãng lai";
                    SdtKhachHang = string.Empty;
                }
                TinhToanHoaDonToanDien();
            }
        }

        private string _tenKhachHang = "Khách vãng lai";
        public string TenKhachHang
        {
            get => _tenKhachHang;
            set { _tenKhachHang = value; OnPropertyChanged(); }
        }

        private string _sdtKhachHang = string.Empty;
        public string SdtKhachHang
        {
            get => _sdtKhachHang;
            set
            {
                if (_sdtKhachHang != value)
                {
                    _sdtKhachHang = value;
                    OnPropertyChanged();

                    if (SdtState != FieldState.Normal && SdtState != FieldState.Success)
                    {
                        SdtState = FieldState.Normal;
                        SdtHelperText = string.Empty;
                    }
                }
            }
        }
        private FieldState _sdtState;
        public FieldState SdtState
        {
            get => _sdtState;
            set
            {
                _sdtState = value;
                OnPropertyChanged();
            }
        }
        private string _sdtHelperText;
        public string SdtHelperText
        {
            get => _sdtHelperText;
            set
            {
                _sdtHelperText = value;
                OnPropertyChanged();
            }
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
                    SdtKhachHang = _khachHangDuocChon.SoDienThoai;
                    IsKhachVangLai = false;
                }
                TinhToanHoaDonToanDien();
            }
        }
        public ObservableCollection<CustomerResponse> DanhSachKhachHang { get; set; } = new();

        #endregion

        #region 3. DỮ LIỆU SÁCH VÀ GIỎ HÀNG (Dữ liệu Core)

        private List<BookSaleModel> _allBooks = new(); // Bộ nhớ đệm lưu trữ toàn bộ sách tải về từ API
        public ObservableCollection<BookSaleModel> DisplayBooks { get; set; } = new(); // Đổ ra WrapPanel hiển thị sách (Chỉ hiện khi có kết quả tìm kiếm)
        public ObservableCollection<CartItemModel> CartItems { get; set; } = new(); // ĐÂY CHÍNH LÀ GIỎ HÀNG CỦA BẠN!

        private BookSaleModel _sachDuocChonXemChiTiet;
        public BookSaleModel SachDuocChonXemChiTiet
        {
            get => _sachDuocChonXemChiTiet;
            set { _sachDuocChonXemChiTiet = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ListKieuTimKiem { get; set; } = new ObservableCollection<string> { "Tên sách", "Mã ISBN" };

        private object _kieuTimKiemSach = "Tên sách";
        public object KieuTimKiemSach
        {
            get => _kieuTimKiemSach;
            set
            {
                if (value == null) return;

                if (_kieuTimKiemSach != value)
                {
                    _kieuTimKiemSach = value;
                    OnPropertyChanged();
                    TrangHienTai = 1;
                    ApplyFilterAndPagination();
                }
            }
        }

        // Danh sách số trang cho ComboBox nhảy trang nhanh nếu UI có sử dụng
        public ObservableCollection<int> PageNumbers { get; set; } = new();

        #endregion

        #region 4. QUẢN LÝ ƯU ĐÃI (Promotions)

        private List<PromotionDTO> _availablePromotions = new(); // Cache mớ ưu đãi đầu sách/tặng quà từ API

        public ObservableCollection<PromotionDTO> PromotionList { get; set; } = new(); // Danh sách mã hiển thị ở ComboBox chọn voucher bill
        public ObservableCollection<PromotionDTO> AppliedPromotionList { get; set; } = new(); // CHỨA NHIỀU ƯU ĐÃI ĐÃ CHỌN CÙNG LÚC

        private PromotionDTO _selectedUuDai;
        public PromotionDTO SelectedUuDai
        {
            get => _selectedUuDai;
            set
            {
                _selectedUuDai = value;
                OnPropertyChanged();

                // Nếu người dùng chọn mã từ ComboBox và mã đó chưa có trong list đã áp dụng
                if (value != null && !AppliedPromotionList.Any(p => p.Code == value.Code))
                {
                    AppliedPromotionList.Add(value);
                    TinhToanHoaDonToanDien();

                    // Ép luồng chạy tống SelectedUuDai về null ngay lập tức để làm sạch chữ hiển thị trên ComboBox
                    Application.Current.Dispatcher.InvokeAsync(() => { SelectedUuDai = null; });
                }
            }
        }

        #endregion

        #region 5. THANH TOÁN VÀ TIỀN BẠC (Billing)

        public decimal TamTinh => CartItems.Sum(x => x.ThanhTien);

        private decimal _giamTien;
        public decimal GiamTien
        {
            get => _giamTien;
            set { _giamTien = value; OnPropertyChanged(); }
        }

        public decimal TongTienThanhToan => Math.Max(0, TamTinh - GiamTien);

        public bool IsThanhToanEnabled
        {
            get => CartItems != null && CartItems.Count > 0 && (IsKhachVangLai || KhachHangDuocChon != null);
        }

        #endregion

        protected override void ApplyFilterAndPagination() //TODO: API Search
        {
            var filtered = _allBooks.AsEnumerable();
            string query = SearchKeyword.ToLower().Trim();
            string kieuTimKiem = KieuTimKiemSach?.ToString() ?? "Tên sách";

            if (kieuTimKiem == "Mã ISBN")
                filtered = filtered.Where(b => b.BookData.ISBN != null && b.BookData.ISBN.ToLower().Contains(query));
            else
                filtered = filtered.Where(b => b.BookData.TenSach != null && b.BookData.TenSach.ToLower().Contains(query));

            var filteredList = filtered.ToList();
            TongBanGhi = filteredList.Count;
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);

            var pageItems = filteredList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize).ToList();

            DisplayBooks.Clear();
            foreach (var item in pageItems)
            {
                // Tự động đồng bộ trạng thái dấu Tick xanh dựa trên những món đang nằm trong giỏ
                item.IsSelected = CartItems.Any(g => g.ISBN == item.BookData.ISBN && !g.IsGift);
                DisplayBooks.Add(item);
            }
        }

        #region COMMANDS & CONSTRUCTOR
        public ICommand MoPopupThanhToanCommand { get; set; }
        public ICommand MoPopupChonKhachHangCommand { get; set; }
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
        public ICommand GopUuDaiCommand { get; set; } // THÊM: Xóa ưu đãi khỏi bill
        public ICommand TimKhachHangTheoSdtCommand { get; set; }

        public SaleViewModel()
        {
            _ = InitializeAsync();

            #region CHỌN SÁCH & XEM CHI TIẾT
            XemChiTietSachCommand = new RelayCommand<BookSaleModel>((selectedBook) =>
            {
                if (selectedBook != null)
                {
                    SachDuocChonXemChiTiet = selectedBook;
                    IsBookDetailOpen = true;
                }
            });

            ChonSachCommand = new RelayCommand<BookSaleModel>((book) =>
            {
                if (book == null || book.BookData.SoLuongTonKho <= 0) return;

                var itemTrongGio = CartItems.FirstOrDefault(i => i.ISBN == book.BookData.ISBN && !i.IsGift);
                if (book.IsSelected)
                {
                    if (itemTrongGio == null)
                        ThucHienThemVaoGioCore(book, 1); // Rút gọn thành hàm Helper
                }
                else
                {
                    if (itemTrongGio != null) CartItems.Remove(itemTrongGio);
                }
                TinhToanHoaDonToanDien();
            });
            #endregion

            #region CHỌN KHÁCH HÀNG
            TimKhachHangTheoSdtCommand = new RelayCommand(async () => await ThucHienTimKhachHangAsync());
            #endregion

            #region GIỎ HÀNG
            // Lắng nghe sự thay đổi bên trong giỏ hàng (Tăng/giảm số lượng)
            CartItems.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (CartItemModel item in e.NewItems)
                    {
                        item.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == nameof(CartItemModel.SoLuongMua) || args.PropertyName == nameof(CartItemModel.ThanhTien))
                            {
                                TinhToanHoaDonToanDien();
                            }
                        };
                    }
                }
                TinhToanHoaDonToanDien();
            };

            ThemVaoGioHangCommand = new RelayCommand<BookSaleModel>((book) =>
            {
                if (book == null || book.BookData.SoLuongTonKho <= 0) return;

                var item = CartItems.FirstOrDefault(i => i.ISBN == book.BookData.ISBN && !i.IsGift);
                if (item != null)
                {
                    if (item.SoLuongMua < item.SoLuongTonKho) item.SoLuongMua++;
                }
                else
                {
                    ThucHienThemVaoGioCore(book, 1); // Rút gọn thành hàm Helper
                }
                TinhToanHoaDonToanDien();
            });

            ThemVaoGioHangTuPopupCommand = new RelayCommand<BookSaleModel>((book) =>
            {
                if (book != null)
                {
                    ThemVaoGioHangCommand.Execute(book);
                    IsBookDetailOpen = false;
                }
            });

            TangSoLuongCommand = new RelayCommand<CartItemModel>((item) =>
            {
                if (item != null && item.SoLuongMua < item.SoLuongTonKho) item.SoLuongMua++;
            });

            GiamSoLuongCommand = new RelayCommand<CartItemModel>((item) =>
            {
                if (item == null) return;
                if (item.SoLuongMua > 1) item.SoLuongMua--;
                else XoaKhoiGioHangCommand.Execute(item); // Nếu = 0 thì gọi lệnh xóa luôn
            });

            XoaKhoiGioHangCommand = new RelayCommand<CartItemModel>((item) =>
            {
                if (item != null)
                {
                    CartItems.Remove(item);
                    TinhToanHoaDonToanDien();
                }
            });
            #endregion

            #region THÔNG TIN THANH TOÁN & POPUPS
            GopUuDaiCommand = new RelayCommand<PromotionDTO>((promo) =>
            {
                if (promo != null)
                {
                    AppliedPromotionList.Remove(promo);
                    TinhToanHoaDonToanDien();
                }
            });

            MoPopupThanhToanCommand = new RelayCommand<object>(
                (p) => { IsConfirmPaymentOpen = true; },
                (p) => CartItems.Any() // Chỉ cần giỏ có đồ là cho mở Popup thanh toán (Khách hàng đã chọn bên ngoài rồi)
            );

            // Đóng Popup / Hủy bỏ giao dịch
            CloseDialogCommand = new RelayCommand<object>((p) => {
                IsConfirmPaymentOpen = false;
                IsBookDetailOpen = false;
                IsSelectCustomerOpen = false; // Tiện tay đóng luôn cái chọn khách nếu có
            });

            HuyBoGiaoDichCommand = new RelayCommand<object>((p) => { IsConfirmPaymentOpen = false; });

            // Xác nhận lưu hóa đơn xuống Database qua API
            XacNhanTaoDonCommand = new RelayCommand<object>(
                (p) => ThucHienTaoDonHang(), // Rút gọn thành hàm Helper
                (p) => IsKhachVangLai || KhachHangDuocChon != null
            );

            #endregion

            #region CHỨC NĂNG LỌC KHỞI TẠO
            XoaBoLocCommand = new RelayCommand<object>((p) =>
            {
                SearchKeyword = string.Empty;
                KieuTimKiemSach = "Tên sách";
                TrangHienTai = 1;
                ApplyFilterAndPagination();
            });
            #endregion
        }
        #endregion

        #region HELPER METHODS (Dành để code chi tiết sau)

        private async Task InitializeAsync()
        {
            try
            {
                // 1. Tải danh sách khách hàng cho Popup
                //var customers = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");
                //if (customers != null && customers.Count > 0)
                //{
                //    DanhSachKhachHang.Clear();
                //    foreach (var c in customers) DanhSachKhachHang.Add(c);
                //}

                // 2. Tải toàn bộ Ưu đãi (Khuyến mãi) đang có
                var promos = await ApiClient.GetAsync<List<PromotionDTO>>("api/UuDai");
                if (promos != null && promos.Count > 0)
                {
                    _availablePromotions = promos;

                    // Lọc ra các Ưu đãi giảm hóa đơn (Loại 0 và 1) đẩy ra ComboBox cho khách tự chọn
                    PromotionList.Clear();
                    var danhSachUuDaiHoaDon = promos.Where(p => p.MaLoaiUuDai == 0 || p.MaLoaiUuDai == 1).ToList();
                    foreach (var p in danhSachUuDaiHoaDon)
                    {
                        PromotionList.Add(p);
                    }
                }

                // 3. Tải toàn bộ Sách hiển thị
                var books = await ApiClient.GetAsync<List<SachDTO>>("api/PhienBanSach");
                if (books != null && books.Count > 0)
                {
                    _allBooks.Clear();
                    int stt = 1;

                    foreach (var item in books)
                    {
                        // BƯỚC 1: Tạo đối tượng BookItem với logic lấy ảnh Y HỆT bên ProductViewModel
                        var newBook = new Models.BookItem
                        {
                            Id = item.Id,
                            STT = stt++,
                            TenSach = item.TenSach,
                            TheLoai = item.TheLoai,
                            MoTa = item.MoTa,
                            SoLuongTonKho = item.SoLuongTonKho,
                            TongDaBan = item.TongDaBan,
                            GiaNiemYet = item.GiaNiemYet,
                            DonGiaBan = item.DonGiaBan,

                            HinhAnh = string.IsNullOrEmpty(item.HinhAnh) ? "/Resources/Images/Books/default_book_cover.jpg" : item.HinhAnh,

                            ISBN = item.ISBN,
                            NamXuatBan = item.NamXuatBan,
                            LanTaiBan = item.LanTaiBan,
                            NhaXuatBan = item.NhaXuatBan,
                            HinhThucBia = item.HinhThucBia
                        };

                        // Nạp danh sách tác giả vào BookItem
                        if (item.DanhSachTacGia != null)
                        {
                            foreach (var tg in item.DanhSachTacGia)
                            {
                                newBook.DanhSachTacGia.Add(tg);
                            }
                        }

                        // BƯỚC 2: Bọc BookItem này vào bên trong BookSaleModel dùng cho nghiệp vụ bán hàng
                        var saleBookItem = new BookSaleModel(newBook);

                        // Thêm vào bộ nhớ đệm toàn cục của màn hình Sale
                        _allBooks.Add(saleBookItem);
                    }

                    // Gọi hàm lọc và phân trang để đồng bộ hiển thị lên giao diện công khai lần đầu
                    ApplyFilterAndPagination();
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc báo lỗi nếu API sập
                MessageBox.Show($"Lỗi tải dữ liệu hệ thống: {ex.Message}", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ThucHienThemVaoGioCore(BookSaleModel book, int soLuong)
        {
            CartItems.Add(new CartItemModel
            {
                ISBN = book.BookData.ISBN,
                TenSach = book.BookData.TenSach,
                OriginalGiaBan = book.BookData.DonGiaBan,
                GiaBan = book.BookData.DonGiaBan,
                SoLuongTonKho = book.BookData.SoLuongTonKho,
                SoLuongMua = soLuong
            });
        }

        private async Task ThucHienTimKhachHangAsync()
        {
            string sdt = SdtKhachHang?.Trim();

            // 1. Validate dữ liệu đầu vào trực tiếp trên Component
            if (string.IsNullOrWhiteSpace(sdt))
            {
                SdtState = FieldState.Error;
                SdtHelperText = "Vui lòng nhập số điện thoại trước khi tìm kiếm!";
                return;
            }

            // 2. Bắt đầu gọi API
            SdtState = FieldState.Warning;
            SdtHelperText = "Đang tra cứu...";

            try
            {
                // Gọi API tìm đúng cái số điện thoại đó
                string url = $"api/KhachHang?sdt={Uri.EscapeDataString(sdt)}";
                var result = await ApiClient.GetAsync<List<CustomerResponse>>(url);

                DanhSachKhachHang.Clear();

                if (result != null && result.Any())
                {
                    // Nạp kết quả vào ComboBox
                    foreach (var customer in result) DanhSachKhachHang.Add(customer);

                    // Bắt được người dùng -> Đẩy thẳng lên ComboBox và báo xanh lá cây
                    KhachHangDuocChon = result.First();
                    SdtState = FieldState.Success;
                    SdtHelperText = "Đã tìm thấy thông tin khách hàng!";
                }
                else
                {
                    // Không tìm thấy -> Báo đỏ
                    SdtState = FieldState.Error;
                    SdtHelperText = "Không có khách hàng nào khớp với số này.";
                    KhachHangDuocChon = null;
                    TenKhachHang = "Khách hàng mới / Chưa đăng ký";
                }
            }
            catch (Exception)
            {
                SdtState = FieldState.Error;
                SdtHelperText = "Lỗi kết nối cơ sở dữ liệu.";
            }
        }

        private void TinhToanHoaDonToanDien()
        {
            if (_isCalculating) return;
            _isCalculating = true;

            try
            {
                // --- BƯỚC 1: RESET TRẠNG THÁI NGUYÊN BẢN CHO GIỎ HÀNG ---
                // Gỡ bỏ toàn bộ các item là hàng tặng tự động (Loại 1 và Loại 3) để tính toán lại từ đầu
                var danhSachHangTangCu = CartItems.Where(x => x.IsGift).ToList();
                foreach (var gift in danhSachHangTangCu)
                {
                    CartItems.Remove(gift);
                }

                // Khôi phục giá gốc cho các mặt hàng thương mại còn lại trong giỏ
                foreach (var item in CartItems)
                {
                    item.GiaBan = item.OriginalGiaBan;
                    item.IsPromotionApplied = false;
                }

                // --- BƯỚC 2: ÁP DỤNG ƯU ĐÃI THEO ĐẦU SÁCH ---
                // Duyệt danh sách ưu đãi gốc từ hệ thống (_availablePromotions)
                foreach (var promo in _availablePromotions)
                {
                    // Kiểm tra điều kiện hạn dùng & đối tượng khách hàng phù hợp
                    if (!KiemTraHopLeUuDai(promo)) continue;

                    // Loại 2: Giảm giá / Đầu sách -> Trực tiếp hạ giá bán của item trong giỏ
                    if (promo.MaLoaiUuDai == 2 && !string.IsNullOrEmpty(promo.ISBNDieuKien))
                    {
                        var matchItem = CartItems.FirstOrDefault(x => x.ISBN == promo.ISBNDieuKien);
                        if (matchItem != null)
                        {
                            matchItem.IsPromotionApplied = true;
                            matchItem.GiaBan = promo.TiLeGiam > 0
                                ? Math.Round(matchItem.OriginalGiaBan * (decimal)(100 - promo.TiLeGiam) / 100)
                                : Math.Max(0, matchItem.OriginalGiaBan - promo.SoTienGiam);
                        }
                    }

                    // Loại 3: Tặng quà / Đầu sách (Mua sách tặng sách) -> Tự động nhét thêm item tặng vào giỏ
                    if (promo.MaLoaiUuDai == 3 && !string.IsNullOrEmpty(promo.ISBNDieuKien) && !string.IsNullOrEmpty(promo.ISBNTang))
                    {
                        var triggerItem = CartItems.FirstOrDefault(x => x.ISBN == promo.ISBNDieuKien);
                        // Điều kiện: Sách gốc phải có trong giỏ và đạt đủ số lượng mua yêu cầu
                        if (triggerItem != null && triggerItem.SoLuongMua >= promo.SoLuongMua)
                        {
                            // Tính số lượng quà được tặng tương ứng theo cấp số nhân
                            int soLuongTangFormat = (triggerItem.SoLuongMua / promo.SoLuongMua) * promo.SoLuongTang;

                            var sachGocHeThong = _allBooks.FirstOrDefault(b => b.BookData.ISBN == promo.ISBNTang);
                            if (sachGocHeThong != null)
                            {
                                CartItems.Add(new CartItemModel
                                {
                                    ISBN = sachGocHeThong.BookData.ISBN,
                                    TenSach = sachGocHeThong.BookData.TenSach + " (HÀNG TẶNG)",
                                    OriginalGiaBan = sachGocHeThong.BookData.DonGiaBan,
                                    GiaBan = 0, // Hàng tặng giá bằng 0
                                    SoLuongMua = soLuongTangFormat,
                                    IsGift = true
                                });
                            }
                        }
                    }
                }

                // Thông báo UI cập nhật lại giá Tạm Tính sau khi đã xử lý xong các ưu đãi trên giỏ hàng
                OnPropertyChanged(nameof(TamTinh));

                // --- BƯỚC 3: ÁP DỤNG ƯU ĐÃI TRÊN TỔNG HÓA ĐƠN ---
                decimal tongTienGiamBill = 0;

                // Quét danh sách các Voucher hóa đơn do người dùng chọn từ ComboBox (AppliedPromotionList)
                var danhSachVoucherHopLe = new List<PromotionDTO>();
                foreach (var promo in AppliedPromotionList.ToList())
                {
                    if (!KiemTraHopLeUuDai(promo) || TamTinh < promo.SoTienToiThieu)
                    {
                        // Nếu khách hàng thay đổi context khiến voucher không còn hợp lệ, tự động gỡ bỏ
                        AppliedPromotionList.Remove(promo);
                        continue;
                    }

                    // Loại 0: Giảm giá / Tổng hóa đơn -> Cộng dồn giá trị giảm giá tiền mặt hoặc phần trăm
                    if (promo.MaLoaiUuDai == 0)
                    {
                        decimal valueGiam = promo.TiLeGiam > 0
                            ? TamTinh * (decimal)(promo.TiLeGiam / 100)
                            : promo.SoTienGiam;

                        // Khống chế mức giảm tối đa nếu voucher có cấu hình GiamToiDa
                        if (promo.GiamToiDa > 0 && valueGiam > promo.GiamToiDa) valueGiam = promo.GiamToiDa;

                        tongTienGiamBill += valueGiam;
                    }

                    // Loại 1: Tặng quà / Tổng hóa đơn (Đạt mốc tiền tặng sách) -> Hiển thị thông tin bill và tự động đẩy sách vào giỏ
                    if (promo.MaLoaiUuDai == 1 && !string.IsNullOrEmpty(promo.ISBNTang))
                    {
                        var sachGocHeThong = _allBooks.FirstOrDefault(b => b.BookData.ISBN == promo.ISBNTang);
                        if (sachGocHeThong != null)
                        {
                            CartItems.Add(new CartItemModel
                            {
                                ISBN = sachGocHeThong.BookData.ISBN,
                                TenSach = sachGocHeThong.BookData.TenSach + " (QUÀ TẶNG HÓA ĐƠN)",
                                OriginalGiaBan = sachGocHeThong.BookData.DonGiaBan,
                                GiaBan = 0,
                                SoLuongMua = promo.SoLuongTang > 0 ? promo.SoLuongTang : 1,
                                IsGift = true
                            });
                        }
                    }
                }

                // Chốt giá trị tiền giảm cuối cùng (Không cho phép vượt quá số tiền tạm tính)
                GiamTien = Math.Min(tongTienGiamBill, TamTinh);
                OnPropertyChanged(nameof(TongTienThanhToan));

                // --- BƯỚC 4: ĐỒNG BỘ TRẠNG THÁI TICK NGOÀI DANH SÁCH HIỂN THỊ ---
                foreach (var book in DisplayBooks)
                {
                    book.IsSelected = CartItems.Any(x => x.ISBN == book.BookData.ISBN && !x.IsGift);
                }
            }
            finally
            {
                _isCalculating = false; // Hạ cờ an toàn
            }
        }

        private void ThucHienTaoDonHang()
        {
            // TODO: Gọi API Post hóa đơn. Sau đó Clear giỏ hàng, đóng popup.
        }

        private bool KiemTraHopLeUuDai(PromotionDTO promo)
        {
            if (promo == null || !promo.CoTheSuDung) return false;
            if (DateTime.Now.Date < promo.NgayBatDau.Date || DateTime.Now.Date > promo.NgayKetThuc.Date) return false;

            // Ép luật loại khách hàng áp dụng
            if (promo.MaLoaiKhachHang == 2 && KhachHangDuocChon == null) return false; // Chỉ áp dụng thành viên
            if (promo.MaLoaiKhachHang == 1 && !IsKhachVangLai) return false; // Chỉ áp dụng vãng lai

            return true;
        }

        private void CapNhatTrangThaiNut()
        {
            OnPropertyChanged(nameof(IsThanhToanEnabled));
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
        #endregion
    }
}
using Bookstore.Share.DTO;
using Bookstore.Share.DTO.Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.Share.DTOs;
using Bookstore.Share.Enums;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using Bookstore.WPF.Views.Components;
using Bookstore.WPF.Views.Popup;
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

        #region QUẢN LÝ POPUP ĐỘC LẬP
        public PaymentConfirmPopupViewModel PaymentConfirmPopupViewModel { get; set; } = new PaymentConfirmPopupViewModel();
        public BookDetailPopupViewModel BookDetailPopupVM { get; set; } = new BookDetailPopupViewModel();
        #endregion

        #region Trạng thái popup

        //private bool _isConfirmPaymentOpen;
        //public bool IsConfirmPaymentOpen
        //{
        //    get => _isConfirmPaymentOpen;
        //    set { _isConfirmPaymentOpen = value; OnPropertyChanged(); }
        //}

        private bool _isSelectCustomerOpen;
        public bool IsSelectCustomerOpen
        {
            get => _isSelectCustomerOpen;
            set { _isSelectCustomerOpen = value; OnPropertyChanged(); }
        }
        #endregion

        #region Thẻ khách hàng (màn hình chính)

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
                TinhToanHoaDon();
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
                TinhToanHoaDon();
            }
        }
        public ObservableCollection<CustomerResponse> DanhSachKhachHang { get; set; } = new();

        #endregion

        #region Giỏ hàng
        public ObservableCollection<CartItemModel> CartItems => CartService.Instance.CartItems;
        #endregion

        #region Xem sách

        private List<BookSaleModel> _allBooks = new(); // Bộ nhớ đệm lưu trữ toàn bộ sách tải về từ API
        public ObservableCollection<BookSaleModel> DisplayBooks { get; set; } = new(); // Đổ ra WrapPanel hiển thị sách (Chỉ hiện khi có kết quả tìm kiếm)

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
        public ObservableCollection<int> PageNumbers { get; set; } = new();

        #endregion

        #region Ưu đãi
        private bool _isUuDaiReadOnly = true; // Mặc định khóa chặn lại khi chưa có giỏ hàng
        public bool IsUuDaiReadOnly
        {
            get => _isUuDaiReadOnly;
            set { _isUuDaiReadOnly = value; OnPropertyChanged(); }
        }

        private FieldState _uuDaiState = FieldState.Normal;
        public FieldState UuDaiState
        {
            get => _uuDaiState;
            set { _uuDaiState = value; OnPropertyChanged(); }
        }

        private string _uuDaiHelperText = string.Empty;
        public string UuDaiHelperText
        {
            get => _uuDaiHelperText;
            set { _uuDaiHelperText = value; OnPropertyChanged(); }
        }
        public ObservableCollection<PromotionDTO> PromotionList { get; set; } = new();
        public ObservableCollection<PromotionDTO> AppliedPromotionList { get; set; } = new();

        private PromotionDTO _selectedUuDai;
        public PromotionDTO SelectedUuDai
        {
            get => _selectedUuDai;
            set
            {
                _selectedUuDai = value;
                OnPropertyChanged();

                if (value != null && !AppliedPromotionList.Any(p => p.Code == value.Code))
                {
                    AppliedPromotionList.Add(value);
                    TinhToanHoaDon();
                    Application.Current.Dispatcher.InvokeAsync(() => { SelectedUuDai = null; });
                }
            }
        }

        #endregion

        #region Phần thanh toán
        public decimal TamTinh => CartItems.Where(x => !x.IsGift).Sum(x => x.OriginalGiaBan * x.SoLuongMua);

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

        //#region Phương thức thanh toán
        //private bool _isThanhToanTienMat = true;
        //public bool IsThanhToanTienMat
        //{
        //    get => _isThanhToanTienMat;
        //    set { _isThanhToanTienMat = value; OnPropertyChanged(); }
        //}

        //private bool _isThanhToanChuyenKhoan;
        //public bool IsThanhToanChuyenKhoan
        //{
        //    get => _isThanhToanChuyenKhoan;
        //    set { _isThanhToanChuyenKhoan = value; OnPropertyChanged(); }
        //}
        //#endregion

        #region COMMANDS & CONSTRUCTOR
        public ICommand MoPopupThanhToanCommand { get; set; }
        public ICommand MoPopupChonKhachHangCommand { get; set; }
        public ICommand CloseDialogCommand { get; set; }
        public ICommand XemChiTietSachCommand { get; set; }
        public ICommand ChonSachCommand { get; set; }
        public ICommand ThemVaoGioHangCommand { get; set; }
        public ICommand TangSoLuongCommand { get; set; }
        public ICommand GiamSoLuongCommand { get; set; }
        public ICommand XoaKhoiGioHangCommand { get; set; }
        public ICommand XoaBoLocCommand { get; set; }
        public ICommand XoaUuDaiCommand { get; set; } // THÊM: Xóa ưu đãi khỏi bill
        public ICommand TimKhachHangTheoSdtCommand { get; set; }

        public SaleViewModel()
        {
            _ = InitializeAsync();

            #region CHỌN SÁCH & XEM CHI TIẾT
            XemChiTietSachCommand = new RelayCommand<BookSaleModel>((selectedBook) =>
            {
                if (selectedBook != null)
                {
                    // GỌI CÁCH MỚI: Truyền sách vào, và dặn nó "Khi nào bấm nút thì gọi lệnh Thêm Vào Giỏ cho tao"
                    BookDetailPopupVM.ShowPopup(selectedBook, onAddToCart: (bookToBuy) =>
                    {
                        ThemVaoGioHangCommand.Execute(bookToBuy);
                    });
                }
            });

            ChonSachCommand = new RelayCommand<BookSaleModel>((book) =>
            {
                if (book == null || book.BookData.SoLuongTonKho <= 0) return;

                var itemTrongGio = CartItems.FirstOrDefault(i => i.ISBN == book.BookData.ISBN && !i.IsGift);
                if (book.IsSelected)
                {
                    if (itemTrongGio == null)
                        CartService.Instance.AddToCart(book, 1);
                }
                else
                {
                    if (itemTrongGio != null) CartItems.Remove(itemTrongGio);
                }
                TinhToanHoaDon();
            });
            #endregion

            #region CHỌN KHÁCH HÀNG

            TimKhachHangTheoSdtCommand = new RelayCommand(async () => await ThucHienTimKhachHangAsync());

            #endregion

            #region GIỎ HÀNG
            CartItems.CollectionChanged += (s, e) =>
            {
                DongBoTrangThaiChonSach();
                TinhToanHoaDon();
                if (e.NewItems != null)
                {
                    foreach (CartItemModel item in e.NewItems)
                    {
                        item.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == nameof(CartItemModel.SoLuongMua))
                            {
                                TinhToanHoaDon();
                            }
                        };
                    }
                }
            };

            ThemVaoGioHangCommand = new RelayCommand<BookSaleModel>((book) =>
            {
                CartService.Instance.AddToCart(book, 1);

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
                    TinhToanHoaDon();
                }
            });
            #endregion

            #region THÔNG TIN THANH TOÁN & POPUPS
            XoaUuDaiCommand = new RelayCommand<PromotionDTO>((promo) =>
            {
                if (promo != null)
                {
                    AppliedPromotionList.Remove(promo);
                    TinhToanHoaDon();
                }
            });

            MoPopupThanhToanCommand = new RelayCommand(
    () =>
    {
        // Kiểm tra lấy ID khách (nếu không chọn thì truyền 0, lát Popup tự đổi thành 1)
        int idKhach = KhachHangDuocChon?.MaKhachHang ?? 0;

        PaymentConfirmPopupViewModel.ShowPopup(
            maKH: idKhach,
            tenKH: TenKhachHang,
            sdtKH: SdtKhachHang,
            items: CartItems,
            tamTinh: TamTinh, // Truyền thêm Tạm tính
            giamGia: GiamTien,
            tongTien: TongTienThanhToan,
            uuDaiDaApDung: AppliedPromotionList.ToList(), // Truyền thêm List ưu đãi
            onConfirm: () =>
            {
                // CALLBACK: Khi Popup báo API đã tạo đơn thành công, mình dọn dẹp SaleView
                CartService.Instance.CartItems.Clear(); // Làm sạch giỏ
                AppliedPromotionList.Clear(); // Gỡ các ưu đãi cũ
                IsKhachVangLai = true; // Reset thông tin khách
            }
        );
    },
    () => IsThanhToanEnabled
);

            #endregion
        }
        #endregion
        protected override void ApplyFilterAndPagination()
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


        #region HELPER METHODS

        private async Task InitializeAsync()
        {
            try
            {
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

        private async Task FetchUuDaiKhaDungAsync()
        {
            // 1. Kiểm tra Giỏ hàng
            if (CartItems == null || CartItems.Count == 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PromotionList.Clear();
                    UuDaiState = FieldState.Warning;
                    UuDaiHelperText = "Vui lòng thêm sản phẩm vào giỏ để xem ưu đãi.";
                    IsUuDaiReadOnly = true; // Khóa ComboBox
                });
                return;
            }

            // 2. Kiểm tra Khách hàng
            if (!IsKhachVangLai && KhachHangDuocChon == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PromotionList.Clear();
                    UuDaiState = FieldState.Warning;
                    UuDaiHelperText = "Vui lòng hoàn tất thông tin khách hàng.";
                    IsUuDaiReadOnly = true; // Khóa ComboBox
                });
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                UuDaiState = FieldState.Normal;
                UuDaiHelperText = "Đang tìm kiếm ưu đãi phù hợp...";
            });

            try
            {
                int targetLoaiKhachId = 1;
                if (!IsKhachVangLai && KhachHangDuocChon != null)
                {
                    targetLoaiKhachId = KhachHangDuocChon.MaLoaiKhachHang ?? 1;
                }

                var request = new CheckPromotionRequest
                {
                    TamTinh = TamTinh,
                    MaLoaiKhachHang = targetLoaiKhachId,
                    CartItems = CartItems.Where(c => !c.IsGift).Select(c => new CartItemRequest
                    {
                        ISBN = c.ISBN,
                        SoLuong = c.SoLuongMua
                    }).ToList()
                };

                var result = await ApiClient.PostAsync<CheckPromotionRequest, List<PromotionDTO>>("api/UuDai/khadung", request);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    PromotionList.Clear();

                    if (result != null && result.Any())
                    {
                        var danhSachMaDaChon = AppliedPromotionList.Select(p => p.Code).ToList();
                        int countThemMoi = 0;

                        foreach (var promo in result)
                        {
                            if (!danhSachMaDaChon.Contains(promo.Code))
                            {
                                PromotionList.Add(promo);
                                countThemMoi++;
                            }
                        }

                        if (countThemMoi > 0)
                        {
                            UuDaiState = FieldState.Success;
                            UuDaiHelperText = $"Có {countThemMoi} ưu đãi khả dụng cho đơn hàng này!";
                            IsUuDaiReadOnly = false; // MỞ KHÓA COMBOBOX
                        }
                        else
                        {
                            UuDaiState = FieldState.Normal;
                            UuDaiHelperText = "Bạn đã áp dụng hết tất cả ưu đãi khả dụng.";
                            IsUuDaiReadOnly = true; // KHÓA LẠI VÌ KHÔNG CÒN GÌ ĐỂ CHỌN
                        }
                    }
                    else
                    {
                        UuDaiState = FieldState.Normal;
                        UuDaiHelperText = "Không có chương trình ưu đãi nào phù hợp lúc này.";
                        IsUuDaiReadOnly = true; // KHÓA LẠI
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UuDaiState = FieldState.Error;
                    UuDaiHelperText = "Lỗi kết nối khi tải danh sách ưu đãi.";
                    IsUuDaiReadOnly = true; // KHÓA LẠI ĐỂ AN TOÀN
                });
            }
        }

        private void TinhToanHoaDon()
        {
            if (_isCalculating) return;
            _isCalculating = true;

            try
            {
                // ====================================================================
                // BƯỚC 1: RESET TRẠNG THÁI NGUYÊN BẢN CHO GIỎ HÀNG
                // ====================================================================
                // Gỡ bỏ toàn bộ các item là hàng tặng tự động để tính toán lại từ đầu
                var danhSachHangTangCu = CartItems.Where(x => x.IsGift).ToList();
                foreach (var gift in danhSachHangTangCu) CartItems.Remove(gift);

                foreach (var item in CartItems)
                {
                    item.GiaBan = item.OriginalGiaBan;
                    item.IsPromotionApplied = false;
                }

                // ====================================================================
                // BƯỚC 2: PHA 1 - ÁP DỤNG ƯU ĐÃI THEO ĐẦU SÁCH (LOẠI 2 & 3)
                // ====================================================================
                // Chỉ quét những ưu đãi khách HÀNG ĐÃ CHỌN (Nằm trong AppliedPromotionList)
                foreach (var promo in AppliedPromotionList
                    .Where(p => p.MaLoaiUuDai == PromotionType.SachGiam || p.MaLoaiUuDai == PromotionType.SachQua).ToList())
                {
                    if (promo.MaLoaiUuDai == PromotionType.SachGiam && !string.IsNullOrEmpty(promo.ISBNDieuKien))
                    {
                        var matchItem = CartItems.FirstOrDefault(x => x.ISBN == promo.ISBNDieuKien && !x.IsGift);
                        if (matchItem != null && matchItem.SoLuongMua >= promo.SoLuongMua)
                        {
                            matchItem.IsPromotionApplied = true;
                            matchItem.GiaBan = promo.TiLeGiam > 0
                                ? Math.Round(matchItem.OriginalGiaBan * (decimal)(100 - promo.TiLeGiam) / 100)
                                : Math.Max(0, matchItem.OriginalGiaBan - promo.SoTienGiam);
                            promo.MucGiamDisplay = "Giảm sách";
                        }
                        else AppliedPromotionList.Remove(promo);
                    }
                    else if (promo.MaLoaiUuDai == PromotionType.SachQua && !string.IsNullOrEmpty(promo.ISBNDieuKien) && !string.IsNullOrEmpty(promo.ISBNTang))
                    {
                        var triggerItem = CartItems.FirstOrDefault(x => x.ISBN == promo.ISBNDieuKien && !x.IsGift);
                        if (triggerItem != null && triggerItem.SoLuongMua >= promo.SoLuongMua)
                        {
                            int soLuongTangFormat = (triggerItem.SoLuongMua / promo.SoLuongMua) * promo.SoLuongTang;
                            ThemQuaTangVaoGio(promo.ISBNTang, soLuongTangFormat);
                            promo.MucGiamDisplay = "Tặng sách";
                        }
                        else AppliedPromotionList.Remove(promo);
                    }
                }

                // ====================================================================
                // BƯỚC 3: PHA 2 - ÁP DỤNG ƯU ĐÃI TRÊN TỔNG HÓA ĐƠN (LOẠI 0 & 1)
                // ====================================================================
                decimal tongTienGiamBill = 0;
                foreach (var promo in AppliedPromotionList
                    .Where(p => p.MaLoaiUuDai == PromotionType.HoaDonGiam || p.MaLoaiUuDai == PromotionType.HoaDonQua).ToList())
                {
                    // --- CỐT LÕI: DÙNG GiaGocTamTinh ĐỂ SO SÁNH ---
                    if (TamTinh < promo.SoTienToiThieu)
                    {
                        AppliedPromotionList.Remove(promo);
                        continue;
                    }

                    if (promo.MaLoaiUuDai == PromotionType.HoaDonGiam)
                    {
                        // Dùng TamTinh để tính % giảm
                        decimal valueGiam = promo.TiLeGiam > 0
                            ? TamTinh * (decimal)(promo.TiLeGiam / 100)
                            : promo.SoTienGiam;

                        if (promo.GiamToiDa > 0 && valueGiam > promo.GiamToiDa) valueGiam = promo.GiamToiDa;
                        tongTienGiamBill += valueGiam;
                        promo.MucGiamDisplay = $"- {valueGiam:N0} đ";
                    }
                    else if (promo.MaLoaiUuDai == PromotionType.HoaDonQua && !string.IsNullOrEmpty(promo.ISBNTang))
                    {
                        ThemQuaTangVaoGio(promo.ISBNTang, promo.SoLuongTang > 0 ? promo.SoLuongTang : 1);
                        promo.MucGiamDisplay = "Tặng quà";
                    }
                }

                // ====================================================================
                // BƯỚC 4: CHỐT SỐ LIỆU VÀ GỌI API CẬP NHẬT
                // ====================================================================
                OnPropertyChanged(nameof(TamTinh));
                GiamTien = tongTienGiamBill;
                OnPropertyChanged(nameof(TongTienThanhToan)); // = TamTinh - GiamTien

                // Cập nhật lại trạng thái Enable của nút Thanh Toán
                OnPropertyChanged(nameof(IsThanhToanEnabled));

                // Kích hoạt API quét lại xem có ưu đãi mới nào vừa "mở khóa" do thay đổi giỏ hàng không
                _ = FetchUuDaiKhaDungAsync();

                // Đồng bộ trạng thái Tick xanh ở lưới sản phẩm bên ngoài
                //foreach (var book in DisplayBooks)
                //{
                //    book.IsSelected = CartItems.Any(x => x.ISBN == book.BookData.ISBN && !x.IsGift);
                //}
                CommandManager.InvalidateRequerySuggested();
            }
            finally
            {
                _isCalculating = false; // Hạ cờ an toàn
            }
        }

        // -------------------------------------------------------------------------
        // HÀM HELPER HỖ TRỢ ĐỂ CODE Ở TRÊN ĐƯỢC GỌN GÀNG, DỄ ĐỌC
        // -------------------------------------------------------------------------
        private void ThemQuaTangVaoGio(string isbn, int soLuong)
        {
            var sachGocHeThong = _allBooks.FirstOrDefault(b => b.BookData.ISBN == isbn);
            if (sachGocHeThong != null)
            {
                CartItems.Add(new CartItemModel
                {
                    ISBN = sachGocHeThong.BookData.ISBN,
                    TenSach = $"{sachGocHeThong.BookData.TenSach}",
                    OriginalGiaBan = sachGocHeThong.BookData.DonGiaBan,
                    GiaBan = 0,
                    SoLuongMua = soLuong,
                    SoLuongTonKho = sachGocHeThong.BookData.SoLuongTonKho,
                    IsGift = true
                });
            }
        }

        private void DongBoTrangThaiChonSach()
        {
            if (DisplayBooks == null || !DisplayBooks.Any()) return;

            foreach (var book in DisplayBooks)
            {
                // Kiểm tra xem sách này có mặt trong giỏ hàng (không tính quà tặng) không
                bool isInCart = CartItems.Any(c => c.ISBN == book.BookData.ISBN && !c.IsGift);

                // Chỉ gán lại nếu trạng thái bị sai lệnh (tránh kích hoạt OnPropertyChanged liên tục gây lag UI)
                if (book.IsSelected != isInCart)
                {
                    book.IsSelected = isInCart;
                }
            }
        }
        private void ThucHienTaoDonHang()
        {
            // TODO: Gọi API Post hóa đơn. Sau đó Clear giỏ hàng, đóng popup.
        }
        #endregion
    }
}
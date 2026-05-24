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
        #region PROPERTIES

        // --- Properties: Book & Cart ---
        private BookItem _sachDuocChonXemChiTiet;
        public BookItem SachDuocChonXemChiTiet
        {
            get => _sachDuocChonXemChiTiet;
            set { _sachDuocChonXemChiTiet = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BookItem> DanhSachSachHienThi { get; set; } = new ObservableCollection<BookItem>();
        public ObservableCollection<CartItemModel> GioHang { get; set; } = new ObservableCollection<CartItemModel>();

        // --- Properties: Customer & Search ---
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
                    KeywordKhachHang = string.Empty;
                    TenKhachHang = "Khách vãng lai";
                    KhachHangDuocChon = null;
                }
                else
                {
                    TenKhachHang = "Chưa chọn khách hàng";
                }
            }
        }

        private string _keywordKhachHang = string.Empty;
        public string KeywordKhachHang
        {
            get => _keywordKhachHang;
            set { _keywordKhachHang = value; OnPropertyChanged(); }
        }

        private string _tenKhachHang = "Chưa chọn khách hàng";
        public string TenKhachHang
        {
            get => _tenKhachHang;
            set { _tenKhachHang = value; OnPropertyChanged(); }
        }

        // Cập nhật kiểu dữ liệu thành CustomerResponse giống CustomerViewModel
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

        // Danh sách gốc lưu toàn bộ khách hàng lấy từ API
        private List<CustomerResponse> _allKhachHangs = new List<CustomerResponse>();

        // --- Properties: Billing & Payment ---
        public decimal TamTinh => GioHang.Sum(item => item.ThanhTien);

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

        private string _phuongThucThanhToan = "Tiền mặt";
        public string PhuongThucThanhToan
        {
            get => _phuongThucThanhToan;
            set { _phuongThucThanhToan = value; OnPropertyChanged(); }
        }

        #endregion

        #region COMMANDS

        public ICommand XemChiTietSachCommand { get; set; }
        public ICommand ThemVaoGioHangCommand { get; set; }
        public ICommand ThemVaoGioHangTuPopupCommand { get; set; }
        public ICommand TangSoLuongCommand { get; set; }
        public ICommand GiamSoLuongCommand { get; set; }
        public ICommand XoaKhoiGioHangCommand { get; set; }
        public ICommand HuyDonHangCommand { get; set; }
        public ICommand MoPopupXacNhanCommand { get; set; }
        public ICommand XacNhanTaoDonCommand { get; set; }
        public ICommand TimKhachHangCommand { get; set; }

        #endregion

        #region CONSTRUCTOR

        public SaleViewModel()
        {
            // Tải dữ liệu khách hàng thực tế từ API bất đồng bộ giống CustomerViewModel
            _ = LoadDanhSachKhachHangAsync();

            GioHang.CollectionChanged += (s, e) => { CapNhatGiaTriHoaDon(); };

            XemChiTietSachCommand = new RelayCommand<BookItem>((selectedBook) => {
                if (selectedBook != null) { SachDuocChonXemChiTiet = selectedBook; DialogHost.OpenDialogCommand.Execute(null, null); }
            });

            ThemVaoGioHangCommand = new RelayCommand<BookItem>((book) => { ThemSachVaoGioHangLogic(book); });

            ThemVaoGioHangTuPopupCommand = new RelayCommand<BookItem>((book) => {
                if (book != null) { ThemSachVaoGioHangLogic(book); DialogHost.CloseDialogCommand.Execute(null, null); }
            });

            TangSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua++; CapNhatGiaTriHoaDon(); }
            });

            GiamSoLuongCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { item.SoLuongMua--; CapNhatGiaTriHoaDon(); }
            });

            XoaKhoiGioHangCommand = new RelayCommand<CartItemModel>((item) => {
                if (item != null) { GioHang.Remove(item); }
            });

            TimKhachHangCommand = new RelayCommand<object>((p) => ThucHienTimKiemKhachHang());

            HuyDonHangCommand = new RelayCommand<object>((param) => {
                GioHang.Clear();
                IsKhachVangLai = false;
                KhachHangDuocChon = null;
                KeywordKhachHang = string.Empty;
                TenKhachHang = "Chưa chọn khách hàng";
                PhanTramGiam = 0;
                PhuongThucThanhToan = "Tiền mặt";
            });

            MoPopupXacNhanCommand = new RelayCommand<object>((param) => {
                if (GioHang.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!IsKhachVangLai && KhachHangDuocChon == null)
                {
                    MessageBox.Show("Vui lòng nhập thông tin khách hàng hoặc tích chọn 'Khách vãng lai'!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DialogHost.OpenDialogCommand.Execute(null, null);
            });

            XacNhanTaoDonCommand = new RelayCommand<object>((param) => {
                MessageBox.Show("Tạo đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogHost.CloseDialogCommand.Execute(null, null);
                HuyDonHangCommand.Execute(null);
            });
        }

        #endregion

        #region HELPER METHODS

        private void CapNhatGiaTriHoaDon()
        {
            OnPropertyChanged(nameof(TamTinh));
            OnPropertyChanged(nameof(TongTienThanhToan));
        }

        private void ThemSachVaoGioHangLogic(BookItem book)
        {
            if (book == null || book.SoLuongTonKho <= 0) return;
            var itemTrongGio = GioHang.FirstOrDefault(i => i.ISBN == book.ISBN);
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
                GioHang.Add(newCartItem);
            }
        }

        private void ThucHienTimKiemKhachHang()
        {
            if (string.IsNullOrWhiteSpace(KeywordKhachHang))
            {
                MessageBox.Show("Vui lòng nhập Mã khách hàng hoặc Số điện thoại để tìm kiếm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var query = KeywordKhachHang.ToLower().Trim();

            // Thực hiện đối sánh dữ liệu trực tiếp trên danh sách DTO CustomerResponse từ API
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
                MessageBox.Show("Không tìm thấy khách hàng khớp với từ khóa vừa nhập!", "Tìm kiếm thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Khởi tạo lấy danh sách khách hàng từ API thay vì MockData
        private async Task LoadDanhSachKhachHangAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");
                if (result != null)
                {
                    _allKhachHangs = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải danh sách khách hàng cho màn hình bán hàng: {ex.Message}",
                                "Lỗi tải dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
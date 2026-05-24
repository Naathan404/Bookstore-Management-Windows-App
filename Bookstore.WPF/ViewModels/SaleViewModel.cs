using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using Bookstore.WPF.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MaterialDesignThemes.Wpf;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

        public decimal ThanhTien => GiaBan * SoLuongMua;
    }

    public class SaleViewModel : BaseViewModel
    {
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

        private string _keywordKhachHang;
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

        private KhachHangItem _khachHangDuocChon;
        public KhachHangItem KhachHangDuocChon
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

        private ObservableCollection<KhachHangItem> _allKhachHangs = new ObservableCollection<KhachHangItem>();

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

        // --- Commands ---
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

        // --- Constructor ---
        public SaleViewModel()
        {
            InitMockData();

            // Đồng bộ tính toán lại hóa đơn khi giỏ hàng thay đổi số lượng phần tử
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

            TimKhachHangCommand = new RelayCommand<object>(
                execute: (p) => {
                    ThucHienTimKiemKhachHang();
                },
                canExecute: (p) => {
                    return true;
                }
             );

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

        // --- Methods ---
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

            // Tìm kiếm đối sánh chính xác/gần đúng trong tập dữ liệu (sau này thay bằng API gọi xuống DB)
            var khachHangFound = _allKhachHangs.FirstOrDefault(k =>
                (k.MaKhachHang != null && k.MaKhachHang.ToLower() == query) ||
                (k.SoDienThoai != null && k.SoDienThoai == query)
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

        private void InitMockData()
        {
            _allKhachHangs.Add(new KhachHangItem { MaKhachHang = "KH001", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0912345678", Email = "vana@gmail.com", DiaChi = "Hà Nội", CongNo = 1500000, LoaiKhach = "Cá nhân", GioiTinh = "Nam", NgaySinh = new DateTime(1990, 5, 15) });
            _allKhachHangs.Add(new KhachHangItem { MaKhachHang = "KH002", TenKhachHang = "Trần Thị B", SoDienThoai = "0987654321", Email = "thib@gmail.com", DiaChi = "TP.HCM", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1995, 8, 20) });
            _allKhachHangs.Add(new KhachHangItem { MaKhachHang = "KH003", TenKhachHang = "Công ty ABC", SoDienThoai = "0977777777", Email = "abc@gmail.com", DiaChi = "Đà Nẵng", CongNo = 3500000, LoaiKhach = "Doanh nghiệp", GioiTinh = "Khác", NgaySinh = new DateTime(2015, 1, 1) });
        }
    }
}
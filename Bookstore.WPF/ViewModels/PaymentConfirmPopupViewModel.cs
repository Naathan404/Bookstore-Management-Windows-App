using Bookstore.Share.DTO;
using Bookstore.Share.DTO.Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.Share.Enums;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using Bookstore.WPF.Views.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class PaymentConfirmPopupViewModel : BaseViewModel
    {
        #region Thông tin chung
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set { _isOpen = value; OnPropertyChanged(); } }
        private Action<int> _onConfirmCallback;
        private Action? _onCancelCallback;
        #endregion

        #region Thông tin hóa đơn
        private int _maKhachHang;
        private string _tenKhachHang;
        public string TenKhachHang { get => _tenKhachHang; set { _tenKhachHang = value; OnPropertyChanged(); } }

        private string _sdtKhachHang;
        public string SdtKhachHang { get => _sdtKhachHang; set { _sdtKhachHang = value; OnPropertyChanged(); } }

        public string NguoiLap => AppState.CurrentUser.Name;
        #endregion

        #region Hóa đơn
        private ObservableCollection<CartItemModel> _cartItems;
        public ObservableCollection<CartItemModel> CartItems { get => _cartItems; set { _cartItems = value; OnPropertyChanged(); } }
        private ObservableCollection<PromotionDTO> _uuDaiDaApDung;
        public ObservableCollection<PromotionDTO> UuDaiDaApDung { get => _uuDaiDaApDung; set { _uuDaiDaApDung = value; OnPropertyChanged(); } }
        #endregion

        #region Thanh toán & Công nợ
        private decimal _tamTinh;
        public decimal TamTinh
        {
            get => _tamTinh;
            set
            {
                _tamTinh = value;
                OnPropertyChanged();
            }
        }

        private decimal _giamTien;
        public decimal GiamTien { get => _giamTien; set { _giamTien = value; OnPropertyChanged(); } }

        private bool _tinhThueVAT;
        public bool TinhThueVAT
        {
            get => _tinhThueVAT;
            set
            {
                if (_tinhThueVAT != value)
                {
                    _tinhThueVAT = value;
                    OnPropertyChanged();

                    _ = TinhThanhTienSauThue();
                }
            }
        }
        private decimal _phanTramVAT;
        public decimal PhanTramVAT { get => _phanTramVAT; set { _phanTramVAT = value; OnPropertyChanged(); } }

        private decimal _thueVAT;
        public decimal ThueVAT { get => _thueVAT; set { _thueVAT = value; OnPropertyChanged(); } }

        private decimal _tongTienThanhToan;
        public decimal TongTienThanhToan { get => _tongTienThanhToan; set { _tongTienThanhToan = value; OnPropertyChanged(); } }

        private decimal? _tienToiThieuCanTra;

        private bool _isThanhToanTienMat = true;
        public bool IsThanhToanTienMat
        {
            get => _isThanhToanTienMat;
            set
            {
                _isThanhToanTienMat = value;
                OnPropertyChanged();
                // Khi quay lại tab tiền mặt, tự động tính toán lại
                if (_isThanhToanTienMat) TinhToanTienThuaVaNo();
            }
        }

        private bool _isThanhToanChuyenKhoan;
        public bool IsThanhToanChuyenKhoan
        {
            get => _isThanhToanChuyenKhoan;
            set
            {
                _isThanhToanChuyenKhoan = value;
                OnPropertyChanged();
                if (_isThanhToanChuyenKhoan)
                {
                    TinhToanTienThuaVaNo();
                }
            }
        }

        private decimal _tienKhachDua;
        public decimal TienKhachDua
        {
            get => _tienKhachDua;
            set
            {
                _tienKhachDua = value;
                OnPropertyChanged();
                TinhToanTienThuaVaNo(); // Tiền khách đưa thay đổi -> Tính lại ngay lập tức
            }
        }

        private decimal _tienTraKhach;
        public decimal TienTraKhach
        {
            get => _tienTraKhach;
            set { _tienTraKhach = value; OnPropertyChanged(); }
        }

        private decimal _tienConNo;
        public decimal TienConNo
        {
            get => _tienConNo;
            set { _tienConNo = value; OnPropertyChanged(); }
        }

        private FieldState _tienKhachDuaState = FieldState.Normal;
        public FieldState TienKhachDuaState
        {
            get => _tienKhachDuaState;
            set { _tienKhachDuaState = value; OnPropertyChanged(); }
        }

        private string _tienKhachDuaHelperText;
        public string TienKhachDuaHelperText
        {
            get => _tienKhachDuaHelperText;
            set { _tienKhachDuaHelperText = value; OnPropertyChanged(); }
        }

        #endregion



        public ICommand XacNhanTaoDonCommand { get; set; }
        public ICommand HuyBoGiaoDichCommand { get; set; }

        public PaymentConfirmPopupViewModel()
        {
            _ = LayThamSoVAT();
            OnPropertyChanged(NguoiLap);

            XacNhanTaoDonCommand = new RelayCommand(async () =>
            {
                if (TienKhachDuaState == FieldState.Error)
                {
                    System.Windows.MessageBox.Show("Khách hàng chưa thanh toán đủ số tiền tối thiểu!", "Lỗi thanh toán", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                try
                {
                    // 1. Map Chi tiết hóa đơn
                    var chiTietList = CartItems.Where(c => c.SoLuongMua > 0).Select(c => new InvoiceDetailRequest
                    {
                        ISBN = c.ISBN,
                        SoLuong = c.SoLuongMua,
                        GiaBan = c.GiaBan
                    }).ToList();

                    // 2. Map dữ liệu Ưu đãi 
                    var uuDaiList = new List<InvoicePromoRequest>();
                    if (_uuDaiDaApDung != null && _uuDaiDaApDung.Any())
                    {
                        foreach (var u in _uuDaiDaApDung)
                        {
                            uuDaiList.Add(new InvoicePromoRequest
                            {
                                MaUuDai = u.MaUuDai,
                                SoTienGiam = u.SoTienGiamThucTe,
                            });
                        }
                    }

                    // 3. Đóng gói Request gốc
                    var request = new InvoiceRequest
                    {
                        NguoiTao = AppState.CurrentUser?.Username ?? "admin",
                        MaKhachHang = _maKhachHang > 0 ? _maKhachHang : (int?)null,
                        TongTienTamTinh = _tamTinh,
                        GiamGia = GiamTien,
                        Thue = _thueVAT,
                        TongTien = TongTienThanhToan,
                        SoTienTra = TienKhachDua > TongTienThanhToan ? TongTienThanhToan : TienKhachDua,
                        ChiTiet = chiTietList,
                        UuDai = uuDaiList
                    };

                    // 4. Gọi API
                    var result = await ApiClient.PostAsync<InvoiceRequest, CreateInvoiceResponse>("api/HoaDon", request);

                    //System.Windows.MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                    IsOpen = false;
                    _onConfirmCallback?.Invoke(result.MaHoaDon);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Lỗi tạo hóa đơn: {ex.Message}", "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });

            HuyBoGiaoDichCommand = new RelayCommand<object>((p) => IsOpen = false);
        }

        public void ShowPopup(
            int maKH, string tenKH, string sdtKH, // Thông tin khách hàng
            ObservableCollection<CartItemModel> items, // Thông tin giỏ hàng
            decimal tamTinh, decimal giamGia, decimal tongTien, // Thông tin thanh toán
            List<PromotionDTO> uuDaiDaApDung, // Thông tin ưu đãi
            Action<int> onConfirm, Action? onCancel = null)
        {
            _maKhachHang = maKH;
            TenKhachHang = tenKH;
            SdtKhachHang = string.IsNullOrEmpty(sdtKH) ? "" : sdtKH;
            CartItems = items;
            _ = TinhTienToiThieuCanTra();

            TamTinh = tamTinh;
            GiamTien = giamGia;
            TongTienThanhToan = tongTien;

            UuDaiDaApDung = new ObservableCollection<PromotionDTO>(uuDaiDaApDung);

            IsThanhToanTienMat = true;
            IsThanhToanChuyenKhoan = false;
            TienKhachDua = tongTien; // Mặc định khách đưa đủ tiền

            _onConfirmCallback = onConfirm;
            _onCancelCallback = onCancel;

            TinhThueVAT = false;
            IsOpen = true;
        }

        private async Task LayThamSoVAT()
        {
            var tsThueVAT = await ApiClient.GetAsync<ThamSoDTO>("api/ThamSo/ThueVAT");
            if (tsThueVAT != null)
            {
                PhanTramVAT = tsThueVAT.GiaTri;
            }
            else
            {
                PhanTramVAT = 0;
            }
        }

        private async Task TinhTienToiThieuCanTra()
        {
            if (_maKhachHang <= 0)
            {
                _tienToiThieuCanTra = null;
                return;
            }

            var khachHang = await ApiClient.GetAsync<CustomerResponse>($"api/KhachHang/{_maKhachHang}");
            if (khachHang == null)
            {
                _tienToiThieuCanTra = null;
                return;
            }

            var loaiKhachHang = await ApiClient.GetAsync<CustomerTierResponse>($"api/LoaiKhachHang/{khachHang.MaLoaiKhachHang}");
            if (loaiKhachHang == null)
            {
                _tienToiThieuCanTra = null;
            }
            else
            {
                decimal tiLeTraToiThieu = (decimal)loaiKhachHang.TiLeTraToiThieu / 100m;
                decimal noToiDa = loaiKhachHang.NoToiDa;
                decimal tienNoHienTai = khachHang.CongNo;

                decimal tienTraTheoTiLe = TongTienThanhToan * tiLeTraToiThieu;
                decimal tienTraDeKhongVuotHanMuc = (tienNoHienTai + TongTienThanhToan) - noToiDa;

                decimal tienToiThieu = Math.Max(tienTraTheoTiLe, tienTraDeKhongVuotHanMuc);

                _tienToiThieuCanTra = Math.Max(0, tienToiThieu);
            }
        }



        private void TinhToanTienThuaVaNo()
        {
            if (TongTienThanhToan <= 0) return;

            if (IsThanhToanChuyenKhoan)
            {
                _tienKhachDua = TongTienThanhToan;
                OnPropertyChanged(nameof(TienKhachDua));

                TienTraKhach = 0;
                TienConNo = 0;

                TienKhachDuaState = FieldState.Success;
                TienKhachDuaHelperText = "Thanh toán chuyển khoản (Quét QR).";

                return;
            }

            if (TienKhachDua >= TongTienThanhToan)
            {
                // TRƯỜNG HỢP 1: Trả đủ hoặc dư tiền
                TienTraKhach = TienKhachDua - TongTienThanhToan;
                TienConNo = 0;

                TienKhachDuaState = FieldState.Success;
                TienKhachDuaHelperText = "Thanh toán đủ.";
            }
            else
            {
                // TRƯỜNG HỢP 2: Khách trả thiếu (Ghi nợ)
                TienTraKhach = 0;
                TienConNo = TongTienThanhToan - TienKhachDua;

                // Tính số tiền bắt buộc phải trả
                decimal tienToiThieuCanTra = _tienToiThieuCanTra ?? TongTienThanhToan;

                if (TienKhachDua >= tienToiThieuCanTra)
                {
                    // Trả thiếu nhưng vẫn ĐẠT mức tối thiểu cho phép
                    TienKhachDuaState = FieldState.Warning;
                    TienKhachDuaHelperText = $"Được phép nợ. Ghi nợ: {TienConNo:N0} đ.";
                }
                else
                {
                    // Trả DƯỚI mức cho phép
                    TienKhachDuaState = FieldState.Error;
                    TienKhachDuaHelperText = $"LỖI: Phải thanh toán tối thiểu {tienToiThieuCanTra:N0} đ";
                }
            }
        }

        private async Task TinhThanhTienSauThue()
        {
            if (TinhThueVAT)
            {

                ThueVAT = Math.Round((TamTinh - GiamTien) * PhanTramVAT / 100m, 0);

            }
            else
            {
                ThueVAT = 0;
            }
            TongTienThanhToan = TamTinh - GiamTien + ThueVAT;
            await TinhTienToiThieuCanTra();
            TinhToanTienThuaVaNo();
        }
    }
}

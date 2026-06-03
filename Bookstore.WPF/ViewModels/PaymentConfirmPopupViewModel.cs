using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
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
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set { _isOpen = value; OnPropertyChanged(); } }

        #region Khách hàng

        private string _tenKhachHang;
        public string TenKhachHang { get => _tenKhachHang; set { _tenKhachHang = value; OnPropertyChanged(); } }

        private string _sdtKhachHang;
        public string SdtKhachHang { get => _sdtKhachHang; set { _sdtKhachHang = value; OnPropertyChanged(); } }
        #endregion

        #region Hóa đơn
        private ObservableCollection<CartItemModel> _cartItems;
        public ObservableCollection<CartItemModel> CartItems { get => _cartItems; set { _cartItems = value; OnPropertyChanged(); } }

        private decimal _giamTien;
        public decimal GiamTien { get => _giamTien; set { _giamTien = value; OnPropertyChanged(); } }

        private decimal _tongTienThanhToan;
        public decimal TongTienThanhToan { get => _tongTienThanhToan; set { _tongTienThanhToan = value; OnPropertyChanged(); } }
        #endregion

        #region Thanh toán & Công nợ

        // TODO: Tỉ lệ này sau này sẽ được gán từ API dựa vào Loại Khách Hàng (Khách VIP, Khách sỉ...)
        private decimal _phanTramTraToiThieu = 50;

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
                    // UX tốt: Đổi sang chuyển khoản thì mặc định gán luôn số tiền khách đưa = Tổng tiền (vì quét QR thường trả đủ)
                    TienKhachDua = TongTienThanhToan;
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

        // --- BIẾN TRẠNG THÁI HIỂN THỊ CẢNH BÁO CHO FORM INPUT ---
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

        private Action _onConfirmCallback;
        private Action _onCancelCallback;

        public ICommand XacNhanTaoDonCommand { get; set; }
        public ICommand HuyBoGiaoDichCommand { get; set; }

        public PaymentConfirmPopupViewModel()
        {
            XacNhanTaoDonCommand = new RelayCommand(() =>
            {
                _onConfirmCallback?.Invoke();
                IsOpen = false;
            });

            HuyBoGiaoDichCommand = new RelayCommand(() =>
            {
                _onCancelCallback?.Invoke();
                IsOpen = false;
            });
        }

        // HÀM CHUẨN ĐỂ TRANG BÁN HÀNG HOẶC TRANG KHÁC GỌI KÍCH HOẠT POPUP
        public void ShowPopup(string tenKH, string sdtKH, ObservableCollection<CartItemModel> items, decimal giamGia, decimal tongTien, Action onConfirm, Action onCancel = null)
        {
            TenKhachHang = tenKH;
            SdtKhachHang = string.IsNullOrEmpty(sdtKH) ? "" : sdtKH;
            CartItems = items;
            GiamTien = giamGia;
            TongTienThanhToan = tongTien;

            IsThanhToanTienMat = true; // Reset phương thức mặc định
            IsThanhToanChuyenKhoan = false;

            _onConfirmCallback = onConfirm;
            _onCancelCallback = onCancel;
            IsOpen = true;
        }

        private void TinhToanTienThuaVaNo()
        {
            if (TongTienThanhToan <= 0) return;

            if (TienKhachDua >= TongTienThanhToan)
            {
                // TRƯỜNG HỢP 1: Trả đủ hoặc dư tiền
                TienTraKhach = TienKhachDua - TongTienThanhToan;
                TienConNo = 0;

                TienKhachDuaState = FieldState.Success;
                TienKhachDuaHelperText = "Khách thanh toán đủ.";
            }
            else
            {
                // TRƯỜNG HỢP 2: Khách trả thiếu (Ghi nợ)
                TienTraKhach = 0;
                TienConNo = TongTienThanhToan - TienKhachDua;

                // Tính số tiền bắt buộc phải trả
                decimal tienToiThieuCanTra = TongTienThanhToan * (_phanTramTraToiThieu / 100m);

                if (TienKhachDua >= tienToiThieuCanTra)
                {
                    // Trả thiếu nhưng vẫn ĐẠT mức tối thiểu cho phép
                    TienKhachDuaState = FieldState.Warning;
                    TienKhachDuaHelperText = $"Khách được phép nợ. Ghi nợ: {TienConNo:N0} đ.";
                }
                else
                {
                    // Trả DƯỚI mức cho phép
                    TienKhachDuaState = FieldState.Error;
                    TienKhachDuaHelperText = $"LỖI: Khách hàng này phải thanh toán tối thiểu {tienToiThieuCanTra:N0} đ ({_phanTramTraToiThieu}%).";
                }
            }
        }
    }
}

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
        private int _maKhachHang;
        private decimal _tamTinh;
        private List<PromotionDTO> _uuDaiDaApDung;
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
            XacNhanTaoDonCommand = new RelayCommand(async () =>
            {
                // Chặn không cho tạo đơn nếu tiền đưa chưa đủ định mức
                if (TienKhachDuaState == FieldState.Error)
                {
                    System.Windows.MessageBox.Show("Khách hàng chưa thanh toán đủ số tiền tối thiểu!", "Lỗi thanh toán", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                try
                {
                    // 1. Map dữ liệu Chi tiết hóa đơn
                    var chiTietList = CartItems.Where(c => c.SoLuongMua > 0).Select(c => new InvoiceDetailRequest
                    {
                        ISBN = c.ISBN,
                        SoLuong = c.SoLuongMua,
                        DonGia = c.GiaBan, // Giá bán đã trừ khuyến mãi (nếu có) hoặc = 0 nếu là hàng tặng
                        GiaVon = 0 // Có thể để Backend tự truy xuất GiaVon từ bảng PhienBanSach
                    }).ToList();

                    // 2. Map dữ liệu Ưu đãi (ĐÃ SỬA LẠI ĐỂ HỖ TRỢ COMBO 1:N)
                    var uuDaiList = new List<InvoicePromoRequest>();
                    if (_uuDaiDaApDung != null && _uuDaiDaApDung.Any())
                    {
                        foreach (var u in _uuDaiDaApDung)
                        {
                            string targetIsbn = null;

                            // Nếu là ưu đãi trên sách, lấy ISBN đầu tiên trong danh sách điều kiện làm đại diện để lưu log
                            if (u.MaLoaiUuDai == PromotionType.SachGiam || u.MaLoaiUuDai == PromotionType.SachQua)
                            {
                                targetIsbn = u.DanhSachSachDieuKien?.FirstOrDefault()?.ISBN;
                            }

                            // TODO Mở rộng: Nếu muốn Kế toán thống kê chi tiết mỗi voucher giảm bao nhiêu tiền, 
                            // bạn có thể Regex chuỗi u.MucGiamDisplay (ví dụ "- 15,000 đ") để bóc tách con số ra.
                            // Hiện tại gán tạm = 0 để API không bị lỗi.
                            decimal soTienGiamThucTe = 0;

                            uuDaiList.Add(new InvoicePromoRequest
                            {
                                MaUuDai = u.MaUuDai,
                                ISBN = targetIsbn,
                                SoTienGiam = soTienGiamThucTe
                            });
                        }
                    }

                    // 3. Đóng gói Request gốc
                    var request = new CreateInvoiceRequest
                    {
                        // Lấy tên User đang đăng nhập từ Session tĩnh
                        NguoiTao = AppState.CurrentUser?.Username ?? "admin",

                        MaKhachHang = _maKhachHang > 0 ? _maKhachHang : 1, // ID 1 = Khách vãng lai
                        TongTienTamTinh = _tamTinh,
                        GiamGia = GiamTien,
                        Thue = 0, // Hiện tại chưa có thuế
                        TongTien = TongTienThanhToan,

                        // Xử lý tiền khách trả (nếu nợ thì lấy tiền khách đưa, nếu trả dư thì chỉ ghi nhận bằng Tổng tiền)
                        SoTienTra = TienKhachDua > TongTienThanhToan ? TongTienThanhToan : TienKhachDua,

                        ChiTiet = chiTietList,
                        UuDai = uuDaiList
                    };

                    // 4. Gọi API
                    var result = await ApiClient.PostAsync<CreateInvoiceRequest, object>("api/HoaDon", request);

                    // 5. Nếu thành công -> Kích hoạt Callback về SaleViewModel để Clear giỏ hàng & Đóng popup
                    System.Windows.MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                    _onConfirmCallback?.Invoke();
                    IsOpen = false;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Lỗi tạo hóa đơn: {ex.Message}", "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });
        }

        // HÀM CHUẨN ĐỂ TRANG BÁN HÀNG HOẶC TRANG KHÁC GỌI KÍCH HOẠT POPUP
        public void ShowPopup(
    int maKH, string tenKH, string sdtKH, // Thêm ID khách hàng
    ObservableCollection<CartItemModel> items,
    decimal tamTinh, decimal giamGia, decimal tongTien, // Thêm TamTinh
    List<PromotionDTO> uuDaiDaApDung, // Thêm danh sách ưu đãi
    Action onConfirm, Action onCancel = null)
        {
            _maKhachHang = maKH;
            TenKhachHang = tenKH;
            SdtKhachHang = string.IsNullOrEmpty(sdtKH) ? "" : sdtKH;
            CartItems = items;

            _tamTinh = tamTinh;
            GiamTien = giamGia;
            TongTienThanhToan = tongTien;

            _uuDaiDaApDung = uuDaiDaApDung;

            IsThanhToanTienMat = true;
            IsThanhToanChuyenKhoan = false;
            TienKhachDua = tongTien; // Mặc định khách đưa đủ tiền

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

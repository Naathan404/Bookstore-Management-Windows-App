using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ReceiptPopupViewModel : BaseViewModel
    {
        #region PROPERTIES
        // --- 1. CÁC BIẾN GIAO DIỆN ---
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set { _isOpen = value; OnPropertyChanged(); } }

        private string _popupTitle = "PHIẾU THU TIỀN";
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private bool _dangSua = false;

        // --- 2. CÁC BIẾN DỮ LIỆU ---
        private ReceiptResponse _phieuThuForm = new();
        public ReceiptResponse PhieuThuForm { get => _phieuThuForm; set { _phieuThuForm = value; OnPropertyChanged(); } }

        private bool _isMaPhieuVisible;
        public bool IsMaPhieuVisible { get => _isMaPhieuVisible; set { _isMaPhieuVisible = value; OnPropertyChanged(); } }

        private ObservableCollection<CustomerResponse> _danhSachKhachHangCombobox = new();
        public ObservableCollection<CustomerResponse> DanhSachKhachHangCombobox
        {
            get => _danhSachKhachHangCombobox;
            set { _danhSachKhachHangCombobox = value; OnPropertyChanged(); }
        }

        private CustomerResponse _selectedKhachHangForm;
        public CustomerResponse SelectedKhachHangForm
        {
            get => _selectedKhachHangForm;
            set
            {
                _selectedKhachHangForm = value;
                OnPropertyChanged();

                decimal noHienTai = value?.CongNo ?? 0;
                ConNoSauKhiThu = noHienTai - FormSoTienThu;
                IsConLaiVisible = (value != null && !_dangSua);
            }
        }
        private bool _isKhachHangEnable = true;
        public bool IsKhachHangEnable
        {
            get => _isKhachHangEnable;
            set
            {
                _isKhachHangEnable = value;
                OnPropertyChanged(); 
            }
        }

        private decimal _conNoSauKhiThu;
        public decimal ConNoSauKhiThu { get => _conNoSauKhiThu; set { _conNoSauKhiThu = value; OnPropertyChanged(); } }

        private bool _isConLaiVisible;
        public bool IsConLaiVisible
        {
            get => _isConLaiVisible;
            set
            {
                _isConLaiVisible = value;
                OnPropertyChanged();
            }
        }
        public decimal TienThuaTraKhach => Math.Abs(ConNoSauKhiThu < 0 ? ConNoSauKhiThu : 0);

        private decimal _formSoTienThu;
        public decimal FormSoTienThu
        {
            get => _formSoTienThu;
            set
            {
                _formSoTienThu = value;
                if (PhieuThuForm != null) PhieuThuForm.SoTienThu = value;
                OnPropertyChanged();

                decimal noHienTai = SelectedKhachHangForm?.CongNo ?? 0;
                ConNoSauKhiThu = noHienTai - value;
                IsConLaiVisible = (SelectedKhachHangForm != null && !_dangSua);
            }
        }
        #endregion

        #region EVENTS
        // Khi lưu thành công, nó sẽ kích hoạt event này để trang cha biết mà tải lại danh sách DataGrid
        public Action OnSavedSuccess { get; set; }
        #endregion

        #region COMMANDS
        public ICommand DongPopupCommand { get; }
        public ICommand LuuPhieuThuCommand { get; }
        #endregion

        public ReceiptPopupViewModel()
        {
            DongPopupCommand = new RelayCommand<object>(p => IsOpen = false);
            LuuPhieuThuCommand = new RelayCommand<object>(ExecuteLuuPhieuThu);
            
            //_ = LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync(int? maKhachHangBatBuoc = null)
        {
            try
            {
                var customers = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");
                if (customers != null)
                {
                    // Lấy người có nợ HOẶC chính là người đang được truyền vào (để sửa/xem)
                    var filtered = customers.Where(x => x.CongNo > 0 || x.MaKhachHang == maKhachHangBatBuoc).ToList();
                    DanhSachKhachHangCombobox = new ObservableCollection<CustomerResponse>(filtered);
                }
            }
            catch { }
        }

        // HÀM MỞ POPUP THÊM MỚI (Dùng cho cả trang Khách Hàng và Phiếu Thu)
        public async void MoPopupThemMoi(CustomerResponse? khachHangMacDinh = null)
        {
            _dangSua = false;
            PopupTitle = "TẠO PHIẾU THU MỚI";
            IsMaPhieuVisible = false;

            // 1. Tải danh sách
            await LoadCustomersAsync(khachHangMacDinh?.MaKhachHang);
            await Task.Delay(50);

            if (khachHangMacDinh != null)
            {
                IsKhachHangEnable = false;
                var match = DanhSachKhachHangCombobox.FirstOrDefault(x => x.MaKhachHang == khachHangMacDinh.MaKhachHang);
                SelectedKhachHangForm = match ?? khachHangMacDinh;
            }
            else
            {
                IsKhachHangEnable = true;
                SelectedKhachHangForm = null;
            }

            PhieuThuForm = new ReceiptResponse
            {
                NgayTao = DateTime.Now,
                TenNguoiTao = AppState.CurrentUser.Name,
                LyDoThu = "Thu tiền"
            };
            FormSoTienThu = 0;
            IsOpen = true;
        }

        // HÀM MỞ POPUP SỬA
        public async void MoPopupSua(ReceiptResponse pt)
        {
            _dangSua = true;
            PopupTitle = "CHỈNH SỬA PHIẾU THU";
            IsMaPhieuVisible = true;

            await LoadCustomersAsync(pt.MaKhachHang);
            IsKhachHangEnable = false;

            PhieuThuForm = new ReceiptResponse
            {
                MaPhieuThuTien = pt.MaPhieuThuTien,
                NgayTao = pt.NgayTao,
                TenNguoiTao = pt.TenNguoiTao,
                LyDoThu = pt.LyDoThu,
                SoTienThu = pt.SoTienThu
            };

            SelectedKhachHangForm = DanhSachKhachHangCombobox.FirstOrDefault(x => x.MaKhachHang == pt.MaKhachHang);
            FormSoTienThu = pt.SoTienThu;

            IsOpen = true;
        }

        private async void ExecuteLuuPhieuThu(object obj)
        {
            // 1. KIỂM TRA ĐIỀU KIỆN LOCAL (Dùng biến của PopupViewModel)
            if (SelectedKhachHangForm == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (FormSoTienThu <= 0)
            {
                MessageBox.Show("Số tiền thu không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. GỌI API LẤY THAM SỐ KIỂM TRA QUY ĐỊNH
                bool choPhepVuotNo = false;
                var tsTienThuLonHonNo = await ApiClient.GetAsync<ThamSoDTO>("api/ThamSo/TienThuLonHonNo");
                if (tsTienThuLonHonNo != null)
                {
                    choPhepVuotNo = (tsTienThuLonHonNo.GiaTri == 1);
                }

                // 3. KIỂM TRA VƯỢT NỢ (Dựa vào số nợ của khách hàng đang được chọn)
                if (!choPhepVuotNo && FormSoTienThu > SelectedKhachHangForm.CongNo)
                {
                    MessageBox.Show("Số tiền thu không được lớn hơn số nợ hiện tại!", "Lỗi quy định", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 4. CHUẨN BỊ PAYLOAD GỬI XUỐNG API
                var requestData = new
                {
                    NguoiTao = AppState.CurrentUser.Username,
                    MaKhachHang = SelectedKhachHangForm.MaKhachHang,
                    SoTienThu = FormSoTienThu,
                    LyDoThu = PhieuThuForm.LyDoThu
                };

                // 5. GỌI API (XỬ LÝ CẢ THÊM MỚI LẪN CHỈNH SỬA)
                bool isSuccess = false;
                if (_dangSua)
                {
                    // Gọi PUT nếu đang ở chế độ sửa
                    var response = await ApiClient.PutAsync<object, object>($"api/PhieuThu/{PhieuThuForm.MaPhieuThuTien}", requestData);
                    isSuccess = (response != null);
                }
                else
                {
                    // Gọi POST nếu tạo mới
                    var response = await ApiClient.PostAsync<object, object>("api/PhieuThu", requestData);
                    isSuccess = (response != null);
                }

                // 6. XỬ LÝ KẾT QUẢ KHI API TRẢ VỀ THÀNH CÔNG
                if (isSuccess)
                {
                    IsOpen = false; // Đóng popup

                    string actionText = _dangSua ? "Cập nhật" : "Tạo";
                    MessageBox.Show($"{actionText} phiếu thu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // GỌI TIẾNG CHUÔNG BÁO CHO TRANG CHA: "TÔI LÀM XONG RỒI, CHA TỰ UPDATE LƯỚI ĐI NHÉ!"
                    OnSavedSuccess?.Invoke();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại. Máy chủ không phản hồi dữ liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ Backend trả về (VD: Lỗi DB, lỗi logic server)
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
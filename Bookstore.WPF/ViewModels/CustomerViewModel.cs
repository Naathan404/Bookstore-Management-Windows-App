using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    // KẾ THỪA TỪ BASE LIST
    public class CustomerViewModel : BaseListViewModel
    {
        #region COMPONENTS (Chứa bộ não Popup Thu Tiền)
        public ReceiptPopupViewModel PopupThuTienVM { get; set; } = new ReceiptPopupViewModel();
        #endregion

        #region PROPERTIES CHUYÊN BIỆT CỦA KHÁCH HÀNG

        // Danh sách gốc tải từ server
        private List<CustomerResponse> _danhSachKhachHangGoc = new();

        // Danh sách đã cắt trang để đưa lên DataGrid
        private ObservableCollection<CustomerResponse> _danhSachKhachHang = new();
        public ObservableCollection<CustomerResponse> DanhSachKhachHang
        {
            get => _danhSachKhachHang;
            set { _danhSachKhachHang = value; OnPropertyChanged(); }
        }

        // --- Bộ Lọc (Filters) ---
        // (Biến SearchKeyword đã có sẵn trong BaseListViewModel)

        private string _kieuTimKiem = "Số điện thoại";
        public string KieuTimKiem
        {
            get => _kieuTimKiem;
            set { _kieuTimKiem = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        private string _locLoaiKhach = "Tất cả";
        public string LocLoaiKhach
        {
            get => _locLoaiKhach;
            set { _locLoaiKhach = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        private string _locCongNo = "Tất cả";
        public string LocCongNo
        {
            get => _locCongNo;
            set { _locCongNo = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        // Danh sách cho ComboBox
        private List<CustomerTierResponse> _danhSachLoaiKhachGocAPI = new();

        private ObservableCollection<string> _danhSachLoaiKhachForm = new();
        public ObservableCollection<string> DanhSachLoaiKhachForm
        {
            get => _danhSachLoaiKhachForm;
            set { _danhSachLoaiKhachForm = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _danhSachLoaiKhachLoc = new();
        public ObservableCollection<string> DanhSachLoaiKhachLoc
        {
            get => _danhSachLoaiKhachLoc;
            set { _danhSachLoaiKhachLoc = value; OnPropertyChanged(); }
        }

        // --- Trạng thái Popup Khách Hàng ---
        private bool _isKhachHangPopupOpen;
        public bool IsKhachHangPopupOpen { get => _isKhachHangPopupOpen; set { _isKhachHangPopupOpen = value; OnPropertyChanged(); } }

        private string _popupTitle = "THÊM KHÁCH HÀNG MỚI";
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private CustomerResponse _khachHangForm = new();
        public CustomerResponse KhachHangForm { get => _khachHangForm; set { _khachHangForm = value; OnPropertyChanged(); } }

        // --- Điều khiển UI cho Popup ---
        private bool _dangSua = false;

        private bool _isCongNoVisible;
        public bool IsCongNoVisible { get => _isCongNoVisible; set { _isCongNoVisible = value; OnPropertyChanged(); } }

        // GIẢI QUYẾT YÊU CẦU: ẨN MÃ KHÁCH HÀNG KHI THÊM MỚI
        private Visibility _maKhachHangVisibility = Visibility.Collapsed;
        public Visibility MaKhachHangVisibility { get => _maKhachHangVisibility; set { _maKhachHangVisibility = value; OnPropertyChanged(); } }

        #endregion

        #region COMMANDS

        public ICommand MoPopupThemCommand { get; }
        public ICommand MoPopupSuaCommand { get; }
        public ICommand XoaKhachHangCommand { get; }
        public ICommand LuuKhachHangCommand { get; }
        public ICommand DongPopupKhachHangCommand { get; }
        public ICommand MoPopupThuTienCommand { get; }
        public ICommand XoaLocCommand { get; }

        #endregion

        public CustomerViewModel()
        {
            MoPopupThemCommand = new RelayCommand<object>(ExecuteMoPopupThem);
            MoPopupSuaCommand = new RelayCommand<CustomerResponse>(ExecuteMoPopupSua);
            XoaKhachHangCommand = new RelayCommand<CustomerResponse>(ExecuteXoaKhachHang);
            LuuKhachHangCommand = new RelayCommand<object>(ExecuteLuuKhachHang);
            DongPopupKhachHangCommand = new RelayCommand<object>(p => IsKhachHangPopupOpen = false);

            MoPopupThuTienCommand = new RelayCommand<CustomerResponse>(p => PopupThuTienVM.MoPopupThemMoi(p));
            PopupThuTienVM.OnSavedSuccess = () => _ = KhoiTaoDuLieuAsync();

            XoaLocCommand = new RelayCommand<object>(ExecuteXoaLoc);

            _ = LoadDanhSachLoaiKhachAsync();
            _ = KhoiTaoDuLieuAsync();
        }

        #region LOGIC LỌC VÀ TẢI DỮ LIỆU

        private void ExecuteXoaLoc(object obj)
        {
            SearchKeyword = "";
            KieuTimKiem = "Số điện thoại";
            LocLoaiKhach = "Tất cả";
            LocCongNo = "Tất cả";
            // Hàm Set sẽ tự động gọi ApplyFilter
        }

        private async Task KhoiTaoDuLieuAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");
                if (result != null && result.Any())
                {
                    // Loại bỏ khách hàng vãng lai
                    _danhSachKhachHangGoc = result.Where(item =>
                        !(item.TenKhachHang != null && item.TenKhachHang.Equals("Khách hàng vãng lai", StringComparison.OrdinalIgnoreCase)) &&
                        item.MaKhachHang != 1
                    ).ToList();
                }
                else
                {
                    _danhSachKhachHangGoc = new List<CustomerResponse>();
                }

                // Trả về trang đầu và bắt đầu lọc
                TrangHienTai = 1;
                ApplyFilterAndPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==========================================
        // GHI ĐÈ HÀM TỪ BASE LIST VIEW MODEL
        // ==========================================
        protected override void ApplyFilterAndPagination()
        {
            if (_danhSachKhachHangGoc == null) return;

            // Chạy ngầm một Task để gọi API lọc nếu người dùng đang tìm bằng SearchKeyword
            _ = DoApiSearchAndFilterAsync();
        }

        private async Task DoApiSearchAndFilterAsync()
        {
            var text = SearchKeyword?.ToLower().Trim() ?? "";
            string endpoint = "api/KhachHang";

            // NẾU CÓ SEARCH -> GỌI API ĐỂ TÌM KIẾM
            if (!string.IsNullOrEmpty(text))
            {
                endpoint += KieuTimKiem switch
                {
                    "Tên khách hàng" => $"?ten={Uri.EscapeDataString(text)}",
                    "Email" => $"?email={Uri.EscapeDataString(text)}",
                    _ => $"?sdt={Uri.EscapeDataString(text)}"
                };

                try
                {
                    var result = await ApiClient.GetAsync<List<CustomerResponse>>(endpoint);
                    var validCustomers = new List<CustomerResponse>();

                    if (result != null)
                    {
                        validCustomers = result.Where(item =>
                            !(item.TenKhachHang != null && item.TenKhachHang.Equals("Khách hàng vãng lai", StringComparison.OrdinalIgnoreCase)) &&
                            item.MaKhachHang != 1
                        ).ToList();
                    }

                    ProcessClientSideFiltering(validCustomers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi lọc API: {ex.Message}");
                }
            }
            else
            {
                // NẾU KHÔNG SEARCH -> DÙNG LẠI DANH SÁCH GỐC
                ProcessClientSideFiltering(_danhSachKhachHangGoc);
            }
        }

        // Hàm hỗ trợ lọc các tiêu chí còn lại (Loại khách, Công nợ) và Phân trang
        private void ProcessClientSideFiltering(List<CustomerResponse> baseList)
        {
            var filtered = baseList.AsEnumerable();

            if (LocLoaiKhach != "Tất cả")
                filtered = filtered.Where(kh => kh.LoaiKhach == LocLoaiKhach);

            if (LocCongNo != "Tất cả")
            {
                filtered = filtered.Where(kh => LocCongNo switch
                {
                    "Không nợ" => kh.CongNo == 0,
                    "Dưới 1 triệu" => kh.CongNo > 0 && kh.CongNo < 1000000,
                    "1 - 2 triệu" => kh.CongNo >= 1000000 && kh.CongNo < 2000000,
                    "2 - 3 triệu" => kh.CongNo >= 2000000 && kh.CongNo < 3000000,
                    "3 - 4 triệu" => kh.CongNo >= 3000000 && kh.CongNo < 4000000,
                    "4 - 5 triệu" => kh.CongNo >= 4000000 && kh.CongNo < 5000000,
                    "Trên 5 triệu" => kh.CongNo >= 5000000,
                    _ => true
                });
            }

            var resultList = filtered.ToList();

            // Đồng bộ dữ liệu phân trang cho Base
            TongBanGhi = resultList.Count;
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;

            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;

            // Cắt dữ liệu đưa lên Grid
            DanhSachKhachHang = new ObservableCollection<CustomerResponse>(
                resultList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize));
        }

        #endregion

        #region LOGIC POPUP (Thêm / Sửa / Xóa)

        private void ExecuteMoPopupThem(object obj)
        {
            _dangSua = false;
            PopupTitle = "THÊM KHÁCH HÀNG MỚI";
            IsCongNoVisible = false;

            // ẨN MÃ KHÁCH HÀNG KHI THÊM
            MaKhachHangVisibility = Visibility.Collapsed;

            KhachHangForm = new CustomerResponse
            {
                NgayTao = DateTime.Now,
                NgaySinh = DateTime.Now,
                LoaiKhach = "Cá nhân",
                GioiTinh = "Nam",
                CongNo = 0
            };
            IsKhachHangPopupOpen = true;
        }

        private void ExecuteMoPopupSua(CustomerResponse kh)
        {
            if (kh == null) return;
            _dangSua = true;
            PopupTitle = "CHỈNH SỬA KHÁCH HÀNG";
            IsCongNoVisible = true;

            // HIỆN MÃ KHÁCH HÀNG KHI SỬA
            MaKhachHangVisibility = Visibility.Visible;

            KhachHangForm = new CustomerResponse
            {
                MaKhachHang = kh.MaKhachHang,
                TenKhachHang = kh.TenKhachHang,
                SoDienThoai = kh.SoDienThoai,
                Email = kh.Email,
                DiaChi = kh.DiaChi,
                MaSoThue = kh.MaSoThue,
                NgaySinh = kh.NgaySinh,
                CongNo = kh.CongNo,
                GioiTinh = kh.GioiTinh,
                LoaiKhach = kh.LoaiKhach,
                NgayTao = kh.NgayTao,
            };
            IsKhachHangPopupOpen = true;
        }

        private async void ExecuteXoaKhachHang(CustomerResponse kh)
        {
            if (kh == null) return;
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{kh.TenKhachHang}'?", "Xác nhận xóa",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    bool isSuccess = await ApiClient.DeleteAsync($"api/KhachHang/{kh.MaKhachHang}");
                    if (isSuccess)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        _ = KhoiTaoDuLieuAsync(); // Gọi lại load dữ liệu chuẩn
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xóa thất bại. Chi tiết: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ExecuteLuuKhachHang(object obj)
        {
            if (string.IsNullOrWhiteSpace(KhachHangForm.TenKhachHang) || string.IsNullOrWhiteSpace(KhachHangForm.SoDienThoai))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin bắt buộc (Tên, Số điện thoại)!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int maLoai = _danhSachLoaiKhachGocAPI.FirstOrDefault(x => x.TenLoaiKhachHang == KhachHangForm.LoaiKhach)?.MaLoaiKhachHang ?? 1;
                int gioiTinhNum = KhachHangForm.GioiTinh == "Nữ" ? 1 : 0;

                var requestData = new
                {
                    MaLoaiKhachHang = maLoai,
                    TenKhachHang = KhachHangForm.TenKhachHang,
                    GioiTinh = gioiTinhNum,
                    NgaySinh = KhachHangForm.NgaySinh?.ToString("yyyy-MM-dd"),
                    SoDienThoai = KhachHangForm.SoDienThoai,
                    Email = string.IsNullOrWhiteSpace(KhachHangForm.Email) ? null : KhachHangForm.Email,
                    DiaChi = string.IsNullOrWhiteSpace(KhachHangForm.DiaChi) ? null : KhachHangForm.DiaChi,
                    MaSoThue = string.IsNullOrWhiteSpace(KhachHangForm.MaSoThue) ? null : KhachHangForm.MaSoThue
                };

                if (_dangSua)
                {
                    var response = await ApiClient.PutAsync<object, CustomerResponse>($"api/KhachHang/{KhachHangForm.MaKhachHang}", requestData);
                    if (response != null)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    var response = await ApiClient.PostAsync<object, CustomerResponse>("api/KhachHang", requestData);
                    if (response != null)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                IsKhachHangPopupOpen = false;
                _ = KhoiTaoDuLieuAsync(); // Gọi API load lại cho chắc ăn
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Backend: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region TẢI DANH MỤC KHÁC
        private async Task LoadDanhSachLoaiKhachAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<CustomerTierResponse>>("api/LoaiKhachHang");
                if (result != null && result.Any())
                {
                    _danhSachLoaiKhachGocAPI = result;
                    var tenLoaiList = result.Select(x => x.TenLoaiKhachHang).ToList();

                    DanhSachLoaiKhachForm = new ObservableCollection<string>(tenLoaiList);

                    tenLoaiList.Insert(0, "Tất cả");
                    DanhSachLoaiKhachLoc = new ObservableCollection<string>(tenLoaiList);
                }
                else
                {
                    SetDefaultLoaiKhach();
                }
            }
            catch
            {
                SetDefaultLoaiKhach();
            }
        }

        private void SetDefaultLoaiKhach()
        {
            DanhSachLoaiKhachForm = new ObservableCollection<string> { "Cá nhân", "Doanh nghiệp" };
            DanhSachLoaiKhachLoc = new ObservableCollection<string> { "Tất cả", "Cá nhân", "Doanh nghiệp" };
        }
        #endregion
    }
}
using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Converters;
using Bookstore.WPF.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        #region PROPERTIES

        // --- Danh sách & Phân trang ---
        private ObservableCollection<CustomerResponse> _danhSachKhachHangGoc = new();

        private ObservableCollection<CustomerResponse> _danhSachKhachHang = new();
        public ObservableCollection<CustomerResponse> DanhSachKhachHang
        {
            get => _danhSachKhachHang;
            set { _danhSachKhachHang = value; OnPropertyChanged(); }
        }

        private int _trangHienTai = 1;
        public int TrangHienTai
        {
            get => _trangHienTai;
            set { _trangHienTai = value; OnPropertyChanged(); }
        }

        private int _tongSoTrang = 1;
        public int TongSoTrang
        {
            get => _tongSoTrang;
            set { _tongSoTrang = value; OnPropertyChanged(); }
        }

        private int _tongBanGhi;
        public int TongBanGhi
        {
            get => _tongBanGhi;
            set { _tongBanGhi = value; OnPropertyChanged(); }
        }

        // --- Bộ Lọc (Filters) ---
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        private string _kieuTimKiem = "Số điện thoại";
        public string KieuTimKiem
        {
            get => _kieuTimKiem;
            set { _kieuTimKiem = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        private string _locLoaiKhach = "Tất cả";
        public string LocLoaiKhach
        {
            get => _locLoaiKhach;
            set { _locLoaiKhach = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        // 1. Danh sách dùng cho ComboBox ở Popup Thêm/Sửa (Không có "Tất cả")
        private List<CustomerTierResponse> _danhSachLoaiKhachGocAPI = new();
        private ObservableCollection<string> _danhSachLoaiKhachForm = new();
        public ObservableCollection<string> DanhSachLoaiKhachForm
        {
            get => _danhSachLoaiKhachForm;
            set { _danhSachLoaiKhachForm = value; OnPropertyChanged(); }
        }

        // 2. Danh sách dùng cho ComboBox Lọc (Có "Tất cả" ở đầu)
        private ObservableCollection<string> _danhSachLoaiKhachLoc = new();
        public ObservableCollection<string> DanhSachLoaiKhachLoc
        {
            get => _danhSachLoaiKhachLoc;
            set { _danhSachLoaiKhachLoc = value; OnPropertyChanged(); }
        }


        private string _locCongNo = "Tất cả";
        public string LocCongNo
        {
            get => _locCongNo;
            set { _locCongNo = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        // --- Trạng thái Popup Khách Hàng ---
        private bool _isKhachHangPopupOpen;
        public bool IsKhachHangPopupOpen
        {
            get => _isKhachHangPopupOpen;
            set { _isKhachHangPopupOpen = value; OnPropertyChanged(); }
        }

        private string _popupTitle = "THÊM KHÁCH HÀNG MỚI";
        public string PopupTitle
        {
            get => _popupTitle;
            set { _popupTitle = value; OnPropertyChanged(); }
        }

        private bool _isCongNoVisible;
        public bool IsCongNoVisible
        {
            get => _isCongNoVisible;
            set { _isCongNoVisible = value; OnPropertyChanged(); }
        }

        private CustomerResponse _khachHangForm = new();
        public CustomerResponse KhachHangForm
        {
            get => _khachHangForm;
            set { _khachHangForm = value; OnPropertyChanged(); }
        }

        // --- Trạng thái Popup Thu Tiền ---
        private bool _isThuTienPopupOpen;
        public bool IsThuTienPopupOpen
        {
            get => _isThuTienPopupOpen;
            set { _isThuTienPopupOpen = value; OnPropertyChanged(); }
        }

        private string _maPhieuThu = "";
        public string MaPhieuThu
        {
            get => _maPhieuThu;
            set { _maPhieuThu = value; OnPropertyChanged(); }
        }
        private ReceiptResponse _phieuThuForm = new();
        public ReceiptResponse PhieuThuForm
        {
            get => _phieuThuForm;
            set { _phieuThuForm = value; OnPropertyChanged(); }
        }

        public decimal FormSoTienThu
        {
            get => PhieuThuForm?.SoTienThu ?? 0;
            set
            {
                if (PhieuThuForm != null)
                {
                    PhieuThuForm.SoTienThu = value;
                }
                OnPropertyChanged();

                decimal noHienTai = KhachHangForm != null ? KhachHangForm.CongNo : 0;
                ConNoSauKhiThu = Math.Max(0, noHienTai - value);
            }
        }

        private decimal _conNoSauKhiThu;
        public decimal ConNoSauKhiThu
        {
            get => _conNoSauKhiThu;
            set { _conNoSauKhiThu = value; OnPropertyChanged(); }
        }

        // Cờ nội bộ để biết đang thêm hay sửa
        private bool _dangSua = false;
        private int _soDongTrenTrang = 10;

        #endregion

        // UTIL region removed: STT is UI-only and calculated per visible row

        #region COMMANDS

        // CRUD Khách hàng
        public ICommand MoPopupThemCommand { get; }
        public ICommand MoPopupSuaCommand { get; }
        public ICommand XoaKhachHangCommand { get; }
        public ICommand LuuKhachHangCommand { get; }
        public ICommand DongPopupKhachHangCommand { get; }

        // Thu tiền
        public ICommand MoPopupThuTienCommand { get; }
        public ICommand XacNhanThuTienCommand { get; }
        public ICommand DongPopupThuTienCommand { get; }

        // Tiện ích
        public ICommand XoaLocCommand { get; }
        public ICommand PhanTrangCommand { get; }
        #endregion

        public CustomerViewModel()
        {
            // Móc nối Commands với các hàm thực thi tương ứng
            MoPopupThemCommand = new RelayCommand<object>(ExecuteMoPopupThem);
            MoPopupSuaCommand = new RelayCommand<CustomerResponse>(ExecuteMoPopupSua);
            XoaKhachHangCommand = new RelayCommand<CustomerResponse>(ExecuteXoaKhachHang);
            LuuKhachHangCommand = new RelayCommand<object>(ExecuteLuuKhachHang);
            DongPopupKhachHangCommand = new RelayCommand<object>(p => IsKhachHangPopupOpen = false);

            MoPopupThuTienCommand = new RelayCommand<CustomerResponse>(ExecuteMoPopupThuTien);
            XacNhanThuTienCommand = new RelayCommand<object>(ExecuteXacNhanThuTien);
            DongPopupThuTienCommand = new RelayCommand<object>(p => IsThuTienPopupOpen = false);

            XoaLocCommand = new RelayCommand<object>(ExecuteXoaLoc);
            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);

            _ = LoadDanhSachLoaiKhachAsync();
            _ = KhoiTaoDuLieuAsync();
        }


        #region METHODS

        // ==================== LOGIC THÊM / SỬA / XÓA ====================
        private void ExecuteMoPopupThem(object obj)
        {
            _dangSua = false;
            PopupTitle = "THÊM KHÁCH HÀNG MỚI";
            IsCongNoVisible = false;

            KhachHangForm = new CustomerResponse
            {
                MaKhachHang = GetNextMaKhachHang(),
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

            // Clone object để tách biệt bộ nhớ
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
                LoaiKhach = kh.LoaiKhach
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
                        var target = _danhSachKhachHangGoc.FirstOrDefault(x => x.MaKhachHang == kh.MaKhachHang);
                        if (target != null)
                        {
                            _danhSachKhachHangGoc.Remove(target);
                            _ = ApplyFilterAsync();
                        }
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                // 1. CHUẨN BỊ PAYLOAD
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

                // 2. GỌI API THEO CHẾ ĐỘ SỬA HOẶC THÊM
                if (_dangSua)
                {
                    var response = await ApiClient.PutAsync<object, CustomerResponse>($"api/KhachHang/{KhachHangForm.MaKhachHang}", requestData);

                    if (response != null)
                    {
                        var target = _danhSachKhachHangGoc.First(x => x.MaKhachHang == KhachHangForm.MaKhachHang);
                        target.TenKhachHang = response.TenKhachHang;
                        target.SoDienThoai = response.SoDienThoai;
                        target.Email = response.Email;
                        target.DiaChi = response.DiaChi;
                        target.MaSoThue = response.MaSoThue;
                        target.NgaySinh = response.NgaySinh;
                        target.GioiTinh = response.GioiTinh;
                        target.LoaiKhach = KhachHangForm.LoaiKhach;

                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại. Máy chủ không phản hồi dữ liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Ngăn không cho form đóng
                    }
                }
                else
                {
                    var response = await ApiClient.PostAsync<object, CustomerResponse>("api/KhachHang", requestData);

                    if (response != null)
                    {
                        response.LoaiKhach = KhachHangForm.LoaiKhach;

                        _danhSachKhachHangGoc.Insert(0, response);
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại. Máy chủ từ chối yêu cầu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Ngăn không cho form đóng nếu lỗi
                    }
                }

                // Chỉ đóng form khi mọi thứ thành công
                IsKhachHangPopupOpen = false;
                _ = ApplyFilterAsync();
            }
            catch (Exception ex)
            {
                // Bắt các lỗi văng ra từ ApiClient (như lỗi trùng số điện thoại)
                MessageBox.Show($"Lỗi Backend: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== LOGIC THU TIỀN ====================
        private void ExecuteMoPopupThuTien(CustomerResponse kh)
        {
            if (kh == null || kh.CongNo <= 0)
            {
                MessageBox.Show("Không có công nợ!");
                return;
            }

            KhachHangForm = kh;

            PhieuThuForm = new ReceiptResponse
            {
                TenKhachHang = kh.TenKhachHang ?? "",
                SoTienThu = 0,
                TenNguoiTao = "admin",
                LyDoThu = "Thu tiền"
            };

            MaPhieuThu = $"PT{DateTime.Now:ddMMyy}";

            OnPropertyChanged(nameof(FormSoTienThu));
            ConNoSauKhiThu = kh.CongNo;

            IsThuTienPopupOpen = true;
        }

        private async void ExecuteXacNhanThuTien(object obj)
        {
            // 1. KIỂM TRA ĐIỀU KIỆN LOCAL TRƯỚC CHO NHANH
            if (PhieuThuForm.SoTienThu <= 0)
            {
                MessageBox.Show("Số tiền thu không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool tienThuLonHonNo = false;
            try
            {
                // 2. GỌI API LẤY THAM SỐ
                var tsTienThuLonHonNo = await ApiClient.GetAsync<ThamSoDTO>("api/ThamSo/TienThuLonHonNo");
                if (tsTienThuLonHonNo != null)
                {
                    tienThuLonHonNo = (tsTienThuLonHonNo.GiaTri == 1);
                }

                // 3. KIỂM TRA VƯỢT NỢ
                if (!tienThuLonHonNo && PhieuThuForm.SoTienThu > KhachHangForm.CongNo)
                {
                    MessageBox.Show("Tiền thu không được lớn hơn số nợ hiện tại!", "Lỗi quy định", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 4. CHUẨN BỊ PAYLOAD GỬI XUỐNG API
                var requestData = new
                {
                    NguoiTao = "admin", // Tạm thời hardcode, sau này lấy từ phiên đăng nhập
                    MaKhachHang = KhachHangForm.MaKhachHang,
                    SoTienThu = PhieuThuForm.SoTienThu,
                    LyDoThu = PhieuThuForm.LyDoThu
                };

                // 5. GỌI API TẠO PHIẾU THU
                var response = await ApiClient.PostAsync<object, ReceiptResponse>("api/PhieuThu", requestData);

                if (response != null)
                {
                    // 6. NẾU API THÀNH CÔNG, CẬP NHẬT LẠI GIAO DIỆN FRONTEND
                    var target = _danhSachKhachHangGoc.FirstOrDefault(x => x.MaKhachHang == KhachHangForm.MaKhachHang);
                    if (target != null)
                    {
                        target.CongNo -= (long)PhieuThuForm.SoTienThu;
                    }

                    IsThuTienPopupOpen = false;
                    _ = ApplyFilterAsync(); // Load lại lưới để hiển thị màu sắc/danh sách đúng
                    MessageBox.Show($"Thu tiền thành công! Còn nợ: {target?.CongNo:N0} VNĐ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Tạo phiếu thu thất bại. Máy chủ không phản hồi!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== LOGIC FILTER & PAGINATION ====================
        private void ExecuteXoaLoc(object obj)
        {
            // Set trực tiếp vào field để không trigger ApplyFilter nhiều lần
            _searchText = "";
            _kieuTimKiem = "Số điện thoại";
            _locLoaiKhach = "Tất cả";
            _locCongNo = "Tất cả";

            // Thông báo UI cập nhật
            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(KieuTimKiem));
            OnPropertyChanged(nameof(LocLoaiKhach));
            OnPropertyChanged(nameof(LocCongNo));

            _ = ApplyFilterAsync();
        }

        private void ExecutePhanTrang(string action)
        {
            switch (action)
            {
                case "First": TrangHienTai = 1; break;
                case "Prev": if (TrangHienTai > 1) TrangHienTai--; break;
                case "Next": if (TrangHienTai < TongSoTrang) TrangHienTai++; break;
                case "Last": TrangHienTai = TongSoTrang; break;
                default:
                    if (int.TryParse(action, out int page)) TrangHienTai = page;
                    break;
            }
            _ = ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync()
        {
            var text = SearchText.ToLower().Trim();
            string endpoint = "api/KhachHang";

            // 1. NỐI API TÌM KIẾM THEO TỪ KHÓA
            if (!string.IsNullOrEmpty(text))
            {
                endpoint += KieuTimKiem switch
                {
                    "Tên khách hàng" => $"?ten={Uri.EscapeDataString(text)}",
                    "Email" => $"?email={Uri.EscapeDataString(text)}",
                    _ => $"?sdt={Uri.EscapeDataString(text)}" // Mặc định là tìm theo Số điện thoại
                };
            }

            try
            {
                // Gọi API để Backend lo phần tìm kiếm tiếng Việt
                var result = await ApiClient.GetAsync<List<CustomerResponse>>(endpoint);
                var validCustomers = new List<CustomerResponse>();

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        // Chặn khách vãng lai và ID = 1
                        if ((item.TenKhachHang != null && item.TenKhachHang.Equals("Khách hàng vãng lai", StringComparison.OrdinalIgnoreCase)) ||
                             item.MaKhachHang == 1)
                        {
                            continue;
                        }
                        validCustomers.Add(item);
                    }
                }

                // 2. LỌC TIẾP TẠI FRONTEND (Loại khách & Công nợ)
                var filtered = validCustomers.AsEnumerable();

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
                TongBanGhi = resultList.Count;

                TongSoTrang = Math.Max(1, (int)Math.Ceiling(TongBanGhi / (double)_soDongTrenTrang));
                if (TrangHienTai > TongSoTrang) TrangHienTai = 1;

                // 3. CẬP NHẬT GIAO DIỆN
                DanhSachKhachHang = new ObservableCollection<CustomerResponse>(
                    resultList.Skip((TrangHienTai - 1) * _soDongTrenTrang).Take(_soDongTrenTrang));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lọc API: {ex.Message}");
            }
        }

        private async Task KhoiTaoDuLieuAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<CustomerResponse>>("api/KhachHang");

                if (result != null && result.Any())
                {
                    var validCustomers = new List<CustomerResponse>();

                    foreach (var item in result)
                    {
                        // Loại bỏ khách vãng lai và ID = 1
                        if ((item.TenKhachHang != null && item.TenKhachHang.Equals("Khách hàng vãng lai", StringComparison.OrdinalIgnoreCase)) ||
                             item.MaKhachHang == 1)
                        {
                            continue;
                        }
                        validCustomers.Add(item);
                    }

                    _danhSachKhachHangGoc = new ObservableCollection<CustomerResponse>(validCustomers);
                }
                else
                {
                    _danhSachKhachHangGoc = new ObservableCollection<CustomerResponse>();
                }
                _ = ApplyFilterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
                    DanhSachLoaiKhachForm = new ObservableCollection<string> { "Cá nhân", "Doanh nghiệp" };
                    DanhSachLoaiKhachLoc = new ObservableCollection<string> { "Tất cả", "Cá nhân", "Doanh nghiệp" };
                }
            }
            catch
            {
                DanhSachLoaiKhachForm = new ObservableCollection<string> { "Cá nhân", "Doanh nghiệp" };
                DanhSachLoaiKhachLoc = new ObservableCollection<string> { "Tất cả", "Cá nhân", "Doanh nghiệp" };
            }
        }

        #endregion

        #region HELPER
        private int GetNextMaKhachHang()
        {
            return (_danhSachKhachHangGoc.Any() ? _danhSachKhachHangGoc.Max(x => x.MaKhachHang) : 0) + 1;
        }

        private int GetNextMaPhieuThu()
        {
            return 1;
        }
        #endregion
    }
}
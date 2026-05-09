using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _kieuTimKiem = "Tên khách hàng";
        public string KieuTimKiem
        {
            get => _kieuTimKiem;
            set { _kieuTimKiem = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _locLoaiKhach = "Tất cả";
        public string LocLoaiKhach
        {
            get => _locLoaiKhach;
            set { _locLoaiKhach = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _locCongNo = "Tất cả";
        public string LocCongNo
        {
            get => _locCongNo;
            set { _locCongNo = value; OnPropertyChanged(); ApplyFilter(); }
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

        private long _soTienThu;
        public long SoTienThu
        {
            get => _soTienThu;
            set
            {
                _soTienThu = value;
                OnPropertyChanged();
                // Tự động tính toán công nợ còn lại mỗi khi người dùng gõ phím
                ConNoSauKhiThu = Math.Max(0, (KhachHangForm?.CongNo ?? 0) - _soTienThu);
            }
        }

        private long _conNoSauKhiThu;
        public long ConNoSauKhiThu
        {
            get => _conNoSauKhiThu;
            set { _conNoSauKhiThu = value; OnPropertyChanged(); }
        }

        // Cờ nội bộ để biết đang thêm hay sửa
        private bool _dangSua = false;
        private int _soDongTrenTrang = 10;

        #endregion

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

            // Khởi chạy dữ liệu ban đầu
            KhoiTaoDuLieu();
        }

        #endregion

        #region HELPER METHODS

        // ==================== LOGIC THÊM / SỬA / XÓA ====================
        private void ExecuteMoPopupThem(object obj)
        {
            _dangSua = false;
            PopupTitle = "THÊM KHÁCH HÀNG MỚI";
            IsCongNoVisible = false;

            KhachHangForm = new CustomerResponse
            {
                MaKhachHang = $"KH{_danhSachKhachHangGoc.Count + 1:D3}",
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

        private void ExecuteXoaKhachHang(CustomerResponse kh)
        {
            if (kh == null) return;
            if (MessageBox.Show($"Xóa '{kh.TenKhachHang}'?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _danhSachKhachHangGoc.Remove(_danhSachKhachHangGoc.First(x => x.MaKhachHang == kh.MaKhachHang));
                ApplyFilter();
            }
        }

        private void ExecuteLuuKhachHang(object obj)
        {
            if (string.IsNullOrWhiteSpace(KhachHangForm.TenKhachHang) || string.IsNullOrWhiteSpace(KhachHangForm.SoDienThoai))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin bắt buộc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_dangSua)
            {
                var target = _danhSachKhachHangGoc.First(x => x.MaKhachHang == KhachHangForm.MaKhachHang);
                target.TenKhachHang = KhachHangForm.TenKhachHang;
                target.SoDienThoai = KhachHangForm.SoDienThoai;
                target.Email = KhachHangForm.Email;
                target.DiaChi = KhachHangForm.DiaChi;
                target.MaSoThue = KhachHangForm.MaSoThue;
                target.NgaySinh = KhachHangForm.NgaySinh;
                target.GioiTinh = KhachHangForm.GioiTinh;
                target.LoaiKhach = KhachHangForm.LoaiKhach;
            }
            else
            {
                _danhSachKhachHangGoc.Add(KhachHangForm);
            }

            IsKhachHangPopupOpen = false;
            ApplyFilter();
        }

        // ==================== LOGIC THU TIỀN ====================
        private void ExecuteMoPopupThuTien(CustomerResponse kh)
        {
            if (kh == null || kh.CongNo <= 0)
            {
                MessageBox.Show("Không có công nợ!");
                return;
            }

            KhachHangForm = kh; // Mượn tạm KhachHangForm để hiển thị tên và công nợ cũ
            MaPhieuThu = $"PT{DateTime.Now:yyyyMMddHHmmss}";
            SoTienThu = 0;

            IsThuTienPopupOpen = true;
        }

        private void ExecuteXacNhanThuTien(object obj)
        {
            if (SoTienThu <= 0) { MessageBox.Show("Số tiền không hợp lệ!"); return; }
            if (SoTienThu > KhachHangForm.CongNo) { MessageBox.Show("Số tiền thu > công nợ!"); return; }

            // Tìm và trừ tiền trong danh sách gốc
            var target = _danhSachKhachHangGoc.First(x => x.MaKhachHang == KhachHangForm.MaKhachHang);
            target.CongNo -= SoTienThu;

            IsThuTienPopupOpen = false;
            ApplyFilter();
            MessageBox.Show($"Thu thành công! Còn nợ: {target.CongNo:N0} VNĐ");
        }

        // ==================== LOGIC FILTER & PAGINATION ====================
        private void ExecuteXoaLoc(object obj)
        {
            // Set trực tiếp vào field để không trigger ApplyFilter nhiều lần
            _searchText = "";
            _kieuTimKiem = "Tên khách hàng";
            _locLoaiKhach = "Tất cả";
            _locCongNo = "Tất cả";

            // Thông báo UI cập nhật
            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(KieuTimKiem));
            OnPropertyChanged(nameof(LocLoaiKhach));
            OnPropertyChanged(nameof(LocCongNo));

            ApplyFilter();
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
            ApplyFilter(); // Chạy lại Skip/Take
        }

        private void ApplyFilter()
        {
            var filtered = _danhSachKhachHangGoc.AsEnumerable();
            var text = SearchText.ToLower().Trim();

            if (!string.IsNullOrEmpty(text))
            {
                filtered = filtered.Where(kh => KieuTimKiem switch
                {
                    "Số điện thoại" => kh.SoDienThoai?.Contains(text) ?? false,
                    "Email" => kh.Email?.ToLower().Contains(text) ?? false,
                    _ => kh.TenKhachHang?.ToLower().Contains(text) ?? false
                });
            }

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
            TongBanGhi = resultList.Count; // Fix: Tổng bản ghi sau khi lọc

            TongSoTrang = Math.Max(1, (int)Math.Ceiling(TongBanGhi / (double)_soDongTrenTrang));
            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;

            // Thực hiện phân trang và gán cho UI
            DanhSachKhachHang = new ObservableCollection<CustomerResponse>(
                resultList.Skip((TrangHienTai - 1) * _soDongTrenTrang).Take(_soDongTrenTrang));
        }

        private void KhoiTaoDuLieu()
        {
            _danhSachKhachHangGoc = new ObservableCollection<CustomerResponse>
            {
                new() { MaKhachHang = "KH001", TenKhachHang = "Nguyễn Văn A", SoDienThoai = "0912345678", Email = "vana@gmail.com", DiaChi = "Hà Nội", CongNo = 1500000, LoaiKhach = "Cá nhân", GioiTinh = "Nam", NgaySinh = new DateTime(1990, 5, 15) },
                new() { MaKhachHang = "KH002", TenKhachHang = "Trần Thị B", SoDienThoai = "0987654321", Email = "thib@gmail.com", DiaChi = "TP.HCM", CongNo = 0, LoaiKhach = "Cá nhân", GioiTinh = "Nữ", NgaySinh = new DateTime(1995, 8, 20) },
                // Thêm các dữ liệu khác...
            };
            ApplyFilter();
        }

        #endregion
    }
}
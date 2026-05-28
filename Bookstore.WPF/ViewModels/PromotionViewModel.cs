using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    // ==================== MODEL ====================
    public class PromotionModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int STT { get; set; }

        private string _maUuDai;
        public string MaUuDai { get => _maUuDai; set { _maUuDai = value; OnPropertyChanged(); } }

        private string _tenChuongTrinh;
        public string TenChuongTrinh { get => _tenChuongTrinh; set { _tenChuongTrinh = value; OnPropertyChanged(); } }

        private string _loaiUuDai;
        public string LoaiUuDai { get => _loaiUuDai; set { _loaiUuDai = value; OnPropertyChanged(); } }

        private string _loaiKhachHangApDung;
        public string LoaiKhachHangApDung { get => _loaiKhachHangApDung; set { _loaiKhachHangApDung = value; OnPropertyChanged(); } }

        private DateTime _thoiGianBatDau = DateTime.Today;
        public DateTime ThoiGianBatDau { get => _thoiGianBatDau; set { _thoiGianBatDau = value; OnPropertyChanged(); } }

        private DateTime _thoiGianKetThuc = DateTime.Today.AddDays(30);
        public DateTime ThoiGianKetThuc { get => _thoiGianKetThuc; set { _thoiGianKetThuc = value; OnPropertyChanged(); } }

        public int SoLuongDaDung { get; set; }

        private int _soLuongToiDa;
        public int SoLuongToiDa { get => _soLuongToiDa; set { _soLuongToiDa = value; OnPropertyChanged(); OnPropertyChanged(nameof(SoLuongToiDaDisplay)); } }
        public string SoLuongToiDaDisplay => SoLuongToiDa == 0 ? "∞" : SoLuongToiDa.ToString();

        private string _moTa;
        public string MoTa { get => _moTa; set { _moTa = value; OnPropertyChanged(); } }

        private string _trangThai;
        public string TrangThai
        {
            get => _trangThai;
            set
            {
                _trangThai = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TrangThaiBackground));
                OnPropertyChanged(nameof(TrangThaiForeground));
                OnPropertyChanged(nameof(ToggleButtonBackground));
                OnPropertyChanged(nameof(ToggleButtonIcon));
                OnPropertyChanged(nameof(ToggleButtonForeground));
            }
        }

        public string TrangThaiBackground => TrangThai switch
        {
            "Đang chạy" => "#D1FAE5",
            "Sắp diễn ra" => "#DBEAFE",
            "Đã kết thúc" => "#FEE2E2",
            "Tạm dừng" => "#FEF3C7",
            _ => "#F3F4F6"
        };

        public string TrangThaiForeground => TrangThai switch
        {
            "Đang chạy" => "#047857",
            "Sắp diễn ra" => "#1E40AF",
            "Đã kết thúc" => "#B91C1C",
            "Tạm dừng" => "#92400E",
            _ => "#6B7280"
        };

        public string ToggleButtonBackground => TrangThai == "Đang chạy" ? "#FEF3C7" : "#D1FAE5";
        public string ToggleButtonIcon => TrangThai == "Đang chạy" ? "Pause" : "Play";
        public string ToggleButtonForeground => TrangThai == "Đang chạy" ? "#F59E0B" : "#10B981";

        // Chi tiết ưu đãi - Loại 1: Giảm giá trên tổng hóa đơn
        private string _giaTriApDungTu;
        public string GiaTriApDungTu { get => _giaTriApDungTu; set { _giaTriApDungTu = value; OnPropertyChanged(); } }

        private string _giaTriApDungDen;
        public string GiaTriApDungDen { get => _giaTriApDungDen; set { _giaTriApDungDen = value; OnPropertyChanged(); } }

        private string _mucGiamGia;
        public string MucGiamGia { get => _mucGiamGia; set { _mucGiamGia = value; OnPropertyChanged(); } }

        private string _giamToiDa;
        public string GiamToiDa { get => _giamToiDa; set { _giamToiDa = value; OnPropertyChanged(); } }

        // Chi tiết ưu đãi - Loại 2: Giảm giá cho các sản phẩm
        private string _sachApDung;
        public string SachApDung { get => _sachApDung; set { _sachApDung = value; OnPropertyChanged(); } }

        private string _soLuongApDung;
        public string SoLuongApDung { get => _soLuongApDung; set { _soLuongApDung = value; OnPropertyChanged(); } }

        // Chi tiết ưu đãi - Loại 3: Tặng quà trên tổng hóa đơn
        private string _sachTang;
        public string SachTang { get => _sachTang; set { _sachTang = value; OnPropertyChanged(); } }

        private string _soLuongTang;
        public string SoLuongTang { get => _soLuongTang; set { _soLuongTang = value; OnPropertyChanged(); } }

        // Chi tiết ưu đãi - Loại 4: Tặng quà khi mua sản phẩm
        private string _sachQuaTang;
        public string SachQuaTang { get => _sachQuaTang; set { _sachQuaTang = value; OnPropertyChanged(); } }

        private string _soLuongApDungTu;
        public string SoLuongApDungTu { get => _soLuongApDungTu; set { _soLuongApDungTu = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ==================== VIEWMODEL ====================
    public class PromotionViewModel : INotifyPropertyChanged
    {
        private const int PageSize = 10;

        public PromotionViewModel()
        {
            PagedPromotions = new ObservableCollection<PromotionModel>();
            PageNumbers = new ObservableCollection<int>();
            SelectedPromotions = new ObservableCollection<PromotionModel>();
            ChiTietGiamGiaHoaDon = new ObservableCollection<PromotionModel>();
            ChiTietTangQuaHoaDon = new ObservableCollection<PromotionModel>();
            ChiTietGiamGiaSach = new ObservableCollection<PromotionModel>();
            ChiTietTangQuaSach = new ObservableCollection<PromotionModel>();

            ListLoaiUuDai = new ObservableCollection<string>
            {
                "Giảm giá trên tổng hóa đơn",
                "Giảm giá cho các sản phẩm",
                "Tặng quà trên tổng hóa đơn",
                "Tặng quà khi mua sản phẩm"
            };
            ListTrangThai = new ObservableCollection<string> { "Tất cả", "Đang chạy", "Sắp diễn ra", "Đã kết thúc", "Tạm dừng" };
            ListLoaiKhachHang = new ObservableCollection<string> { "Tất cả", "Thành viên", "VIP", "Khách lẻ" };
            ListSach = new ObservableCollection<string> { "Sách Văn học", "Sách Thiếu nhi", "Sách Khoa học", "Sách Kỹ năng", "Sách Ngoại ngữ" };

            // Commands
            OpenAddPopupCommand = new RelayCommand(OpenAddPopup);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            SavePromotionCommand = new RelayCommand(SavePromotion);
            ClearFilterCommand = new RelayCommand(ClearFilters);
            RefreshCommand = new RelayCommand(RefreshData);
            ExportExcelCommand = new RelayCommand(ExportExcel);
            GenerateCodeCommand = new RelayCommand(GenerateCode);
            FirstPageCommand = new RelayCommand(FirstPage);
            PrevPageCommand = new RelayCommand(PrevPage);
            NextPageCommand = new RelayCommand(NextPage);
            LastPageCommand = new RelayCommand(LastPage);
            BulkActivateCommand = new RelayCommand(BulkActivate);
            BulkDeactivateCommand = new RelayCommand(BulkDeactivate);
            AddGiamGiaHoaDonRowCommand = new RelayCommand(AddGiamGiaHoaDonRow);
            AddTangQuaHoaDonRowCommand = new RelayCommand(AddTangQuaHoaDonRow);
            AddGiamGiaSachRowCommand = new RelayCommand(AddGiamGiaSachRow);
            AddTangQuaSachRowCommand = new RelayCommand(AddTangQuaSachRow);

            OpenEditPopupCommand = new RelayCommand<PromotionModel>(OpenEditPopup);
            DeletePromotionCommand = new RelayCommand<PromotionModel>(DeletePromotion);
            ToggleStatusCommand = new RelayCommand<PromotionModel>(ToggleStatus);
            GoToPageCommand = new RelayCommand<int>(GoToPage);

            LoadSampleData();
            UpdateStatistics();
            UpdatePagination();
        }

        // ============ COLLECTIONS ============
        public ObservableCollection<PromotionModel> PagedPromotions { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
        public ObservableCollection<PromotionModel> SelectedPromotions { get; set; }
        public ObservableCollection<PromotionModel> ChiTietGiamGiaHoaDon { get; set; }
        public ObservableCollection<PromotionModel> ChiTietTangQuaHoaDon { get; set; }
        public ObservableCollection<PromotionModel> ChiTietGiamGiaSach { get; set; }
        public ObservableCollection<PromotionModel> ChiTietTangQuaSach { get; set; }
        public ObservableCollection<string> ListLoaiUuDai { get; set; }
        public ObservableCollection<string> ListTrangThai { get; set; }
        public ObservableCollection<string> ListLoaiKhachHang { get; set; }
        public ObservableCollection<string> ListSach { get; set; }

        // ============ SEARCH/FILTER (Dùng cho filter ở ngoài DataGrid) ============
        private string _searchTenKM;
        public string SearchTenKM { get => _searchTenKM; set { _searchTenKM = value; OnPropertyChanged(); ApplyFilters(); } }

        private string _selectedLoaiUuDaiFilter;
        public string SelectedLoaiUuDaiFilter
        {
            get => _selectedLoaiUuDaiFilter;
            set { _selectedLoaiUuDaiFilter = value; OnPropertyChanged(); ApplyFilters(); }
        }

        private string _selectedTrangThai;
        public string SelectedTrangThai { get => _selectedTrangThai; set { _selectedTrangThai = value; OnPropertyChanged(); ApplyFilters(); } }

        private DateTime? _searchNgayApDung;
        public DateTime? SearchNgayApDung { get => _searchNgayApDung; set { _searchNgayApDung = value; OnPropertyChanged(); ApplyFilters(); } }

        // ============ POPUP (Dùng riêng cho combobox trong Popup) ============
        private string _selectedLoaiUuDaiInPopup;
        public string SelectedLoaiUuDaiInPopup
        {
            get => _selectedLoaiUuDaiInPopup;
            set { _selectedLoaiUuDaiInPopup = value; OnPropertyChanged(); }
        }

        // ============ STATISTICS ============
        private int _activePromotionsCount;
        public int ActivePromotionsCount { get => _activePromotionsCount; set { _activePromotionsCount = value; OnPropertyChanged(); } }

        private int _upcomingPromotionsCount;
        public int UpcomingPromotionsCount { get => _upcomingPromotionsCount; set { _upcomingPromotionsCount = value; OnPropertyChanged(); } }

        // ============ POPUP ============
        private bool _isPopupVisible;
        public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        private string _popupTitle = "PHIẾU THÔNG TIN ƯU ĐÃI";
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private string _saveButtonText = "LƯU PHIẾU ƯU ĐÃI";
        public string SaveButtonText { get => _saveButtonText; set { _saveButtonText = value; OnPropertyChanged(); } }

        private PromotionModel _editingPromotion;
        public PromotionModel EditingPromotion { get => _editingPromotion; set { _editingPromotion = value; OnPropertyChanged(); } }

        public string NgayLap => DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        // ============ PAGINATION ============
        private int _currentPage = 1;
        public int CurrentPage { get => _currentPage; set { _currentPage = value; OnPropertyChanged(); UpdatePagedData(); } }

        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        private ObservableCollection<PromotionModel> _allPromotions = new ObservableCollection<PromotionModel>();
        private ObservableCollection<PromotionModel> _filteredPromotions = new ObservableCollection<PromotionModel>();

        // ============ MULTI-SELECT ============
        private bool _isMultipleSelected;
        public bool IsMultipleSelected { get => _isMultipleSelected; set { _isMultipleSelected = value; OnPropertyChanged(); } }

        private int _selectedCount;
        public int SelectedCount { get => _selectedCount; set { _selectedCount = value; OnPropertyChanged(); } }

        // ============ COMMANDS ============
        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenEditPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public ICommand SavePromotionCommand { get; }
        public ICommand DeletePromotionCommand { get; }
        public ICommand ToggleStatusCommand { get; }
        public ICommand BulkActivateCommand { get; }
        public ICommand BulkDeactivateCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand GenerateCodeCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand GoToPageCommand { get; }
        public ICommand AddGiamGiaHoaDonRowCommand { get; }
        public ICommand AddTangQuaHoaDonRowCommand { get; }
        public ICommand AddGiamGiaSachRowCommand { get; }
        public ICommand AddTangQuaSachRowCommand { get; }

        // ============ METHODS ============
        public void UpdateSelectedPromotions(System.Collections.IList selectedItems)
        {
            SelectedPromotions.Clear();
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                    if (item is PromotionModel pm) SelectedPromotions.Add(pm);
            }
            IsMultipleSelected = SelectedPromotions.Count > 0;
            SelectedCount = SelectedPromotions.Count;
        }

        private void OpenAddPopup()
        {
            EditingPromotion = new PromotionModel();
            PopupTitle = "PHIẾU THÔNG TIN ƯU ĐÃI";
            SaveButtonText = "LƯU PHIẾU ƯU ĐÃI";
            SelectedLoaiUuDaiInPopup = "Giảm giá trên tổng hóa đơn";
            IsPopupVisible = true;
        }

        private void OpenEditPopup(PromotionModel p)
        {
            if (p == null) return;
            EditingPromotion = new PromotionModel
            {
                Id = p.Id,
                MaUuDai = p.MaUuDai,
                TenChuongTrinh = p.TenChuongTrinh,
                LoaiUuDai = p.LoaiUuDai,
                ThoiGianBatDau = p.ThoiGianBatDau,
                ThoiGianKetThuc = p.ThoiGianKetThuc,
                SoLuongToiDa = p.SoLuongToiDa,
                LoaiKhachHangApDung = p.LoaiKhachHangApDung,
                MoTa = p.MoTa,
                GiaTriApDungTu = p.GiaTriApDungTu,
                GiaTriApDungDen = p.GiaTriApDungDen,
                MucGiamGia = p.MucGiamGia,
                GiamToiDa = p.GiamToiDa,
                SachApDung = p.SachApDung,
                SoLuongApDung = p.SoLuongApDung,
                SachTang = p.SachTang,
                SoLuongTang = p.SoLuongTang,
                SachQuaTang = p.SachQuaTang,
                SoLuongApDungTu = p.SoLuongApDungTu
            };
            PopupTitle = "CHỈNH SỬA PHIẾU ƯU ĐÃI";
            SaveButtonText = "CẬP NHẬT";
            SelectedLoaiUuDaiInPopup = p.LoaiUuDai;
            IsPopupVisible = true;
        }

        private void ClosePopup() { IsPopupVisible = false; }

        private void SavePromotion()
        {
            if (EditingPromotion == null) return;
            if (string.IsNullOrWhiteSpace(EditingPromotion.TenChuongTrinh))
            {
                MessageBox.Show("Vui lòng nhập tên chương trình!");
                return;
            }
            if (string.IsNullOrWhiteSpace(EditingPromotion.MaUuDai))
            {
                GenerateCode();
            }

            // Lấy giá trị từ biến popup, không phải từ biến filter
            EditingPromotion.LoaiUuDai = SelectedLoaiUuDaiInPopup;

            if (!ValidatePromotionDetail()) return;

            if (EditingPromotion.Id == 0)
            {
                EditingPromotion.Id = _allPromotions.Count + 1;
                EditingPromotion.STT = _allPromotions.Count + 1;
                EditingPromotion.TrangThai = DetermineTrangThai(EditingPromotion.ThoiGianBatDau, EditingPromotion.ThoiGianKetThuc);
                _allPromotions.Add(EditingPromotion);
            }
            else
            {
                var existing = _allPromotions.FirstOrDefault(p => p.Id == EditingPromotion.Id);
                if (existing != null)
                {
                    existing.MaUuDai = EditingPromotion.MaUuDai;
                    existing.TenChuongTrinh = EditingPromotion.TenChuongTrinh;
                    existing.LoaiUuDai = EditingPromotion.LoaiUuDai;
                    existing.ThoiGianBatDau = EditingPromotion.ThoiGianBatDau;
                    existing.ThoiGianKetThuc = EditingPromotion.ThoiGianKetThuc;
                    existing.SoLuongToiDa = EditingPromotion.SoLuongToiDa;
                    existing.LoaiKhachHangApDung = EditingPromotion.LoaiKhachHangApDung;
                    existing.MoTa = EditingPromotion.MoTa;
                    existing.TrangThai = DetermineTrangThai(EditingPromotion.ThoiGianBatDau, EditingPromotion.ThoiGianKetThuc);
                    existing.GiaTriApDungTu = EditingPromotion.GiaTriApDungTu;
                    existing.GiaTriApDungDen = EditingPromotion.GiaTriApDungDen;
                    existing.MucGiamGia = EditingPromotion.MucGiamGia;
                    existing.GiamToiDa = EditingPromotion.GiamToiDa;
                    existing.SachApDung = EditingPromotion.SachApDung;
                    existing.SoLuongApDung = EditingPromotion.SoLuongApDung;
                    existing.SachTang = EditingPromotion.SachTang;
                    existing.SoLuongTang = EditingPromotion.SoLuongTang;
                    existing.SachQuaTang = EditingPromotion.SachQuaTang;
                    existing.SoLuongApDungTu = EditingPromotion.SoLuongApDungTu;
                }
            }
            IsPopupVisible = false;
            ApplyFilters();
            RefreshData();
            MessageBox.Show("Lưu thành công!");
        }

        private bool ValidatePromotionDetail()
        {
            // Dùng SelectedLoaiUuDaiInPopup thay vì SelectedLoaiUuDai
            if (SelectedLoaiUuDaiInPopup == "Giảm giá trên tổng hóa đơn")
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.GiaTriApDungTu) ||
                    string.IsNullOrWhiteSpace(EditingPromotion.GiaTriApDungDen))
                {
                    MessageBox.Show("Vui lòng nhập giá trị áp dụng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.MucGiamGia))
                {
                    MessageBox.Show("Vui lòng nhập mức giảm giá!");
                    return false;
                }
            }
            else if (SelectedLoaiUuDaiInPopup == "Giảm giá cho các sản phẩm")
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.SachApDung))
                {
                    MessageBox.Show("Vui lòng chọn sách áp dụng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.MucGiamGia))
                {
                    MessageBox.Show("Vui lòng nhập mức giảm giá!");
                    return false;
                }
            }
            else if (SelectedLoaiUuDaiInPopup == "Tặng quà trên tổng hóa đơn")
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.GiaTriApDungTu) ||
                    string.IsNullOrWhiteSpace(EditingPromotion.GiaTriApDungDen))
                {
                    MessageBox.Show("Vui lòng nhập giá trị áp dụng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.SachTang))
                {
                    MessageBox.Show("Vui lòng chọn sách tặng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.SoLuongTang))
                {
                    MessageBox.Show("Vui lòng nhập số lượng tặng!");
                    return false;
                }
            }
            else if (SelectedLoaiUuDaiInPopup == "Tặng quà khi mua sản phẩm")
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.SachApDung))
                {
                    MessageBox.Show("Vui lòng chọn sách áp dụng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.SachQuaTang))
                {
                    MessageBox.Show("Vui lòng chọn sách quà tặng!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingPromotion.SoLuongTang))
                {
                    MessageBox.Show("Vui lòng nhập số lượng tặng!");
                    return false;
                }
            }
            return true;
        }

        private string DetermineTrangThai(DateTime start, DateTime end)
        {
            var today = DateTime.Today;
            if (today < start) return "Sắp diễn ra";
            if (today > end) return "Đã kết thúc";
            return "Đang chạy";
        }

        private void DeletePromotion(PromotionModel p)
        {
            if (p == null) return;
            if (MessageBox.Show($"Xóa '{p.TenChuongTrinh}'?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _allPromotions.Remove(p);
                UpdateStatistics();
                ApplyFilters();
            }
        }

        private void ToggleStatus(PromotionModel p)
        {
            if (p == null) return;
            p.TrangThai = p.TrangThai switch
            {
                "Đang chạy" => "Tạm dừng",
                "Tạm dừng" => "Đang chạy",
                "Sắp diễn ra" => "Đang chạy",
                _ => p.TrangThai
            };
            UpdateStatistics();
        }

        private void BulkActivate()
        {
            foreach (var p in SelectedPromotions)
                if (p.TrangThai != "Đã kết thúc") p.TrangThai = "Đang chạy";
            UpdateStatistics();
        }

        private void BulkDeactivate()
        {
            foreach (var p in SelectedPromotions)
                if (p.TrangThai == "Đang chạy") p.TrangThai = "Tạm dừng";
            UpdateStatistics();
        }

        private void GenerateCode()
        {
            if (EditingPromotion != null)
                EditingPromotion.MaUuDai = "KM" + new Random().Next(100000, 999999);
        }

        private void AddGiamGiaHoaDonRow() => ChiTietGiamGiaHoaDon.Add(new PromotionModel { STT = ChiTietGiamGiaHoaDon.Count + 1 });
        private void AddTangQuaHoaDonRow() => ChiTietTangQuaHoaDon.Add(new PromotionModel { STT = ChiTietTangQuaHoaDon.Count + 1 });
        private void AddGiamGiaSachRow() => ChiTietGiamGiaSach.Add(new PromotionModel { STT = ChiTietGiamGiaSach.Count + 1 });
        private void AddTangQuaSachRow() => ChiTietTangQuaSach.Add(new PromotionModel { STT = ChiTietTangQuaSach.Count + 1 });

        private void ClearFilters()
        {
            SearchTenKM = null;
            SelectedLoaiUuDaiFilter = null;
            SelectedTrangThai = null;
            SearchNgayApDung = null;
        }

        private void ApplyFilters()
        {
            var filtered = _allPromotions.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTenKM))
                filtered = filtered.Where(p => p.TenChuongTrinh.ToLower().Contains(SearchTenKM.ToLower()));

            if (!string.IsNullOrWhiteSpace(SelectedLoaiUuDaiFilter) && SelectedLoaiUuDaiFilter != "Tất cả")
                filtered = filtered.Where(p => p.LoaiUuDai == SelectedLoaiUuDaiFilter);

            if (!string.IsNullOrWhiteSpace(SelectedTrangThai) && SelectedTrangThai != "Tất cả")
                filtered = filtered.Where(p => p.TrangThai == SelectedTrangThai);

            if (SearchNgayApDung.HasValue)
            {
                var date = SearchNgayApDung.Value.Date;
                filtered = filtered.Where(p => p.ThoiGianBatDau <= date && p.ThoiGianKetThuc >= date);
            }

            _filteredPromotions.Clear();
            foreach (var item in filtered)
                _filteredPromotions.Add(item);

            CurrentPage = 1;
            UpdatePagination();
            UpdateStatistics();
        }

        private void RefreshData()
        {
            ApplyFilters();
        }

        private void UpdateStatistics()
        {
            ActivePromotionsCount = _filteredPromotions.Count(p => p.TrangThai == "Đang chạy");
            UpcomingPromotionsCount = _filteredPromotions.Count(p => p.TrangThai == "Sắp diễn ra");
        }

        private void UpdatePagination()
        {
            TotalPages = Math.Max(1, (int)Math.Ceiling((double)_filteredPromotions.Count / PageSize));
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;
            PageNumbers.Clear();
            for (int i = 1; i <= TotalPages; i++) PageNumbers.Add(i);
            UpdatePagedData();
        }

        private void UpdatePagedData()
        {
            PagedPromotions.Clear();
            var paged = _filteredPromotions.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            foreach (var item in paged)
                PagedPromotions.Add(item);
        }

        private void FirstPage() => CurrentPage = 1;
        private void PrevPage() { if (CurrentPage > 1) CurrentPage--; }
        private void NextPage() { if (CurrentPage < TotalPages) CurrentPage++; }
        private void LastPage() => CurrentPage = TotalPages;
        private void GoToPage(int page) { if (page >= 1 && page <= TotalPages) CurrentPage = page; }
        private void ExportExcel() => MessageBox.Show("Xuất Excel thành công!");

        private void LoadSampleData()
        {
            var today = DateTime.Today;
            _allPromotions.Add(new PromotionModel
            {
                Id = 1,
                STT = 1,
                MaUuDai = "KM001",
                TenChuongTrinh = "Giảm 20% tổng hóa đơn",
                LoaiUuDai = "Giảm giá trên tổng hóa đơn",
                LoaiKhachHangApDung = "Tất cả",
                ThoiGianBatDau = today.AddDays(-5),
                ThoiGianKetThuc = today.AddDays(25),
                SoLuongDaDung = 15,
                SoLuongToiDa = 100,
                TrangThai = DetermineTrangThai(today.AddDays(-5), today.AddDays(25)),
                GiaTriApDungTu = "500000",
                GiaTriApDungDen = "1000000",
                MucGiamGia = "20%",
                GiamToiDa = "200000"
            });
            _allPromotions.Add(new PromotionModel
            {
                Id = 2,
                STT = 2,
                MaUuDai = "KM002",
                TenChuongTrinh = "Tặng sách thiếu nhi",
                LoaiUuDai = "Tặng quà trên tổng hóa đơn",
                LoaiKhachHangApDung = "Thành viên",
                ThoiGianBatDau = today,
                ThoiGianKetThuc = today.AddDays(15),
                SoLuongDaDung = 0,
                SoLuongToiDa = 50,
                TrangThai = DetermineTrangThai(today, today.AddDays(15)),
                GiaTriApDungTu = "300000",
                GiaTriApDungDen = "500000",
                SachTang = "Sách Thiếu nhi",
                SoLuongTang = "1"
            });
            _allPromotions.Add(new PromotionModel
            {
                Id = 3,
                STT = 3,
                MaUuDai = "KM003",
                TenChuongTrinh = "Giảm giá sách văn học",
                LoaiUuDai = "Giảm giá cho các sản phẩm",
                LoaiKhachHangApDung = "VIP",
                ThoiGianBatDau = today.AddDays(-10),
                ThoiGianKetThuc = today.AddDays(-1),
                SoLuongDaDung = 30,
                SoLuongToiDa = 200,
                TrangThai = DetermineTrangThai(today.AddDays(-10), today.AddDays(-1)),
                SachApDung = "Sách Văn học",
                SoLuongApDung = "2",
                MucGiamGia = "15%",
                GiamToiDa = "50000"
            });
            _allPromotions.Add(new PromotionModel
            {
                Id = 4,
                STT = 4,
                MaUuDai = "KM004",
                TenChuongTrinh = "Tặng bookmark",
                LoaiUuDai = "Tặng quà khi mua sản phẩm",
                LoaiKhachHangApDung = "Tất cả",
                ThoiGianBatDau = today.AddDays(-3),
                ThoiGianKetThuc = today.AddDays(2),
                SoLuongDaDung = 45,
                SoLuongToiDa = 0,
                TrangThai = DetermineTrangThai(today.AddDays(-3), today.AddDays(2)),
                SachApDung = "Sách Khoa học",
                SoLuongApDungTu = "2",
                SachQuaTang = "Bookmark",
                SoLuongTang = "1"
            });
            _allPromotions.Add(new PromotionModel
            {
                Id = 5,
                STT = 5,
                MaUuDai = "KM005",
                TenChuongTrinh = "Flash Sale 30%",
                LoaiUuDai = "Giảm giá trên tổng hóa đơn",
                LoaiKhachHangApDung = "Khách lẻ",
                ThoiGianBatDau = today.AddDays(1),
                ThoiGianKetThuc = today.AddDays(30),
                SoLuongDaDung = 0,
                SoLuongToiDa = 50,
                TrangThai = DetermineTrangThai(today.AddDays(1), today.AddDays(30)),
                GiaTriApDungTu = "200000",
                GiaTriApDungDen = "500000",
                MucGiamGia = "30%",
                GiamToiDa = "150000"
            });

            ApplyFilters();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
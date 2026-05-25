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

        // Chi tiết ưu đãi
        public string GiaTriTu { get; set; }
        public string GiaTriDen { get; set; }
        public bool IsGiamTien { get; set; }
        public bool IsGiamPhanTram { get; set; }
        public string GiaTriGiam { get; set; }
        public string SachApDungDisplay { get; set; }
        public string SoLuongApDung { get; set; }
        public string SachTangDisplay { get; set; }
        public string SoLuongTang { get; set; }

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
                "Tặng quà trên tổng hóa đơn",
                "Giảm giá trên đầu sách",
                "Tặng quà khi mua sách"
            };
            ListTrangThai = new ObservableCollection<string> { "Tất cả", "Đang chạy", "Sắp diễn ra", "Đã kết thúc", "Tạm dừng" };
            ListLoaiKhachHang = new ObservableCollection<string> { "Tất cả", "Thành viên", "VIP", "Khách lẻ" };

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

        // ============ SEARCH/FILTER ============
        private string _searchTenKM;
        public string SearchTenKM { get => _searchTenKM; set { _searchTenKM = value; OnPropertyChanged(); ApplyFilters(); } }

        private string _selectedLoaiUuDai;
        public string SelectedLoaiUuDai
        {
            get => _selectedLoaiUuDai;
            set { _selectedLoaiUuDai = value; OnPropertyChanged(); UpdatePanelVisibility(); ApplyFilters(); }
        }

        private string _selectedTrangThai;
        public string SelectedTrangThai { get => _selectedTrangThai; set { _selectedTrangThai = value; OnPropertyChanged(); ApplyFilters(); } }

        private DateTime? _searchNgayApDung;
        public DateTime? SearchNgayApDung { get => _searchNgayApDung; set { _searchNgayApDung = value; OnPropertyChanged(); ApplyFilters(); } }

        // ============ STATISTICS ============
        private int _activePromotionsCount;
        public int ActivePromotionsCount { get => _activePromotionsCount; set { _activePromotionsCount = value; OnPropertyChanged(); } }

        private int _upcomingPromotionsCount;
        public int UpcomingPromotionsCount { get => _upcomingPromotionsCount; set { _upcomingPromotionsCount = value; OnPropertyChanged(); } }

        // ============ POPUP ============
        private bool _isPopupVisible;
        public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        private string _popupTitle = "PHIẾU THÔNG TIN ƯU ĐÃI ";
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private string _saveButtonText = "LƯU PHIẾU ƯU ĐÃI";
        public string SaveButtonText { get => _saveButtonText; set { _saveButtonText = value; OnPropertyChanged(); } }

        private PromotionModel _editingPromotion;
        public PromotionModel EditingPromotion { get => _editingPromotion; set { _editingPromotion = value; OnPropertyChanged(); } }

        public string NgayLap => DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        // ============ PAGINATION ============
        private int _currentPage = 1;
        public int CurrentPage { get => _currentPage; set { _currentPage = value; OnPropertyChanged(); } }

        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        // ============ MULTI-SELECT ============
        private bool _isMultipleSelected;
        public bool IsMultipleSelected { get => _isMultipleSelected; set { _isMultipleSelected = value; OnPropertyChanged(); } }

        private int _selectedCount;
        public int SelectedCount { get => _selectedCount; set { _selectedCount = value; OnPropertyChanged(); } }

        // ============ PANEL VISIBILITY ============
        private bool _isGiamGiaHoaDon;
        public bool IsGiamGiaHoaDon { get => _isGiamGiaHoaDon; set { _isGiamGiaHoaDon = value; OnPropertyChanged(); } }

        private bool _isTangQuaHoaDon;
        public bool IsTangQuaHoaDon { get => _isTangQuaHoaDon; set { _isTangQuaHoaDon = value; OnPropertyChanged(); } }

        private bool _isGiamGiaSach;
        public bool IsGiamGiaSach { get => _isGiamGiaSach; set { _isGiamGiaSach = value; OnPropertyChanged(); } }

        private bool _isTangQuaSach;
        public bool IsTangQuaSach { get => _isTangQuaSach; set { _isTangQuaSach = value; OnPropertyChanged(); } }

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
            foreach (var item in selectedItems)
                if (item is PromotionModel pm) SelectedPromotions.Add(pm);
            IsMultipleSelected = SelectedPromotions.Count > 0;
            SelectedCount = SelectedPromotions.Count;
        }

        private void OpenAddPopup()
        {
            EditingPromotion = new PromotionModel();
            PopupTitle = "PHIẾU THÔNG TIN ƯU ĐÃI ";
            SaveButtonText = "LƯU PHIẾU ƯU ĐÃI";
            SelectedLoaiUuDai = "Giảm giá trên tổng hóa đơn";
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
                MoTa = p.MoTa
            };
            PopupTitle = "CHỈNH SỬA PHIẾU ƯU ĐÃI";
            SaveButtonText = "CẬP NHẬT";
            SelectedLoaiUuDai = p.LoaiUuDai;
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
            IsPopupVisible = false;
            RefreshData();
            MessageBox.Show("Lưu thành công!");
        }

        private void DeletePromotion(PromotionModel p)
        {
            if (p == null) return;
            if (MessageBox.Show($"Xóa '{p.TenChuongTrinh}'?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PagedPromotions.Remove(p);
                UpdateStatistics();
                UpdatePagination();
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

        private void UpdatePanelVisibility()
        {
            IsGiamGiaHoaDon = SelectedLoaiUuDai == "Giảm giá trên tổng hóa đơn";
            IsTangQuaHoaDon = SelectedLoaiUuDai == "Tặng quà trên tổng hóa đơn";
            IsGiamGiaSach = SelectedLoaiUuDai == "Giảm giá trên đầu sách";
            IsTangQuaSach = SelectedLoaiUuDai == "Tặng quà khi mua sách";
        }

        private void ClearFilters()
        {
            _searchTenKM = null; _selectedLoaiUuDai = null; _selectedTrangThai = null; _searchNgayApDung = null;
            OnPropertyChanged(nameof(SearchTenKM)); OnPropertyChanged(nameof(SelectedLoaiUuDai));
            OnPropertyChanged(nameof(SelectedTrangThai)); OnPropertyChanged(nameof(SearchNgayApDung));
            ApplyFilters();
        }

        private void ApplyFilters() { CurrentPage = 1; UpdatePagination(); }
        private void RefreshData() { UpdateStatistics(); UpdatePagination(); }

        private void UpdateStatistics()
        {
            ActivePromotionsCount = PagedPromotions.Count(p => p.TrangThai == "Đang chạy");
            UpcomingPromotionsCount = PagedPromotions.Count(p => p.TrangThai == "Sắp diễn ra");
        }

        private void UpdatePagination()
        {
            TotalPages = Math.Max(1, (int)Math.Ceiling((double)PagedPromotions.Count / PageSize));
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;
            PageNumbers.Clear();
            for (int i = 1; i <= TotalPages; i++) PageNumbers.Add(i);
        }

        private void FirstPage() => CurrentPage = 1;
        private void PrevPage() { if (CurrentPage > 1) CurrentPage--; }
        private void NextPage() { if (CurrentPage < TotalPages) CurrentPage++; }
        private void LastPage() => CurrentPage = TotalPages;
        private void GoToPage(int page) { if (page >= 1 && page <= TotalPages) CurrentPage = page; }
        private void ExportExcel() => MessageBox.Show("Xuất Excel thành công!");

        private void LoadSampleData()
        {
            PagedPromotions.Add(new PromotionModel { Id = 1, STT = 1, MaUuDai = "KM001", TenChuongTrinh = "Giảm 20% tổng hóa đơn", LoaiUuDai = "Giảm giá trên tổng hóa đơn", LoaiKhachHangApDung = "Tất cả", ThoiGianBatDau = DateTime.Today.AddDays(-5), ThoiGianKetThuc = DateTime.Today.AddDays(25), SoLuongDaDung = 15, SoLuongToiDa = 100, TrangThai = "Đang chạy" });
            PagedPromotions.Add(new PromotionModel { Id = 2, STT = 2, MaUuDai = "KM002", TenChuongTrinh = "Tặng sách thiếu nhi", LoaiUuDai = "Tặng quà trên tổng hóa đơn", LoaiKhachHangApDung = "Thành viên", ThoiGianBatDau = DateTime.Today, ThoiGianKetThuc = DateTime.Today.AddDays(15), SoLuongDaDung = 0, SoLuongToiDa = 50, TrangThai = "Sắp diễn ra" });
            PagedPromotions.Add(new PromotionModel { Id = 3, STT = 3, MaUuDai = "KM003", TenChuongTrinh = "Giảm giá sách văn học", LoaiUuDai = "Giảm giá trên đầu sách", LoaiKhachHangApDung = "VIP", ThoiGianBatDau = DateTime.Today.AddDays(-10), ThoiGianKetThuc = DateTime.Today.AddDays(-1), SoLuongDaDung = 30, SoLuongToiDa = 200, TrangThai = "Đã kết thúc" });
            PagedPromotions.Add(new PromotionModel { Id = 4, STT = 4, MaUuDai = "KM004", TenChuongTrinh = "Tặng bookmark", LoaiUuDai = "Tặng quà khi mua sách", LoaiKhachHangApDung = "Tất cả", ThoiGianBatDau = DateTime.Today.AddDays(-3), ThoiGianKetThuc = DateTime.Today.AddDays(2), SoLuongDaDung = 45, SoLuongToiDa = 0, TrangThai = "Đang chạy" });
            PagedPromotions.Add(new PromotionModel { Id = 5, STT = 5, MaUuDai = "KM005", TenChuongTrinh = "Flash Sale 30%", LoaiUuDai = "Giảm giá trên tổng hóa đơn", LoaiKhachHangApDung = "Khách lẻ", ThoiGianBatDau = DateTime.Today.AddDays(1), ThoiGianKetThuc = DateTime.Today.AddDays(30), SoLuongDaDung = 0, SoLuongToiDa = 50, TrangThai = "Sắp diễn ra" });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
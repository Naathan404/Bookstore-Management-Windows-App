using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.Share.DTOs;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using Bookstore.WPF.ViewModels.Base;
using OfficeOpenXml.Export.HtmlExport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class PromotionViewModel : BaseListViewModel
    {
        private List<PromotionDTO> _allPromotions = new List<PromotionDTO>();

        private ObservableCollection<PromotionDTO> _pagedPromotions = new ObservableCollection<PromotionDTO>();
        public ObservableCollection<PromotionDTO> PagedPromotions
        {
            get => _pagedPromotions;
            set { _pagedPromotions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CustomerTierResponse> _listLoaiKhachHang = new ObservableCollection<CustomerTierResponse>();
        public ObservableCollection<CustomerTierResponse> ListLoaiKhachHang
        {
            get => _listLoaiKhachHang;
            set { _listLoaiKhachHang = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BookItem> _listSach = new ObservableCollection<BookItem>();
        public ObservableCollection<BookItem> ListSach
        {
            get => _listSach;
            set { _listSach = value; OnPropertyChanged(); }
        }

        private string _searchTenKM;
        public string SearchTenKM 
        { 
            get => _searchTenKM; 
            set 
            { _searchTenKM = value; 
                OnPropertyChanged();
                ApplyFilterAndPagination();
            } 
        }

        public ObservableCollection<string> ListTieuChiTimKiem { get; set; } = new ObservableCollection<string> { "Tất cả", "Tên chương trình", "Mã Code" };
        private string _selectedTieuChiTimKiem = "Tất cả";
        public string SelectedTieuChiTimKiem
        {
            get => _selectedTieuChiTimKiem;
            set { _selectedTieuChiTimKiem = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        public ObservableCollection<string> ListLoaiUuDai { get; set; } = new ObservableCollection<string>();
        private string _selectedLoaiUuDaiFilter;
        public string SelectedLoaiUuDaiFilter 
        { 
            get => _selectedLoaiUuDaiFilter; 
            set 
            { 
                _selectedLoaiUuDaiFilter = value; 
                OnPropertyChanged();
                ApplyFilterAndPagination();
            } 
        }

        public ObservableCollection<string> ListTrangThai { get; set; } = new ObservableCollection<string>();
        private string _selectedTrangThai;
        public string SelectedTrangThai 
        { 
            get => _selectedTrangThai; 
            set 
            { 
                _selectedTrangThai = value; OnPropertyChanged();
                ApplyFilterAndPagination();
            }
        }

        private DateTime? _searchTuNgay;
        public DateTime? SearchTuNgay
        {
            get => _searchTuNgay;
            set 
            { _searchTuNgay = value; 
                OnPropertyChanged(); 
                ApplyFilterAndPagination(); 
            }
        }

        private DateTime? _searchDenNgay;
        public DateTime? SearchDenNgay
        {
            get => _searchDenNgay;
            set 
            { 
                _searchDenNgay = value; 
                OnPropertyChanged(); 
                ApplyFilterAndPagination(); 
            }
        }

        // POPUP THÊM/SỬA
        private bool _isPopupVisible;
        public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        private string _popupTitle = string.Empty;
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private string _saveButtonText = string.Empty;
        public string SaveButtonText { get => _saveButtonText; set { _saveButtonText = value; OnPropertyChanged(); } }

        private PromotionDTO _editingPromotion = new PromotionDTO();
        public PromotionDTO EditingPromotion { get => _editingPromotion; set { _editingPromotion = value; OnPropertyChanged(); } }

        private bool _isAddMode;

        private bool _isCoreEditingAllowed;
        public bool IsCoreEditingAllowed
        {
            get => _isCoreEditingAllowed;
            set { _isCoreEditingAllowed = value; OnPropertyChanged(); }
        }

        private bool _isGiamTienMode;
        public bool IsGiamTienMode
        {
            get => _isGiamTienMode;
            set { _isGiamTienMode = value; OnPropertyChanged(); }
        }

        private bool _isGiamPhanTramMode;
        public bool IsGiamPhanTramMode
        {
            get => _isGiamPhanTramMode;
            set { _isGiamPhanTramMode = value; OnPropertyChanged(); }
        }

        // commands
        public ICommand OpenAddPopupCommand { get; set; }
        public ICommand OpenEditPopupCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand SavePromotionCommand { get; set; }
        public ICommand DeletePromotionCommand { get; set; }
        public ICommand ToggleStatusCommand { get; set; }

        // Commands cho Filter & Pagination
        public ICommand ClearFilterCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public PromotionViewModel()
        {
            InitCommands();
            _ = InitDropdownData();
            _ = LoadDataAsync();
        }

        private async Task InitDropdownData()
        {
            var customerTypes = await ApiClient.GetAsync<List<CustomerTierResponse>>("api/LoaiKhachHang");
            if (customerTypes != null)
            {
                foreach (var c in customerTypes)
                {
                    ListLoaiKhachHang.Add(new CustomerTierResponse { MaLoaiKhachHang = c.MaLoaiKhachHang, TenLoaiKhachHang = c.TenLoaiKhachHang });
                }
            }

            var books = await ApiClient.GetAsync<List<SachDTO>>("api/PhienBanSach");
            if (books != null)
            {
                foreach (var b in books)
                {
                    if (string.IsNullOrEmpty(b.ISBN))
                    {
                        MessageBox.Show($"Báo động: Cuốn sách '{b.TenSach}' bị mất mã ISBN từ Backend trả về! Kiểm tra lại SachDTO ngay!", "Lỗi mapping JSON");
                    }
                    ListSach.Add(new BookItem { ISBN = b.ISBN, TenSach = b.TenSach });
                }
            }


            
            ListLoaiUuDai.Add("Tất cả loại ưu đãi");
            ListLoaiUuDai.Add("Giảm giá/ Tổng hóa đơn");
            ListLoaiUuDai.Add("Tặng quà / Tổng hóa đơn");
            ListLoaiUuDai.Add("Giảm giá / Đầu sách");
            ListLoaiUuDai.Add("Tặng quà / Đầu sách");
            SelectedLoaiUuDaiFilter = "Tất cả loại ưu đãi";

            ListTrangThai.Add("Tất cả trạng thái");
            ListTrangThai.Add("Đang áp dụng");
            ListTrangThai.Add("Chưa áp dụng");
            ListTrangThai.Add("Tạm dừng");
            ListTrangThai.Add("Hết hạn");
            SelectedTrangThai = "Tất cả trạng thái";
        }

        private void InitCommands()
        {
            OpenAddPopupCommand = new RelayCommand<object>(p =>
            {
                _isAddMode = true;
                PopupTitle = "LẬP PHIẾU THÔNG TIN ƯU ĐÃI MỚI";
                SaveButtonText = "LƯU PHIẾU ƯU ĐÃI";
                EditingPromotion = new PromotionDTO
                {
                    NgayTao = DateTime.Now,
                    NgayBatDau = DateTime.Today,
                    NgayKetThuc = DateTime.Today.AddDays(30),
                    SoLuongToiDa = 50,
                    MaLoaiUuDai = 0,
                    MaLoaiKhachHang = 1 
                };
                IsPopupVisible = true;
                IsCoreEditingAllowed = true;
                IsGiamTienMode = true;
                IsGiamPhanTramMode = false;
            });

            OpenEditPopupCommand = new RelayCommand<PromotionDTO>(promo =>
            {
                if (promo == null) return;
                _isAddMode = false;
                PopupTitle = "CẬP NHẬT CHI TIẾT PHIẾU ƯU ĐÃI";
                SaveButtonText = "CẬP NHẬT PHIẾU";

                IsCoreEditingAllowed = promo.SoLuongDaDung == 0;
                if (promo.TiLeGiam > 0)
                {
                    IsGiamTienMode = false;
                    IsGiamPhanTramMode = true;
                }
                else
                {
                    IsGiamTienMode = true;
                    IsGiamPhanTramMode = false;
                }

                EditingPromotion = new PromotionDTO
                {
                    MaUuDai = promo.MaUuDai,
                    NgayTao = promo.NgayTao,
                    Code = promo.Code,
                    TenChuongTrinh = promo.TenChuongTrinh,
                    MoTa = promo.MoTa,
                    NgayBatDau = promo.NgayBatDau,
                    NgayKetThuc = promo.NgayKetThuc,
                    SoLuongToiDa = promo.SoLuongToiDa,
                    MaLoaiKhachHang = promo.MaLoaiKhachHang,
                    MaLoaiUuDai = promo.MaLoaiUuDai,
                    SoTienToiThieu = promo.SoTienToiThieu,
                    SoTienToiDa = promo.SoTienToiDa,
                    SoTienGiam = promo.SoTienGiam,
                    TiLeGiam = promo.TiLeGiam,
                    GiamToiDa = promo.GiamToiDa,
                    ISBNDieuKien = promo.ISBNDieuKien,
                    SoLuongMua = promo.SoLuongMua,
                    ISBNTang = promo.ISBNTang,
                    SoLuongTang = promo.SoLuongTang,
                    CoTheSuDung = promo.CoTheSuDung
                };
                IsPopupVisible = true;
            });

            ClosePopupCommand = new RelayCommand<object>(p => IsPopupVisible = false);
            SavePromotionCommand = new RelayCommand<object>(async p => await SavePromotionAsync());

            ToggleStatusCommand = new RelayCommand<PromotionDTO>(async promo =>
            {
                if (promo == null) return;
                try
                {
                    bool success = await ApiClient.PutAsync<object, bool>($"api/UuDai/ToggleStatus/{promo.MaUuDai}", null);
                    if (success) await LoadDataAsync();
                }
                catch { promo.CoTheSuDung = !promo.CoTheSuDung; PagedPromotions = new ObservableCollection<PromotionDTO>(_allPromotions); }
            });

            DeletePromotionCommand = new RelayCommand<PromotionDTO>(async promo =>
            {
                if (promo == null) return;
                var res = MessageBox.Show($"Xóa vĩnh viễn mã ưu đãi {promo.Code}?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool success = await ApiClient.DeleteAsync($"api/UuDai/{promo.MaUuDai}");
                        if (success) await LoadDataAsync();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
                }
            });

            // Khởi tạo Commands giả cho thanh Tìm kiếm & Phân trang
            ClearFilterCommand = new RelayCommand<object>(p => { 
                SearchTenKM = ""; 
                SelectedTieuChiTimKiem = "Tất cả";
                SelectedLoaiUuDaiFilter = "Tất cả loại ưu đãi";
                SelectedTrangThai = "Tất cả trạng thái";
                SearchTuNgay = null;  
                SearchDenNgay = null;
            });
            RefreshCommand = new RelayCommand<object>(async p => await LoadDataAsync());
        }

        protected override void ApplyFilterAndPagination()
        {
            var filtered = _allPromotions.AsEnumerable();

            // 1. Lọc theo Loại ưu đãi
            if (!string.IsNullOrEmpty(SelectedLoaiUuDaiFilter) && SelectedLoaiUuDaiFilter != "Tất cả loại ưu đãi")
            {
                filtered = filtered.Where(x => x.LoaiUuDai == SelectedLoaiUuDaiFilter);
            }

            // 2. Lọc theo Trạng thái
            if (!string.IsNullOrEmpty(SelectedTrangThai) && SelectedTrangThai != "Tất cả trạng thái")
            {
                filtered = filtered.Where(x => x.TrangThai == SelectedTrangThai);
            }

            // 3. Lọc theo khoảng thời gian
            if (SearchTuNgay.HasValue)
            {
                var tuNgay = SearchTuNgay.Value.Date;
                filtered = filtered.Where(x => x.NgayKetThuc.Date >= tuNgay);
            }

            if (SearchDenNgay.HasValue)
            {
                var denNgay = SearchDenNgay.Value.Date;
                filtered = filtered.Where(x => x.NgayBatDau.Date <= denNgay);
            }

            // 4. Lọc theo Từ khóa tìm kiếm (Lưu ý: Nếu dùng BaseListViewModel, bạn có thể cân nhắc dùng biến SearchKeyword thay cho SearchTenKM)
            if (!string.IsNullOrWhiteSpace(SearchTenKM))
            {
                var keyword = SearchTenKM.Trim();
                if (SelectedTieuChiTimKiem == "Tên chương trình")
                {
                    filtered = filtered.Where(x => x.TenChuongTrinh != null && x.TenChuongTrinh.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedTieuChiTimKiem == "Mã Code")
                {
                    filtered = filtered.Where(x => x.Code != null && x.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    filtered = filtered.Where(x =>
                        (x.TenChuongTrinh != null && x.TenChuongTrinh.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (x.Code != null && x.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    );
                }
            }

            // ==========================================
            // KHU VỰC CHUẨN HÓA PHÂN TRANG (ĐÃ FIX LỖI)
            // ==========================================
            var resultList = filtered.ToList();

            // Cập nhật biến Tổng bản ghi cho BaseListViewModel để hiển thị lên UI
            TongBanGhi = resultList.Count;

            // Tính tổng số trang
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;

            // Kiểm tra an toàn cho trang hiện tại
            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;

            // Cắt lấy dữ liệu của trang hiện tại
            var pagedData = resultList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize).ToList();

            // Đánh số thứ tự (STT)
            int index = (TrangHienTai - 1) * PageSize + 1;
            foreach (var item in pagedData)
            {
                item.STT = index++;
            }
            PagedPromotions = new ObservableCollection<PromotionDTO>(pagedData);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var response = await ApiClient.GetAsync<List<PromotionDTO>>("api/UuDai");

                if (response != null)
                {
                    _allPromotions = response;
                }
                else
                {
                    MessageBox.Show("API trả về NULL. Vui lòng kiểm tra lại Swagger (Backend) xem có bị lỗi 500 không, hoặc kiểm tra các trường DTO có khớp chữ Hoa/Thường với JSON không!", "Lỗi Binding DTO");
                }

                ApplyFilterAndPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gọi API: {ex.Message}\nChi tiết: {ex.StackTrace}", "Phát hiện lỗi nghiêm trọng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SavePromotionAsync()
        {
            MessageBox.Show($"Mã sách tặng đang là: '{EditingPromotion.ISBNTang}'");
            if (string.IsNullOrWhiteSpace(EditingPromotion.Code) || string.IsNullOrWhiteSpace(EditingPromotion.TenChuongTrinh))
            {
                MessageBox.Show("Mã Code và Tên chương trình ưu đãi không được bỏ trống!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EditingPromotion.NgayBatDau.Date > EditingPromotion.NgayKetThuc.Date)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!", "Lỗi logic thời gian", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EditingPromotion.SoLuongToiDa <= 0)
            {
                MessageBox.Show("Số lượng phát hành tối đa phải là số dương (> 0)!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int loai = EditingPromotion.MaLoaiUuDai;
            if (loai == 0 || loai == 1)
            {
                if (EditingPromotion.SoTienToiThieu < 0 || EditingPromotion.SoTienToiDa < 0)
                {
                    MessageBox.Show("Số tiền hóa đơn yêu cầu không được là số âm!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (EditingPromotion.SoTienToiDa > 0 && EditingPromotion.SoTienToiThieu > EditingPromotion.SoTienToiDa)
                {
                    MessageBox.Show("Số tiền tối đa không được nhỏ hơn số tiền tối thiểu!", "Lỗi logic", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (loai == 2 || loai == 3)
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.ISBNDieuKien))
                {
                    MessageBox.Show("Vui lòng chọn [Sách bắt buộc mua] đối với loại ưu đãi này!", "Thiếu dữ kiện", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (EditingPromotion.SoLuongMua <= 0)
                {
                    MessageBox.Show("Số lượng sách yêu cầu mua phải từ 1 trở lên!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Nhóm Giảm Giá (Loại 0 và 2)
            if (loai == 0 || loai == 2)
            {
                if (IsGiamTienMode)
                {
                    if (EditingPromotion.SoTienGiam <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập Số tiền giảm hợp lệ (> 0)!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // Dọn rác dữ liệu ẩn
                    EditingPromotion.TiLeGiam = 0;
                    EditingPromotion.GiamToiDa = 0;
                    EditingPromotion.ISBNTang = null;
                    EditingPromotion.SoLuongTang = 0;
                }
                else if (IsGiamPhanTramMode)
                {
                    if (EditingPromotion.TiLeGiam <= 0 || EditingPromotion.TiLeGiam > 100)
                    {
                        MessageBox.Show("Tỉ lệ phần trăm giảm giá phải nằm trong khoảng từ 1% đến 100%!", "Lỗi tỉ lệ", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (EditingPromotion.GiamToiDa <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập Mức giảm tối đa hợp lệ (> 0)!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // Dọn rác dữ liệu ẩn
                    EditingPromotion.SoTienGiam = 0;
                    EditingPromotion.ISBNTang = null;
                    EditingPromotion.SoLuongTang = 0;
                }
            }

            else if (loai == 1 || loai == 3)
            {
                if (string.IsNullOrWhiteSpace(EditingPromotion.ISBNTang))
                {
                    MessageBox.Show("Vui lòng chọn Sách làm quà tặng!", "Thiếu dữ kiện", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (EditingPromotion.SoLuongTang <= 0)
                {
                    MessageBox.Show("Số lượng sách tặng phải từ 1 trở lên!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                EditingPromotion.SoTienGiam = 0;
                EditingPromotion.TiLeGiam = 0;
                EditingPromotion.GiamToiDa = 0;
            }

            try
            {
                bool success;
                if (_isAddMode) success = await ApiClient.PostAsync<PromotionDTO, bool>("api/UuDai", EditingPromotion);
                else success = await ApiClient.PutAsync<PromotionDTO, bool>($"api/UuDai/{EditingPromotion.MaUuDai}", EditingPromotion);

                if (success)
                {
                    IsPopupVisible = false;
                    if (_isAddMode) MessageBox.Show("Thêm phiếu ưu đãi thành công", "Thông báo", MessageBoxButton.OK);
                    else MessageBox.Show("Cập nhật phiếu ưu đãi thành công", "Thông báo", MessageBoxButton.OK);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi xử lý"); }
        }
    }
}
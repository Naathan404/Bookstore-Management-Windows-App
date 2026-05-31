using Bookstore.Share.DTO;
using Bookstore.WPF.Services; 
using Bookstore.WPF.Utils;    
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Bookstore.WPF.ViewModels
{
    public class SupplierViewModel : BaseViewModel
    {
        private List<SupplierDTO> _allSuppliers = new List<SupplierDTO>();

        #region Properties Trạng thái & Dữ liệu
        // Skeleton Loading
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SupplierDTO> _pagedSuppliers = new ObservableCollection<SupplierDTO>();
        public ObservableCollection<SupplierDTO> PagedSuppliers
        {
            get => _pagedSuppliers;
            set { _pagedSuppliers = value; OnPropertyChanged(); }
        }

        private PackIconKind _popUpIcon = PackIconKind.TruckAdd;
        public PackIconKind PopUpIcon
        {
            get => _popUpIcon;
            set
            {
                _popUpIcon = value;
                OnPropertyChanged(nameof(PopUpIcon));
            }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get => _totalRecords;
            set { _totalRecords = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties Tìm kiếm
        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                ApplyFilterAndPagination(); 
            }
        }

        public List<string> SearchTypes { get; set; } = new List<string> { "Tìm tất cả", "Tìm tên nhà cung cấp", "Tìm số điện thoại", "Tìm người đại diện" };

        private string _selectedSearchType;
        public string SelectedSearchType
        {
            get => _selectedSearchType;
            set { _selectedSearchType = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        public List<string> SearchStatus { get; set; } = new List<string> { "Tất cả trạng thái", "Đang giao dịch", "Ngưng giao dịch" };
        private string _selectedSearchStatus;
        public string SelectedSearchStatus
        {
            get => _selectedSearchStatus;
            set
            {
                _selectedSearchStatus = value;
                OnPropertyChanged(nameof(SelectedSearchStatus));
                ApplyFilterAndPagination();
            }
        }

        #endregion

        #region Properties Phân trang
        private int _currentPage = 1;
        public int CurrentPage { get => _currentPage; set { _currentPage = value; OnPropertyChanged(); } }

        private int _totalPages = 1;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        private int _pageSize = 10;
        public int PageSize { get => _pageSize; set { _pageSize = value; OnPropertyChanged(); CurrentPage = 1; ApplyFilterAndPagination(); } }

        private ObservableCollection<int> _pageNumbers = new ObservableCollection<int>();
        public ObservableCollection<int> PageNumbers { get => _pageNumbers; set { _pageNumbers = value; OnPropertyChanged(); } }

        #endregion

        #region Properties Popup Form
        private bool _isPopupVisible;
        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set { _isPopupVisible = value; OnPropertyChanged(); }
        }

        private string _popupTitle = string.Empty;
        public string PopupTitle
        {
            get => _popupTitle;
            set { _popupTitle = value; OnPropertyChanged(); }
        }

        private SupplierDTO _editingSupplier = new SupplierDTO();
        public SupplierDTO EditingSupplier
        {
            get => _editingSupplier;
            set { _editingSupplier = value; OnPropertyChanged(); }
        }

        private bool _isAddMode;
        public bool IsAddMode
        {
            get => _isAddMode;
            set { _isAddMode = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand ClearFilterCommand { get; set; }
        public ICommand FirstPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }

        public ICommand OpenAddPopupCommand { get; set; }
        public ICommand OpenEditPopupCommand { get; set; }
        public ICommand DeleteSupplierCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand SaveSupplierCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion

        public SupplierViewModel()
        {
            InitCommands();
            _ = LoadSuppliersAsync();
        }

        private void InitCommands()
        {
            // Lọc & Phân trang
            ClearFilterCommand = new RelayCommand<object>(p =>
            {
                SearchKeyword = string.Empty;
                SelectedSearchType = null;
                SelectedSearchStatus = null;
            });

            FirstPageCommand = new RelayCommand<object>(p => { _currentPage = 1; ApplyFilterAndPagination(); }, p => _currentPage > 1);
            PrevPageCommand = new RelayCommand<object>(p => { _currentPage--; ApplyFilterAndPagination(); }, p => _currentPage > 1);
            NextPageCommand = new RelayCommand<object>(p => { _currentPage++; ApplyFilterAndPagination(); }, p => _currentPage < _totalPages);
            LastPageCommand = new RelayCommand<object>(p => { _currentPage = _totalPages; ApplyFilterAndPagination(); }, p => _currentPage < _totalPages);
            GoToPageCommand = new RelayCommand<int>(page => { _currentPage = page; ApplyFilterAndPagination(); });

            // Popup Mở/Đóng
            OpenAddPopupCommand = new RelayCommand<object>(p =>
            {
                PopupTitle = "THÊM NHÀ CUNG CẤP MỚI";
                _isAddMode = true;
                EditingSupplier = new SupplierDTO();
                IsPopupVisible = true;
                PopUpIcon = PackIconKind.TruckAdd;
                IsAddMode = true;
            });

            OpenEditPopupCommand = new RelayCommand<SupplierDTO>(supplier =>
            {
                if (supplier == null) return;
                PopupTitle = "CẬP NHẬT NHÀ CUNG CẤP";
                _isAddMode = false;

                EditingSupplier = new SupplierDTO
                {
                    MaNhaCungCap = supplier.MaNhaCungCap,
                    TenNhaCungCap = supplier.TenNhaCungCap,
                    SoDienThoai = supplier.SoDienThoai,
                    Email = supplier.Email,
                    MaSoThue = supplier.MaSoThue,
                    SoTaiKhoan = supplier.SoTaiKhoan,
                    TenNganHang = supplier.TenNganHang,
                    DiaChi = supplier.DiaChi,
                    NguoiDaiDien = supplier.NguoiDaiDien,
                    ConHoatDong = supplier.ConHoatDong
                };
                IsPopupVisible = true;
                IsAddMode = false;

            });

            ClosePopupCommand = new RelayCommand<object>(p => IsPopupVisible = false);
            RefreshCommand = new RelayCommand<object>(async p => await LoadSuppliersAsync());

            // Nghiệp vụ
            SaveSupplierCommand = new RelayCommand<object>(async p => await SaveSupplierAsync());
            DeleteSupplierCommand = new RelayCommand<SupplierDTO>(async supplier => await DeleteSupplierAsync(supplier));


            // Khởi tạo Commands cho Phân trang
            FirstPageCommand = new RelayCommand<object>(p => { CurrentPage = 1; ApplyFilterAndPagination(); });

            PrevPageCommand = new RelayCommand<object>(p => {
                if (CurrentPage > 1) { CurrentPage--; ApplyFilterAndPagination(); }
            });

            NextPageCommand = new RelayCommand<object>(p => {
                if (CurrentPage < TotalPages) { CurrentPage++; ApplyFilterAndPagination(); }
            });

            LastPageCommand = new RelayCommand<object>(p => { CurrentPage = TotalPages; ApplyFilterAndPagination(); });

            GoToPageCommand = new RelayCommand<int>(page => { CurrentPage = page; ApplyFilterAndPagination(); });
        }

        private void ApplyFilterAndPagination()
        {
            var filtered = _allSuppliers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                string keyword = SearchKeyword.ToLower();
                switch (SelectedSearchType)
                {
                    case "Tìm tên nhà cung cấp":
                        filtered = filtered.Where(x => x.TenNhaCungCap?.ToLower().Contains(keyword) == true);
                        break;
                    case "Tìm số điện thoại":
                        filtered = filtered.Where(x => x.SoDienThoai?.Contains(keyword) == true);
                        break;
                    case "Tìm người đại diện":
                        filtered = filtered.Where(x => x.NguoiDaiDien?.ToLower().Contains(keyword) == true);
                        break;
                    default:
                        break;
                }
            }

            switch (SelectedSearchStatus)
            {
                case "Đang giao dịch":
                    filtered = filtered.Where(x => x.ConHoatDong == true);
                    break;
                case "Ngưng giao dịch":
                    filtered = filtered.Where(x => x.ConHoatDong == false);
                    break;
                default:
                    break;
            }

            int stt = 1;
            foreach (var b in filtered)
            {
                b.STT = stt++;
            }

            var resultList = filtered.ToList();
            int totalRecords = resultList.Count;
            TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            if (TotalPages == 0) TotalPages = 1;

            if (CurrentPage > TotalPages) CurrentPage = TotalPages;
            if (CurrentPage < 1) CurrentPage = 1;

            PageNumbers = new ObservableCollection<int>();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageNumbers.Add(i);
            }

            var pagedData = resultList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            int index = (CurrentPage - 1) * PageSize + 1;
            foreach (var item in pagedData)
            {
                item.STT = index++;
            }

            PagedSuppliers = new ObservableCollection<SupplierDTO>(pagedData);

        }

        // --- HÀM TẢI DỮ LIỆU TỪ SERVER ---
        private async Task LoadSuppliersAsync()
        {
            IsLoading = true;
            try
            {
                var response = await ApiClient.GetAsync<List<SupplierDTO>>("api/NhaCungCap");
                if (response != null)
                {
                    _allSuppliers = response;
                }
                ApplyFilterAndPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // --- HÀM XỬ LÝ LƯU THÔNG TIN ---
        private async Task SaveSupplierAsync()
        {
            if (string.IsNullOrWhiteSpace(EditingSupplier.TenNhaCungCap) || string.IsNullOrWhiteSpace(EditingSupplier.SoDienThoai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên nhà cung cấp và Số điện thoại!", "Cảnh báo nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_isAddMode)
                {
                    // POST tới Server
                    bool isSuccess = await ApiClient.PostAsync<SupplierDTO, bool>("api/NhaCungCap", EditingSupplier);
                    if (isSuccess) MessageBox.Show("Thêm mới nhà cung cấp thành công!", "Thông báo");
                }
                else
                {
                    // PUT cập nhật
                    bool isSuccess = await ApiClient.PutAsync<SupplierDTO, bool>($"api/NhaCungCap/{EditingSupplier.MaNhaCungCap}", EditingSupplier);
                    if (isSuccess) MessageBox.Show("Cập nhật thông tin nhà cung cấp thành công!", "Thông báo");
                }

                IsPopupVisible = false;
                await LoadSuppliersAsync(); // Refresh DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- HÀM XỬ LÝ XÓA ---
        private async Task DeleteSupplierAsync(SupplierDTO supplier)
        {
            if (supplier == null) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{supplier.TenNhaCungCap}' khỏi hệ thống không?",
                                         "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // DELETE trên Server
                    bool isSuccess = await ApiClient.DeleteAsync($"api/NhaCungCap/{supplier.MaNhaCungCap}");
                    if (isSuccess)
                    {
                        MessageBox.Show("Đã xóa nhà cung cấp thành công!", "Thông báo");
                        await LoadSuppliersAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xóa thất bại: {ex.Message}", "Lỗi xóa dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
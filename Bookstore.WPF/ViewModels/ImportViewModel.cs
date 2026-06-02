using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using Bookstore.WPF.Utils; // Để lấy AppState.CurrentUser
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ImportViewModel : BaseListViewModel
    {
        private List<ImportOrderResponse> _danhSachGoc = new();

        private ObservableCollection<ImportOrderResponse> _importOrders = new();
        public ObservableCollection<ImportOrderResponse> ImportOrders
        {
            get => _importOrders;
            set { _importOrders = value; OnPropertyChanged(); }
        }

        public ObservableCollection<NhaCungCapDto> SupplierList { get; set; } = new();

        private NhaCungCapDto _selectedSupplier;
        public NhaCungCapDto SelectedSupplier
        {
            get => _selectedSupplier;
            set { _selectedSupplier = value; OnPropertyChanged(); TrangHienTai = 1; ApplyFilterAndPagination(); }
        }

        private DateTime? _filterFromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        public DateTime? FilterFromDate
        {
            get => _filterFromDate;
            set { _filterFromDate = value; OnPropertyChanged(); TrangHienTai = 1; ApplyFilterAndPagination(); }
        }

        private DateTime? _filterToDate = DateTime.Today;
        public DateTime? FilterToDate
        {
            get => _filterToDate;
            set { _filterToDate = value; OnPropertyChanged(); TrangHienTai = 1; ApplyFilterAndPagination(); }
        }

        private bool _isDetailPopupOpen;
        public bool IsDetailPopupOpen { get => _isDetailPopupOpen; set { _isDetailPopupOpen = value; OnPropertyChanged(); } }

        private ImportOrderDetailResponse _detailImportOrder;
        public ImportOrderDetailResponse DetailImportOrder { get => _detailImportOrder; set { _detailImportOrder = value; OnPropertyChanged(); } }

        private bool _isAddPopupOpen;
        public bool IsAddPopupOpen { get => _isAddPopupOpen; set { _isAddPopupOpen = value; OnPropertyChanged(); } }

        private ImportOrderRequestUI _newImportOrder;
        public ImportOrderRequestUI NewImportOrder { get => _newImportOrder; set { _newImportOrder = value; OnPropertyChanged(); } }

        private string _searchBookKeyword = "";
        public string SearchBookKeyword { get => _searchBookKeyword; set { _searchBookKeyword = value; OnPropertyChanged(); } }

        private ObservableCollection<BookSearchResponse> _searchBookResults;
        public ObservableCollection<BookSearchResponse> SearchBookResults { get => _searchBookResults; set { _searchBookResults = value; OnPropertyChanged(); } }

        private BookSearchResponse _selectedSearchBook;
        public BookSearchResponse SelectedSearchBook { get => _selectedSearchBook; set { _selectedSearchBook = value; OnPropertyChanged(); } }

        public ICommand ClearFilterCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailCommand { get; }
        public ICommand DeleteImportOrderCommand { get; }
        public ICommand OpenAddImportCommand { get; }
        public ICommand ClosePopupCommand { get; }

        // Lệnh trong Popup Add
        public ICommand SearchBookCommand { get; }
        public ICommand AddBookToImportCommand { get; }
        public ICommand RemoveImportDetailCommand { get; }
        public ICommand SaveImportOrderCommand { get; }
        public ICommand UpdateGhiChuCommand { get; }


        public ImportViewModel()
        {
            // Commands Main
            ClearFilterCommand = new RelayCommand<object>(_ =>
            {
                SearchKeyword = "";
                FilterFromDate = null;
                FilterToDate = null;
                SelectedSupplier = null;
            });
            RefreshCommand = new RelayCommand<object>(async (p) => await LoadDataAsync());
            ViewDetailCommand = new RelayCommand<ImportOrderResponse>(async (p) => await LoadDetailAsync(p));
            DeleteImportOrderCommand = new RelayCommand<ImportOrderResponse>(ExecuteDelete);
            OpenAddImportCommand = new RelayCommand<object>((p) => PrepareAddPopup());
            ClosePopupCommand = new RelayCommand<object>((p) =>
            {
                IsDetailPopupOpen = false;
                IsAddPopupOpen = false;
            });

            // Commands Popup
            SearchBookCommand = new RelayCommand<object>(async (p) => await ExecuteSearchBook());
            AddBookToImportCommand = new RelayCommand<object>((p) => ExecuteAddBook());
            RemoveImportDetailCommand = new RelayCommand<ImportOrderItemUI>(ExecuteRemoveBook);
            SaveImportOrderCommand = new RelayCommand<object>(async (p) => await ExecuteSaveImportOrder());
            UpdateGhiChuCommand = new RelayCommand<object>(async (p) => await ExecuteUpdateGhiChu());


            _ = LoadSuppliersAsync();
            _ = LoadDataAsync();
        }


        private async Task LoadSuppliersAsync()
        {
            var data = await ApiClient.GetAsync<List<NhaCungCapDto>>("api/NhaCungCap");
            if (data != null)
            {
                SupplierList.Clear();
                foreach (var item in data) SupplierList.Add(item);
            }
        }

        private async Task LoadDataAsync()
        {
            var data = await ApiClient.GetAsync<List<ImportOrderResponse>>("api/PhieuNhap");
            if (data != null)
            {
                _danhSachGoc = data;
                ApplyFilterAndPagination();
            }
        }

        private async Task LoadDetailAsync(ImportOrderResponse order)
        {
            if (order == null) return;
            try
            {
                var data = await ApiClient.GetAsync<ImportOrderDetailResponse>($"api/PhieuNhap/{order.MaPhieuNhap}");
                if (data != null)
                {
                    DetailImportOrder = data;
                    IsDetailPopupOpen = true;
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối hoặc không lấy được dữ liệu chi tiết từ máy chủ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void ApplyFilterAndPagination()
        {
            if (_danhSachGoc == null) return;
            var filtered = _danhSachGoc.AsEnumerable();

            var kw = SearchKeyword?.ToLower().Trim() ?? "";
            if (!string.IsNullOrEmpty(kw))
            {
                filtered = filtered.Where(x =>
                    $"PN{x.NgayNhap:ddMMyy}{x.MaPhieuNhap:D3}".ToLower().Contains(kw) ||
                    x.TenNhaCungCap.ToLower().Contains(kw) ||
                    x.TenNguoiTao.ToLower().Contains(kw));
            }

            if (SelectedSupplier != null)
                filtered = filtered.Where(x => x.MaNhaCungCap == SelectedSupplier.MaNhaCungCap);

            if (FilterFromDate.HasValue)
                filtered = filtered.Where(x => x.NgayNhap.Date >= FilterFromDate.Value.Date);

            if (FilterToDate.HasValue)
                filtered = filtered.Where(x => x.NgayNhap.Date <= FilterToDate.Value.Date);

            var resultList = filtered.ToList();
            TongBanGhi = resultList.Count;
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;
            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;

            ImportOrders = new ObservableCollection<ImportOrderResponse>(
                resultList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize));
        }

        private void PrepareAddPopup()
        {
            NewImportOrder = new ImportOrderRequestUI();
            SearchBookKeyword = "";
            SearchBookResults = null;
            IsAddPopupOpen = true;
        }

        private async void ExecuteDelete(ImportOrderResponse order)
        {
            if (order == null) return;
            if (MessageBox.Show($"Bạn có chắc muốn xóa phiếu nhập này? Tồn kho sẽ bị trừ đi tương ứng.",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool success = await ApiClient.DeleteAsync($"api/PhieuNhap/{order.MaPhieuNhap}");
                if (success)
                {
                    MessageBox.Show("Xóa phiếu nhập thành công!");
                    await LoadDataAsync();
                }
            }
        }

        private async Task ExecuteSearchBook()
        {
            if (string.IsNullOrWhiteSpace(SearchBookKeyword)) return;
            var data = await ApiClient.GetAsync<List<BookSearchResponse>>($"api/PhieuNhap/search-books?keyword={SearchBookKeyword}");
            SearchBookResults = data != null ? new ObservableCollection<BookSearchResponse>(data) : new();
        }

        private async Task ExecuteUpdateGhiChu()
        {
            if (DetailImportOrder == null) return;

            var payload = new { GhiChu = DetailImportOrder.GhiChu };

            bool success = await ApiClient.PutAndCheckSuccessAsync($"api/PhieuNhap/{DetailImportOrder.MaPhieuNhap}/ghichu", payload);

            if (success)
            {
                MessageBox.Show("Cập nhật ghi chú phiếu nhập thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Cập nhật ghi chú thất bại! Vui lòng kiểm tra lại kết nối.", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAddBook()
        {
            if (SelectedSearchBook == null)
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách từ kết quả tìm kiếm trước khi thêm!", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewImportOrder.ChiTiet.Any(x => x.ISBN == SelectedSearchBook.ISBN))
            {
                MessageBox.Show("Sách này đã có trong danh sách nhập! Vui lòng chỉnh sửa số lượng ở bảng bên dưới.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            NewImportOrder.ChiTiet.Add(new ImportOrderItemUI
            {
                STT = NewImportOrder.ChiTiet.Count + 1,
                ISBN = SelectedSearchBook.ISBN,
                TenSach = SelectedSearchBook.TenSach,
                TacGia = SelectedSearchBook.TacGia,
                SoLuong = 1, 
                DonGia = 0,
                TargetValueChanged = () => NewImportOrder.OnDetailChanged()
            });
            NewImportOrder.OnDetailChanged();
        }

        private void ExecuteRemoveBook(ImportOrderItemUI item)
        {
            if (item != null)
            {
                NewImportOrder.ChiTiet.Remove(item);
                for (int i = 0; i < NewImportOrder.ChiTiet.Count; i++) NewImportOrder.ChiTiet[i].STT = i + 1;
                NewImportOrder.OnDetailChanged();
            }
        }

        private async Task ExecuteSaveImportOrder()
        {
            if (NewImportOrder.SelectedSupplier == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Lỗi dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NewImportOrder.ChiTiet.Count == 0)
            {
                MessageBox.Show("Phiếu nhập phải có ít nhất 1 cuốn sách!", "Lỗi dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var item in NewImportOrder.ChiTiet)
            {
                if (item.SoLuong <= 0)
                {
                    MessageBox.Show($"Sách '{item.TenSach}' có số lượng không hợp lệ. Phải lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (item.DonGia < 0)
                {
                    MessageBox.Show($"Sách '{item.TenSach}' có đơn giá không hợp lệ. Không thể là số âm!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var request = new ImportOrderRequest
            {
                MaNhaCungCap = NewImportOrder.SelectedSupplier.MaNhaCungCap,
                NguoiTao = AppState.CurrentUser?.Username ?? "admin",
                GhiChu = NewImportOrder.GhiChu,
                ChiTiet = NewImportOrder.ChiTiet.Select(c => new ImportOrderItemRequest
                {
                    ISBN = c.ISBN,
                    SoLuong = c.SoLuong,
                    DonGia = c.DonGia
                }).ToList()
            };

            bool success = await ApiClient.PostAndCheckSuccessAsync("api/PhieuNhap", request);
            if (success)
            {
                MessageBox.Show("Lưu phiếu nhập và cập nhật tồn kho thành công!");
                IsAddPopupOpen = false;
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show("Lưu thất bại!");
            }
        }
    }

    public class NhaCungCapDto
    {
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; } = "";
    }

    public class ImportOrderRequestUI : BaseViewModel
    {
        public NhaCungCapDto SelectedSupplier { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        public string GhiChu { get; set; } = "";

        public string TenNguoiTao => AppState.CurrentUser?.Name ?? "Quản trị viên";
        public decimal TongTien => ChiTiet.Sum(c => c.ThanhTien);

        public ObservableCollection<ImportOrderItemUI> ChiTiet { get; set; } = new();

        public void OnDetailChanged()
        {
            OnPropertyChanged(nameof(TongTien));
        }
    }

    public class ImportOrderItemUI : BaseViewModel
    {
        public int STT { get; set; }
        public string ISBN { get; set; } = "";
        public string TenSach { get; set; } = "";
        public string TacGia { get; set; } = "";
        public string GhiChu { get; set; } = "";

        public Action TargetValueChanged { get; set; }

        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThanhTien)); TargetValueChanged?.Invoke(); } }

        private decimal _donGia;
        public decimal DonGia { get => _donGia; set { _donGia = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThanhTien)); TargetValueChanged?.Invoke(); } }

        public decimal ThanhTien => SoLuong * DonGia;
    }
}
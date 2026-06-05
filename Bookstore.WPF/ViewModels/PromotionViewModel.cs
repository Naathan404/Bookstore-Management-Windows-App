using Bookstore.Share.DTO;
using Bookstore.Share.DTO.Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.Share.DTOs;
using Bookstore.Share.Enums;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using Bookstore.WPF.ViewModels.Base;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Export.HtmlExport;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace Bookstore.WPF.ViewModels
{
    public class PromotionViewModel : BaseListViewModel
    {
        public override bool CanEdit => true;
        #region Dữ liệu và tìm kiếm
        private List<PromotionDTO> _allPromotions = new List<PromotionDTO>();
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

        private ObservableCollection<PromotionDTO> _pagedPromotions = new ObservableCollection<PromotionDTO>();
        public ObservableCollection<PromotionDTO> PagedPromotions
        {
            get => _pagedPromotions;
            set { _pagedPromotions = value; OnPropertyChanged(); }
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
        #endregion

        #region Thông tin popup
        public PromotionEditPopupViewModel PromotionEditPopupVM { get; set; } = new PromotionEditPopupViewModel();

        //private bool _isPopupVisible;
        //public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        //private string _popupTitle = string.Empty;
        //public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        //private string _saveButtonText = string.Empty;
        //public string SaveButtonText { get => _saveButtonText; set { _saveButtonText = value; OnPropertyChanged(); } }

        //private bool _isAddMode;

        //private bool _isCoreEditingAllowed;
        //public bool IsCoreEditingAllowed
        //{
        //    get => _isCoreEditingAllowed;
        //    set { _isCoreEditingAllowed = value; OnPropertyChanged(); }
        //}
        public ObservableCollection<PromotionTypeResponse> AvailablePromotionTypes { get; set; }
            = new ObservableCollection<PromotionTypeResponse>();
        //public PromotionType SelectedMaLoaiUuDai
        //{
        //    get => EditingPromotion?.MaLoaiUuDai ?? default;
        //    set
        //    {
        //        if (EditingPromotion != null)
        //        {
        //            EditingPromotion.MaLoaiUuDai = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        private PromotionDTO _editingPromotion = new PromotionDTO();
        public PromotionDTO EditingPromotion { get => _editingPromotion; set { _editingPromotion = value; OnPropertyChanged(); } }
        #endregion

        #region Thông tin chi tiết ưu đãi
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
        #endregion

        public ICommand ToggleStatusCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand DeletePromotionCommand { get; set; }
        public ICommand OpenAddPopupCommand { get; set; }
        public ICommand OpenEditPopupCommand { get; set; }
        public ICommand SavePromotionCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }

        public async Task LoadMasterData()
        {
            await InitDropdownData();
            await LoadDataAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadMasterData();
        }

        public PromotionViewModel()
        {
            InitCommands();
            _ =  InitializeViewModelAsync();
            //_ = InitDropdownData();
            //_ = LoadDataAsync();
        }
        private void InitCommands()
        {
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

            ClearFilterCommand = new RelayCommand<object>(p => { 
                SearchTenKM = ""; 
                SelectedTieuChiTimKiem = "Tất cả";
                SelectedLoaiUuDaiFilter = "Tất cả loại ưu đãi";
                SelectedTrangThai = "Tất cả trạng thái";
                SearchTuNgay = null;  
                SearchDenNgay = null;
            });

            RefreshCommand = new RelayCommand<object>(async p => await LoadDataAsync());

            OpenAddPopupCommand = new RelayCommand<object>(p =>
            {
                var newPromo = new PromotionDTO
                {
                    NgayTao = DateTime.Now,
                    NguoiTao = AppState.CurrentUser.Username,
                    NgayBatDau = DateTime.Today,
                    NgayKetThuc = DateTime.Today.AddDays(30),
                    SoLuongToiDa = 50,
                    MaLoaiUuDai = PromotionType.HoaDonGiam,
                    MaLoaiKhachHang = 0,
                    // Khởi tạo các list rỗng cho an toàn
                    DanhSachSachDieuKien = new List<SachDieuKienDTO>(),
                    DanhSachSachTang = new List<SachTangDTO>()
                };

                // GỌI SANG POPUP MỚI
                PromotionEditPopupVM.ListLoaiKhachHang = ListLoaiKhachHang;
                PromotionEditPopupVM.ListSach = ListSach;
                PromotionEditPopupVM.AvailablePromotionTypes = AvailablePromotionTypes;

                PromotionEditPopupVM.ShowPopup(newPromo, isEdit: false, async (promoToSave) =>
                {
                    await SavePromotionAsync(promoToSave, isAddMode: true);
                });
            });

            OpenEditPopupCommand = new RelayCommand<PromotionDTO>(promo =>
            {
                if (promo == null) return;

                // Phải Deep Copy để tránh lỡ sửa mà bấm Hủy thì List ngoài giao diện bị dính theo
                var editingPromo = new PromotionDTO
                {
                    MaUuDai = promo.MaUuDai,
                    NgayTao = promo.NgayTao,
                    Code = promo.Code,
                    TenChuongTrinh = promo.TenChuongTrinh,
                    MoTa = promo.MoTa,
                    NgayBatDau = promo.NgayBatDau,
                    NgayKetThuc = promo.NgayKetThuc,
                    SoLuongToiDa = promo.SoLuongToiDa,
                    SoLuongDaDung = promo.SoLuongDaDung,
                    MaLoaiKhachHang = promo.MaLoaiKhachHang ?? 0,
                    MaLoaiUuDai = promo.MaLoaiUuDai,
                    SoTienToiThieu = promo.SoTienToiThieu,
                    SoTienToiDa = promo.SoTienToiDa,
                    SoTienGiam = promo.SoTienGiam,
                    TiLeGiam = promo.TiLeGiam,
                    GiamToiDa = promo.GiamToiDa,

                    // Xài bộ danh sách mới 1:N
                    DanhSachSachDieuKien = promo.DanhSachSachDieuKien?.Select(d => new SachDieuKienDTO
                    {
                        ISBN = d.ISBN,
                        TenSach = d.TenSach,  
                        SoLuongMua = d.SoLuongMua
                    }).ToList() ?? new List<SachDieuKienDTO>(),

                    DanhSachSachTang = promo.DanhSachSachTang?.Select(t => new SachTangDTO
                    {
                        ISBN = t.ISBN,
                        TenSach = t.TenSach,
                        SoLuongTang = t.SoLuongTang
                    }).ToList() ?? new List<SachTangDTO>(),
                    CoTheSuDung = promo.CoTheSuDung
                };

                PromotionEditPopupVM.ListLoaiKhachHang = ListLoaiKhachHang;
                PromotionEditPopupVM.ListSach = ListSach;
                PromotionEditPopupVM.AvailablePromotionTypes = AvailablePromotionTypes;

                PromotionEditPopupVM.ShowPopup(editingPromo, isEdit: true, async (promoToSave) =>
                {
                    await SavePromotionAsync(promoToSave, isAddMode: false);
                });
            });

            //ClosePopupCommand = new RelayCommand<object>(p => IsPopupVisible = false);

            //SavePromotionCommand = new RelayCommand<object>(async p => await SavePromotionAsync());
        }
        private async Task InitDropdownData()
        {
            ListLoaiKhachHang.Clear();
            ListLoaiKhachHang.Add(new CustomerTierResponse
            {
                MaLoaiKhachHang = 0,
                TenLoaiKhachHang = "Tất cả khách hàng"
            });
            var customerTypes = await ApiClient.GetAsync<List<CustomerTierResponse>>("api/LoaiKhachHang");
            if (customerTypes != null)
            {
                foreach (var c in customerTypes)
                {
                    ListLoaiKhachHang.Add(new CustomerTierResponse { 
                        MaLoaiKhachHang = c.MaLoaiKhachHang, 
                        TenLoaiKhachHang = c.TenLoaiKhachHang 
                    });
                }
            }

            ListSach.Clear();
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

            ListLoaiUuDai.Clear();
            AvailablePromotionTypes.Clear();
            ListLoaiUuDai.Add("Tất cả loại ưu đãi");
            SelectedLoaiUuDaiFilter = "Tất cả loại ưu đãi";
            var promotionTypes = await ApiClient.GetAsync<List<PromotionTypeResponse>>("api/LoaiUuDai");
            if (promotionTypes != null)
            {
                foreach (var pt in promotionTypes)
                {
                    ListLoaiUuDai.Add(pt.TenLoaiUuDai);
                    AvailablePromotionTypes.Add(pt);
                }
            }

            ListTrangThai.Clear();
            ListTrangThai.Add("Tất cả trạng thái");
            ListTrangThai.Add("Đang áp dụng");
            ListTrangThai.Add("Chưa áp dụng");
            ListTrangThai.Add("Tạm dừng");
            ListTrangThai.Add("Hết hạn");
            SelectedTrangThai = "Tất cả trạng thái";
            SelectedLoaiUuDaiFilter = "Tất cả loại ưu đãi";
        }
        private async Task LoadDataAsync()
        {
            try
            {
                var response = await ApiClient.GetAsync<List<PromotionDTO>>("api/UuDai");

                if (response != null)
                {
                    foreach (var item in response)
                    {
                        if (item.MaLoaiKhachHang == null || item.MaLoaiKhachHang == 0)
                        {
                            item.LoaiKhachHangApDung = "Tất cả khách hàng";
                        }
                        else
                        {
                            var loaiKH = ListLoaiKhachHang.FirstOrDefault(k => k.MaLoaiKhachHang == item.MaLoaiKhachHang);
                            if (loaiKH != null)
                            {
                                item.LoaiKhachHangApDung = loaiKH.TenLoaiKhachHang;
                            }
                        }
                    }
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
        private async Task SavePromotionAsync(PromotionDTO dto, bool isAddMode)
        {
            if (string.IsNullOrWhiteSpace(dto.Code) || string.IsNullOrWhiteSpace(dto.TenChuongTrinh))
            {
                MessageBox.Show("Mã Code và Tên chương trình ưu đãi không được bỏ trống!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dto.NgayBatDau.Date > dto.NgayKetThuc.Date)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!", "Lỗi logic thời gian", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dto.SoLuongToiDa <= 0)
            {
                MessageBox.Show("Số lượng phát hành tối đa phải là số dương (> 0)!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PromotionType loai = dto.MaLoaiUuDai;

            // VALIDATE TIỀN TỆ
            if (loai == PromotionType.HoaDonGiam || loai == PromotionType.HoaDonQua)
            {
                if (dto.SoTienToiThieu < 0 || dto.SoTienToiDa < 0)
                {
                    MessageBox.Show("Số tiền hóa đơn yêu cầu không được là số âm!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (dto.SoTienToiDa > 0 && dto.SoTienToiThieu > dto.SoTienToiDa)
                {
                    MessageBox.Show("Số tiền tối đa không được nhỏ hơn số tiền tối thiểu!", "Lỗi logic", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // VALIDATE ĐIỀU KIỆN SÁCH (1:N)
            if (loai == PromotionType.SachGiam || loai == PromotionType.SachQua)
            {
                if (dto.DanhSachSachDieuKien == null || !dto.DanhSachSachDieuKien.Any())
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một Sách bắt buộc mua vào danh sách điều kiện!", "Thiếu dữ kiện", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // VALIDATE QUÀ TẶNG (1:N)
            if (loai == PromotionType.HoaDonQua || loai == PromotionType.SachQua)
            {
                if (dto.DanhSachSachTang == null || !dto.DanhSachSachTang.Any())
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một Sách vào danh sách quà tặng!", "Thiếu dữ kiện", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Dọn dẹp dữ liệu rác về giá
                dto.SoTienGiam = 0; dto.TiLeGiam = 0; dto.GiamToiDa = 0;
            }

            // VALIDATE GIẢM GIÁ
            if (loai == PromotionType.HoaDonGiam || loai == PromotionType.SachGiam)
            {
                if (dto.SoTienGiam <= 0 && dto.TiLeGiam <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Số tiền giảm HOẶC Tỉ lệ phần trăm giảm!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (dto.TiLeGiam > 0 && dto.TiLeGiam > 100)
                {
                    MessageBox.Show("Tỉ lệ phần trăm giảm giá phải nằm trong khoảng từ 1% đến 100%!", "Lỗi tỉ lệ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (dto.TiLeGiam > 0 && dto.GiamToiDa <= 0)
                {
                    MessageBox.Show("Nếu chọn Giảm theo % thì bắt buộc phải nhập Mức giảm tối đa (> 0)!", "Lỗi số liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Dọn rác quà tặng
                dto.DanhSachSachTang.Clear();
            }

            if (dto.MaLoaiKhachHang == 0) dto.MaLoaiKhachHang = null;

            try
            {
                bool success;
                if (isAddMode) success = await ApiClient.PostAsync<PromotionDTO, bool>("api/UuDai", dto);
                else success = await ApiClient.PutAsync<PromotionDTO, bool>($"api/UuDai/{dto.MaUuDai}", dto);

                if (success)
                {
                    PromotionEditPopupVM.IsPopupVisible = false;

                    if (isAddMode) MessageBox.Show("Thêm phiếu ưu đãi thành công", "Thông báo", MessageBoxButton.OK);
                    else MessageBox.Show("Cập nhật phiếu ưu đãi thành công", "Thông báo", MessageBoxButton.OK);

                    await LoadDataAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi xử lý"); }
        }
    }
}
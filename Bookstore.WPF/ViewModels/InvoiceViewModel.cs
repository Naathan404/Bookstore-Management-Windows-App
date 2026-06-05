using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class InvoiceViewModel : BaseListViewModel
    {
        public InvoiceDetailPopupViewModel InvoiceDetailPopupVM { get; set; } = new InvoiceDetailPopupViewModel();

        #region DATA
        private ObservableCollection<InvoiceResponse> _danhSachHoaDonGoc = new();

        private ObservableCollection<InvoiceResponse> _danhSachHoaDon = new();
        public ObservableCollection<InvoiceResponse> DanhSachHoaDon
        {
            get => _danhSachHoaDon;
            set { _danhSachHoaDon = value; OnPropertyChanged(); }
        }
        #endregion

        #region BỘ LỌC TÙY CHỈNH (Ngoài SearchKeyword đã có ở Base)

        private DateTime? _fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        private DateTime? _toDate = DateTime.Today;
        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        public ObservableCollection<string> ListTrangThaiNo { get; set; } = new ObservableCollection<string>() { "Tất cả trạng thái", "Đã thanh toán", "Còn nợ" };

        private string _selectedTrangThaiNo = "Tất cả trạng thái";
        public string SelectedTrangThaiNo
        {
            get => _selectedTrangThaiNo;
            set
            {
                _selectedTrangThaiNo = value;
                OnPropertyChanged(nameof(SelectedTrangThaiNo));
                ApplyFilterAndPagination();
            }
        }

        #endregion

        #region COMMANDS
        public ICommand XoaLocCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand XemChiTietCommand { get; }
        #endregion


        public InvoiceViewModel()
        {
            // Thiết lập số dòng trên trang mặc định (kế thừa từ BaseListViewModel)
            PageSize = 10;
            SelectedTrangThaiNo = ListTrangThaiNo[0];
            XoaLocCommand = new RelayCommand<object>(ExecuteXoaLoc);

            // Gán logic tải lại API vào nút Refresh
            RefreshCommand = new RelayCommand<object>(async (p) =>
            {
                ExecuteXoaLoc(null);
                await LoadDataAsync();
            });

            XemChiTietCommand = new RelayCommand<InvoiceResponse>(ExecuteXemChiTiet);

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<InvoiceResponse>>("api/HoaDon");
                if (result != null)
                {
                    _danhSachHoaDonGoc = new ObservableCollection<InvoiceResponse>(result);
                }

                // Thay vì gọi ApplyFilterAsync cũ, ta gọi thẳng hàm chuẩn của class Base
                ApplyFilterAndPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lịch sử hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======================================================================
        // HÀM LÕI: GHI ĐÈ LẠI HÀM CỦA BASE CLASS ĐỂ THỰC HIỆN LỌC RIÊNG CHO HÓA ĐƠN
        // ======================================================================
        protected override void ApplyFilterAndPagination()
        {
            if (_danhSachHoaDonGoc == null) return;

            var filtered = _danhSachHoaDonGoc.AsEnumerable();

            // Dùng SearchKeyword thay cho SearchText cũ
            var text = SearchKeyword?.ToLower().Trim() ?? "";

            if (!string.IsNullOrEmpty(text))
            {
                //filtered = filtered.Where(hd =>
                //    (hd.TenKhachHang != null && hd.TenKhachHang.ToLower().Contains(text)) ||
                //    (hd.TenNguoiTao != null && hd.TenNguoiTao.ToLower().Contains(text)));
                filtered = filtered.Where(hd =>
                    (hd.TenKhachHang != null && hd.TenKhachHang.ToLower().Contains(text)));
            }

            if (FromDate.HasValue)
            {
                filtered = filtered.Where(hd => hd.NgayTao.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                filtered = filtered.Where(hd => hd.NgayTao.Date <= ToDate.Value.Date);
            }

            if(!string.IsNullOrEmpty(SelectedTrangThaiNo))
            {
                if(SelectedTrangThaiNo == ListTrangThaiNo[1])
                {
                    filtered = filtered.Where(hd => hd.ConLai <= 0);
                }
                else if (SelectedTrangThaiNo == ListTrangThaiNo[2])
                {
                    filtered = filtered.Where(hd => hd.ConLai > 0);
                }
            }

            // Chốt danh sách sau khi lọc
            var resultList = filtered.ToList();
            TongBanGhi = resultList.Count;

            // Tính toán phân trang dựa vào PageSize của class mẹ
            TongSoTrang = Math.Max(1, (int)Math.Ceiling(TongBanGhi / (double)PageSize));
            if (TrangHienTai > TongSoTrang) TrangHienTai = 1;

            // Cắt lấy dữ liệu cho trang hiện tại
            DanhSachHoaDon = new ObservableCollection<InvoiceResponse>(
                resultList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize));
        }

        private void ExecuteXoaLoc(object obj)
        {
            // Reset Date (nhưng không gọi Apply ngay lập tức để tránh tính toán nhiều lần)
            _fromDate = null;
            _toDate = null;
            SelectedTrangThaiNo = ListTrangThaiNo[0];
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));

            // Khi gán lại SearchKeyword, hàm set trong BaseListViewModel sẽ tự động gọi ApplyFilterAndPagination()
            SearchKeyword = string.Empty;

            // Phòng hờ trường hợp SearchKeyword vốn đã rỗng thì phải ép nó load lại
            if (string.IsNullOrEmpty(SearchKeyword))
            {
                ApplyFilterAndPagination();
            }
        }

        private async void ExecuteXemChiTiet(InvoiceResponse hd)
        {
            if (hd == null) return;

            try
            {
                // 1. Gọi API lấy Chi tiết hóa đơn (CT_HoaDon)
                var details = await ApiClient.GetAsync<List<InvoiceDetailResponse>>($"api/CT_HoaDon/HoaDon/{hd.MaHoaDon}");

                // Đảm bảo không bị văng lỗi nếu API trả về null (dù hiếm khi xảy ra)
                if (details == null) details = new List<InvoiceDetailResponse>();

                // 2. Gọi API lấy Ưu đãi đã áp dụng (HoaDon_UuDai)
                var promos = await ApiClient.GetAsync<List<InvoicePromoResponse>>($"api/HoaDon_UuDai/HoaDon/{hd.MaHoaDon}");

                if (promos == null) promos = new List<InvoicePromoResponse>();

                // 3. Bật Popup lên và truyền dữ liệu thật vào
                InvoiceDetailPopupVM.ShowPopup(hd, details, promos);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Lỗi tải chi tiết hóa đơn: {ex.Message}", "Lỗi kết nối", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
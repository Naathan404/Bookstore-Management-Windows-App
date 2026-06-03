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
    public class InvoiceViewModel : BaseListViewModel
    {
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

        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); ApplyFilterAndPagination(); }
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
                filtered = filtered.Where(hd =>
                    (hd.TenKhachHang != null && hd.TenKhachHang.ToLower().Contains(text)) ||
                    (hd.TenNguoiTao != null && hd.TenNguoiTao.ToLower().Contains(text)));
            }

            if (FromDate.HasValue)
            {
                filtered = filtered.Where(hd => hd.NgayTao.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                filtered = filtered.Where(hd => hd.NgayTao.Date <= ToDate.Value.Date);
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

        private void ExecuteXemChiTiet(InvoiceResponse hd)
        {
            if (hd == null) return;
            string maHdFormat = $"HD{hd.NgayTao:ddMMyy}{hd.MaHoaDon:D3}";
            MessageBox.Show($"Xem chi tiết hóa đơn: {maHdFormat}", "Thông báo");
        }
    }
}
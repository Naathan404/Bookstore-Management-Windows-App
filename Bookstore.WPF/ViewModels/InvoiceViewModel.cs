using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class InvoiceViewModel : BaseViewModel
    {
        #region PROPERTIES
        private ObservableCollection<InvoiceResponse> _danhSachHoaDonGoc = new();

        private ObservableCollection<InvoiceResponse> _danhSachHoaDon = new();
        public ObservableCollection<InvoiceResponse> DanhSachHoaDon
        {
            get => _danhSachHoaDon;
            set { _danhSachHoaDon = value; OnPropertyChanged(); }
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

        private int _soDongTrenTrang = 10;

        // --- Filters ---
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); _ = ApplyFilterAsync(); }
        }
        #endregion

        #region COMMANDS
        public ICommand XoaLocCommand { get; }
        public ICommand PhanTrangCommand { get; }
        public ICommand XemChiTietCommand { get; }
        #endregion

        public InvoiceViewModel()
        {
            XoaLocCommand = new RelayCommand<object>(ExecuteXoaLoc);
            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);
            XemChiTietCommand = new RelayCommand<InvoiceResponse>(ExecuteXemChiTiet);

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                // Khi có API thật, endpoint sẽ là api/HoaDon
                var result = await ApiClient.GetAsync<List<InvoiceResponse>>("api/HoaDon");
                if (result != null)
                {
                    _danhSachHoaDonGoc = new ObservableCollection<InvoiceResponse>(result);
                }
                _ = ApplyFilterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lịch sử hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ApplyFilterAsync()
        {
            if (_danhSachHoaDonGoc == null) return;

            var filtered = _danhSachHoaDonGoc.AsEnumerable();
            var text = SearchText.ToLower().Trim();

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

            var resultList = filtered.ToList();
            TongBanGhi = resultList.Count;

            TongSoTrang = Math.Max(1, (int)Math.Ceiling(TongBanGhi / (double)_soDongTrenTrang));
            if (TrangHienTai > TongSoTrang) TrangHienTai = 1;

            DanhSachHoaDon = new ObservableCollection<InvoiceResponse>(
                resultList.Skip((TrangHienTai - 1) * _soDongTrenTrang).Take(_soDongTrenTrang));

            await Task.CompletedTask;
        }

        private void ExecuteXoaLoc(object obj)
        {
            _searchText = "";
            _fromDate = null;
            _toDate = null;

            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));

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
            }
            _ = ApplyFilterAsync();
        }

        private void ExecuteXemChiTiet(InvoiceResponse hd)
        {
            if (hd == null) return;
            // Logic mở popup xem chi tiết các CT_HoaDon (Sẽ xử lý sau khi làm popup)
            string maHdFormat = $"HD{hd.NgayTao:ddMMyy}{hd.MaHoaDon:D3}";
            MessageBox.Show($"Xem chi tiết hóa đơn: {maHdFormat}", "Thông báo");
        }
    }
}
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ReceiptViewModel : BaseViewModel
    {
        #region COMPONENTS (Chứa bộ não Popup)
        // Đây là Component sẽ lo việc Mở popup Thêm/Sửa và Gọi API Lưu
        public ReceiptPopupViewModel PopupThuTienVM { get; set; } = new ReceiptPopupViewModel();
        #endregion

        #region PROPERTIES (Biến Binding của trang Cha)

        // --- Danh sách & Phân trang ---
        private ObservableCollection<ReceiptResponse> _danhSachPhieuThuGoc = new();

        private ObservableCollection<ReceiptResponse> _danhSachPhieuThu = new();
        public ObservableCollection<ReceiptResponse> DanhSachPhieuThu
        {
            get => _danhSachPhieuThu;
            set { _danhSachPhieuThu = value; OnPropertyChanged(); }
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

        // --- Bộ lọc (Filters) ---
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

        #region COMMANDS (Các nút bấm trên lưới và tiêu đề)

        public ICommand MoPopupThemCommand { get; }
        public ICommand MoPopupSuaCommand { get; }
        public ICommand XoaPhieuThuCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand XoaLocCommand { get; }
        public ICommand PhanTrangCommand { get; }

        #endregion

        public ReceiptViewModel()
        {
            // --- GIAO VIỆC MỞ POPUP CHO THẰNG CON (PopupThuTienVM) ---
            MoPopupThemCommand = new RelayCommand<object>(p => PopupThuTienVM.MoPopupThemMoi());
            MoPopupSuaCommand = new RelayCommand<ReceiptResponse>(p => PopupThuTienVM.MoPopupSua(p));

            // --- CÁC VIỆC TRANG CHA TỰ LÀM ---
            XoaPhieuThuCommand = new RelayCommand<ReceiptResponse>(ExecuteXoaPhieuThu);
            XoaLocCommand = new RelayCommand<object>(ExecuteXoaLoc);
            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);
            ExportExcelCommand = new RelayCommand<object>(p => MessageBox.Show("Tính năng xuất Excel đang được xây dựng!"));

            // Khi thằng con (Popup) báo lưu thành công, thằng cha tự động tải lại DataGrid
            PopupThuTienVM.OnSavedSuccess = () => _ = LoadDataAsync();

            // Lần đầu mở trang thì tự động tải dữ liệu
            _ = LoadDataAsync();
        }

        #region LOGIC TRANG CHA (Tải dữ liệu, Lọc, Xóa)

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

        private async void ExecuteXoaPhieuThu(ReceiptResponse pt)
        {
            if (pt == null) return;

            // Định dạng chuỗi hiển thị mã phiếu cho đẹp
            string maPtFormat = $"PT{pt.NgayTao:ddMMyy}{pt.MaPhieuThuTien:D3}";

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {maPtFormat} của khách hàng {pt.TenKhachHang} không?\nSố tiền nợ sẽ bị cộng lại như cũ.",
                                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    bool isSuccess = await ApiClient.DeleteAsync($"api/PhieuThu/{pt.MaPhieuThuTien}");
                    if (isSuccess)
                    {
                        MessageBox.Show("Đã xóa phiếu thu và hoàn lại công nợ!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        _ = LoadDataAsync(); // Tải lại danh sách sau khi xóa
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Tắt comment khi có API thật:
                var result = await ApiClient.GetAsync<List<ReceiptResponse>>("api/PhieuThu");
                if (result != null)
                {
                    _danhSachPhieuThuGoc = new ObservableCollection<ReceiptResponse>(result);
                }

                // --- DỮ LIỆU GIẢ LẬP ĐỂ TEST ---
                var dummyData = new List<ReceiptResponse>
                {
                    new ReceiptResponse { MaPhieuThuTien = 1, NgayTao = DateTime.Now.AddDays(-2), MaKhachHang = 1, TenKhachHang = "Nguyễn Văn A", TenNguoiTao = "admin", LyDoThu = "Thu tiền nợ tháng trước", SoTienThu = 1500000 },
                    new ReceiptResponse { MaPhieuThuTien = 2, NgayTao = DateTime.Now.AddDays(-1), MaKhachHang = 2, TenKhachHang = "Công ty TNHH Vạn Phát", TenNguoiTao = "nhanvien1", LyDoThu = "Thu tiền mua sỉ", SoTienThu = 5000000 },
                    new ReceiptResponse { MaPhieuThuTien = 3, NgayTao = DateTime.Now, MaKhachHang = 3, TenKhachHang = "Trần Thị B", TenNguoiTao = "admin", LyDoThu = "Thanh toán một phần", SoTienThu = 300000 }
                };
                _danhSachPhieuThuGoc = new ObservableCollection<ReceiptResponse>(dummyData);
                // -------------------------------

                _ = ApplyFilterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ApplyFilterAsync()
        {
            if (_danhSachPhieuThuGoc == null) return;

            var filtered = _danhSachPhieuThuGoc.AsEnumerable();
            var text = SearchText.ToLower().Trim();

            // 1. Lọc theo chữ (Tên KH hoặc Tên người thu)
            if (!string.IsNullOrEmpty(text))
            {
                filtered = filtered.Where(pt =>
                    (pt.TenKhachHang != null && pt.TenKhachHang.ToLower().Contains(text)) ||
                    (pt.TenNguoiTao != null && pt.TenNguoiTao.ToLower().Contains(text)));
            }

            // 2. Lọc Từ ngày
            if (FromDate.HasValue)
            {
                filtered = filtered.Where(pt => pt.NgayTao.Date >= FromDate.Value.Date);
            }

            // 3. Lọc Đến ngày
            if (ToDate.HasValue)
            {
                filtered = filtered.Where(pt => pt.NgayTao.Date <= ToDate.Value.Date);
            }

            // Thực hiện tính toán phân trang
            var resultList = filtered.ToList();
            TongBanGhi = resultList.Count;

            TongSoTrang = Math.Max(1, (int)Math.Ceiling(TongBanGhi / (double)_soDongTrenTrang));
            if (TrangHienTai > TongSoTrang) TrangHienTai = 1;

            // Cắt data theo trang và đưa lên giao diện
            DanhSachPhieuThu = new ObservableCollection<ReceiptResponse>(
                resultList.Skip((TrangHienTai - 1) * _soDongTrenTrang).Take(_soDongTrenTrang));

            await Task.CompletedTask;
        }

        #endregion
    }
}
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
    // KẾ THỪA TỪ BASE LIST VIEW MODEL
    public class ReceiptViewModel : BaseListViewModel
    {
        #region COMPONENTS
        public ReceiptPopupViewModel PopupThuTienVM { get; set; } = new ReceiptPopupViewModel();
        #endregion

        #region PROPERTIES CHUYÊN BIỆT CỦA PHIẾU THU

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                TrangHienTai = 1;
                SearchKeyword = value;

                ApplyFilterAndPagination();
            }
        }

        // Danh sách gốc tải từ server
        private List<ReceiptResponse> _danhSachPhieuThuGoc = new();

        // Danh sách đã cắt trang để hiển thị lên DataGrid
        private ObservableCollection<ReceiptResponse> _danhSachPhieuThu = new();
        public ObservableCollection<ReceiptResponse> DanhSachPhieuThu
        {
            get => _danhSachPhieuThu;
            set { _danhSachPhieuThu = value; OnPropertyChanged(); }
        }

        // Các bộ lọc riêng biệt (Từ ngày - Đến ngày)
        // (Lưu ý: Biến SearchKeyword đã có sẵn trong BaseListViewModel)

        private DateTime? _fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
                TrangHienTai = 1;             
                ApplyFilterAndPagination();
            }
        }

        private DateTime? _toDate = DateTime.Today;
        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
                TrangHienTai = 1;             // Khi đổi ngày thì tự nhảy về trang 1
                ApplyFilterAndPagination();
            }
        }

        #endregion

        #region COMMANDS 

        public ICommand MoPopupThemCommand { get; }
        public ICommand MoPopupSuaCommand { get; }
        public ICommand XoaPhieuThuCommand { get; }
        public ICommand XoaLocCommand { get; }

        // Không cần khai báo PhanTrangCommand vì Base đã có!

        #endregion

        public ReceiptViewModel()
        {
            // --- GIAO VIỆC MỞ POPUP CHO THẰNG CON ---
            MoPopupThemCommand = new RelayCommand<object>(p => PopupThuTienVM.MoPopupThemMoi());
            MoPopupSuaCommand = new RelayCommand<ReceiptResponse>(p => PopupThuTienVM.MoPopupSua(p));

            // --- CÁC VIỆC TRANG CHA TỰ LÀM ---
            XoaPhieuThuCommand = new RelayCommand<ReceiptResponse>(ExecuteXoaPhieuThu);

            XoaLocCommand = new RelayCommand<object>(p =>
            {
                SearchText = "";
                SearchKeyword = "";
                FromDate = null;
                ToDate = null;
            });

            PopupThuTienVM.OnSavedSuccess = () => _ = LoadDataAsync();

            // Lần đầu mở trang thì tự động tải dữ liệu
            _ = LoadDataAsync();
        }

        #region LOGIC TẢI VÀ LỌC DỮ LIỆU

        public async Task LoadDataAsync()
        {
            try
            {
                var result = await ApiClient.GetAsync<List<ReceiptResponse>>("api/PhieuThu");
                if (result != null)
                {
                    _danhSachPhieuThuGoc = result;
                }

                // Mặc định gọi hàm chia trang của Base
                ApplyFilterAndPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==========================================
        // GHI ĐÈ HÀM LỌC TỪ BASE CLASS
        // ==========================================
        protected override void ApplyFilterAndPagination()
        {
            if (_danhSachPhieuThuGoc == null) return;

            var filtered = _danhSachPhieuThuGoc.AsEnumerable();

            // Dùng SearchKeyword của BaseListViewModel
            var text = SearchKeyword?.ToLower().Trim() ?? "";

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

            // 4. Đồng bộ số liệu cho Base (Tổng bản ghi, Tổng trang)
            var resultList = filtered.ToList();
            TongBanGhi = resultList.Count;

            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;

            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;

            // 5. Cắt data theo trang (Dùng PageSize của Base) và đẩy lên UI
            DanhSachPhieuThu = new ObservableCollection<ReceiptResponse>(
                resultList.Skip((TrangHienTai - 1) * PageSize).Take(PageSize));
        }

        #endregion

        #region LOGIC XÓA

        private async void ExecuteXoaPhieuThu(ReceiptResponse pt)
        {
            if (pt == null) return;

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
                        _ = LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region PROPERTIES: QUY ĐỊNH NHẬP - XUẤT KHO
        private int _soLuongNhapToiThieu;
        public int SoLuongNhapToiThieu
        {
            get => _soLuongNhapToiThieu;
            set
            {
                _soLuongNhapToiThieu = value; OnPropertyChanged();
                CheckChanges();
            }
        }

        private bool _isInitializing = false;

        private int _soLuongTonToiDaCoTheNhap;
        public int SoLuongTonToiDaCoTheNhap { get => _soLuongTonToiDaCoTheNhap; set { _soLuongTonToiDaCoTheNhap = value; OnPropertyChanged(); CheckChanges(); } }

        private int _soLuongTonToiThieu;
        public int SoLuongTonToiThieu { get => _soLuongTonToiThieu; set { _soLuongTonToiThieu = value; OnPropertyChanged(); CheckChanges(); } }
        #endregion

        #region PROPERTIES: QUY ĐỊNH BÁN HÀNG & TÀI CHÍNH
        private int _thueVAT;
        public int ThueVAT { get => _thueVAT; set { _thueVAT = value; OnPropertyChanged(); CheckChanges(); } }

        private int _tiLeDonGiaBan;
        public int TiLeDonGiaBan { get => _tiLeDonGiaBan; set { _tiLeDonGiaBan = value; OnPropertyChanged(); CheckChanges(); } }

        private bool _tienThuLonHonNo;
        public bool TienThuLonHonNo { get => _tienThuLonHonNo; set { _tienThuLonHonNo = value; OnPropertyChanged(); CheckChanges(); } }
        #endregion

        #region PROPERTIES: QUY ĐỊNH KHUYẾN MÃI
        private bool _choPhepKetThucUuDai;
        public bool ChoPhepKetThucUuDai { get => _choPhepKetThucUuDai; set { _choPhepKetThucUuDai = value; OnPropertyChanged(); CheckChanges(); } }

        private bool _coKhoangCachCacKhoangGia;
        public bool CoKhoangCachCacKhoangGia { get => _coKhoangCachCacKhoangGia; set { _coKhoangCachCacKhoangGia = value; OnPropertyChanged(); CheckChanges(); } }

        private int _soLuongUuDaiToiThieu;
        public int SoLuongUuDaiToiThieu { get => _soLuongUuDaiToiThieu; set { _soLuongUuDaiToiThieu = value; OnPropertyChanged(); CheckChanges(); } }

        private int _soLuongUuDaiToiDa;
        public int SoLuongUuDaiToiDa { get => _soLuongUuDaiToiDa; set { _soLuongUuDaiToiDa = value; OnPropertyChanged(); CheckChanges(); } }
        #endregion
        private Dictionary<string, int> _originalValues = new();

        // Biến kích hoạt ẩn/hiện nút
        private bool _hasChanges;
        public bool HasChanges
        {
            get => _hasChanges;
            set { _hasChanges = value; OnPropertyChanged(); }
        }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

        public ICommand LuuCaiDatCommand { get; }
        public ICommand TaiLaiCommand { get; }

        public SettingViewModel()
        {
            LuuCaiDatCommand = new RelayCommand<object>(ExecuteLuuCaiDat);
            TaiLaiCommand = new RelayCommand<object>(async (p) => await LoadDataAsync());

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            _isInitializing = true;
            try
            {
                _originalValues.Clear(); // Xóa ảnh chụp cũ trước khi tải mới
                HasChanges = false;

                var tasks = new List<Task>()
                {
                    FetchSetting("ChoPhepKetThucUuDai", v => ChoPhepKetThucUuDai = v == 1),
                    FetchSetting("CoKhoangCachCacKhoangGia", v => CoKhoangCachCacKhoangGia = v == 1),
                    FetchSetting("SoLuongNhapToiThieu", v => SoLuongNhapToiThieu = v),
                    FetchSetting("SoLuongTonToiDaCoTheNhap", v => SoLuongTonToiDaCoTheNhap = v),
                    FetchSetting("SoLuongTonToiThieu", v => SoLuongTonToiThieu = v),
                    FetchSetting("SoLuongUuDaiToiDa", v => SoLuongUuDaiToiDa = v),
                    FetchSetting("SoLuongUuDaiToiThieu", v => SoLuongUuDaiToiThieu = v),
                    FetchSetting("ThueVAT", v => ThueVAT = v),
                    FetchSetting("TienThuLonHonNo", v => TienThuLonHonNo = v == 1),
                    FetchSetting("TiLeDonGiaBan", v => TiLeDonGiaBan = v)
                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isInitializing = false; 
                HasChanges = false;    
                IsLoading = false;
            }
        }

        private async void ExecuteLuuCaiDat(object obj)
        {
            IsLoading = true;
            try
            {
                var tasks = new List<Task>()
                {
                    UpdateSetting("ChoPhepKetThucUuDai", ChoPhepKetThucUuDai ? 1 : 0),
                    UpdateSetting("CoKhoangCachCacKhoangGia", CoKhoangCachCacKhoangGia ? 1 : 0),
                    UpdateSetting("SoLuongNhapToiThieu", SoLuongNhapToiThieu),
                    UpdateSetting("SoLuongTonToiDaCoTheNhap", SoLuongTonToiDaCoTheNhap),
                    UpdateSetting("SoLuongTonToiThieu", SoLuongTonToiThieu),
                    UpdateSetting("SoLuongUuDaiToiDa", SoLuongUuDaiToiDa),
                    UpdateSetting("SoLuongUuDaiToiThieu", SoLuongUuDaiToiThieu),
                    UpdateSetting("ThueVAT", ThueVAT),
                    UpdateSetting("TienThuLonHonNo", TienThuLonHonNo ? 1 : 0),
                    UpdateSetting("TiLeDonGiaBan", TiLeDonGiaBan)
                };

                await Task.WhenAll(tasks);
                MessageBox.Show("Đã lưu tất cả cấu hình thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task FetchSetting(string tenThamSo, Action<int> assignAction)
        {
            var res = await ApiClient.GetAsync<ThamSoDTO>($"api/ThamSo/{tenThamSo}");
            if (res != null)
            {
                _originalValues[tenThamSo] = res.GiaTri;
                assignAction(res.GiaTri);
            }
        }

        private async Task UpdateSetting(string tenThamSo, int giaTri)
        {
            var request = new ThamSoDTO { TenThamSo = tenThamSo, GiaTri = giaTri };
            await ApiClient.PutAsync<object, object>($"api/ThamSo/{tenThamSo}", request);
        }

        private void CheckChanges()
        {
            if (_isInitializing || _originalValues.Count < 10) return;

            HasChanges =
                GetOrig("SoLuongNhapToiThieu") != SoLuongNhapToiThieu ||
                GetOrig("SoLuongTonToiDaCoTheNhap") != SoLuongTonToiDaCoTheNhap ||
                GetOrig("SoLuongTonToiThieu") != SoLuongTonToiThieu ||
                GetOrig("ThueVAT") != ThueVAT ||
                GetOrig("TiLeDonGiaBan") != TiLeDonGiaBan ||
                GetOrig("TienThuLonHonNo") != (TienThuLonHonNo ? 1 : 0) ||
                GetOrig("ChoPhepKetThucUuDai") != (ChoPhepKetThucUuDai ? 1 : 0) ||
                GetOrig("CoKhoangCachCacKhoangGia") != (CoKhoangCachCacKhoangGia ? 1 : 0) ||
                GetOrig("SoLuongUuDaiToiThieu") != SoLuongUuDaiToiThieu ||
                GetOrig("SoLuongUuDaiToiDa") != SoLuongUuDaiToiDa;
        }
        private int GetOrig(string key) => _originalValues.ContainsKey(key) ? _originalValues[key] : 0;
    }
}
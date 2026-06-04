using Bookstore.Share.DTO;
using Bookstore.Share.DTO.Bookstore.Share.DTO;
using Bookstore.Share.Enums;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class PromotionEditPopupViewModel : BaseViewModel
    {
        private bool _isPopupVisible;
        public bool IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        public string PopupTitle => IsEditMode ? "ĐIỀU CHỈNH ƯU ĐÃI" : "LẬP PHIẾU ƯU ĐÃI MỚI";
        public string SaveButtonText => IsEditMode ? "LƯU THAY ĐỔI" : "TẠO ƯU ĐÃI";

        private bool _isEditMode;
        public bool IsEditMode { get => _isEditMode; set { _isEditMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(PopupTitle)); OnPropertyChanged(nameof(SaveButtonText)); } }

        public bool IsCodeEnabled => !IsEditMode;

        private bool _isCoreEditingAllowed = true;
        public bool IsCoreEditingAllowed { get => _isCoreEditingAllowed; set { _isCoreEditingAllowed = value; OnPropertyChanged(); } }

        private PromotionDTO _editingPromotion;
        public PromotionDTO EditingPromotion { get => _editingPromotion; set { _editingPromotion = value; OnPropertyChanged(); } }

        // CẦU NỐI ĐỂ KÍCH HOẠT TRIGGER ẨN HIỆN GIAO DIỆN
        public PromotionType SelectedMaLoaiUuDai
        {
            get => EditingPromotion?.MaLoaiUuDai ?? PromotionType.HoaDonGiam;
            set
            {
                if (EditingPromotion != null)
                {
                    EditingPromotion.MaLoaiUuDai = value;
                    OnPropertyChanged();
                }
            }
        }

        // ==========================================
        // DỮ LIỆU DANH MỤC ĐỂ CHỌN (ĐÃ FIX ONPROPERTYCHANGED)
        // ==========================================
        private ObservableCollection<CustomerTierResponse> _listLoaiKhachHang = new();
        public ObservableCollection<CustomerTierResponse> ListLoaiKhachHang { get => _listLoaiKhachHang; set { _listLoaiKhachHang = value; OnPropertyChanged(); } }

        private ObservableCollection<PromotionTypeResponse> _availablePromotionTypes = new();
        public ObservableCollection<PromotionTypeResponse> AvailablePromotionTypes { get => _availablePromotionTypes; set { _availablePromotionTypes = value; OnPropertyChanged(); } }

        private ObservableCollection<BookItem> _listSach = new();
        public ObservableCollection<BookItem> ListSach { get => _listSach; set { _listSach = value; OnPropertyChanged(); } }

        // BỘ ĐỆM ĐỂ GIAO DIỆN WPF CÓ THỂ TỰ ĐỘNG CẬP NHẬT KHI THÊM/XÓA
        public ObservableCollection<SachDieuKienDTO> UI_DanhSachSachDieuKien { get; set; } = new();
        public ObservableCollection<SachTangDTO> UI_DanhSachSachTang { get; set; } = new();

        // ==========================================
        // BIẾN TẠM ĐỂ THÊM SÁCH VÀO DANH SÁCH 1:N
        // ==========================================
        private string _tempIsbnDieuKien;
        public string TempIsbnDieuKien { get => _tempIsbnDieuKien; set { _tempIsbnDieuKien = value; OnPropertyChanged(); } }
        private int _tempSoLuongDieuKien = 1;
        public int TempSoLuongDieuKien { get => _tempSoLuongDieuKien; set { _tempSoLuongDieuKien = value; OnPropertyChanged(); } }

        private string _tempIsbnTang;
        public string TempIsbnTang { get => _tempIsbnTang; set { _tempIsbnTang = value; OnPropertyChanged(); } }
        private int _tempSoLuongTang = 1;
        public int TempSoLuongTang { get => _tempSoLuongTang; set { _tempSoLuongTang = value; OnPropertyChanged(); } }

        private bool _isGiamTienMode = true;
        public bool IsGiamTienMode { get => _isGiamTienMode; set { _isGiamTienMode = value; OnPropertyChanged(); } }

        private bool _isGiamPhanTramMode;
        public bool IsGiamPhanTramMode { get => _isGiamPhanTramMode; set { _isGiamPhanTramMode = value; OnPropertyChanged(); } }

        // ==========================================
        // COMMANDS
        // ==========================================
        public ICommand ClosePopupCommand { get; }
        public ICommand SavePromotionCommand { get; }
        public ICommand AddSachDieuKienCommand { get; }
        public ICommand RemoveSachDieuKienCommand { get; }
        public ICommand AddSachTangCommand { get; }
        public ICommand RemoveSachTangCommand { get; }

        private Action<PromotionDTO> _onSaveCallback;

        public PromotionEditPopupViewModel()
        {
            ClosePopupCommand = new RelayCommand<object>((p) => IsPopupVisible = false);

            SavePromotionCommand = new RelayCommand<object>((p) =>
            {
                if (IsGiamTienMode)
                {
                    EditingPromotion.TiLeGiam = 0;
                    EditingPromotion.GiamToiDa = 0;
                }
                else if (IsGiamPhanTramMode)
                {
                    EditingPromotion.SoTienGiam = 0;
                }

                EditingPromotion.DanhSachSachDieuKien = UI_DanhSachSachDieuKien.ToList();
                EditingPromotion.DanhSachSachTang = UI_DanhSachSachTang.ToList();

                _onSaveCallback?.Invoke(EditingPromotion);
            });

            // LOGIC XỬ LÝ 1:N CHO SÁCH ĐIỀU KIỆN (Thao tác trên ObservableCollection)
            AddSachDieuKienCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TempIsbnDieuKien) || TempSoLuongDieuKien <= 0) return;

                var existing = UI_DanhSachSachDieuKien.FirstOrDefault(x => x.ISBN == TempIsbnDieuKien);
                if (existing != null)
                {
                    existing.SoLuongMua += TempSoLuongDieuKien;
                    // Mẹo update list để UI nhận diện dòng vừa thay đổi
                    UI_DanhSachSachDieuKien.Remove(existing);
                    UI_DanhSachSachDieuKien.Add(existing);
                }
                else
                {
                    var sachDuocChon = ListSach.FirstOrDefault(x => x.ISBN == TempIsbnDieuKien);
                    string tenSach = sachDuocChon != null ? sachDuocChon.TenSach : "Sách không xác định";
                    // Gán thêm TenSach vào đây
                    UI_DanhSachSachDieuKien.Add(new SachDieuKienDTO
                    {
                        ISBN = TempIsbnDieuKien,
                        TenSach = tenSach,
                        SoLuongMua = TempSoLuongDieuKien
                    });
                }


                TempIsbnDieuKien = null; TempSoLuongDieuKien = 1;
            });

            RemoveSachDieuKienCommand = new RelayCommand<SachDieuKienDTO>((item) =>
            {
                if (item != null) UI_DanhSachSachDieuKien.Remove(item);
            });

            // LOGIC XỬ LÝ 1:N CHO SÁCH TẶNG
            AddSachTangCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TempIsbnTang) || TempSoLuongTang <= 0) return;

                var existing = UI_DanhSachSachTang.FirstOrDefault(x => x.ISBN == TempIsbnTang);
                if (existing != null)
                {
                    existing.SoLuongTang += TempSoLuongTang;
                    UI_DanhSachSachTang.Remove(existing);
                    UI_DanhSachSachTang.Add(existing);
                }
                else
                {
                    var sachDuocChon = ListSach.FirstOrDefault(x => x.ISBN == TempIsbnTang);
                    string tenSach = sachDuocChon != null ? sachDuocChon.TenSach : "Sách không xác định";

                    UI_DanhSachSachTang.Add(new SachTangDTO
                    {
                        ISBN = TempIsbnTang,
                        TenSach = tenSach, // GÁN TÊN SÁCH ĐỂ LÊN UI CÓ CÁI HIỂN THỊ
                        SoLuongTang = TempSoLuongTang
                    });
                }
                TempIsbnTang = null;
                TempSoLuongTang = 1;
            });

            RemoveSachTangCommand = new RelayCommand<SachTangDTO>((item) =>
            {
                if (item != null) UI_DanhSachSachTang.Remove(item);
            });
        }

        public void ShowPopup(PromotionDTO promo, bool isEdit, Action<PromotionDTO> onSave)
        {
            IsEditMode = isEdit;
            EditingPromotion = promo;
            _onSaveCallback = onSave;

            // Nạp dữ liệu vào bộ đệm UI
            UI_DanhSachSachDieuKien.Clear();
            if (promo.DanhSachSachDieuKien != null)
                foreach (var item in promo.DanhSachSachDieuKien) UI_DanhSachSachDieuKien.Add(item);

            UI_DanhSachSachTang.Clear();
            if (promo.DanhSachSachTang != null)
                foreach (var item in promo.DanhSachSachTang) UI_DanhSachSachTang.Add(item);

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

            IsCoreEditingAllowed = !(isEdit && promo.SoLuongDaDung > 0);
            OnPropertyChanged(nameof(SelectedMaLoaiUuDai));
            IsPopupVisible = true;
        }
    }
}
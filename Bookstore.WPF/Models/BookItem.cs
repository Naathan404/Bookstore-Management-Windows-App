using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.WPF.Models
{
    public class BookItem : BaseViewModel
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int NamXuatBan { get; set; }
        public int LanTaiBan { get; set; }
        public string HinhThucBia { get; set; } = string.Empty;
        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }

        private int _stt;
        public int STT { get => _stt; set { _stt = value; OnPropertyChanged(); } }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        private string _theLoai;
        public string TheLoai { get => _theLoai; set { _theLoai = value; OnPropertyChanged(); } }

        private string _moTa;
        public string MoTa { get => _moTa; set { _moTa = value; OnPropertyChanged(); } }

        private string _hinhAnh = "default_book_cover.jpg";
        public string HinhAnh { get => _hinhAnh; set { _hinhAnh = value; OnPropertyChanged(); } }

        private string _nhaXuatBan;
        public string NhaXuatBan { get => _nhaXuatBan; set { _nhaXuatBan = value; OnPropertyChanged(); } }

        private ObservableCollection<TacGiaDTO> _danhSachTacGia = new();
        public ObservableCollection<TacGiaDTO> DanhSachTacGia { get => _danhSachTacGia; set { _danhSachTacGia = value; OnPropertyChanged(); } }

        private bool _isCanhBaoTonKho;
        public bool IsCanhBaoTonKho { get => _isCanhBaoTonKho; set { _isCanhBaoTonKho = value; OnPropertyChanged(); } }

        private decimal _tiLeGiaBan = 1.0m;
        public decimal TiLeGiaBan
        {
            get => _tiLeGiaBan;
            set
            {
                if (_tiLeGiaBan != value)
                {
                    _tiLeGiaBan = value;
                    OnPropertyChanged();

                    _isManualDonGiaBan = false; // Reset cờ vì người dùng muốn tính lại theo tỷ lệ
                    TinhLaiDonGiaBan();
                }
            }
        }

        private decimal _giaNiemYet;
        public decimal GiaNiemYet
        {
            get => _giaNiemYet;
            set
            {
                if (_giaNiemYet != value)
                {
                    _giaNiemYet = value;
                    OnPropertyChanged();

                    _isManualDonGiaBan = false; // QUAN TRỌNG: Phá cờ nhập tay để tính lại giá bán mới
                    TinhLaiDonGiaBan();
                }
            }
        }

        private decimal _donGiaBan;
        public decimal DonGiaBan
        {
            get => _donGiaBan;
            set
            {
                if (_donGiaBan != value)
                {
                    _donGiaBan = value;
                    _isManualDonGiaBan = true; // Đánh dấu là đã nhập tay
                    OnPropertyChanged();
                }
            }
        }

        private bool _isManualDonGiaBan = false;

        public void ResetManualFlag()
        {
            _isManualDonGiaBan = false;
            TinhLaiDonGiaBan();
        }

        private void TinhLaiDonGiaBan()
        {
            if (!_isManualDonGiaBan)
            {
                _donGiaBan = Math.Round(_giaNiemYet * _tiLeGiaBan);
                OnPropertyChanged(nameof(DonGiaBan));
            }
        }
    }
}

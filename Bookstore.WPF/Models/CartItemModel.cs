using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.WPF.Models
{
    public class CartItemModel : BaseViewModel
    {
        public string ISBN { get; set; }
        public string TenSach { get; set; }
        private decimal _giaBan;
        public decimal GiaBan
        {
            get => _giaBan;
            set
            {
                if (_giaBan != value)
                {
                    _giaBan = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        private decimal _originalGiaBan;
        public decimal OriginalGiaBan { get => _originalGiaBan; set { _originalGiaBan = value; OnPropertyChanged(); } }

        public int SoLuongTonKho { get; set; }

        private bool _isGift;
        public bool IsGift { get => _isGift; set { _isGift = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); } }

        private bool _isPromotionApplied;
        public bool IsPromotionApplied { get => _isPromotionApplied; set { _isPromotionApplied = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPrice)); } }

        public bool IsNormalPrice => !IsGift && !IsPromotionApplied;

        private int _soLuongMua;
        public int SoLuongMua
        {
            get => _soLuongMua;
            set
            {
                if (_soLuongMua != value)
                {
                    if (value > SoLuongTonKho) _soLuongMua = SoLuongTonKho;
                    else if (value < 0) _soLuongMua = 0;
                    else _soLuongMua = value;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public decimal ThanhTien => GiaBan * _soLuongMua;
    }
}

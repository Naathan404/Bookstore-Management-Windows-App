using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.WPF.Models
{
    public class BookSaleModel : BaseViewModel
    {
        public BookItem BookData { get; set; }

        // Trạng thái chọn trong giỏ hàng/danh sách
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        // Giá khuyến mãi
        private decimal _promotionPrice;
        public decimal PromotionPrice
        {
            get => _promotionPrice;
            set { _promotionPrice = value; OnPropertyChanged(); }
        }

        // Các trạng thái (Khi 1 cờ thay đổi, báo cho UI biết IsNormalPrice cũng thay đổi theo)
        private bool _hasPromotion;
        public bool HasPromotion
        {
            get => _hasPromotion;
            set
            {
                _hasPromotion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNormalPrice));
            }
        }

        private bool _isGift;
        public bool IsGift
        {
            get => _isGift;
            set
            {
                _isGift = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNormalPrice));
            }
        }

        // Thuộc tính Read-only, tự động tính toán dựa vào 2 cờ trên
        public bool IsNormalPrice => !HasPromotion && !IsGift;

        // Constructor để bắt buộc phải có BookItem khi tạo Model này
        public BookSaleModel(BookItem bookItem)
        {
            BookData = bookItem ?? throw new ArgumentNullException(nameof(bookItem));
        }
    }
}

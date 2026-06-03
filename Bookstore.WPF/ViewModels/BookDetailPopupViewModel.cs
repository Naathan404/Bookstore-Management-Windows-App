using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class BookDetailPopupViewModel : BaseViewModel
    {
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set { _isOpen = value; OnPropertyChanged(); } }

        // Biến chứa toàn bộ thông tin sách để hiển thị lên UI
        private BookSaleModel _bookDetails;
        public BookSaleModel BookDetails { get => _bookDetails; set { _bookDetails = value; OnPropertyChanged(); } }

        // Callback để gửi lệnh "Thêm vào giỏ" ngược về trang gọi nó (Trang Bán hàng)
        private Action<BookSaleModel> _onAddToCartCallback;

        public ICommand CloseCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }

        public BookDetailPopupViewModel()
        {
            CloseCommand = new RelayCommand(() => IsOpen = false);

            AddToCartCommand = new RelayCommand(() =>
            {
                if (BookDetails != null)
                {
                    _onAddToCartCallback?.Invoke(BookDetails); // Kích hoạt callback truyền sách về
                    IsOpen = false; // Thêm xong thì đóng popup luôn cho mượt
                }
            });
        }

        // HÀM GỌI TỪ CÁC TRANG KHÁC ĐỂ HIỂN THỊ POPUP
        public void ShowPopup(BookSaleModel book, Action<BookSaleModel> onAddToCart)
        {
            BookDetails = book;
            _onAddToCartCallback = onAddToCart;
            IsOpen = true;
        }
    }
}
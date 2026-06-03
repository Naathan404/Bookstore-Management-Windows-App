using Bookstore.WPF.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bookstore.WPF.Services
{
    public class CartService
    {
        // Khởi tạo Singleton: Đảm bảo toàn bộ phần mềm chỉ dùng chung duy nhất 1 giỏ hàng
        private static CartService _instance;
        public static CartService Instance => _instance ??= new CartService();

        // Nơi lưu trữ giỏ hàng tập trung
        public ObservableCollection<CartItemModel> CartItems { get; } = new ObservableCollection<CartItemModel>();

        private CartService() { }

        // Hàm xử lý thêm vào giỏ lõi (Được bê từ SaleViewModel sang)
        public void AddToCart(BookSaleModel book, int soLuong = 1)
        {
            if (book == null || book.BookData.SoLuongTonKho <= 0) return;

            var itemTrongGio = CartItems.FirstOrDefault(i => i.ISBN == book.BookData.ISBN && !i.IsGift);

            if (itemTrongGio != null)
            {
                if (itemTrongGio.SoLuongMua < itemTrongGio.SoLuongTonKho)
                    itemTrongGio.SoLuongMua += soLuong;
            }
            else
            {
                CartItems.Add(new CartItemModel
                {
                    ISBN = book.BookData.ISBN,
                    TenSach = book.BookData.TenSach,
                    OriginalGiaBan = book.BookData.DonGiaBan,
                    GiaBan = book.BookData.DonGiaBan,
                    SoLuongTonKho = book.BookData.SoLuongTonKho,
                    SoLuongMua = soLuong
                });
            }
        }
    }
}
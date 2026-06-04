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

        public void SyncCartWithFreshData(List<BookSaleModel> freshBooks)
        {
            // Phải duyệt vòng lặp ngược (từ cuối lên đầu) vì chúng ta có thể sẽ xóa item khỏi List trong lúc duyệt
            for (int i = CartItems.Count - 1; i >= 0; i--)
            {
                var cartItem = CartItems[i];

                // Quà tặng thì bỏ qua, vì hàm TinhToanHoaDon() của SaleViewModel sẽ tự động xóa và tính lại quà tặng sau
                if (cartItem.IsGift) continue;

                var freshBook = freshBooks.FirstOrDefault(b => b.BookData.ISBN == cartItem.ISBN);

                // TRƯỜNG HỢP 1: Sách đã bị xóa khỏi hệ thống, hoặc số lượng tồn kho về 0
                if (freshBook == null || freshBook.BookData.SoLuongTonKho <= 0)
                {
                    CartItems.RemoveAt(i);
                }
                else
                {
                    // TRƯỜNG HỢP 2: Cập nhật thông tin mới nhất (Tên, Giá, Tồn kho)
                    cartItem.TenSach = freshBook.BookData.TenSach;
                    cartItem.OriginalGiaBan = freshBook.BookData.DonGiaBan;
                    cartItem.SoLuongTonKho = freshBook.BookData.SoLuongTonKho;

                    // TRƯỜNG HỢP 3: Tồn kho mới bị hụt xuống, ít hơn số lượng khách đang định mua
                    if (cartItem.SoLuongMua > freshBook.BookData.SoLuongTonKho)
                    {
                        cartItem.SoLuongMua = freshBook.BookData.SoLuongTonKho;
                    }

                    // Note: Thuộc tính GiaBan sẽ được hàm TinhToanHoaDon bên SaleViewModel đè lại sau
                }
            }
        }
    }
}
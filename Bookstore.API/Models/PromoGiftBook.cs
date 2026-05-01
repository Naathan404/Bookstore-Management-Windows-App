namespace Bookstore.API.Models
{
    public class PromoGiftBook
        //QUATANG_SACH
    {
        public int DetailID { get; set; }
        public required string ISBN { get; set; }
        public int GiftQuantity { get; set; }
    }
}

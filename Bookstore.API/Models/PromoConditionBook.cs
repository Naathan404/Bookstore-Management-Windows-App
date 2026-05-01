namespace Bookstore.API.Models
{
    public class PromoConditionBook
        //DIEUKIEN_SACH
    {
        public int DetailID { get; set; }
        public required string ISBN { get; set; }
        public int MinQuantity { get; set; }

    }
}

namespace Bookstore.API.Models
{
    public class PromotionApplied : IEntity
        //HOADON_UUDAI
    {
        public int PromotionAppliedID { get; set; }
        public int InvoiceID { get; set; }
        public int PromotionID { get; set; }
        public required string ISBN { get; set; }
        public decimal DiscountAmount { get; set; }
        public int GetID() => PromotionAppliedID;

    }
}

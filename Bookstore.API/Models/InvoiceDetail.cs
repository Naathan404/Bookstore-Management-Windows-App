namespace Bookstore.API.Models
{
    public class InvoiceDetail : IEntity
    {
        public int InvoiceDetailID { get; set; }
        public int InvoiceID { get; set; }
        public int BookID { get; set; }
        public int StockID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int PromotionID { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalPrice { get; set; }

        public int GetID() => InvoiceDetailID;
    }
}

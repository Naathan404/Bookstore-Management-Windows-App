namespace Bookstore.API.Models
{
    public class Invoice : IEntity
    {
        public int InvoiceID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }


        public int CustomerID { get; set; }
        public int PromotionID { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal CustomerPaid { get; set; }
        public decimal DebtIncurred { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;

        public int GetID() => InvoiceID;
    }
}

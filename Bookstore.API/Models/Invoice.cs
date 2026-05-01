namespace Bookstore.API.Models
{
    public class Invoice : IEntity
        // HOADON
    {
        public int InvoiceID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public int GetID() => InvoiceID;
    }
}

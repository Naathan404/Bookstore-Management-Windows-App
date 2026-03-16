namespace Bookstore.API.Models
{
    public class Receipt : IEntity
    {
        public int ReceiptID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public int CustomerID { get; set; }
        public int InvoiceID { get; set; }
        public decimal ReceiptValue { get; set; }
        public int PaymentMethod { get; set; } // 0: cash, 1: banking

        public int GetID() => ReceiptID;
    }
}

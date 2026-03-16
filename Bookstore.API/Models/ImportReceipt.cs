namespace Bookstore.API.Models
{
    public class ImportReceipt : IEntity
    {
        public int ImportReceiptID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ReviewedDate { get; set; } 
        public int ReviewedID { get; set; }
        public int Status { get; set; } // 0: draft, 1: pending, 2: approved, 3: rejected

        public int SupplierID { get; set; }
        public int StockID { get; set; }
        public decimal TotalAmount { get; set; }

        public int GetID() => ImportReceiptID;
    }
}

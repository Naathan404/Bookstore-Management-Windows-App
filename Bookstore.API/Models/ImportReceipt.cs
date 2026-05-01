namespace Bookstore.API.Models
{
    public class ImportReceipt : IEntity
        // PHIEUNHAPSACH
    {
        public int ImportReceiptID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int SupplierID { get; set; }
        public decimal TotalAmount { get; set; }
        public int GetID() => ImportReceiptID;
    }
}

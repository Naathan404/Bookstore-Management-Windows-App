namespace Bookstore.API.Models
{
    public class Receipt : IEntity
        //PHIEUTHU
    {
        public int ReceiptID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerID { get; set; }
        public decimal ReceiptAmount { get; set; }
        public int GetID() => ReceiptID;
    }
}

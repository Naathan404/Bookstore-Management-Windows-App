using Microsoft.Identity.Client;

namespace Bookstore.API.Models
{
    public class StockTransfer : IEntity
    {
        public int TransferID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int Status { get; set; } // 0: pending, 1: approved, 2: rejected

        public int FromStockID { get; set; }
        public int ToStockID { get; set; }
        public string Note { get; set; } = string.Empty;

        public int GetID() => TransferID;
    }
}

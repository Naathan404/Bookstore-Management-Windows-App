namespace Bookstore.API.Models
{
    public class StockTransferDetail : IEntity
    {
        public int StockTransferDetailID { get; set; }
        public int TransferID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }

        public int GetID() => StockTransferDetailID;
    }
}

namespace Bookstore.API.Models
{
    public class Stock : IEntity
    {
        public int StockID { get; set; }
        public string StockName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Description { get; set; } = string.Empty;

        public int GetID() => StockID;
    }
}

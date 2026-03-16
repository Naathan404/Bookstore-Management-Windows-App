namespace Bookstore.API.Models
{
    public class BookCount : IEntity
    {
        public int BookCountID { get; set; }
        public int BookID { get; set; }
        public int StockID { get; set; }
        public int Count { get; set; }
        public int Threshold { get; set; }

        public int GetID() => BookCountID;
    }
}

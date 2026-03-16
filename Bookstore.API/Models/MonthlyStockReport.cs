namespace Bookstore.API.Models
{
    public class MonthlyStockReport : IEntity
    {
        public int ReportID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int StockID { get; set; }
        public int BookID { get; set; }

        public int StartCount { get; set; }
        public int ImportedCount { get; set; }
        public int ExportedCount { get; set; }
        public int EndCount { get; set; }

        public int GetID() => ReportID;
    }
}

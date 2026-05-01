namespace Bookstore.API.Models
{
    public class MonthlyBookReport : IEntity
        //BAOCAOSACH
    {
        public int ReportID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public int GetID() => ReportID;
    }
}

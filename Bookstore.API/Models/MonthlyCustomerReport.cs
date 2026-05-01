namespace Bookstore.API.Models
{
    public class MonthlyCustomerReport : IEntity
        //BAOCAOKHACHHANG
    {
        public int ReportID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalDebt { get; set; }
        public int GetID() => ReportID;
    }
}

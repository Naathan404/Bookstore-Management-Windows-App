namespace Bookstore.API.Models
{
    public class MonthlyDebtReport : IEntity
    {
        public int ReportID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CustomerID { get; set; }

        public decimal StartDebt { get; set; }
        public decimal IncurredDebt { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal EndDebt { get; set; }

        public int GetID() => ReportID;
    }
}

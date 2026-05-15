namespace Bookstore.Share.DTOs
{
    public class DashboardOverviewDto
    {
        public decimal Sale { get; set; }
        public decimal Profit { get; set; }
        public int CustNum { get; set; }
        public int ReceiptNum { get; set; }

        public List<RevenueDataDto> RevenueSeries { get; set; }
        public List<CategoryShareDto> CategoryShares { get; set; }

        public List<TopBookDto> TopBooks { get; set; }
        public List<CustomerRankingDto> TopCustomers { get; set; }
        public List<StaffRankingDto> TopStaffs { get; set; }

        public List<OrderDto> RecentOrders { get; set; }
        public List<ImportDto> RecentImports { get; set; }
        public List<PaymentDto> RecentPayments { get; set; }
        public List<StockWarningDto> StockWarnings { get; set; }
    }
    // DTOs con cho các phần dữ liệu chi tiết hơn
    public class RevenueDataDto { public string Date { get; set; } public double Value { get; set; } }
    public class CategoryShareDto { public string CategoryName { get; set; } public double Percentage { get; set; } }
    public class TopBookDto { public int Rank { get; set; } public string BookImage { get; set; } }
    public class CustomerRankingDto { public string Name { get; set; } public decimal TotalSpent { get; set; } }
    public class StaffRankingDto { public string Name { get; set; } public decimal SalesAmount { get; set; } }
    public class OrderDto { public string ReceiptNum { get; set; } public string CustomerName { get; set; } public decimal TotalCost { get; set; } }
    public class ImportDto { public string ImportId { get; set; } public string SupplierName { get; set; } public int TotalQuantity { get; set; } }
    public class PaymentDto { public string PaymentId { get; set; } public string Reason { get; set; } public decimal Amount { get; set; } }
    public class StockWarningDto { public string Name { get; set; } public int RemainingQuantity { get; set; } }
}
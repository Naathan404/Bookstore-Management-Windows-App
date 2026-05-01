using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class MonthlyCustomerReportDetail : IEntity
        // CT_BC_KHACHHANG
    {
        public int ReportID { get; set; }
        public int CustomerID { get; set; }

        public int TotalInvoices { get; set; } // SoHoaDon
        public decimal Revenue { get; set; } // DoanhThu

        public decimal InitialDebt { get; set; } // NoDau
        public decimal IncurredDebt { get; set; } // PhatSinh

        public decimal PaidAmount { get; set; } // DaTra

        public decimal FinalDebt { get; set; }
        public int GetID() => ReportID;
    }
}

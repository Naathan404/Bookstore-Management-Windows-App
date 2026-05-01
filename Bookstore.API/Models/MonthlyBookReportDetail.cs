using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class MonthlyBookReportDetail
        //CT_BC_SACH
    {
        public int ReportId { get; set; }
        public required string ISBN { get; set; }

        public int InitialStock { get; set; } // TonDau
        public int ImportQuantity { get; set; } // TongNhap
        public int ExportQuantity { get; set; } // TongXuat
        public int FinalStock { get; set; } // TonCuoi
        public decimal Revenue { get; set; }
    }
}

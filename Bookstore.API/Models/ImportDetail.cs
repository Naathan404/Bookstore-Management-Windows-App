using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Bookstore.API.Models
{
    public class ImportDetail
        //CT_PHIEUNHAP
    {
        public int ImportReceiptID { get; set; }
        public int ISBN { get; set; }
        public int Quantity { get; set; }
        public decimal ImportedPrice { get; set; }
    }
}

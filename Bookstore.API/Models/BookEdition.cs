using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class BookEdition
        // PHIENBANSACH
    {
        [Key]
        [StringLength(20)]
        public required string ISBN { get; set; } // Khóa chính là chuỗi nên không dùng IEntity (nếu IEntity ép kiểu int)

        public int BookId { get; set; }
        public int PublisherId { get; set; }

        public int PublishYear { get; set; }
        public int EditionNumber { get; set; } = 1;
        public string CoverType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ListPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }
        public int TotalSold { get; set; }
    }
}

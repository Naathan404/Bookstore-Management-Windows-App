using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PromoDetail_InvoiceDiscount : IEntity
        //CTUD_HD_GIAM
    {
        [Key]
        public int DetailID { get; set; }
        public int PromotionID { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal MaxOrderAmount { get; set; }
        public decimal DiscountAmount { get; set; } 
        public double DiscountPercentage { get; set; } // Tỷ lệ giảm (ví dụ: 0.1 cho 10%)
        public decimal MaxDiscountAmount { get; set; } // Số tiền giảm tối đa (cap)
        public int GetID() => DetailID;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PromoDetail_BookDiscount : IEntity
        //CTUD_SACH_GIAM
    {
        [Key]
        public int DetailID { get; set; }
        public int PromotionID { get; set; }
        public decimal DiscountAmount { get; set; }
        public double DiscountPercentage { get; set; }
        public decimal MaxDiscountAmount { get; set; }
        public int GetID() => DetailID;
    }
}

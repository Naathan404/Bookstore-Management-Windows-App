using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class PromoDetail_BookGift : IEntity
        //CTUD_SACH_QUA
    {
        [Key]
        public int DetailID { get; set; }
        public int PromotionID { get; set; }
        public int GetID() => DetailID;
    }
}

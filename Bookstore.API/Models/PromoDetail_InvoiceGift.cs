using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PromoDetail_InvoiceGift : IEntity
        //CTUD_HD_QUA
    {
        public int DetailID { get; set; }
        public int PromotionID { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal MaxOrderAmount { get; set; }

        public int GetID() => DetailID;
    }
}

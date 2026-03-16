namespace Bookstore.API.Models
{
    public class Promotion : IEntity
    {
  //      - Phân loại 3 ưu đãi:
  //- 0: Giảm theo hóa đơn
  //- 1: Giảm theo đầu sách
  //- 2: Mua hàng tặng hàng
        public int PromotionID { get; set; }
        public required string Code { get; set; }
        public string Description { get; set; }  = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public int PromoType { get; set; } // 0, 1, 2

        // Dieu kien
        public decimal MinimumOrderValue { get; set; }
        public int RequiredBookID { get; set; }
        public int RequiredQuantity { get; set; }

        // Phan thuong
        public int DiscountRate { get; set; }
        public decimal MaxDiscountAmount { get; set; }
        public int GiftBookID { get; set; }
        public int GiftQuantity { get; set; }

        // Gioi han
        public int TotalUsed {  get; set; } = 0;
        public int LimitUsed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public int GetID() => PromotionID;

    }
}

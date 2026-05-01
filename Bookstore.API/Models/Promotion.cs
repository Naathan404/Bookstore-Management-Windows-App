namespace Bookstore.API.Models
{
    public class Promotion : IEntity
        // UUDAI
    {
        public int PromotionCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int PromotionTypeID { get; set; }
        public string PromotionName { get; set; } = string.Empty;
        public string Description { get; set; }  = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxQuantity { get; set; }
        public int UsedQuantity { get; set; }
        public int CustomerTierID { get; set; }
        public bool IsActive { get; set; }
        public int GetID() => PromotionCode;

    }
}

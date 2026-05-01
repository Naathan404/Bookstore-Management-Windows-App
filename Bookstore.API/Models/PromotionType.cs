namespace Bookstore.API.Models
{
    public class PromotionType : IEntity
        // LOAI UU DAI
    {
        public int PromotionTypeID { get; set; }
        public string PromotionTypeName { get; set; } = string.Empty;
        public int MaxStackingLimit { get; set; }
        public int GetID() => PromotionTypeID;
    }
}

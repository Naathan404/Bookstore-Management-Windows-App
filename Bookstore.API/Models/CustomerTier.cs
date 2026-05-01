namespace Bookstore.API.Models
{
    public class CustomerTier : IEntity
        // LOAIKHACHHANG
    {
        public int CustomerTierID { get; set; }
        public required string TierName { get; set; }
        public decimal MaxDept { get; set; }
        public double MinPaymentRatio { get; set; }
        public int GetID() => CustomerTierID;
    }
}

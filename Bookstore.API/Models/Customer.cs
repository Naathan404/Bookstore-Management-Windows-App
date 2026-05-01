using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class Customer : IEntity
        // KHACHHANG
    {
        [Key]
        public int CustomerID { get; set; }
        public int CustomerTierID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Gender { get; set; } // 0: Nam, 1: Nu
        public DateOnly DateOfBirth { get; set;} = new DateOnly();
        public string TaxCode { get; set; } = string.Empty;
        public string Address { get; set; }  = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal TotalPurchaseValue { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalDebt { get; set; }
        public int GetID() => CustomerID;
    }
}

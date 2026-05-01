namespace Bookstore.API.Models
{
    public class Supplier : IEntity
        // NHACUNGCAP
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public string BankAccount { get; set; } = string.Empty;
        public int GetID() => SupplierID;
    }
}

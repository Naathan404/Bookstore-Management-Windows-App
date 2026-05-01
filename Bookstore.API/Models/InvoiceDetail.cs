namespace Bookstore.API.Models
{
    public class InvoiceDetail
        // CT_HOADON
    {
        public int InvoiceID { get; set; }
        public int ISBN { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

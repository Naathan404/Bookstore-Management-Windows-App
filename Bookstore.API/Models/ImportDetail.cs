using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Bookstore.API.Models
{
    public class ImportDetail : IEntity
    {
        public int ImportDetailID { get; set; }
        public int ImportReceiptID { get; set; }

        public int BookID { get; set; }
        public int Quantity { get; set; }
        public decimal ImportedPrice { get; set; }
        public string Note { get; set; } = string.Empty;

        public int GetID() => ImportDetailID;
    }
}

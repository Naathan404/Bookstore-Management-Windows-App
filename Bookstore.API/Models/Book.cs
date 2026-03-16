using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class Book : IEntity
    {
        public int BookID { get; set; }
        public required string ISBN { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        //---
        public required string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int Edition { get; set; } = 1;
        public int CategoryID { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal ListPrice { get; set; }
        public decimal ImportedPrice { get; set; }
        //---
        public int StockQuantity { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public int GetID() => BookID;
    }
}

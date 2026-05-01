using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class Book : IEntity
        // SACH
    {
        [Key]
        public int BookId { get; set; }
        public required string Title { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; } = string.Empty;

        public int GetID() => BookId;
    }
}

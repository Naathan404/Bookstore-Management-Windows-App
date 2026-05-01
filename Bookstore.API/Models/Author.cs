using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class Author : IEntity
        // TACGIA
    {
        [Key]
        public int AuthorID { get; set; }
        public required string AuthorName { get; set; }
        public int GetID() => AuthorID;
    }
}

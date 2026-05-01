using System.ComponentModel.DataAnnotations;
namespace Bookstore.API.Models
{
    public class Category : IEntity
        // THELOAI
    {
        [Key] 
        public int CategoryID { get; set;}
        public required string CategoryName { get; set; }
        public int GetID() => CategoryID;
    }
}

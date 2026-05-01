using System.ComponentModel.DataAnnotations;
namespace Bookstore.API.Models
{
    public class TheLoai : IEntity<int>
        // THELOAI
    {
        [Key] 
        public int MaTheLoai { get; set;}
        public required string TenTheLoai { get; set; }
        public int GetID() => MaTheLoai;
    }
}

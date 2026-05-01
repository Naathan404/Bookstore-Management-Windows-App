using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class TacGia : IEntity<int>
        // TACGIA
    {
        [Key]
        public int MaTacGia { get; set; }
        public required string TenTacGia { get; set; }
        public int GetID() => MaTacGia;
    }
}

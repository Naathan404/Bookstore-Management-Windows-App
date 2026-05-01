using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class ChucNang : IEntity
        // CHUCNANG
    {
        [Key]
        public int MaChucNang { get; set; }
        public required string TenChucNang { get; set; }
        public string TenManHinh { get; set; } = string.Empty;
        public int GetID() => MaChucNang;
    }
}

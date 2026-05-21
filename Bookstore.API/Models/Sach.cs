using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class Sach : IEntity<int>
        // SACH
    {
        [Key]
        public int MaSach { get; set; }
        public required string TenSach { get; set; }
        public int MaTheLoai { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public int GetID() => MaSach;

        public string ImageUrl { get; set; } = "/Resources/Images/Books/default_book_cover.jpg";

        [ForeignKey("MaTheLoai")]
        public virtual TheLoai? TheLoai { get; set; }
    }
}

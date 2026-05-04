using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class HoaDon_UuDai : IEntity<int>
        //HOADON_UUDAI
    {
        [Key]
        public int MaCT_HoaDon_UuDai { get; set; }
        public int MaHoaDon { get; set; }
        public int MaUuDai { get; set; }
        public string? ISBN { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienGiam { get; set; }
        public int GetID() => MaCT_HoaDon_UuDai;

    }
}

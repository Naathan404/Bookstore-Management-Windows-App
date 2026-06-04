using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    /// <summary>
    /// Lịch sử áp dụng ưu đãi cho hóa đơn, có thể áp dụng nhiều ưu đãi cho một hóa đơn
    /// </summary>
    public class HoaDon_UuDai : IEntity<int>
    {
        [Key]
        public int MaCT_HoaDon_UuDai { get; set; }
        public int MaHoaDon { get; set; }
        public int MaUuDai { get; set; }
        //public string? ISBN { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienGiam { get; set; }
        public int GetID() => MaCT_HoaDon_UuDai;
        [ForeignKey("MaHoaDon")]
        public virtual HoaDon? HoaDon { get; set; }
        [ForeignKey("MaUuDai")]
        public virtual UuDai? UuDai { get; set; }
    }
}

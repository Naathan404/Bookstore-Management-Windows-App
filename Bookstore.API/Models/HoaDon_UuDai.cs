using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class HoaDon_UuDai : IEntity
        //HOADON_UUDAI
    {
        [Key]
        public int MaCT_HoaDon_UuDai { get; set; }
        public int MaHoaDon { get; set; }
        public int MaUuDai { get; set; }
        public required string ISBN { get; set; }
        public decimal SoTienGiam { get; set; }
        public int GetID() => MaCT_HoaDon_UuDai;

    }
}

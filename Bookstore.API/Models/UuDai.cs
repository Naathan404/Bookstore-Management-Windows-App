using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class UuDai : IEntity<int>
        // UUDAI
    {
        [Key]
        public int MaUuDai { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; } = string.Empty;
        public int MaLoaiUuDai { get; set; }
        public string TenUuDai { get; set; } = string.Empty;
        public string MoTa { get; set; }  = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SoLuongToiDa { get; set; }
        public int SoLuongDaDung { get; set; }
        public int MaLoaiKhachHang { get; set; }
        public bool CoTheSuDung { get; set; }
        public int GetID() => MaUuDai;

    }
}

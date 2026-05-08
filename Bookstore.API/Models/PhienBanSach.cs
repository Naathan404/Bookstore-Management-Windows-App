using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PhienBanSach : IEntity<string>
        // PHIENBANSACH
    {
        [Key]
        public required string ISBN { get; set; } // Khóa chính là chuỗi nên không dùng IEntity (nếu IEntity ép kiểu int)

        public int MaSach { get; set; }
        public int MaNhaXuatBan { get; set; }

        public int NamXuatBan { get; set; }
        public int LanTaiBan { get; set; } = 1;
        public string HinhThucBia { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")] public decimal GiaNiemYet { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal DonGiaBan { get; set; }
        public int TonKho { get; set; }
        public int TongSoDaBan { get; set; }
        public string GetID() => ISBN;


        public virtual NhaXuatBan NhaXuatBan { get; set; }
    }
}

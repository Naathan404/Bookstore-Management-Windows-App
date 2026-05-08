using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_PhieuNhapSach
        //CT_PHIEUNHAP
    {
        public int MaPhieuNhapSach { get; set; }
        public required string ISBN { get; set; }
        public int SoLuong { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal DonGiaNhap { get; set; }

        [ForeignKey("MaPhieuNhapSach")]
        public virtual PhieuNhapSach? PhieuNhapSach { get; set; }

        [ForeignKey("ISBN")]
        public virtual PhienBanSach? PhienBanSach { get; set; }
    }
}

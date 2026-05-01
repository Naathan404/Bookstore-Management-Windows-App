using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class CT_PhieuNhapSach
        //CT_PHIEUNHAP
    {
        public int MaPhieuNhapSach { get; set; }
        public required string ISBN { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_BC_Sach
        //CT_BC_SACH
    {
        public int MaBaoCaoSach { get; set; }
        public required string ISBN { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal ChiPhiNhap { get; set; }

        public int TonDau { get; set; } 
        public int TongNhap { get; set; } 
        public int TongXuat { get; set; } 
        public int TonCuoi { get; set; }
    }
}

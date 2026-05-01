using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CTUD_HoaDon_Giam : IEntity
        //CTUD_HD_GIAM
    {
        [Key]
        public int MaCT { get; set; }
        public int MaUuDai { get; set; }
        public decimal SoTienToiThieu { get; set; }
        public decimal SoTienToiDa { get; set; }
        public decimal SoTienGiam { get; set; } 
        public double TiLeGiam { get; set; } // Tỷ lệ giảm (ví dụ: 0.1 cho 10%)
        public decimal GiamToiDa { get; set; } // Số tiền giảm tối đa (cap)
        public int GetID() => MaCT;
    }
}

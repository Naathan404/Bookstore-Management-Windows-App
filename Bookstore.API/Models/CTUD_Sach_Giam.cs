using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CTUD_Sach_Giam : IEntity<int>
        //CTUD_SACH_GIAM
    {
        [Key]
        public int MaCT { get; set; }
        public int MaUuDai { get; set; }
        public decimal SoTienGiam { get; set; }
        public double TiLeGiam { get; set; }
        public decimal GiamToiDa { get; set; }
        public int GetID() => MaCT;
    }
}

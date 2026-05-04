using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CTUD_HoaDon_Qua : IEntity<int>
        //CTUD_HD_QUA
    {
        [Key]
        public int MaCT { get; set; }
        public int MaUuDai { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienToiThieu { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienToiDa { get; set; }

        public int GetID() => MaCT;
    }
}

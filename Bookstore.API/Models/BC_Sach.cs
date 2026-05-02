using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class BC_Sach : IEntity<int>
        //BAOCAOSACH
    {
        [Key]
        public int MaBaoCaoSach { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongDoanhThu { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongChiPhi { get; set; }
        public int GetID() => MaBaoCaoSach;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class BC_Sach : IEntity<int>
        //BAOCAOSACH
    {
        [Key]
        public int MaBaoCaoSach { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal TongChiPhi { get; set; }
        public int GetID() => MaBaoCaoSach;
    }
}

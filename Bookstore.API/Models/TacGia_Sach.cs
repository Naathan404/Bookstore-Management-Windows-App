using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class TacGia_Sach
        //CT_TAGCIA
    {
        public int MaTacGia { get; set; }
        public int MaSach { get; set; }

        [ForeignKey("MaTacGia")]
        public virtual TacGia TacGia { get; set;}
        [ForeignKey("MaSach")]
        public virtual Sach Sach { get; set; }
    }
}

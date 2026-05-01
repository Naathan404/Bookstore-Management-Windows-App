using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class CTUD_Sach_Qua : IEntity
        //CTUD_SACH_QUA
    {
        [Key]
        public int MaCT { get; set; }
        public int MaUuDai { get; set; }
        public int GetID() => MaCT;
    }
}

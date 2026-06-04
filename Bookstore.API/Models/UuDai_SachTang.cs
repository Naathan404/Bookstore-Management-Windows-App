using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class UuDai_SachTang
        //QUATANG_SACH
    {
        public int MaUuDai { get; set; }
        public required string ISBN { get; set; }
        public int SoLuongTang { get; set; }
    }
}

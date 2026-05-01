using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class NhomNguoiDung : IEntity
        //NHOM NGUOI DUNG
    {
        [Key]
        public int MaNhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; } = string.Empty;
        public int GetID() => MaNhomNguoiDung;
    }
}

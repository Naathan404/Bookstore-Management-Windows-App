using System.ComponentModel.DataAnnotations;
namespace Bookstore.API.Models
{
    public class NhaXuatBan : IEntity<int>
        //NHACUNGCAP
    {
        [Key]
        public int MaNhaXuatBan { get; set; }
        public required string TenNhaXuatBan { get; set; }
        public int GetID() => MaNhaXuatBan;
    }
}

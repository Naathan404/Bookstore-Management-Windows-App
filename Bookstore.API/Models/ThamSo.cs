using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class ThamSo : IEntity<string>
    {
        [Key]
        public required string TenThamSo { get; set; }
        public int GiaTri { get; set; }
        public string GetID() => TenThamSo;
    }
}

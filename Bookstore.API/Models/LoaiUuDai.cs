using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class LoaiUuDai : IEntity<int>
        // LOAI UU DAI
    {
        [Key]
        public int MaLoaiUuDai { get; set; }
        public string TenLoaiUuDai { get; set; } = string.Empty;
        public int ApDungToiDa { get; set; }
        public int GetID() => MaLoaiUuDai;
    }
}

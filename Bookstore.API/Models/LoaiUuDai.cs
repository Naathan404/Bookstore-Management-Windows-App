using Bookstore.Share.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class LoaiUuDai : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public PromotionType MaLoaiUuDai { get; set; }
        public string TenLoaiUuDai { get; set; } = string.Empty;
        public int ApDungToiDa { get; set; }
        public int GetID() => (int)MaLoaiUuDai;
    }
}

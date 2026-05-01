using System.ComponentModel.DataAnnotations;
namespace Bookstore.API.Models
{
    public class Pulisher : IEntity
        //NHACUNGCAP
    {
        [Key]
        public int PublisherId { get; set; }
        public required string PulisherName { get; set; }
        public int GetID() => PublisherId;
    }
}

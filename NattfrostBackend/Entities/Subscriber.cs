using System.ComponentModel.DataAnnotations;

namespace NattfrostBackend.Entities
{
    public class Subscriber
    {
        [Key]
        public string Email { get; set; }
        public string City { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

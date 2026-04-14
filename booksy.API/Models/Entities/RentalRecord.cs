using System.ComponentModel.DataAnnotations.Schema;

namespace booksy.API.Models.Entities
{
    public class RentalRecord : BaseEntity
    {
        public int HardwareId { get; set; }

        [ForeignKey(nameof(HardwareId))]
        public Hardware Hardware { get; set; } = null!;

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public DateTime RentedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnedAt { get; set; }
    }
}

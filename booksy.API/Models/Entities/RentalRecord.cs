using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booksy.API.Models.Entities
{
    public class RentalRecord : BaseEntity
    {

        [Required(ErrorMessage = "Hardware ID is required")]
        public int HardwareId { get; set; }

        [ForeignKey(nameof(HardwareId))]
        public Hardware Hardware { get; set; } = null!;

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Rented At date is required")]
        public DateTime RentedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using booksy.API.Models.Enums;

namespace booksy.API.Models.Entities
{
    public class Hardware : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Brand { get; set; } = string.Empty;

        public DateOnly? PurchaseDate { get; set; }

        [Required]
        public HardwareStatus Status { get; set; } = HardwareStatus.Available;

        [StringLength(1000)]
        public string? Notes { get; set; }


        public ICollection<RentalRecord> RentalHistory { get; set; } = [];
    }
}

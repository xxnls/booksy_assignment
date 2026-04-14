using System.ComponentModel.DataAnnotations;
using booksy.API.Models.Enums;

namespace booksy.API.Models.Entities
{
    public class Hardware : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Brand must be between 2 and 100 characters")]
        public string Brand { get; set; } = string.Empty;

        public DateOnly? PurchaseDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public HardwareStatus Status { get; set; } = HardwareStatus.Available;

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        [StringLength(2000, ErrorMessage = "History cannot exceed 2000 characters")]
        public string? History { get; set; }


        public ICollection<RentalRecord> RentalHistory { get; set; } = [];
    }
}

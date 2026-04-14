using booksy.API.Models.Enums;

namespace booksy.API.Models.Entities
{
    public class Hardware : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public DateOnly? PurchaseDate { get; set; }

        public HardwareStatus Status { get; set; } = HardwareStatus.Available;

        public string? Notes { get; set; }

        public string? History { get; set; }

        public RentalRecord? RentalRecord { get; set; }
    }
}

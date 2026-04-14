using booksy.API.Models.Enums;

namespace booksy.API.Models.DTOs
{
    public class HardwareDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public DateOnly? PurchaseDate { get; set; }
        public HardwareStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? History { get; set; }
        public DateTime DateCreated { get; set; }

        // single active rental for display
        public RentalRecordDto? ActiveRental { get; set; }
    }

    public class CreateHardwareDto
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public DateOnly? PurchaseDate { get; set; }
        public HardwareStatus Status { get; set; } = HardwareStatus.Available;
        public string? Notes { get; set; }
        public string? History { get; set; }
        public string? AssignedTo { get; set; }
    }

    public class UpdateHardwareDto
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public HardwareStatus? Status { get; set; }
        public string? Notes { get; set; }
        public string? History { get; set; }
    }
}

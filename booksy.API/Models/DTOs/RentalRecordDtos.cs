namespace booksy.API.Models.DTOs
{
    public class RentalRecordDto
    {
        public int Id { get; set; }
        public int HardwareId { get; set; }
        public string HardwareName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTime RentedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class CreateRentalRecordDto
    {
        public int HardwareId { get; set; }
        public int UserId { get; set; }
        public DateTime RentedAt { get; set; } = DateTime.UtcNow;
    }

    public class UpdateRentalRecordDto
    {
        public DateTime? ReturnedAt { get; set; }
    }
}

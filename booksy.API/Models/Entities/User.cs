using System.ComponentModel.DataAnnotations;
using booksy.API.Models.Enums;

namespace booksy.API.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Employee;


        public ICollection<RentalRecord> Rentals { get; set; } = [];
    }
}

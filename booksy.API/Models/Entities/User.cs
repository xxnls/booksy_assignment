using System.ComponentModel.DataAnnotations.Schema;
using booksy.API.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace booksy.API.Models.Entities
{
    public class User : IdentityUser<int>
    {
        public UserRole Role { get; set; } = UserRole.User;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        [NotMapped]
        public bool IsDeleted => DateDeleted is not null;

        public ICollection<RentalRecord> Rentals { get; set; } = [];
    }
}

using Microsoft.AspNetCore.Identity;

namespace Core.Concretes.Entities
{
    public class Member : IdentityUser<string>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber2 { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public DateTime? DriverLicenseExpiryDate { get; set; }
        public string? NationalIdNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsBlacklisted { get; set; } = false;
        public string? BlacklistReason { get; set; }
        public DateTime? BlacklistDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Address> Addresses { get; set; } = [];
        public virtual ICollection<Rental> Rentals { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
        public virtual ICollection<Document> Documents { get; set; } = [];
        public virtual ICollection<Penalty> Penalties { get; set; } = [];
    }
}

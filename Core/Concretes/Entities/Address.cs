using Core.Abstracts.Bases;
namespace Core.Concretes.Entities
{
    public class Address : BaseEntity
    {
        public string MemberId { get; set; }
        public string Title { get; set; } // Ev, İş, Diğer
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDefault { get; set; } = false;
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Member Member { get; set; }
    }
}

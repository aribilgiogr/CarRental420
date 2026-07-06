using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } // Şube adı
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Manager { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsOpen24Hours { get; set; } = false;
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<Rental> RentalsStart { get; set; } = new List<Rental>();
        public virtual ICollection<Rental> RentalsEnd { get; set; } = new List<Rental>();
    }
}

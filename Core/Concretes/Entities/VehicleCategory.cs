using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class VehicleCategory : BaseEntity
    {
        public string Name { get; set; } // Örn: Ekonomi, Konfor, Lüks
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public int MaxPassengers { get; set; }
        public int BaggageCapacity { get; set; }
        public bool HasAirConditioning { get; set; } = true;
        public bool HasAutomaticTransmission { get; set; }
        public bool HasGPS { get; set; } = true;
        public string? ImageUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}

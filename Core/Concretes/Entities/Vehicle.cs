using Core.Abstracts.Bases;
using Core.Concretes.Enums;

namespace Core.Concretes.Entities
{
    public class Vehicle : BaseEntity
    {
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Kilometers { get; set; }
        public decimal PricePerDay { get; set; }
        public VehicleType VehicleType { get; set; }
        public int VehicleCategoryId { get; set; }
        public string? VIN { get; set; } // Araç Şasi Numarası
        public string? EngineNumber { get; set; }
        public string? Color { get; set; }
        public int Seats { get; set; }
        public decimal FuelTankCapacity { get; set; }
        public string? FuelType { get; set; } // Benzin, Dizel, Elektrik vb.
        public string? TransmissionType { get; set; } // Manuel, Otomatik
        public bool HasInsurance { get; set; } = true;
        public DateTime? InsuranceExpiryDate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool RequiresInspection { get; set; } = false;
        public string? Notes { get; set; }

        // Foreign Keys
        public int LocationId { get; set; }

        // Navigation Properties
        public virtual VehicleCategory VehicleCategory { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
        public virtual ICollection<CampaignVehicle> CampaignVehicles { get; set; } = new List<CampaignVehicle>();
    }
}

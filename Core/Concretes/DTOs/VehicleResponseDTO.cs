using Core.Concretes.Enums;

namespace Core.Concretes.DTOs.Vehicle
{
    public class VehicleResponseDTO
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Kilometers { get; set; }
        public decimal PricePerDay { get; set; }
        public VehicleType VehicleType { get; set; }
        public int VehicleCategoryId { get; set; }
        public string? VIN { get; set; } 
        public string? EngineNumber { get; set; }
        public string? Color { get; set; }
        public int Seats { get; set; }
        public decimal FuelTankCapacity { get; set; }
        public string? FuelType { get; set; } 
        public string? TransmissionType { get; set; } 
        public bool HasInsurance { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool RequiresInspection { get; set; }
        public string? Notes { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
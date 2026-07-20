namespace Core.Concretes.DTOs.VehicleCategory
{
    public class CreateVehicleCategoryDTO
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public int MaxPassengers { get; set; }
        public int BaggageCapacity { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasAutomaticTransmission { get; set; }
        public bool HasGPS { get; set; }
        public string? ImageUrl { get; set; }
    }
}
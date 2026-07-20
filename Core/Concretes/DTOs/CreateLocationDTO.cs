namespace Core.Concretes.DTOs.Location
{
    public class CreateLocationDTO
    {
        public string Name { get; set; } 
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
        public bool IsOpen24Hours { get; set; }
        public string? Notes { get; set; }
    }
}
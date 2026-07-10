namespace Core.Concretes.DTOs.Address
{
    public class UpdateAddressDTO
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDefault { get; set; }
        public string? Notes { get; set; }
        public bool Active { get; set; }
    }
}
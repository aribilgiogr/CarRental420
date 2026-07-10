namespace Core.Concretes.DTOs.VehicleImage
{
    public class CreateVehicleImageDTO
    {
        public int VehicleId { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public bool IsMainImage { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
    }
}
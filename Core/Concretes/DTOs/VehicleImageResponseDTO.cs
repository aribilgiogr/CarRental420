namespace Core.Concretes.DTOs.VehicleImage
{
    public class VehicleImageResponseDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public bool IsMainImage { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
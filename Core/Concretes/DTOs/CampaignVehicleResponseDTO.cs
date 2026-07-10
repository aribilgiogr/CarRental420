namespace Core.Concretes.DTOs.CampaignVehicle
{
    public class CampaignVehicleResponseDTO
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
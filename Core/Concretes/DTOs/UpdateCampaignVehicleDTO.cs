namespace Core.Concretes.DTOs.CampaignVehicle
{
    public class UpdateCampaignVehicleDTO
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int VehicleId { get; set; }
        public bool Active { get; set; }
    }
}
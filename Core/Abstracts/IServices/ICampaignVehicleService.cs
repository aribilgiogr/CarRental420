using Core.Concretes.DTOs.CampaignVehicle;

namespace Business.Services
{
    public interface ICampaignVehicleService
    {
        Task<CampaignVehicleResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CampaignVehicleResponseDTO>> GetAllAsync();
        Task<IEnumerable<CampaignVehicleResponseDTO>> GetByCampaignAsync(int campaignId);
        Task<IEnumerable<CampaignVehicleResponseDTO>> GetByVehicleAsync(int vehicleId);
        Task<CampaignVehicleResponseDTO> CreateAsync(CreateCampaignVehicleDTO dto);
        Task UpdateAsync(UpdateCampaignVehicleDTO dto);
        Task DeleteAsync(int id);
        Task<bool> RelationshipExistsAsync(int campaignId, int vehicleId);
        Task DeleteByCampaignAsync(int campaignId);
        Task DeleteByVehicleAsync(int vehicleId);
    }
}

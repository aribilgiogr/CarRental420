using Core.Concretes.DTOs.RentalCampaign;

namespace Business.Services
{
    public interface IRentalCampaignService
    {
        Task<RentalCampaignResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<RentalCampaignResponseDTO>> GetAllAsync();
        Task<IEnumerable<RentalCampaignResponseDTO>> GetByRentalAsync(int rentalId);
        Task<IEnumerable<RentalCampaignResponseDTO>> GetByCampaignAsync(int campaignId);
        Task<RentalCampaignResponseDTO> CreateAsync(CreateRentalCampaignDTO dto);
        Task UpdateAsync(UpdateRentalCampaignDTO dto);
        Task DeleteAsync(int id);
        Task<bool> RelationshipExistsAsync(int rentalId, int campaignId);
        Task<decimal> GetTotalDiscountByRentalAsync(int rentalId);
        Task DeleteByRentalAsync(int rentalId);
    }
}

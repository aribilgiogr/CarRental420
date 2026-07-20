using Core.Concretes.DTOs.Campaign;
using Core.Concretes.Enums;

namespace Business.Services
{
    public interface ICampaignService
    {
        Task<CampaignResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CampaignResponseDTO>> GetAllAsync();
        Task<IEnumerable<CampaignResponseDTO>> GetActiveCampaignsAsync();
        Task<IEnumerable<CampaignResponseDTO>> GetByTypeAsync(CampaignType campaignType);
        Task<IEnumerable<CampaignResponseDTO>> GetByScopeAsync(CampaignScope campaignScope);
        Task<CampaignResponseDTO?> GetByCouponCodeAsync(string couponCode);
        Task<IEnumerable<CampaignResponseDTO>> GetTimeBasedCampaignsAsync();
        Task<IEnumerable<CampaignResponseDTO>> GetUnlimitedCampaignsAsync();
        Task<CampaignResponseDTO> CreateAsync(CreateCampaignDTO dto);
        Task UpdateAsync(UpdateCampaignDTO dto);
        Task DeleteAsync(int id);
        Task<bool> CampaignExistsAsync(int id);
        Task<bool> CouponCodeExistsAsync(string couponCode);
        Task ActivateCampaignAsync(int campaignId);
        Task DeactivateCampaignAsync(int campaignId);
        Task<decimal> CalculateDiscountAsync(int campaignId, decimal rentalPrice);
        Task<bool> CanApplyCampaignAsync(int campaignId, string memberId, decimal rentalAmount);
        Task IncrementUsageCountAsync(int campaignId);
        Task<IEnumerable<CampaignResponseDTO>> GetApplicableCampaignsAsync(decimal rentalAmount);
    }
}

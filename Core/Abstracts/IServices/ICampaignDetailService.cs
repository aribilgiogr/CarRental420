using Core.Concretes.DTOs.CampaignDetail;

namespace Business.Services
{
    public interface ICampaignDetailService
    {
        Task<CampaignDetailResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<CampaignDetailResponseDTO>> GetAllAsync();
        Task<IEnumerable<CampaignDetailResponseDTO>> GetByCampaignAsync(int campaignId);
        Task<IEnumerable<CampaignDetailResponseDTO>> GetByTypeAsync(string detailType);
        Task<CampaignDetailResponseDTO> CreateAsync(CreateCampaignDetailDTO dto);
        Task UpdateAsync(UpdateCampaignDetailDTO dto);
        Task DeleteAsync(int id);
        Task<bool> DetailExistsAsync(int id);
        Task DeleteByCampaignAsync(int campaignId);
    }
}

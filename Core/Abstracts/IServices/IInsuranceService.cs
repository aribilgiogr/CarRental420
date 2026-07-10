using Core.Concretes.DTOs.Insurance;

namespace Business.Services
{
    public interface IInsuranceService
    {
        Task<InsuranceResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceResponseDTO>> GetAllAsync();
        Task<IEnumerable<InsuranceResponseDTO>> GetDefaultInsurancesAsync();
        Task<InsuranceResponseDTO> CreateAsync(CreateInsuranceDTO dto);
        Task UpdateAsync(UpdateInsuranceDTO dto);
        Task DeleteAsync(int id);
        Task<bool> InsuranceExistsAsync(int id);
        Task<IEnumerable<InsuranceResponseDTO>> GetInsurancesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}

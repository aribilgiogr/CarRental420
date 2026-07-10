using Core.Concretes.DTOs.VehicleCategory;

namespace Business.Services
{
    public interface IVehicleCategoryService
    {
        Task<VehicleCategoryResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VehicleCategoryResponseDTO>> GetAllAsync();
        Task<VehicleCategoryResponseDTO> CreateAsync(CreateVehicleCategoryDTO dto);
        Task UpdateAsync(UpdateVehicleCategoryDTO dto);
        Task DeleteAsync(int id);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CategoryNameExistsAsync(string name);
        Task<IEnumerable<VehicleCategoryResponseDTO>> GetCategoriesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}

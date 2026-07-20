using Core.Concretes.DTOs.VehicleImage;

namespace Business.Services
{
    public interface IVehicleImageService
    {
        Task<VehicleImageResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VehicleImageResponseDTO>> GetAllAsync();
        Task<IEnumerable<VehicleImageResponseDTO>> GetByVehicleAsync(int vehicleId);
        Task<VehicleImageResponseDTO?> GetMainImageByVehicleAsync(int vehicleId);
        Task<VehicleImageResponseDTO> CreateAsync(CreateVehicleImageDTO dto);
        Task UpdateAsync(UpdateVehicleImageDTO dto);
        Task DeleteAsync(int id);
        Task<bool> ImageExistsAsync(int id);
        Task SetMainImageAsync(int imageId);
        Task DeleteByVehicleAsync(int vehicleId);
    }
}

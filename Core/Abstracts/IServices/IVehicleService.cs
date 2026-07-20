using Core.Concretes.DTOs.Vehicle;
using Core.Concretes.Enums;

namespace Business.Services
{
    public interface IVehicleService
    {
        Task<VehicleResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VehicleResponseDTO>> GetAllAsync();
        Task<IEnumerable<VehicleResponseDTO>> GetByTypeAsync(VehicleType vehicleType);
        Task<IEnumerable<VehicleResponseDTO>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<VehicleResponseDTO>> GetByLocationAsync(int locationId);
        Task<IEnumerable<VehicleResponseDTO>> GetAvailableVehiclesAsync();
        Task<IEnumerable<VehicleResponseDTO>> GetUnavailableVehiclesAsync();
        Task<IEnumerable<VehicleResponseDTO>> GetVehiclesRequiringInspectionAsync();
        Task<VehicleResponseDTO> CreateAsync(CreateVehicleDTO dto);
        Task UpdateAsync(UpdateVehicleDTO dto);
        Task DeleteAsync(int id);
        Task<bool> VehicleExistsAsync(int id);
        Task<bool> LicensePlateExistsAsync(string licensePlate);
        Task MarkAsAvailableAsync(int vehicleId);
        Task MarkAsUnavailableAsync(int vehicleId);
        Task MarkForInspectionAsync(int vehicleId);
        Task UpdateKilometersAsync(int vehicleId, int kilometers);
    }
}

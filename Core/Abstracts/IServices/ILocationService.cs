using Core.Concretes.DTOs.Location;

namespace Business.Services
{
    public interface ILocationService
    {
        Task<LocationResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<LocationResponseDTO>> GetAllAsync();
        Task<IEnumerable<LocationResponseDTO>> GetByCityAsync(string city);
        Task<IEnumerable<LocationResponseDTO>> GetOpenLocationsAsync();
        Task<IEnumerable<LocationResponseDTO>> Get24HourLocationsAsync();
        Task<LocationResponseDTO> CreateAsync(CreateLocationDTO dto);
        Task UpdateAsync(UpdateLocationDTO dto);
        Task DeleteAsync(int id);
        Task<bool> LocationExistsAsync(int id);
        Task<bool> IsOpenAsync(int locationId, DateTime dateTime);
        Task<IEnumerable<LocationResponseDTO>> GetLocationsByDistanceAsync(double latitude, double longitude, double radiusInKm);
    }
}

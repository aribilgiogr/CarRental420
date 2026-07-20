using Core.Concretes.DTOs.Rental;

namespace Business.Services
{
    public interface IRentalService
    {
        Task<RentalResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<RentalResponseDTO>> GetAllAsync();
        Task<IEnumerable<RentalResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<RentalResponseDTO>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<RentalResponseDTO>> GetActiveRentalsAsync();
        Task<IEnumerable<RentalResponseDTO>> GetCompletedRentalsAsync();
        Task<IEnumerable<RentalResponseDTO>> GetOverdueRentalsAsync();
        Task<RentalResponseDTO> CreateAsync(CreateRentalDTO dto);
        Task UpdateAsync(UpdateRentalDTO dto);
        Task CompleteRentalAsync(int rentalId, int endOdometer);
        Task ReturnVehicleAsync(int rentalId, int endOdometer, bool hasDamage = false, string? damageDescription = null);
        Task DeleteAsync(int id);
        Task<bool> RentalExistsAsync(int id);
        Task<decimal> CalculateRentalPriceAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<RentalResponseDTO>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

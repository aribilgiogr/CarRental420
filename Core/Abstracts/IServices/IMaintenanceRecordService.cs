using Core.Concretes.DTOs.MaintenanceRecord;

namespace Business.Services
{
    public interface IMaintenanceRecordService
    {
        Task<MaintenanceRecordResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetAllAsync();
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetByTypeAsync(string maintenanceType);
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetPendingMaintenanceAsync();
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetCompletedMaintenanceAsync();
        Task<MaintenanceRecordResponseDTO> CreateAsync(CreateMaintenanceRecordDTO dto);
        Task UpdateAsync(UpdateMaintenanceRecordDTO dto);
        Task DeleteAsync(int id);
        Task<bool> MaintenanceRecordExistsAsync(int id);
        Task CompleteMaintenanceAsync(int maintenanceId);
        Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId);
        Task<decimal> GetTotalMaintenanceCostByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceRecordResponseDTO>> GetVehicleMaintenanceHistoryAsync(int vehicleId);
    }
}

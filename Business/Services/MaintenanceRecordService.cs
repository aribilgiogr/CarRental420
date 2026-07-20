using Core.Concretes.DTOs.MaintenanceRecord;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class MaintenanceRecordService : IMaintenanceRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaintenanceRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MaintenanceRecordResponseDTO> CreateAsync(CreateMaintenanceRecordDTO dto)
        {
            var entity = new MaintenanceRecord
            {
                VehicleId = dto.VehicleId,
                MaintenanceDate = dto.MaintenanceDate,
                MaintenanceType = dto.MaintenanceType,
                Description = dto.Description,
                Cost = dto.Cost,
                ServiceProvider = dto.ServiceProvider,
                MaintenanceStatus = dto.MaintenanceStatus ?? "Beklemede",
                CompletionDate = dto.CompletionDate,
                Odometer = dto.Odometer,
                Notes = dto.Notes,
                Attachments = dto.Attachments,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<MaintenanceRecord>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateMaintenanceRecordDTO dto)
        {
            var entity = await _unitOfWork.Repository<MaintenanceRecord>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.VehicleId = dto.VehicleId;
            entity.MaintenanceDate = dto.MaintenanceDate;
            entity.MaintenanceType = dto.MaintenanceType;
            entity.Description = dto.Description;
            entity.Cost = dto.Cost;
            entity.ServiceProvider = dto.ServiceProvider;
            entity.MaintenanceStatus = dto.MaintenanceStatus;
            entity.CompletionDate = dto.CompletionDate;
            entity.Odometer = dto.Odometer;
            entity.Notes = dto.Notes;
            entity.Attachments = dto.Attachments;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<MaintenanceRecord>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<MaintenanceRecord>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<MaintenanceRecord>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> MaintenanceRecordExistsAsync(int id)
        {
            return await _unitOfWork.Repository<MaintenanceRecord>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<MaintenanceRecordResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<MaintenanceRecord>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetByTypeAsync(string maintenanceType)
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.MaintenanceType == maintenanceType && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetPendingMaintenanceAsync()
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.MaintenanceStatus == "Beklemede" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetCompletedMaintenanceAsync()
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.MaintenanceStatus == "Tamamlandı" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task CompleteMaintenanceAsync(int maintenanceId)
        {
            var entity = await _unitOfWork.Repository<MaintenanceRecord>().FindByIdAsync(maintenanceId);
            if (entity != null && !entity.Deleted)
            {
                entity.MaintenanceStatus = "Tamamlandı";
                entity.CompletionDate = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<MaintenanceRecord>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return items.Select(x => x.Cost).Sum();
        }

        public async Task<decimal> GetTotalMaintenanceCostByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.MaintenanceDate >= startDate && x.MaintenanceDate <= endDate && !x.Deleted);
            return items.Select(x => x.Cost).Sum();
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<MaintenanceRecord>().FindManyAsync(x => x.MaintenanceDate >= startDate && x.MaintenanceDate <= endDate && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<MaintenanceRecordResponseDTO>> GetVehicleMaintenanceHistoryAsync(int vehicleId)
        {
            return await GetByVehicleAsync(vehicleId);
        }

        private static MaintenanceRecordResponseDTO MapToResponseDTO(MaintenanceRecord entity)
        {
            return new MaintenanceRecordResponseDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                MaintenanceDate = entity.MaintenanceDate,
                MaintenanceType = entity.MaintenanceType,
                Description = entity.Description,
                Cost = entity.Cost,
                ServiceProvider = entity.ServiceProvider,
                MaintenanceStatus = entity.MaintenanceStatus,
                CompletionDate = entity.CompletionDate,
                Odometer = entity.Odometer,
                Notes = entity.Notes,
                Attachments = entity.Attachments,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
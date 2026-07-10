using Core.Concretes.DTOs.VehicleCategory;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleCategoryResponseDTO> CreateAsync(CreateVehicleCategoryDTO dto)
        {
            var entity = new VehicleCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                DailyPrice = dto.DailyPrice,
                WeeklyPrice = dto.WeeklyPrice,
                MonthlyPrice = dto.MonthlyPrice,
                MaxPassengers = dto.MaxPassengers,
                BaggageCapacity = dto.BaggageCapacity,
                HasAirConditioning = dto.HasAirConditioning,
                HasAutomaticTransmission = dto.HasAutomaticTransmission,
                HasGPS = dto.HasGPS,
                ImageUrl = dto.ImageUrl,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<VehicleCategory>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateVehicleCategoryDTO dto)
        {
            var entity = await _unitOfWork.Repository<VehicleCategory>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.DailyPrice = dto.DailyPrice;
            entity.WeeklyPrice = dto.WeeklyPrice;
            entity.MonthlyPrice = dto.MonthlyPrice;
            entity.MaxPassengers = dto.MaxPassengers;
            entity.BaggageCapacity = dto.BaggageCapacity;
            entity.HasAirConditioning = dto.HasAirConditioning;
            entity.HasAutomaticTransmission = dto.HasAutomaticTransmission;
            entity.HasGPS = dto.HasGPS;
            entity.ImageUrl = dto.ImageUrl;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<VehicleCategory>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<VehicleCategory>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<VehicleCategory>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _unitOfWork.Repository<VehicleCategory>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<bool> CategoryNameExistsAsync(string name)
        {
            return await _unitOfWork.Repository<VehicleCategory>().AnyAsync(x => x.Name == name && !x.Deleted);
        }

        public async Task<IEnumerable<VehicleCategoryResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<VehicleCategory>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<VehicleCategoryResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<VehicleCategory>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<VehicleCategoryResponseDTO>> GetCategoriesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var items = await _unitOfWork.Repository<VehicleCategory>().FindManyAsync(x => x.DailyPrice >= minPrice && x.DailyPrice <= maxPrice && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static VehicleCategoryResponseDTO MapToResponseDTO(VehicleCategory entity)
        {
            return new VehicleCategoryResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DailyPrice = entity.DailyPrice,
                WeeklyPrice = entity.WeeklyPrice,
                MonthlyPrice = entity.MonthlyPrice,
                MaxPassengers = entity.MaxPassengers,
                BaggageCapacity = entity.BaggageCapacity,
                HasAirConditioning = entity.HasAirConditioning,
                HasAutomaticTransmission = entity.HasAutomaticTransmission,
                HasGPS = entity.HasGPS,
                ImageUrl = entity.ImageUrl,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
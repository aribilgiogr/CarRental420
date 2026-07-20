using Core.Concretes.DTOs.Insurance;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InsuranceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InsuranceResponseDTO> CreateAsync(CreateInsuranceDTO dto)
        {
            var entity = new Insurance
            {
                Name = dto.Name,
                Description = dto.Description,
                DailyPrice = dto.DailyPrice,
                WeeklyPrice = dto.WeeklyPrice,
                MonthlyPrice = dto.MonthlyPrice,
                CoverageAmount = dto.CoverageAmount,
                Deductible = dto.Deductible,
                IsIncludedByDefault = dto.IsIncludedByDefault,
                CoverageDetails = dto.CoverageDetails,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Insurance>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateInsuranceDTO dto)
        {
            var entity = await _unitOfWork.Repository<Insurance>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.DailyPrice = dto.DailyPrice;
            entity.WeeklyPrice = dto.WeeklyPrice;
            entity.MonthlyPrice = dto.MonthlyPrice;
            entity.CoverageAmount = dto.CoverageAmount;
            entity.Deductible = dto.Deductible;
            entity.IsIncludedByDefault = dto.IsIncludedByDefault;
            entity.CoverageDetails = dto.CoverageDetails;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Insurance>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Insurance>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Insurance>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> InsuranceExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Insurance>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<InsuranceResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Insurance>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<InsuranceResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Insurance>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<InsuranceResponseDTO>> GetDefaultInsurancesAsync()
        {
            var items = await _unitOfWork.Repository<Insurance>().FindManyAsync(x => x.IsIncludedByDefault && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InsuranceResponseDTO>> GetInsurancesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var items = await _unitOfWork.Repository<Insurance>().FindManyAsync(x => x.DailyPrice >= minPrice && x.DailyPrice <= maxPrice && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static InsuranceResponseDTO MapToResponseDTO(Insurance entity)
        {
            return new InsuranceResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DailyPrice = entity.DailyPrice,
                WeeklyPrice = entity.WeeklyPrice,
                MonthlyPrice = entity.MonthlyPrice,
                CoverageAmount = entity.CoverageAmount,
                Deductible = entity.Deductible,
                IsIncludedByDefault = entity.IsIncludedByDefault,
                CoverageDetails = entity.CoverageDetails,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
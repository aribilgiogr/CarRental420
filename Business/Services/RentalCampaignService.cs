using Core.Concretes.DTOs.RentalCampaign;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class RentalCampaignService : IRentalCampaignService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalCampaignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RentalCampaignResponseDTO> CreateAsync(CreateRentalCampaignDTO dto)
        {
            var entity = new RentalCampaign
            {
                RentalId = dto.RentalId,
                CampaignId = dto.CampaignId,
                DiscountAmount = dto.DiscountAmount,
                AppliedDate = dto.AppliedDate,
                AppliedBy = dto.AppliedBy,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<RentalCampaign>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateRentalCampaignDTO dto)
        {
            var entity = await _unitOfWork.Repository<RentalCampaign>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.RentalId = dto.RentalId;
            entity.CampaignId = dto.CampaignId;
            entity.DiscountAmount = dto.DiscountAmount;
            entity.AppliedDate = dto.AppliedDate;
            entity.AppliedBy = dto.AppliedBy;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<RentalCampaign>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<RentalCampaign>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<RentalCampaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<RentalCampaignResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<RentalCampaign>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<RentalCampaignResponseDTO>> GetByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<RentalCampaign>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<RentalCampaignResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<RentalCampaign>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<RentalCampaignResponseDTO>> GetByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<RentalCampaign>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<decimal> GetTotalDiscountByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<RentalCampaign>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Sum(x => x.DiscountAmount);
        }

        public async Task<bool> RelationshipExistsAsync(int rentalId, int campaignId)
        {
            return await _unitOfWork.Repository<RentalCampaign>().AnyAsync(x => x.RentalId == rentalId && x.CampaignId == campaignId && !x.Deleted);
        }

        public async Task DeleteByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<RentalCampaign>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            foreach (var item in items)
            {
                item.Deleted = true;
                item.Active = false;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<RentalCampaign>().UpdateManyAsync(items);
            await _unitOfWork.CommitAsync();
        }

        private static RentalCampaignResponseDTO MapToResponseDTO(RentalCampaign entity)
        {
            return new RentalCampaignResponseDTO
            {
                Id = entity.Id,
                RentalId = entity.RentalId,
                CampaignId = entity.CampaignId,
                DiscountAmount = entity.DiscountAmount,
                AppliedDate = entity.AppliedDate,
                AppliedBy = entity.AppliedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
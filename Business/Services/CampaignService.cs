using Core.Concretes.DTOs.Campaign;
using Core.Concretes.Entities;
using Core.Concretes.Enums;
using Core.Utils;

namespace Business.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampaignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CampaignResponseDTO> CreateAsync(CreateCampaignDTO dto)
        {
            var entity = new Campaign
            {
                Name = dto.Name,
                Description = dto.Description,
                CampaignType = dto.CampaignType,
                CampaignScope = dto.CampaignScope,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DiscountPercentage = dto.DiscountPercentage,
                DiscountFixedAmount = dto.DiscountFixedAmount,
                CouponCode = dto.CouponCode,
                MaxUsageCount = dto.MaxUsageCount,
                CurrentUsageCount = 0, 
                MaxUsagePerMember = dto.MaxUsagePerMember,
                MinimumRentalAmount = dto.MinimumRentalAmount,
                TargetedVehicleType = dto.TargetedVehicleType,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                Active = true,
                Deleted = false
            };

            await _unitOfWork.Repository<Campaign>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateCampaignDTO dto)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.CampaignType = dto.CampaignType;
            entity.CampaignScope = dto.CampaignScope;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.DiscountPercentage = dto.DiscountPercentage;
            entity.DiscountFixedAmount = dto.DiscountFixedAmount;
            entity.CouponCode = dto.CouponCode;
            entity.MaxUsageCount = dto.MaxUsageCount;
            entity.MaxUsagePerMember = dto.MaxUsagePerMember;
            entity.MinimumRentalAmount = dto.MinimumRentalAmount;
            entity.TargetedVehicleType = dto.TargetedVehicleType;
            entity.IsActive = dto.IsActive;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<CampaignResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetActiveCampaignsAsync()
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.IsActive);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetByTypeAsync(CampaignType campaignType)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CampaignType == campaignType);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetByScopeAsync(CampaignScope campaignScope)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CampaignScope == campaignScope);
            return items.Select(MapToResponseDTO);
        }

        public async Task<CampaignResponseDTO?> GetByCouponCodeAsync(string couponCode)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CouponCode == couponCode);
            var entity = items.FirstOrDefault();
            
            return entity != null ? MapToResponseDTO(entity) : null;
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetTimeBasedCampaignsAsync()
        {
            return await GetByTypeAsync(CampaignType.TimeLimited);
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetUnlimitedCampaignsAsync()
        {
            return await GetByTypeAsync(CampaignType.Unlimited);
        }

        public async Task<bool> CampaignExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Campaign>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<bool> CouponCodeExistsAsync(string couponCode)
        {
            return await _unitOfWork.Repository<Campaign>().AnyAsync(x => x.CouponCode == couponCode && !x.Deleted);
        }

        public async Task ActivateCampaignAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsActive = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeactivateCampaignAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<decimal> CalculateDiscountAsync(int campaignId, decimal rentalPrice)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (campaign == null || campaign.Deleted || !campaign.IsActive) return 0;

            decimal discount = 0;

            if (campaign.DiscountFixedAmount.HasValue && campaign.DiscountFixedAmount > 0)
            {
                discount += campaign.DiscountFixedAmount.Value;
            }
            
            if (campaign.DiscountPercentage > 0)
            {
                discount += (rentalPrice * campaign.DiscountPercentage) / 100;
            }

            return discount > rentalPrice ? rentalPrice : discount;
        }

        public async Task<bool> CanApplyCampaignAsync(int campaignId, string memberId, decimal rentalAmount)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            
            if (campaign == null || campaign.Deleted || !campaign.IsActive) 
                return false;

            if (campaign.MinimumRentalAmount.HasValue && rentalAmount < campaign.MinimumRentalAmount.Value) 
                return false;

            if (campaign.MaxUsageCount.HasValue && campaign.CurrentUsageCount >= campaign.MaxUsageCount.Value) 
                return false;

            if (campaign.CampaignType == CampaignType.TimeLimited)
            {
                var now = DateTime.UtcNow;
                if ((campaign.StartDate.HasValue && now < campaign.StartDate.Value) || 
                    (campaign.EndDate.HasValue && now > campaign.EndDate.Value))
                {
                    return false;
                }
            }

            // Üye bazlı kullanım sınırı için RentalCampaign geçmişi kontrolü gerekir.
            // Sadece bu entity tabanlı kısıtlamalar yukarıda yapılmıştır.
            return true;
        }

        public async Task IncrementUsageCountAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.CurrentUsageCount = (entity.CurrentUsageCount ?? 0) + 1;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<CampaignResponseDTO>> GetApplicableCampaignsAsync(decimal rentalAmount)
        {
            var now = DateTime.UtcNow;
            
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => 
                !x.Deleted && 
                x.IsActive && 
                (!x.MinimumRentalAmount.HasValue || x.MinimumRentalAmount <= rentalAmount) &&
                (!x.MaxUsageCount.HasValue || x.CurrentUsageCount < x.MaxUsageCount) &&
                (x.CampaignType != CampaignType.TimeLimited || ((!x.StartDate.HasValue || x.StartDate <= now) && (!x.EndDate.HasValue || x.EndDate >= now)))
            );

            return items.Select(MapToResponseDTO);
        }

        // Manuel Mapping Metodu
        private static CampaignResponseDTO MapToResponseDTO(Campaign entity)
        {
            return new CampaignResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CampaignType = entity.CampaignType,
                CampaignScope = entity.CampaignScope,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                DiscountPercentage = entity.DiscountPercentage,
                DiscountFixedAmount = entity.DiscountFixedAmount,
                CouponCode = entity.CouponCode,
                MaxUsageCount = entity.MaxUsageCount,
                CurrentUsageCount = entity.CurrentUsageCount,
                MaxUsagePerMember = entity.MaxUsagePerMember,
                MinimumRentalAmount = entity.MinimumRentalAmount,
                TargetedVehicleType = entity.TargetedVehicleType,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
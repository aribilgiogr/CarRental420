using Core.Concretes.DTOs.CampaignDetail;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class CampaignDetailService : ICampaignDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampaignDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CampaignDetailResponseDTO> CreateAsync(CreateCampaignDetailDTO dto)
        {
            var entity = new CampaignDetail
            {
                CampaignId = dto.CampaignId,
                DetailType = dto.DetailType,
                Content = dto.Content,
                DisplayOrder = dto.DisplayOrder,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CampaignDetail>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateCampaignDetailDTO dto)
        {
            var entity = await _unitOfWork.Repository<CampaignDetail>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.CampaignId = dto.CampaignId;
            entity.DetailType = dto.DetailType;
            entity.Content = dto.Content;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<CampaignDetail>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<CampaignDetail>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<CampaignDetail>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeleteByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            foreach (var item in items)
            {
                item.Deleted = true;
                item.Active = false;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<CampaignDetail>().UpdateManyAsync(items);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> DetailExistsAsync(int id)
        {
            return await _unitOfWork.Repository<CampaignDetail>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<CampaignDetailResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<CampaignDetailResponseDTO>> GetByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<CampaignDetailResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<CampaignDetail>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<CampaignDetailResponseDTO>> GetByTypeAsync(string detailType)
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => x.DetailType == detailType && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static CampaignDetailResponseDTO MapToResponseDTO(CampaignDetail entity)
        {
            return new CampaignDetailResponseDTO
            {
                Id = entity.Id,
                CampaignId = entity.CampaignId,
                DetailType = entity.DetailType,
                Content = entity.Content,
                DisplayOrder = entity.DisplayOrder,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
using AutoMapper;
using Core.Concretes.DTOs.CampaignDetail;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class CampaignDetailService : ICampaignDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CampaignDetailResponseDTO> CreateAsync(CreateCampaignDetailDTO dto)
        {
            var entity = _mapper.Map<CampaignDetail>(dto);
            await _unitOfWork.Repository<CampaignDetail>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CampaignDetailResponseDTO>(entity);
        }

        public async Task UpdateAsync(UpdateCampaignDetailDTO dto)
        {
            var entity = await _unitOfWork.Repository<CampaignDetail>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity = _mapper.Map<CampaignDetail>(dto);

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
            return _mapper.Map<IEnumerable<CampaignDetailResponseDTO>>(items);
        }

        public async Task<IEnumerable<CampaignDetailResponseDTO>> GetByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignDetailResponseDTO>>(items);
        }

        public async Task<CampaignDetailResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<CampaignDetail>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return _mapper.Map<CampaignDetailResponseDTO>(entity);
        }

        public async Task<IEnumerable<CampaignDetailResponseDTO>> GetByTypeAsync(string detailType)
        {
            var items = await _unitOfWork.Repository<CampaignDetail>().FindManyAsync(x => x.DetailType == detailType && !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignDetailResponseDTO>>(items);
        }
    }
}
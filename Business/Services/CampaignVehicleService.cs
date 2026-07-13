using AutoMapper;
using Core.Concretes.DTOs.CampaignVehicle;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class CampaignVehicleService : ICampaignVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CampaignVehicleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CampaignVehicleResponseDTO> CreateAsync(CreateCampaignVehicleDTO dto)
        {
            var entity = _mapper.Map<CampaignVehicle>(dto);

            await _unitOfWork.Repository<CampaignVehicle>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CampaignVehicleResponseDTO>(entity);
        }

        public async Task UpdateAsync(UpdateCampaignVehicleDTO dto)
        {
            var entity = await _unitOfWork.Repository<CampaignVehicle>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity = _mapper.Map<CampaignVehicle>(dto);

            await _unitOfWork.Repository<CampaignVehicle>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<CampaignVehicle>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<CampaignVehicle>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeleteByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<CampaignVehicle>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            foreach (var item in items)
            {
                item.Deleted = true;
                item.Active = false;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<CampaignVehicle>().UpdateManyAsync(items);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<CampaignVehicle>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            foreach (var item in items)
            {
                item.Deleted = true;
                item.Active = false;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<CampaignVehicle>().UpdateManyAsync(items);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CampaignVehicleResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<CampaignVehicle>().FindManyAsync(x => !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignVehicleResponseDTO>>(items);
        }

        public async Task<IEnumerable<CampaignVehicleResponseDTO>> GetByCampaignAsync(int campaignId)
        {
            var items = await _unitOfWork.Repository<CampaignVehicle>().FindManyAsync(x => x.CampaignId == campaignId && !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignVehicleResponseDTO>>(items);
        }

        public async Task<CampaignVehicleResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<CampaignVehicle>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return _mapper.Map<CampaignVehicleResponseDTO>(entity);
        }

        public async Task<IEnumerable<CampaignVehicleResponseDTO>> GetByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<CampaignVehicle>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignVehicleResponseDTO>>(items);
        }

        public async Task<bool> RelationshipExistsAsync(int campaignId, int vehicleId)
        {
            return await _unitOfWork.Repository<CampaignVehicle>().AnyAsync(x => x.CampaignId == campaignId && x.VehicleId == vehicleId && !x.Deleted);
        }
    }
}
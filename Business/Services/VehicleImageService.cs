using Core.Concretes.DTOs.VehicleImage;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class VehicleImageService : IVehicleImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleImageResponseDTO> CreateAsync(CreateVehicleImageDTO dto)
        {
            var entity = new VehicleImage
            {
                VehicleId = dto.VehicleId,
                ImageUrl = dto.ImageUrl,
                ImageName = dto.ImageName,
                IsMainImage = dto.IsMainImage,
                DisplayOrder = dto.DisplayOrder,
                Description = dto.Description,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<VehicleImage>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateVehicleImageDTO dto)
        {
            var entity = await _unitOfWork.Repository<VehicleImage>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.VehicleId = dto.VehicleId;
            entity.ImageUrl = dto.ImageUrl;
            entity.ImageName = dto.ImageName;
            entity.IsMainImage = dto.IsMainImage;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.Description = dto.Description;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<VehicleImage>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<VehicleImage>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<VehicleImage>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeleteByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<VehicleImage>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            foreach (var item in items)
            {
                item.Deleted = true;
                item.Active = false;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<VehicleImage>().UpdateManyAsync(items);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ImageExistsAsync(int id)
        {
            return await _unitOfWork.Repository<VehicleImage>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<VehicleImageResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<VehicleImage>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<VehicleImageResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<VehicleImage>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<VehicleImageResponseDTO>> GetByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<VehicleImage>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<VehicleImageResponseDTO?> GetMainImageByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<VehicleImage>().FindManyAsync(x => x.VehicleId == vehicleId && x.IsMainImage && !x.Deleted);
            var entity = items.FirstOrDefault();
            return entity != null ? MapToResponseDTO(entity) : null;
        }

        public async Task SetMainImageAsync(int imageId)
        {
            var targetImage = await _unitOfWork.Repository<VehicleImage>().FindByIdAsync(imageId);
            if (targetImage == null || targetImage.Deleted) return;

            var vehicleImages = await _unitOfWork.Repository<VehicleImage>().FindManyAsync(x => x.VehicleId == targetImage.VehicleId && !x.Deleted);
            
            foreach (var image in vehicleImages)
            {
                image.IsMainImage = (image.Id == imageId);
                image.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<VehicleImage>().UpdateManyAsync(vehicleImages);
            await _unitOfWork.CommitAsync();
        }

        private static VehicleImageResponseDTO MapToResponseDTO(VehicleImage entity)
        {
            return new VehicleImageResponseDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                ImageUrl = entity.ImageUrl,
                ImageName = entity.ImageName,
                IsMainImage = entity.IsMainImage,
                DisplayOrder = entity.DisplayOrder,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
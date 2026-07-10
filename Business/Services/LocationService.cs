using Core.Concretes.DTOs.Location;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LocationResponseDTO> CreateAsync(CreateLocationDTO dto)
        {
            var entity = new Location
            {
                Name = dto.Name,
                City = dto.City,
                District = dto.District,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Manager = dto.Manager,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
                IsOpen24Hours = dto.IsOpen24Hours,
                Notes = dto.Notes,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Location>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateLocationDTO dto)
        {
            var entity = await _unitOfWork.Repository<Location>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.Name = dto.Name;
            entity.City = dto.City;
            entity.District = dto.District;
            entity.Street = dto.Street;
            entity.PostalCode = dto.PostalCode;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.Manager = dto.Manager;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.OpeningTime = dto.OpeningTime;
            entity.ClosingTime = dto.ClosingTime;
            entity.IsOpen24Hours = dto.IsOpen24Hours;
            entity.Notes = dto.Notes;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Location>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Location>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Location>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> LocationExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Location>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Location>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<LocationResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Location>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetByCityAsync(string city)
        {
            var items = await _unitOfWork.Repository<Location>().FindManyAsync(x => x.City == city && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<LocationResponseDTO>> Get24HourLocationsAsync()
        {
            var items = await _unitOfWork.Repository<Location>().FindManyAsync(x => x.IsOpen24Hours && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetOpenLocationsAsync()
        {
            var nowTime = DateTime.UtcNow.TimeOfDay;
            var items = await _unitOfWork.Repository<Location>().FindManyAsync(x => 
                !x.Deleted && 
                (x.IsOpen24Hours || (nowTime >= x.OpeningTime && nowTime <= x.ClosingTime))
            );
            return items.Select(MapToResponseDTO);
        }

        public async Task<bool> IsOpenAsync(int locationId, DateTime dateTime)
        {
            var entity = await _unitOfWork.Repository<Location>().FindByIdAsync(locationId);
            if (entity == null || entity.Deleted) return false;
            
            if (entity.IsOpen24Hours) return true;

            var time = dateTime.TimeOfDay;
            return time >= entity.OpeningTime && time <= entity.ClosingTime;
        }

        public Task<IEnumerable<LocationResponseDTO>> GetLocationsByDistanceAsync(double latitude, double longitude, double radiusInKm)
        {
            // Veritabanı tarafında koordinat mesafesi hesaplama yetenekleri (Geography/Spatial türleri vs.) bilinmediği için varsayım yapılmamıştır.
            throw new NotImplementedException();
        }

        private static LocationResponseDTO MapToResponseDTO(Location entity)
        {
            return new LocationResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
                District = entity.District,
                Street = entity.Street,
                PostalCode = entity.PostalCode,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                Manager = entity.Manager,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                OpeningTime = entity.OpeningTime,
                ClosingTime = entity.ClosingTime,
                IsOpen24Hours = entity.IsOpen24Hours,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
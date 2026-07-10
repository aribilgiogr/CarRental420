using Core.Concretes.DTOs.Vehicle;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleResponseDTO> CreateAsync(CreateVehicleDTO dto)
        {
            var entity = new Vehicle
            {
                LicensePlate = dto.LicensePlate,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                Kilometers = dto.Kilometers,
                PricePerDay = dto.PricePerDay,
                VehicleType = dto.VehicleType,
                VehicleCategoryId = dto.VehicleCategoryId,
                VIN = dto.VIN,
                EngineNumber = dto.EngineNumber,
                Color = dto.Color,
                Seats = dto.Seats,
                FuelTankCapacity = dto.FuelTankCapacity,
                FuelType = dto.FuelType,
                TransmissionType = dto.TransmissionType,
                HasInsurance = dto.HasInsurance,
                InsuranceExpiryDate = dto.InsuranceExpiryDate,
                IsAvailable = dto.IsAvailable,
                RequiresInspection = dto.RequiresInspection,
                Notes = dto.Notes,
                LocationId = dto.LocationId,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Vehicle>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateVehicleDTO dto)
        {
            var entity = await _unitOfWork.Repository<Vehicle>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.LicensePlate = dto.LicensePlate;
            entity.Brand = dto.Brand;
            entity.Model = dto.Model;
            entity.Year = dto.Year;
            entity.Kilometers = dto.Kilometers;
            entity.PricePerDay = dto.PricePerDay;
            entity.VehicleType = dto.VehicleType;
            entity.VehicleCategoryId = dto.VehicleCategoryId;
            entity.VIN = dto.VIN;
            entity.EngineNumber = dto.EngineNumber;
            entity.Color = dto.Color;
            entity.Seats = dto.Seats;
            entity.FuelTankCapacity = dto.FuelTankCapacity;
            entity.FuelType = dto.FuelType;
            entity.TransmissionType = dto.TransmissionType;
            entity.HasInsurance = dto.HasInsurance;
            entity.InsuranceExpiryDate = dto.InsuranceExpiryDate;
            entity.IsAvailable = dto.IsAvailable;
            entity.RequiresInspection = dto.RequiresInspection;
            entity.Notes = dto.Notes;
            entity.LocationId = dto.LocationId;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Vehicle>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Vehicle>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.IsAvailable = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Vehicle>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> VehicleExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Vehicle>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Vehicle>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<VehicleResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Vehicle>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetByLocationAsync(int locationId)
        {
            var items = await _unitOfWork.Repository<Vehicle>().FindManyAsync(x => x.LocationId == locationId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetByCategoryAsync(int categoryId)
        {
            var items = await _unitOfWork.Repository<Vehicle>().FindManyAsync(x => x.VehicleCategoryId == categoryId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetAvailableVehiclesAsync()
        {
            var items = await _unitOfWork.Repository<Vehicle>().FindManyAsync(x => x.IsAvailable && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetVehiclesNeedingInspectionAsync()
        {
            var items = await _unitOfWork.Repository<Vehicle>().FindManyAsync(x => x.RequiresInspection && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task UpdateKilometersAsync(int vehicleId, int newKilometers)
        {
            var entity = await _unitOfWork.Repository<Vehicle>().FindByIdAsync(vehicleId);
            if (entity != null && !entity.Deleted)
            {
                entity.Kilometers = newKilometers;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Vehicle>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SetAvailabilityAsync(int vehicleId, bool isAvailable)
        {
            var entity = await _unitOfWork.Repository<Vehicle>().FindByIdAsync(vehicleId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsAvailable = isAvailable;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Vehicle>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        private static VehicleResponseDTO MapToResponseDTO(Vehicle entity)
        {
            return new VehicleResponseDTO
            {
                Id = entity.Id,
                LicensePlate = entity.LicensePlate,
                Brand = entity.Brand,
                Model = entity.Model,
                Year = entity.Year,
                Kilometers = entity.Kilometers,
                PricePerDay = entity.PricePerDay,
                VehicleType = entity.VehicleType,
                VehicleCategoryId = entity.VehicleCategoryId,
                VIN = entity.VIN,
                EngineNumber = entity.EngineNumber,
                Color = entity.Color,
                Seats = entity.Seats,
                FuelTankCapacity = entity.FuelTankCapacity,
                FuelType = entity.FuelType,
                TransmissionType = entity.TransmissionType,
                HasInsurance = entity.HasInsurance,
                InsuranceExpiryDate = entity.InsuranceExpiryDate,
                IsAvailable = entity.IsAvailable,
                RequiresInspection = entity.RequiresInspection,
                Notes = entity.Notes,
                LocationId = entity.LocationId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
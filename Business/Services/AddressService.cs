using Core.Concretes.DTOs.Address;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressResponseDTO> CreateAsync(CreateAddressDTO dto)
        {
            var entity = new Address
            {
                MemberId = dto.MemberId,
                Title = dto.Title,
                Street = dto.Street,
                City = dto.City,
                District = dto.District,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                PhoneNumber = dto.PhoneNumber,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsDefault = dto.IsDefault,
                Notes = dto.Notes,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Address>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Address>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Address>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> AddressExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Address>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<AddressResponseDTO>> GetAllAsync()
        {
            var addresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => !x.Deleted);
            return addresses.Select(MapToResponseDTO);
        }

        public async Task<AddressResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Address>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<AddressResponseDTO>> GetByMemberAsync(string memberId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return addresses.Select(MapToResponseDTO);
        }

        public async Task<AddressResponseDTO?> GetDefaultAddressAsync(string memberId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => x.MemberId == memberId && x.IsDefault && !x.Deleted);
            var defaultAddress = addresses.FirstOrDefault();
            
            if (defaultAddress == null) return null;
            return MapToResponseDTO(defaultAddress);
        }

        public async Task<IEnumerable<AddressResponseDTO>> GetMemberAddressesAsync(string memberId)
        {
            return await GetByMemberAsync(memberId);
        }

        public async Task SetDefaultAddressAsync(int addressId)
        {
            var targetAddress = await _unitOfWork.Repository<Address>().FindByIdAsync(addressId);
            if (targetAddress == null || targetAddress.Deleted) return;

            var memberAddresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => x.MemberId == targetAddress.MemberId && !x.Deleted);
            
            foreach (var address in memberAddresses)
            {
                address.IsDefault = (address.Id == addressId);
                address.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Repository<Address>().UpdateManyAsync(memberAddresses);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(UpdateAddressDTO dto)
        {
            var entity = await _unitOfWork.Repository<Address>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.Title = dto.Title;
            entity.Street = dto.Street;
            entity.City = dto.City;
            entity.District = dto.District;
            entity.PostalCode = dto.PostalCode;
            entity.Country = dto.Country;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.IsDefault = dto.IsDefault;
            entity.Notes = dto.Notes;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Address>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        // Manuel Mapping Metodu (Kütüphane bağımlılığını önlemek için)
        private static AddressResponseDTO MapToResponseDTO(Address entity)
        {
            return new AddressResponseDTO
            {
                Id = entity.Id,
                MemberId = entity.MemberId,
                Title = entity.Title,
                Street = entity.Street,
                City = entity.City,
                District = entity.District,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
                PhoneNumber = entity.PhoneNumber,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                IsDefault = entity.IsDefault,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
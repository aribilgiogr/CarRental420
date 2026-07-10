using AutoMapper;
using Core.Concretes.DTOs.Address;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddressResponseDTO> CreateAsync(CreateAddressDTO dto)
        {
            var entity = _mapper.Map<Address>(dto);
            await _unitOfWork.Repository<Address>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AddressResponseDTO>(entity);
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
            return _mapper.Map<IEnumerable<AddressResponseDTO>>(addresses);
        }

        public async Task<AddressResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Address>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return _mapper.Map<AddressResponseDTO>(entity);
        }

        public async Task<IEnumerable<AddressResponseDTO>> GetByMemberAsync(string memberId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return _mapper.Map<IEnumerable<AddressResponseDTO>>(addresses);
        }

        public async Task<AddressResponseDTO?> GetDefaultAddressAsync(string memberId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindManyAsync(x => x.MemberId == memberId && x.IsDefault && !x.Deleted);
            var defaultAddress = addresses.FirstOrDefault();

            if (defaultAddress == null) return null;
            return _mapper.Map<AddressResponseDTO>(defaultAddress);
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

            entity = _mapper.Map<Address>(dto);
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Address>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
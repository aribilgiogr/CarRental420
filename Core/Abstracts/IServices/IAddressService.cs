using Core.Concretes.DTOs.Address;

namespace Business.Services
{
    public interface IAddressService
    {
        Task<AddressResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<AddressResponseDTO>> GetAllAsync();
        Task<IEnumerable<AddressResponseDTO>> GetByMemberAsync(string memberId);
        Task<AddressResponseDTO?> GetDefaultAddressAsync(string memberId);
        Task<AddressResponseDTO> CreateAsync(CreateAddressDTO dto);
        Task UpdateAsync(UpdateAddressDTO dto);
        Task DeleteAsync(int id);
        Task<bool> AddressExistsAsync(int id);
        Task SetDefaultAddressAsync(int addressId);
        Task<IEnumerable<AddressResponseDTO>> GetMemberAddressesAsync(string memberId);
    }
}

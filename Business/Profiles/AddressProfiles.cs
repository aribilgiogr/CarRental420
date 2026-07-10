using AutoMapper;
using Core.Concretes.DTOs.Address;
using Core.Concretes.Entities;

namespace Business.Profiles
{
    public class AddressProfiles : Profile
    {
        public AddressProfiles()
        {
            CreateMap<CreateAddressDTO, Address>();
            CreateMap<UpdateAddressDTO, Address>();

            CreateMap<Address, AddressResponseDTO>();
        }
    }
}

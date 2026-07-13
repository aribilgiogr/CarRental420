using AutoMapper;
using Core.Concretes.DTOs.Address;
using Core.Concretes.DTOs.Campaign;
using Core.Concretes.DTOs.CampaignDetail;
using Core.Concretes.DTOs.CampaignVehicle;
using Core.Concretes.DTOs.Document;
using Core.Concretes.DTOs.Insurance;
using Core.Concretes.DTOs.Invoice;
using Core.Concretes.DTOs.Location;
using Core.Concretes.DTOs.MaintenanceRecord;
using Core.Concretes.DTOs.Member;
using Core.Concretes.DTOs.Payment;
using Core.Concretes.DTOs.Penalty;
using Core.Concretes.DTOs.RentalCampaign;
using Core.Concretes.DTOs.Rental;
using Core.Concretes.DTOs.Review;
using Core.Concretes.DTOs.VehicleCategory;
using Core.Concretes.DTOs.VehicleImage;
using Core.Concretes.DTOs.Vehicle;
using Core.Concretes.Entities;

namespace Business.Profiles
{
    public class GeneralProfiles : Profile
    {
        public GeneralProfiles()
        {
            ConfigureMapping<Address, CreateAddressDTO, UpdateAddressDTO, AddressResponseDTO>();
            ConfigureMapping<CampaignDetail, CreateCampaignDetailDTO, UpdateCampaignDetailDTO, CampaignDetailResponseDTO>();
            ConfigureMapping<Campaign, CreateCampaignDTO, UpdateCampaignDTO, CampaignResponseDTO>();
            ConfigureMapping<CampaignVehicle, CreateCampaignVehicleDTO, UpdateCampaignVehicleDTO, CampaignVehicleResponseDTO>();
            ConfigureMapping<Document, CreateDocumentDTO, UpdateDocumentDTO, DocumentResponseDTO>();
            ConfigureMapping<Insurance, CreateInsuranceDTO, UpdateInsuranceDTO, InsuranceResponseDTO>();
            ConfigureMapping<Invoice, CreateInvoiceDTO, UpdateInvoiceDTO, InvoiceResponseDTO>();
            ConfigureMapping<Location, CreateLocationDTO, UpdateLocationDTO, LocationResponseDTO>();
            ConfigureMapping<MaintenanceRecord, CreateMaintenanceRecordDTO, UpdateMaintenanceRecordDTO, MaintenanceRecordResponseDTO>();
            ConfigureMapping<Member, CreateMemberDTO, UpdateMemberDTO, MemberResponseDTO>();
            ConfigureMapping<Payment, CreatePaymentDTO, UpdatePaymentDTO, PaymentResponseDTO>();
            ConfigureMapping<Penalty, CreatePenaltyDTO, UpdatePenaltyDTO, PenaltyResponseDTO>();
            ConfigureMapping<RentalCampaign, CreateRentalCampaignDTO, UpdateRentalCampaignDTO, RentalCampaignResponseDTO>();
            ConfigureMapping<Rental, CreateRentalDTO, UpdateRentalDTO, RentalResponseDTO>();
            ConfigureMapping<Review, CreateReviewDTO, UpdateReviewDTO, ReviewResponseDTO>();
            ConfigureMapping<VehicleCategory, CreateVehicleCategoryDTO, UpdateVehicleCategoryDTO, VehicleCategoryResponseDTO>();
            ConfigureMapping<VehicleImage, CreateVehicleImageDTO, UpdateVehicleImageDTO, VehicleImageResponseDTO>();
            ConfigureMapping<Vehicle, CreateVehicleDTO, UpdateVehicleDTO, VehicleResponseDTO>();
        }

        private void ConfigureMapping<TEntity, TCreateDTO, TUpdateDTO, TResponseDTO>()
        {
            CreateMap<TCreateDTO, TEntity>();
            CreateMap<TUpdateDTO, TEntity>()
                .ForMember("UpdatedAt", opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<TEntity, TResponseDTO>().ReverseMap();
        }
    }
}
using System;
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
            // Address
            CreateMap<CreateAddressDTO, Address>();
            CreateMap<UpdateAddressDTO, Address>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Address, AddressResponseDTO>();

            // CampaignDetail
            CreateMap<CreateCampaignDetailDTO, CampaignDetail>();
            CreateMap<UpdateCampaignDetailDTO, CampaignDetail>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CampaignDetail, CampaignDetailResponseDTO>();

            // Campaign
            CreateMap<CreateCampaignDTO, Campaign>();
            CreateMap<UpdateCampaignDTO, Campaign>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Campaign, CampaignResponseDTO>();

            // CampaignVehicle
            CreateMap<CreateCampaignVehicleDTO, CampaignVehicle>();
            CreateMap<UpdateCampaignVehicleDTO, CampaignVehicle>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CampaignVehicle, CampaignVehicleResponseDTO>();

            // Document
            CreateMap<CreateDocumentDTO, Document>();
            CreateMap<UpdateDocumentDTO, Document>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Document, DocumentResponseDTO>();

            // Insurance
            CreateMap<CreateInsuranceDTO, Insurance>();
            CreateMap<UpdateInsuranceDTO, Insurance>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Insurance, InsuranceResponseDTO>();

            // Invoice
            CreateMap<CreateInvoiceDTO, Invoice>();
            CreateMap<UpdateInvoiceDTO, Invoice>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Invoice, InvoiceResponseDTO>();

            // Location
            CreateMap<CreateLocationDTO, Location>();
            CreateMap<UpdateLocationDTO, Location>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Location, LocationResponseDTO>();

            // MaintenanceRecord
            CreateMap<CreateMaintenanceRecordDTO, MaintenanceRecord>();
            CreateMap<UpdateMaintenanceRecordDTO, MaintenanceRecord>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<MaintenanceRecord, MaintenanceRecordResponseDTO>();

            // Member
            CreateMap<CreateMemberDTO, Member>();
            CreateMap<UpdateMemberDTO, Member>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Member, MemberResponseDTO>();

            // Payment
            CreateMap<CreatePaymentDTO, Payment>();
            CreateMap<UpdatePaymentDTO, Payment>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Payment, PaymentResponseDTO>();

            // Penalty
            CreateMap<CreatePenaltyDTO, Penalty>();
            CreateMap<UpdatePenaltyDTO, Penalty>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Penalty, PenaltyResponseDTO>();

            // RentalCampaign
            CreateMap<CreateRentalCampaignDTO, RentalCampaign>();
            CreateMap<UpdateRentalCampaignDTO, RentalCampaign>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<RentalCampaign, RentalCampaignResponseDTO>();

            // Rental
            CreateMap<CreateRentalDTO, Rental>();
            CreateMap<UpdateRentalDTO, Rental>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Rental, RentalResponseDTO>();

            // Review
            CreateMap<CreateReviewDTO, Review>();
            CreateMap<UpdateReviewDTO, Review>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Review, ReviewResponseDTO>();

            // VehicleCategory
            CreateMap<CreateVehicleCategoryDTO, VehicleCategory>();
            CreateMap<UpdateVehicleCategoryDTO, VehicleCategory>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<VehicleCategory, VehicleCategoryResponseDTO>();

            // VehicleImage
            CreateMap<CreateVehicleImageDTO, VehicleImage>();
            CreateMap<UpdateVehicleImageDTO, VehicleImage>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<VehicleImage, VehicleImageResponseDTO>();

            // Vehicle
            CreateMap<CreateVehicleDTO, Vehicle>();
            CreateMap<UpdateVehicleDTO, Vehicle>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Vehicle, VehicleResponseDTO>();
        }
    }
}
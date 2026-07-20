using Business.Profiles;
using Business.Services;
using CarRental420.Data.Contexts;
using Core.Concretes.Entities;
using Core.Utils;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class IoC
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(configuration.GetConnectionString("app_db")));

            services.AddIdentity<Member, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Profil, Yönetim ve Kimlik Doğrulama Servisleri
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAuthService, AuthService>();

            // Diğer İş Servisleri
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICampaignDetailService, CampaignDetailService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignVehicleService, CampaignVehicleService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IMaintenanceRecordService, MaintenanceRecordService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPenaltyService, PenaltyService>();
            services.AddScoped<IRentalCampaignService, RentalCampaignService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
            services.AddScoped<IVehicleImageService, VehicleImageService>();
            services.AddScoped<IVehicleService, VehicleService>();

            // AutoMapper Entegrasyonu (Eğer yoksa projeye eklenmesi gerekir)
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<GeneralProfiles>();
            });

            return services;
        }
    }
}
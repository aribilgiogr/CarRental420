using Business.Services;
using CarRental420.Data.Contexts;
using Core.Concretes.Entities;
using Core.Utils;
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

            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICampaignDetailService, CampaignDetailService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignVehicleService, CampaignVehicleService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IMaintenanceRecordService, MaintenanceRecordService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped<IPenaltyService, PenaltyService>();

            return services;
        }
    }
}

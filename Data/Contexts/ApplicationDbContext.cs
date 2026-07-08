using Core.Concretes.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental420.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<Member, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region DbSets - Vehicles & Categories
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        #endregion

        #region DbSets - Locations
        public DbSet<Location> Locations { get; set; }
        #endregion

        #region DbSets - Rentals & Payments
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        #endregion

        #region DbSets - Insurance
        public DbSet<Insurance> Insurances { get; set; }
        #endregion

        #region DbSets - Member Related
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        #endregion

        #region DbSets - Maintenance
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        #endregion

        #region DbSets - Campaigns
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignDetail> CampaignDetails { get; set; }
        public DbSet<CampaignVehicle> CampaignVehicles { get; set; }
        public DbSet<RentalCampaign> RentalCampaigns { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Vehicle Configurations
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.LicensePlate)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VIN)
                    .HasMaxLength(50);

                entity.Property(e => e.EngineNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.Color)
                    .HasMaxLength(50);

                entity.Property(e => e.FuelType)
                    .HasMaxLength(50);

                entity.Property(e => e.TransmissionType)
                    .HasMaxLength(50);

                entity.Property(e => e.PricePerDay)
                    .HasPrecision(10, 2);

                entity.HasOne(e => e.VehicleCategory)
                    .WithMany(vc => vc.Vehicles)
                    .HasForeignKey(e => e.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Location)
                    .WithMany(l => l.Vehicles)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.LicensePlate)
                    .IsUnique();

                entity.HasIndex(e => e.VIN)
                    .IsUnique();
            });

            modelBuilder.Entity<VehicleCategory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DailyPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.WeeklyPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.MonthlyPrice)
                    .HasPrecision(10, 2);
            });

            modelBuilder.Entity<VehicleImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ImageUrl)
                    .IsRequired();

                entity.Property(e => e.ImageName)
                    .HasMaxLength(255);

                entity.HasOne(e => e.Vehicle)
                    .WithMany(v => v.VehicleImages)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Location Configurations
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(100);

                entity.Property(e => e.Manager)
                    .HasMaxLength(100);
            });
            #endregion

            #region Rental Configurations
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.RentalStatus)
                    .HasMaxLength(50);

                entity.Property(e => e.TotalPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.DiscountAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.FinalPrice)
                    .HasPrecision(10, 2);

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Rentals)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vehicle)
                    .WithMany(v => v.Rentals)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StartLocation)
                    .WithMany(l => l.RentalsStart)
                    .HasForeignKey(e => e.StartLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EndLocation)
                    .WithMany(l => l.RentalsEnd)
                    .HasForeignKey(e => e.EndLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Insurance)
                    .WithMany(i => i.Rentals)
                    .HasForeignKey(e => e.InsuranceId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.MemberId);
                entity.HasIndex(e => e.VehicleId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionNumber)
                    .HasMaxLength(100);

                entity.Property(e => e.CardLastFourDigits)
                    .HasMaxLength(4);

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50);

                entity.HasOne(e => e.Rental)
                    .WithMany(r => r.Payments)
                    .HasForeignKey(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.RentalId);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.SubTotal)
                    .HasPrecision(10, 2);

                entity.Property(e => e.TaxAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.DiscountAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.InvoiceStatus)
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50);

                entity.HasOne(e => e.Rental)
                    .WithMany()
                    .HasForeignKey(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Member)
                    .WithMany()
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.InvoiceNumber)
                    .IsUnique();
                entity.HasIndex(e => e.MemberId);
            });
            #endregion

            #region Insurance Configurations
            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DailyPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.WeeklyPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.MonthlyPrice)
                    .HasPrecision(10, 2);

                entity.Property(e => e.CoverageAmount)
                    .HasPrecision(10, 2);
            });
            #endregion

            #region Member Related Configurations
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Addresses)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.MemberId);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DocumentType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.DocumentPath)
                    .HasMaxLength(500);

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(450);

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Documents)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.MemberId);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Comment)
                    .IsRequired();

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Reviews)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vehicle)
                    .WithMany()
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.MemberId);
            });

            modelBuilder.Entity<Penalty>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.PenaltyType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.PenaltyAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.PenaltyStatus)
                    .HasMaxLength(50);

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(450);

                entity.Property(e => e.PhotosPath)
                    .HasMaxLength(500);

                entity.HasOne(e => e.Rental)
                    .WithMany(r => r.Penalties)
                    .HasForeignKey(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Member)
                    .WithMany(m => m.Penalties)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vehicle)
                    .WithMany()
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MemberId);
                entity.HasIndex(e => e.RentalId);
            });
            #endregion

            #region Maintenance Configurations
            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MaintenanceType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.Cost)
                    .HasPrecision(10, 2);

                entity.Property(e => e.ServiceProvider)
                    .HasMaxLength(100);

                entity.Property(e => e.MaintenanceStatus)
                    .HasMaxLength(50);

                entity.Property(e => e.Attachments)
                    .HasMaxLength(500);

                entity.HasOne(e => e.Vehicle)
                    .WithMany(v => v.MaintenanceRecords)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.VehicleId);
            });
            #endregion

            #region Campaign Configurations
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DiscountPercentage)
                    .HasPrecision(5, 2);

                entity.Property(e => e.DiscountFixedAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.CouponCode)
                    .HasMaxLength(50);

                entity.Property(e => e.MinimumRentalAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.TargetedVehicleType)
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(450);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(450);

                entity.HasIndex(e => e.CouponCode)
                    .IsUnique();
            });

            modelBuilder.Entity<CampaignDetail>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DetailType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.HasOne(e => e.Campaign)
                    .WithMany(c => c.CampaignDetails)
                    .HasForeignKey(e => e.CampaignId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CampaignId);
            });

            modelBuilder.Entity<CampaignVehicle>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Campaign)
                    .WithMany(c => c.CampaignVehicles)
                    .HasForeignKey(e => e.CampaignId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Vehicle)
                    .WithMany(v => v.CampaignVehicles)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.CampaignId, e.VehicleId })
                    .IsUnique();
            });

            modelBuilder.Entity<RentalCampaign>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DiscountAmount)
                    .HasPrecision(10, 2);

                entity.Property(e => e.AppliedBy)
                    .HasMaxLength(450);

                entity.HasOne(e => e.Rental)
                    .WithMany(r => r.RentalCampaigns)
                    .HasForeignKey(e => e.RentalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Campaign)
                    .WithMany(c => c.RentalCampaigns)
                    .HasForeignKey(e => e.CampaignId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.RentalId);
            });
            #endregion

            #region Member Entity Configuration
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber2)
                    .HasMaxLength(20);

                entity.Property(e => e.DriverLicenseNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.NationalIdNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.ProfileImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.BlacklistReason)
                    .HasMaxLength(500);

                entity.HasIndex(e => e.NationalIdNumber)
                    .IsUnique();

                entity.HasIndex(e => e.DriverLicenseNumber)
                    .IsUnique();
            });
            #endregion
        }
    }
}
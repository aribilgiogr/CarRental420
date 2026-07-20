using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Rental : BaseEntity
    {
        public string MemberId { get; set; }
        public int VehicleId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public int StartOdometer { get; set; }
        public int? EndOdometer { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? RentalStatus { get; set; } // Aktif, Tamamlandı, İptal, Gecikmeli
        public bool IsReturned { get; set; } = false;
        public bool HasDamage { get; set; } = false;
        public string? DamageDescription { get; set; }
        public string? Notes { get; set; }
        public int RentalDays { get; set; }

        // Foreign Keys
        public int? InsuranceId { get; set; }

        // Navigation Properties
        public virtual Member Member { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Location StartLocation { get; set; }
        public virtual Location EndLocation { get; set; }
        public virtual Insurance Insurance { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<RentalCampaign> RentalCampaigns { get; set; } = new List<RentalCampaign>();
        public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
    }
}

using Core.Abstracts.Bases;
using Core.Concretes.Enums;

namespace Core.Concretes.Entities
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public CampaignType CampaignType { get; set; } // Süreli, Süresiz, KuponKodu
        public CampaignScope CampaignScope { get; set; } // TekAraç, ÇoğulAraç, TümAraçlar
        public DateTime? StartDate { get; set; } // Süreli kampanyalar için
        public DateTime? EndDate { get; set; } // Süreli kampanyalar için
        public decimal DiscountPercentage { get; set; }
        public decimal? DiscountFixedAmount { get; set; }
        public string? CouponCode { get; set; } // Kupon kampanyaları için
        public int? MaxUsageCount { get; set; } // Kaç kez kullanılabilir
        public int? CurrentUsageCount { get; set; } // Şu ana kadar kaç kez kullanıldı
        public int? MaxUsagePerMember { get; set; } // Üye başına max kullanım
        public decimal? MinimumRentalAmount { get; set; } // Minimum kiralama tutarı
        public string? TargetedVehicleType { get; set; } // Belirli araç türü için (isteğe bağlı)
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<CampaignDetail> CampaignDetails { get; set; } = new List<CampaignDetail>();
        public virtual ICollection<CampaignVehicle> CampaignVehicles { get; set; } = new List<CampaignVehicle>();
        public virtual ICollection<RentalCampaign> RentalCampaigns { get; set; } = new List<RentalCampaign>();
    }
}

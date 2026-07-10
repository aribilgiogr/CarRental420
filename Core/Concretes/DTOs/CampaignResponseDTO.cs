using Core.Concretes.Enums;

namespace Core.Concretes.DTOs.Campaign
{
    public class CampaignResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public CampaignType CampaignType { get; set; } 
        public CampaignScope CampaignScope { get; set; } 
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public decimal DiscountPercentage { get; set; }
        public decimal? DiscountFixedAmount { get; set; }
        public string? CouponCode { get; set; } 
        public int? MaxUsageCount { get; set; } 
        public int? CurrentUsageCount { get; set; } 
        public int? MaxUsagePerMember { get; set; } 
        public decimal? MinimumRentalAmount { get; set; } 
        public string? TargetedVehicleType { get; set; } 
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
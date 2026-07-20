using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class RentalCampaign : BaseEntity
    {
        public int RentalId { get; set; }
        public int CampaignId { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        public string? AppliedBy { get; set; }

        // Navigation Properties
        public virtual Rental Rental { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}

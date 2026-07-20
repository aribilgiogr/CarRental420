namespace Core.Concretes.DTOs.RentalCampaign
{
    public class RentalCampaignResponseDTO
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int CampaignId { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? AppliedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
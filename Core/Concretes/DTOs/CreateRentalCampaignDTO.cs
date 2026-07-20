namespace Core.Concretes.DTOs.RentalCampaign
{
    public class CreateRentalCampaignDTO
    {
        public int RentalId { get; set; }
        public int CampaignId { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? AppliedBy { get; set; }
    }
}
namespace Core.Concretes.DTOs.CampaignDetail
{
    public class CreateCampaignDetailDTO
    {
        public int CampaignId { get; set; }
        public string DetailType { get; set; } 
        public string Content { get; set; }
        public int DisplayOrder { get; set; }
    }
}
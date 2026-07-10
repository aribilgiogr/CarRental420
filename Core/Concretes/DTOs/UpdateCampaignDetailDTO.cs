namespace Core.Concretes.DTOs.CampaignDetail
{
    public class UpdateCampaignDetailDTO
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string DetailType { get; set; } 
        public string Content { get; set; }
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }
    }
}
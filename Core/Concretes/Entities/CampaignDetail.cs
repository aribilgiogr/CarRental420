using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class CampaignDetail : BaseEntity
    {
        public int CampaignId { get; set; }
        public string DetailType { get; set; } // Açıklama, Koşul, Uyarı vb.
        public string Content { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation Properties
        public virtual Campaign Campaign { get; set; }
    }
}

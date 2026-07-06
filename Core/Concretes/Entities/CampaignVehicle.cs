using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class CampaignVehicle : BaseEntity
    {
        public int CampaignId { get; set; }
        public int VehicleId { get; set; }

        // Navigation Properties
        public virtual Campaign Campaign { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}

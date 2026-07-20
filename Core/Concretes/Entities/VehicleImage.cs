using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class VehicleImage : BaseEntity
    {
        public int VehicleId { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public bool IsMainImage { get; set; } = false;
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; }
    }
}

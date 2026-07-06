using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Penalty : BaseEntity
    {
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public int VehicleId { get; set; }
        public string PenaltyType { get; set; } // Hasar, Geç İade, Yakıt Eksikliği, Diğer
        public string Description { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string? PenaltyStatus { get; set; } // Beklemede, Onaylandı, Ödendi, Kaldırıldı
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PhotosPath { get; set; } // Hasar fotoğrafları
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Rental Rental { get; set; }
        public virtual Member Member { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}

using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Review : BaseEntity
    {
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Title { get; set; }
        public string Comment { get; set; }
        public int? VehicleRating { get; set; } // Araç durumu
        public int? ServiceRating { get; set; } // Hizmet kalitesi
        public int? CleanlinessRating { get; set; } // Temizlik
        public int? StaffRating { get; set; } // Personel
        public bool IsVerified { get; set; } = false; // Gerçek kiralama sonucu
        public bool IsPublished { get; set; } = true;
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int? VehicleId { get; set; }

        // Navigation Properties
        public virtual Member Member { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}

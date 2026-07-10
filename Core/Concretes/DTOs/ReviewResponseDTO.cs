namespace Core.Concretes.DTOs.Review
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public int Rating { get; set; } 
        public string Title { get; set; }
        public string Comment { get; set; }
        public int? VehicleRating { get; set; } 
        public int? ServiceRating { get; set; } 
        public int? CleanlinessRating { get; set; } 
        public int? StaffRating { get; set; } 
        public bool IsVerified { get; set; } 
        public bool IsPublished { get; set; } 
        public DateTime ReviewDate { get; set; }
        public int? VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
namespace Core.Concretes.DTOs.Penalty
{
    public class CreatePenaltyDTO
    {
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public int VehicleId { get; set; }
        public string PenaltyType { get; set; } 
        public string Description { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string? PenaltyStatus { get; set; } 
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PhotosPath { get; set; }
        public string? Notes { get; set; }
    }
}
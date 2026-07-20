namespace Core.Concretes.DTOs.Rental
{
    public class CreateRentalDTO
    {
        public string MemberId { get; set; }
        public int VehicleId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public int StartOdometer { get; set; }
        public int? EndOdometer { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? RentalStatus { get; set; } 
        public bool IsReturned { get; set; }
        public bool HasDamage { get; set; }
        public string? DamageDescription { get; set; }
        public string? Notes { get; set; }
        public int RentalDays { get; set; }
        public int? InsuranceId { get; set; }
    }
}
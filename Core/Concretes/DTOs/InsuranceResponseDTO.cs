namespace Core.Concretes.DTOs.Insurance
{
    public class InsuranceResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal? WeeklyPrice { get; set; }
        public decimal? MonthlyPrice { get; set; }
        public decimal CoverageAmount { get; set; } 
        public int Deductible { get; set; } 
        public bool IsIncludedByDefault { get; set; }
        public string? CoverageDetails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
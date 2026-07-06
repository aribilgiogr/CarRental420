using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Insurance : BaseEntity
    {
        public string Name { get; set; } // Örn: Kasko, Kişisel Kaza, Yolcu Sigorta
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal? WeeklyPrice { get; set; }
        public decimal? MonthlyPrice { get; set; }
        public decimal CoverageAmount { get; set; } // Sigorta kapsamı
        public int Deductible { get; set; } // Muhasır payı
        public bool IsIncludedByDefault { get; set; } = false;
        public string? CoverageDetails { get; set; }

        // Navigation Properties
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}

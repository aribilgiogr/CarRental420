using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Payment : BaseEntity
    {
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Kredi Kartı, Banka Transferi, Nakit vb.
        public string? PaymentStatus { get; set; } // Beklemede, Başarılı, Başarısız
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? TransactionNumber { get; set; }
        public string? CardLastFourDigits { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Rental Rental { get; set; }
    }
}

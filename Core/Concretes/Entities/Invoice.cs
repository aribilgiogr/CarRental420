using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? InvoiceStatus { get; set; } // Hazırlanıyor, Gönderildi, Ödendi, Ödenmiş vb.
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string? InvoicePdfPath { get; set; }

        // Navigation Properties
        public virtual Rental Rental { get; set; }
        public virtual Member Member { get; set; }
    }
}

namespace Core.Concretes.DTOs.Payment
{
    public class CreatePaymentDTO
    {
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } 
        public string? PaymentStatus { get; set; } 
        public DateTime PaymentDate { get; set; }
        public string? TransactionNumber { get; set; }
        public string? CardLastFourDigits { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
namespace Core.Concretes.DTOs.Invoice
{
    public class CreateInvoiceDTO
    {
        public string InvoiceNumber { get; set; }
        public int RentalId { get; set; }
        public string MemberId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? InvoiceStatus { get; set; } 
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string? InvoicePdfPath { get; set; }
    }
}
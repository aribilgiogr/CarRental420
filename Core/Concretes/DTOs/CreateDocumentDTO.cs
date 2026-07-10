namespace Core.Concretes.DTOs.Document
{
    public class CreateDocumentDTO
    {
        public string MemberId { get; set; }
        public string DocumentType { get; set; } 
        public string? DocumentNumber { get; set; }
        public string? DocumentPath { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
        public string? Notes { get; set; }
    }
}
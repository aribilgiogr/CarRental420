namespace Core.Concretes.DTOs.Document
{
    public class DocumentResponseDTO
    {
        public int Id { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
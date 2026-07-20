using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Document : BaseEntity
    {
        public string MemberId { get; set; }
        public string DocumentType { get; set; } // Ehliyet, Kimlik, Vekalet, Pasaport vb.
        public string? DocumentNumber { get; set; }
        public string? DocumentPath { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovalDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Member Member { get; set; }
    }
}

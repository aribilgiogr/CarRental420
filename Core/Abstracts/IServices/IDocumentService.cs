using Core.Concretes.DTOs.Document;

namespace Business.Services
{
    public interface IDocumentService
    {
        Task<DocumentResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<DocumentResponseDTO>> GetAllAsync();
        Task<IEnumerable<DocumentResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<DocumentResponseDTO>> GetByTypeAsync(string documentType);
        Task<IEnumerable<DocumentResponseDTO>> GetVerifiedDocumentsAsync();
        Task<IEnumerable<DocumentResponseDTO>> GetApprovedDocumentsAsync();
        Task<IEnumerable<DocumentResponseDTO>> GetPendingDocumentsAsync();
        Task<IEnumerable<DocumentResponseDTO>> GetExpiredDocumentsAsync();
        Task<DocumentResponseDTO> CreateAsync(CreateDocumentDTO dto);
        Task UpdateAsync(UpdateDocumentDTO dto);
        Task DeleteAsync(int id);
        Task<bool> DocumentExistsAsync(int id);
        Task VerifyDocumentAsync(int documentId);
        Task ApproveDocumentAsync(int documentId, string approvedBy);
        Task RejectDocumentAsync(int documentId, string rejectionReason);
        Task<bool> IsMemberDocumentsCompleteAsync(string memberId);
        Task<IEnumerable<DocumentResponseDTO>> GetMemberApprovedDocumentsAsync(string memberId);
    }
}

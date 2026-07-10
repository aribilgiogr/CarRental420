using Core.Concretes.DTOs.Document;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentResponseDTO> CreateAsync(CreateDocumentDTO dto)
        {
            var entity = new Document
            {
                MemberId = dto.MemberId,
                DocumentType = dto.DocumentType,
                DocumentNumber = dto.DocumentNumber,
                DocumentPath = dto.DocumentPath,
                ExpiryDate = dto.ExpiryDate,
                UploadDate = dto.UploadDate,
                IsVerified = dto.IsVerified,
                IsApproved = dto.IsApproved,
                ApprovalDate = dto.ApprovalDate,
                ApprovedBy = dto.ApprovedBy,
                RejectionReason = dto.RejectionReason,
                Notes = dto.Notes,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Document>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateDocumentDTO dto)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.MemberId = dto.MemberId;
            entity.DocumentType = dto.DocumentType;
            entity.DocumentNumber = dto.DocumentNumber;
            entity.DocumentPath = dto.DocumentPath;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.UploadDate = dto.UploadDate;
            entity.IsVerified = dto.IsVerified;
            entity.IsApproved = dto.IsApproved;
            entity.ApprovalDate = dto.ApprovalDate;
            entity.ApprovedBy = dto.ApprovedBy;
            entity.RejectionReason = dto.RejectionReason;
            entity.Notes = dto.Notes;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Document>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Document>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> DocumentExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Document>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<DocumentResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetByTypeAsync(string documentType)
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.DocumentType == documentType && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetVerifiedDocumentsAsync()
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.IsVerified && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetApprovedDocumentsAsync()
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.IsApproved && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetPendingDocumentsAsync()
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => !x.IsApproved && string.IsNullOrEmpty(x.RejectionReason) && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetExpiredDocumentsAsync()
        {
            var now = DateTime.UtcNow;
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value < now && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task VerifyDocumentAsync(int documentId)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(documentId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsVerified = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Document>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task ApproveDocumentAsync(int documentId, string approvedBy)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(documentId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsApproved = true;
                entity.ApprovalDate = DateTime.UtcNow;
                entity.ApprovedBy = approvedBy;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Document>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task RejectDocumentAsync(int documentId, string rejectionReason)
        {
            var entity = await _unitOfWork.Repository<Document>().FindByIdAsync(documentId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsApproved = false;
                entity.RejectionReason = rejectionReason;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Document>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<DocumentResponseDTO>> GetMemberApprovedDocumentsAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Document>().FindManyAsync(x => x.MemberId == memberId && x.IsApproved && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public Task<bool> IsMemberDocumentsCompleteAsync(string memberId)
        {
            throw new NotImplementedException();
        }

        private static DocumentResponseDTO MapToResponseDTO(Document entity)
        {
            return new DocumentResponseDTO
            {
                Id = entity.Id,
                MemberId = entity.MemberId,
                DocumentType = entity.DocumentType,
                DocumentNumber = entity.DocumentNumber,
                DocumentPath = entity.DocumentPath,
                ExpiryDate = entity.ExpiryDate,
                UploadDate = entity.UploadDate,
                IsVerified = entity.IsVerified,
                IsApproved = entity.IsApproved,
                ApprovalDate = entity.ApprovalDate,
                ApprovedBy = entity.ApprovedBy,
                RejectionReason = entity.RejectionReason,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
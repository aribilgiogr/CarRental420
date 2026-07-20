using Core.Concretes.DTOs.Invoice;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvoiceResponseDTO> CreateAsync(CreateInvoiceDTO dto)
        {
            var entity = new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                RentalId = dto.RentalId,
                MemberId = dto.MemberId,
                InvoiceDate = dto.InvoiceDate,
                SubTotal = dto.SubTotal,
                TaxAmount = dto.TaxAmount,
                DiscountAmount = dto.DiscountAmount,
                TotalAmount = dto.TotalAmount,
                InvoiceStatus = dto.InvoiceStatus ?? "Hazırlanıyor",
                DueDate = dto.DueDate,
                PaidDate = dto.PaidDate,
                PaymentMethod = dto.PaymentMethod,
                Notes = dto.Notes,
                InvoicePdfPath = dto.InvoicePdfPath,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Invoice>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateInvoiceDTO dto)
        {
            var entity = await _unitOfWork.Repository<Invoice>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.InvoiceNumber = dto.InvoiceNumber;
            entity.RentalId = dto.RentalId;
            entity.MemberId = dto.MemberId;
            entity.InvoiceDate = dto.InvoiceDate;
            entity.SubTotal = dto.SubTotal;
            entity.TaxAmount = dto.TaxAmount;
            entity.DiscountAmount = dto.DiscountAmount;
            entity.TotalAmount = dto.TotalAmount;
            entity.InvoiceStatus = dto.InvoiceStatus;
            entity.DueDate = dto.DueDate;
            entity.PaidDate = dto.PaidDate;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.Notes = dto.Notes;
            entity.InvoicePdfPath = dto.InvoicePdfPath;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Invoice>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Invoice>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Invoice>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> InvoiceExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Invoice>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<InvoiceResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Invoice>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetByStatusAsync(string status)
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => x.InvoiceStatus == status && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetOverdueInvoicesAsync()
        {
            var now = DateTime.UtcNow;
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => 
                x.DueDate.HasValue && x.DueDate.Value < now && x.InvoiceStatus != "Ödendi" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetUnpaidInvoicesAsync()
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => x.InvoiceStatus != "Ödendi" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task MarkAsPaidAsync(int invoiceId)
        {
            var entity = await _unitOfWork.Repository<Invoice>().FindByIdAsync(invoiceId);
            if (entity != null && !entity.Deleted)
            {
                entity.InvoiceStatus = "Ödendi";
                entity.PaidDate = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Invoice>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public Task SendInvoiceAsync(int invoiceId)
        {
            // Gönderim altyapısı (Email, SMS vb.) bilinmediği için varsayım yapılmamıştır.
            throw new NotImplementedException();
        }

        public async Task<decimal> GetTotalInvoiceAmountByMemberAsync(string memberId)
        {
            var invoices = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return invoices.Sum(x => x.TotalAmount);
        }

        public async Task<IEnumerable<InvoiceResponseDTO>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<Invoice>().FindManyAsync(x => 
                x.InvoiceDate >= startDate && x.InvoiceDate <= endDate && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static InvoiceResponseDTO MapToResponseDTO(Invoice entity)
        {
            return new InvoiceResponseDTO
            {
                Id = entity.Id,
                InvoiceNumber = entity.InvoiceNumber,
                RentalId = entity.RentalId,
                MemberId = entity.MemberId,
                InvoiceDate = entity.InvoiceDate,
                SubTotal = entity.SubTotal,
                TaxAmount = entity.TaxAmount,
                DiscountAmount = entity.DiscountAmount,
                TotalAmount = entity.TotalAmount,
                InvoiceStatus = entity.InvoiceStatus,
                DueDate = entity.DueDate,
                PaidDate = entity.PaidDate,
                PaymentMethod = entity.PaymentMethod,
                Notes = entity.Notes,
                InvoicePdfPath = entity.InvoicePdfPath,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
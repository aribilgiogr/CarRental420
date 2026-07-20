using Core.Concretes.DTOs.Payment;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResponseDTO> CreateAsync(CreatePaymentDTO dto)
        {
            var entity = new Payment
            {
                RentalId = dto.RentalId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus ?? "Beklemede",
                PaymentDate = dto.PaymentDate,
                TransactionNumber = dto.TransactionNumber,
                CardLastFourDigits = dto.CardLastFourDigits,
                InvoiceNumber = dto.InvoiceNumber,
                Notes = dto.Notes,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Payment>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdatePaymentDTO dto)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.RentalId = dto.RentalId;
            entity.Amount = dto.Amount;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.PaymentStatus = dto.PaymentStatus;
            entity.PaymentDate = dto.PaymentDate;
            entity.TransactionNumber = dto.TransactionNumber;
            entity.CardLastFourDigits = dto.CardLastFourDigits;
            entity.InvoiceNumber = dto.InvoiceNumber;
            entity.Notes = dto.Notes;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Payment>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Payment>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> PaymentExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Payment>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Payment>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<PaymentResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<Payment>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public Task<IEnumerable<PaymentResponseDTO>> GetByMemberAsync(string memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetByStatusAsync(string status)
        {
            var items = await _unitOfWork.Repository<Payment>().FindManyAsync(x => x.PaymentStatus == status && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task MarkAsCompleteAsync(int paymentId)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(paymentId);
            if (entity != null && !entity.Deleted)
            {
                entity.PaymentStatus = "Başarılı";
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Payment>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task MarkAsFailedAsync(int paymentId)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(paymentId);
            if (entity != null && !entity.Deleted)
            {
                entity.PaymentStatus = "Başarısız";
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Payment>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task MarkAsPendingAsync(int paymentId)
        {
            var entity = await _unitOfWork.Repository<Payment>().FindByIdAsync(paymentId);
            if (entity != null && !entity.Deleted)
            {
                entity.PaymentStatus = "Beklemede";
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Payment>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<decimal> GetTotalPaymentByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<Payment>().FindManyAsync(x => x.RentalId == rentalId && x.PaymentStatus == "Başarılı" && !x.Deleted);
            return items.Select(x => x.Amount).Sum();
        }

        public Task<decimal> GetTotalPaymentByMemberAsync(string memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<Payment>().FindManyAsync(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static PaymentResponseDTO MapToResponseDTO(Payment entity)
        {
            return new PaymentResponseDTO
            {
                Id = entity.Id,
                RentalId = entity.RentalId,
                Amount = entity.Amount,
                PaymentMethod = entity.PaymentMethod,
                PaymentStatus = entity.PaymentStatus,
                PaymentDate = entity.PaymentDate,
                TransactionNumber = entity.TransactionNumber,
                CardLastFourDigits = entity.CardLastFourDigits,
                InvoiceNumber = entity.InvoiceNumber,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}
using Core.Concretes.DTOs.Payment;

namespace Business.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentResponseDTO>> GetAllAsync();
        Task<IEnumerable<PaymentResponseDTO>> GetByRentalAsync(int rentalId);
        Task<IEnumerable<PaymentResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<PaymentResponseDTO>> GetByStatusAsync(string status);
        Task<PaymentResponseDTO> CreateAsync(CreatePaymentDTO dto);
        Task UpdateAsync(UpdatePaymentDTO dto);
        Task DeleteAsync(int id);
        Task<bool> PaymentExistsAsync(int id);
        Task MarkAsCompleteAsync(int paymentId);
        Task MarkAsFailedAsync(int paymentId);
        Task MarkAsPendingAsync(int paymentId);
        Task<decimal> GetTotalPaymentByRentalAsync(int rentalId);
        Task<decimal> GetTotalPaymentByMemberAsync(string memberId);
        Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

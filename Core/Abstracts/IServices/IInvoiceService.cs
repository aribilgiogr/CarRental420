using Core.Concretes.DTOs.Invoice;

namespace Business.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<InvoiceResponseDTO>> GetAllAsync();
        Task<IEnumerable<InvoiceResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<InvoiceResponseDTO>> GetByRentalAsync(int rentalId);
        Task<IEnumerable<InvoiceResponseDTO>> GetByStatusAsync(string status);
        Task<IEnumerable<InvoiceResponseDTO>> GetOverdueInvoicesAsync();
        Task<IEnumerable<InvoiceResponseDTO>> GetUnpaidInvoicesAsync();
        Task<InvoiceResponseDTO> CreateAsync(CreateInvoiceDTO dto);
        Task UpdateAsync(UpdateInvoiceDTO dto);
        Task DeleteAsync(int id);
        Task<bool> InvoiceExistsAsync(int id);
        Task MarkAsPaidAsync(int invoiceId);
        Task SendInvoiceAsync(int invoiceId);
        Task<decimal> GetTotalInvoiceAmountByMemberAsync(string memberId);
        Task<IEnumerable<InvoiceResponseDTO>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

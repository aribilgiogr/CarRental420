using Core.Concretes.DTOs.Penalty;

namespace Business.Services
{
    public interface IPenaltyService
    {
        Task<PenaltyResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<PenaltyResponseDTO>> GetAllAsync();
        Task<IEnumerable<PenaltyResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<PenaltyResponseDTO>> GetByRentalAsync(int rentalId);
        Task<IEnumerable<PenaltyResponseDTO>> GetByTypeAsync(string penaltyType);
        Task<IEnumerable<PenaltyResponseDTO>> GetPendingPenaltiesAsync();
        Task<IEnumerable<PenaltyResponseDTO>> GetApprovedPenaltiesAsync();
        Task<IEnumerable<PenaltyResponseDTO>> GetPaidPenaltiesAsync();
        Task<PenaltyResponseDTO> CreateAsync(CreatePenaltyDTO dto);
        Task UpdateAsync(UpdatePenaltyDTO dto);
        Task DeleteAsync(int id);
        Task<bool> PenaltyExistsAsync(int id);
        Task ApprovePenaltyAsync(int penaltyId, string approvedBy);
        Task RejectPenaltyAsync(int penaltyId);
        Task MarkAsPaidAsync(int penaltyId);
        Task<decimal> GetTotalPenaltyByMemberAsync(string memberId);
        Task<decimal> GetTotalPenaltyByRentalAsync(int rentalId);
        Task<IEnumerable<PenaltyResponseDTO>> GetPenaltiesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

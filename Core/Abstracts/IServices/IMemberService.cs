using Core.Concretes.DTOs.Member;

namespace Business.Services
{
    public interface IMemberService
    {
        Task<MemberResponseDTO?> GetByIdAsync(string id);
        Task<IEnumerable<MemberResponseDTO>> GetAllAsync();
        Task<IEnumerable<MemberResponseDTO>> GetBlacklistedMembersAsync();
        Task<IEnumerable<MemberResponseDTO>> GetVerifiedMembersAsync();
        Task<MemberResponseDTO> CreateAsync(CreateMemberDTO dto);
        Task UpdateAsync(UpdateMemberDTO dto);
        Task DeleteAsync(string id);
        Task<bool> MemberExistsAsync(string id);
        Task VerifyMemberAsync(string memberId);
        Task BlacklistMemberAsync(string memberId, string reason);
        Task UnblacklistMemberAsync(string memberId);
    }
}

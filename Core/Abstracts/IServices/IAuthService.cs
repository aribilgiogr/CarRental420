using Core.Concretes.DTOs.Member;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(CreateMemberDTO dto, string password);
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
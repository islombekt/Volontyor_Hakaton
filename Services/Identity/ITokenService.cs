using Volontyor_Hakaton.DTOs.Identity;

namespace Volontyor_Hakaton.Services.Identity
{
    public interface ITokenService
    {
        Task<TokenResponse> LoginAsync(LoginRequest model);
        Task<string> RegisterAsync(RegisterUser user);
    }
}

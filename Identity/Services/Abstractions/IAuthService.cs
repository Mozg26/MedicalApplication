using Identity.Models;
using Identity.Models.Request;
using IdentityDatabase.Models;

namespace Identity.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
        Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
    }
}

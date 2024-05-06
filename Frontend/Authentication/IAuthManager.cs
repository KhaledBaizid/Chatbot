using System.Security.Claims;
using Shared;

namespace Frontend.Authentication;

public interface IAuthManager
{
    public Task LoginAsync(User? user);
    
    Task<int> GetUserId();
    public Task LogoutAsync();
    public Task<ClaimsPrincipal> GetAuthAsync();

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
    public Task<int> GetUserIdFromCache();
}
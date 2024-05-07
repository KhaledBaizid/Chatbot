using System.Security.Claims;
using Shared;

namespace Frontend.Authentication;

public interface IAuthManager
{
    public Task<Admin> LoginAsync(Admin? user);
    
    Task<int> GetUserId();
    public Task LogoutAsync();
    public Task<ClaimsPrincipal> GetAuthAsync();

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
    public Task<int> GetUserIdFromCache();
}
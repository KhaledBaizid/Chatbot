namespace Backend.DataAccessObjects.AuthenticationDAO;

public interface IAuthenticationInterface
{
    public Task<Shared.Admin?> GetLoginAdminIdAsync(string mail, string password);
}
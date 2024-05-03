namespace Backend.DataAccessObjects.LoginDAO;

public interface ILoginInterface
{
    public Task<Shared.Admin?> GetLoginAdminIdAsync(string mail, string password);
}
namespace Backend.DataAccessObjects.Admin;

public interface ICredentialsInterface
{
    public Task<Shared.Admin> CreateAdminAccountAsync(Shared.Admin admin);
   // public Task<Shared.Admin?> GetLoginAdminIdAsync(string mail, string password);
    public Task<string> EditPasswordAsync(int id,string password, string newPassword, string reenteredPassword);
    public Task<string> EditMailAsync(int id, string password, string newMail, string reenteredMail);
}
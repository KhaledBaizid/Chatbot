using Backend.EFCData;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataAccessObjects.LoginDAO;

public class LoginImplementation : ILoginInterface
{
    private readonly DataContext _systemContext;
    public LoginImplementation(DataContext systemContext)
    {
        _systemContext = systemContext;
    }

    public async Task<Shared.Admin?> GetLoginAdminIdAsync(string mail, string password)
    {
        
        try
        {
            var findAdmin = await _systemContext.Admins.FirstOrDefaultAsync(e => e.Mail.ToLower() == mail.ToLower());
            if (findAdmin?.Password == password)
            {
                findAdmin.Password = null;
                return findAdmin;
            }

            var noAdminFound = new Shared.Admin { Id = -1, Mail = "", Password = "" };
            return noAdminFound;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
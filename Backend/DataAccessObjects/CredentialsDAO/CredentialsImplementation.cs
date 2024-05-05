using Backend.EFCData;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataAccessObjects.Admin;

public class CredentialsImplementation : ICredentialsInterface
{
    private readonly DataContext _systemContext;

    public CredentialsImplementation(DataContext systemContext)
    {
        _systemContext = systemContext;
    }

    public async Task<Shared.Admin> CreateAdminAccountAsync(Shared.Admin admin)
    {
        try
        {
            await _systemContext.Admins.AddAsync(admin);
            await _systemContext.SaveChangesAsync();
            var adminFound = await _systemContext.Admins.FirstAsync(u => u.Mail.ToLower() == admin.Mail.ToLower());
            adminFound.Password = null;
            return adminFound;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    // public async Task<Shared.Admin?> GetLoginAdminIdAsync(string mail, string password)
    // {
    //     try
    //     {
    //         var findAdmin = await _systemContext.Admins.FirstOrDefaultAsync(e => e.Mail.ToLower() == mail.ToLower());
    //         if (findAdmin?.Password == password)
    //         {
    //             findAdmin.Password = null;
    //             return findAdmin;
    //         }
    //
    //         var noAdminFound = new Shared.Admin { Id = -1, Mail = "", Password = "" };
    //         return noAdminFound;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    //
    // }

    public async Task<string> EditPasswordAsync(int id, string password, string newPassword, string reenteredPassword)
    {
        try
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(reenteredPassword))
            {
                return "New Password and re-entered Password cannot be empty";
            }
            
            var findAdmin = await _systemContext.Admins.FirstOrDefaultAsync(e => e.Id == id);
            if (findAdmin?.Password == password)
            {
                if (newPassword == reenteredPassword)
                {
                    findAdmin.Password = newPassword;
                    await _systemContext.SaveChangesAsync();
                    return "Password Changed";
                }

                return "New Password and re-entered Password do not match";
            }

            return "Current password is incorrect";

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        
    }

    public async Task<string> EditMailAsync(int id, string password, string newMail, string reenteredMail)
    {
        if (string.IsNullOrEmpty(newMail) || string.IsNullOrEmpty(reenteredMail))
        {
            return "New Mail and re-entered Mail cannot be empty";
        }
        try
        {
            var findAdmin = await _systemContext.Admins.FirstOrDefaultAsync(e => e.Id == id);
            if (findAdmin?.Password == password)
            {
                if (newMail.ToLower() == reenteredMail.ToLower())
                {
                    findAdmin.Mail = newMail;
                    await _systemContext.SaveChangesAsync();
                    return "EMail Changed";
                }

                return "New Mail and re-entered Mail do not match";
            }

            return "Current password is incorrect";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }
}
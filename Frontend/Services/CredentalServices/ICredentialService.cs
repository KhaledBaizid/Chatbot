namespace Frontend.Services.CredentalServices;

public interface ICredentialService
{
    Task<string?> ChangePassword(int id, string current, string newPassword, string repeatPassword);
    Task<string?> ChangeMail(int id, string password, string newMail, string repeatNewMail);
    
}
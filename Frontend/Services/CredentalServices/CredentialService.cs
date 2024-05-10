using System.Net.Http.Json;

namespace Frontend.Services.CredentalServices;

public class CredentialService : ICredentialService
{
    private readonly HttpClient httpClient;

    public CredentialService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    
    public async Task<string?> ChangePassword(int id ,string current, string newPasswordd, string repeatPassword)
    {
        
        var endpointUrl = $"{httpClient.BaseAddress}Credentials/password?id={id}&password={current}&newPassword={newPasswordd}&reenteredPassword={repeatPassword}";

        var response = await httpClient.PostAsJsonAsync(endpointUrl, new { });

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return errorContent;
        }
    }

    public async Task<string?> ChangeMail(int id, string password, string newMail, string repeatNewMail)
    {
        var endpointUrl = $"{httpClient.BaseAddress}Credentials/mail?id={id}&password={password}&newMail={newMail}&reenteredMail={repeatNewMail}";

        var response = await httpClient.PostAsJsonAsync(endpointUrl, new { });

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return errorContent;
        }
    }
}
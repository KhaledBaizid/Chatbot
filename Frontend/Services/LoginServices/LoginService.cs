using System.Net.Http.Json;
using Frontend.Services.LoginServices;
using Shared;

namespace Frontend.Services;

public class LoginService : ILoginService
{
    private readonly HttpClient httpClient;

    public LoginService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Admin?> Login(Admin admin)
    {
        string mail = admin.Mail;
        string password = admin.Password;
        var response = await httpClient.GetFromJsonAsync<Admin>($"/Authentication?mail={mail}&password={password}");
        return response;
             
    }
}
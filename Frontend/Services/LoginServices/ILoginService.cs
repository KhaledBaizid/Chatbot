using Shared;

namespace Frontend.Services.LoginServices;

public interface ILoginService
{
    Task<Admin?> Login(Admin user);
}
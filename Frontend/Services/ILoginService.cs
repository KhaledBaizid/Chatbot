using Shared;

namespace Frontend.Services;

public interface ILoginService
{
    Task<Admin?> Login(Admin user);
}
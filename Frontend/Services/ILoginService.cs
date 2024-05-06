using Shared;

namespace Frontend.Services;

public interface ILoginService
{
    Task<User?> Login(User user);
}
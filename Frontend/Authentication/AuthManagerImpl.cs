using System.Security.Claims;
using System.Text.Json;
using Frontend.Services;
using Microsoft.JSInterop;
using Shared;

namespace Frontend.Authentication;

public class AuthManagerImpl : IAuthManager
{
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } =
        null!; // assigning to null! to suppress null warning.

    private readonly ILoginService userService;
    private readonly IJSRuntime jsRuntime;

    private int userId;

    public AuthManagerImpl(ILoginService userService, IJSRuntime jsRuntime)
    {
        this.userService = userService;
        this.jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(User? user)
    {
        User? returnedUser = await userService.Login(user); // Get user from database
        int password = returnedUser.Id;
        userId = returnedUser.Id;

        ValidateLoginCredentials(password); // Validate input data against data from database
        //returnedUser.Password = "12345";								   // validation success

        await CacheUserAsync(returnedUser!); // Cache the user object in the browser 

        ClaimsPrincipal principal = CreateClaimsPrincipal(returnedUser); // convert user object to ClaimsPrincipal

        OnAuthStateChanged?.Invoke(principal); // notify interested classes in the change of authentication state
    }

    public async Task<int> GetUserIdFromCache()
    {
        User? user = await GetUserFromCacheAsync();
        return user?.Id ?? 0;
    }

    public async Task<int> GetUserId()
    {
        return userId;
    }

    public async Task LogoutAsync()
    {
        userId = 0;
        await ClearUserFromCacheAsync(); // remove the user object from browser cache
        ClaimsPrincipal principal = CreateClaimsPrincipal(null); // create a new ClaimsPrincipal with nothing.
        OnAuthStateChanged?.Invoke(principal); // notify about change in authentication state
    }

    public async Task<ClaimsPrincipal>
        GetAuthAsync() // this method is called by the authentication framework, whenever user credentials are reguired
    {
        User? user = await GetUserFromCacheAsync(); // retrieve cached user, if any

        ClaimsPrincipal principal = CreateClaimsPrincipal(user); // create ClaimsPrincipal

        return principal;
    }

    private async Task<User?> GetUserFromCacheAsync()
    {
        string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        if (string.IsNullOrEmpty(userAsJson)) return null;
        User user = JsonSerializer.Deserialize<User>(userAsJson)!;

        return user;
    }


    public static void ValidateLoginCredentials(int password)
    {
        if (password < 0)
        {
            throw new Exception("Credentials are not correct, please check your mail and password again!");
        }
    }


    private static ClaimsPrincipal CreateClaimsPrincipal(User? user)
    {
        if (user != null)
        {
            ClaimsIdentity identity = ConvertUserToClaimsIdentity(user);
            return new ClaimsPrincipal(identity);
        }

        return new ClaimsPrincipal();
    }

    private async Task CacheUserAsync(User? user)
    {
        string serialisedData = JsonSerializer.Serialize(user);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
    }

    private async Task ClearUserFromCacheAsync()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
    }

    private static ClaimsIdentity ConvertUserToClaimsIdentity(User user)
    {
        /*
        // here we take the information of the User object and convert to Claims
        // this is (probably) the only method, which needs modifying for your own user type
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Username),

            //new Claim("SecurityLevel", user.SecurityLevel.ToString()),
            //new Claim("BirthYear", user.BirthYear.ToString()),
            //new Claim("Domain", user.Domain)
        };

        return new ClaimsIdentity(claims, "apiauth_type");
        */
        var claims = new List<Claim>();

        if (!string.IsNullOrEmpty(user.Mail))
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Mail));
        }

        // Add similar checks for other properties of the User object

        return new ClaimsIdentity(claims, "authenticationType");
    }
}
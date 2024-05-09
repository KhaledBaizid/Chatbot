using Backend.DataAccessObjects.AuthenticationDAO;
using Backend.DataAccessObjects.AuthenticationDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.LoginController;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationInterface _authenticationInterface;

    public AuthenticationController(IAuthenticationInterface authenticationInterface)
    {
        _authenticationInterface = authenticationInterface;
    }

    [EnableCors] 
    [HttpGet]
    public async Task<ActionResult<Shared.Admin>> GetLoginAdminIdAsync(string mail, string password)
    {
        try
        {
            return StatusCode(200,await _authenticationInterface.GetLoginAdminIdAsync(mail,password)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
}
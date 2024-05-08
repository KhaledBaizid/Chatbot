using Backend.DataAccessObjects.LoginDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.LoginController;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private ILoginInterface _loginInterface;

    public LoginController(ILoginInterface loginInterface)
    {
        _loginInterface = loginInterface;
    }

    [EnableCors] 
    [HttpGet]
    public async Task<ActionResult<Shared.Admin>> GetLoginAdminIdAsync(string mail, string password)
    {
        try
        {
            return StatusCode(200,await _loginInterface.GetLoginAdminIdAsync(mail,password)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
}
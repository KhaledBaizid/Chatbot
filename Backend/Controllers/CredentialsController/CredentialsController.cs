using Backend.DataAccessObjects.Admin;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.AdminController;

[ApiController]
[Route("[controller]")]
public class CredentialsController : ControllerBase
{
    private ICredentialsInterface _credentialsInterface ;
    
    public CredentialsController(ICredentialsInterface credentialsInterface) 
    {
        _credentialsInterface = credentialsInterface;
    }
   
    
    // [EnableCors]
    // [HttpPost]
    // public async Task<ActionResult<Shared.Admin>> CreateAdminAccountAsync(Shared.Admin admin)
    // {
    //     try
    //     {
    //         return StatusCode(200,await _adminInterface.CreateAdminAccountAsync(admin)); 
    //     }
    //     catch (Exception e)
    //     {
    //         return   StatusCode(500, e.Message);
    //     }
    // }
    
    [EnableCors]
    [HttpPut]
   
    public async Task<ActionResult<string>> EditPasswordAsync(int id, string password, string newPassword, string reenteredPassword)
    {
        try
        {
            return StatusCode(200,await _credentialsInterface.EditPasswordAsync(id,password,newPassword,reenteredPassword)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
    
    [EnableCors]
    [HttpPut]
    [Route("mail")]
    public async Task<ActionResult<string>> EditMailAsync(int id, string password, string newMail, string reenteredMail)
    {
        try
        {
            return StatusCode(200,await _credentialsInterface.EditMailAsync(id,password,newMail,reenteredMail)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
   
}
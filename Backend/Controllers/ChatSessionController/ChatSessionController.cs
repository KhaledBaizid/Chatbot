using Backend.DataAccessObjects.ChatSessionDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.ChatSessionController;

[ApiController]
[Route("[controller]")]
public class ChatSessionController : ControllerBase
{
    private readonly IChatSessionInterface _chatSessionInterface;

    public ChatSessionController(IChatSessionInterface chatSessionInterface)
    {
        _chatSessionInterface = chatSessionInterface;
    }
    
    [EnableCors]
    [HttpGet]
    public async Task<ActionResult<int>> StartChatSessionAsync()
    {
        try
        {
            return StatusCode(200,await _chatSessionInterface.StartChatSessionAsync()); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
}
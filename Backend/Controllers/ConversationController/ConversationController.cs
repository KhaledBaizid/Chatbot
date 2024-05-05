using Backend.DataAccessObjects.ConversationDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers.ConversationController;

[ApiController]
[Route("[controller]")]
public class ConversationController: ControllerBase
{
    private readonly IConversationInterface _conversationInterface;

    public ConversationController(IConversationInterface conversationInterface)
    {
        _conversationInterface = conversationInterface;
    }
    
    [EnableCors]
    [HttpGet]
    public async Task<ActionResult<Chat_session>> GetConversationByChatSessionId(int chatSessionId,string question)
    {
        try
        {
            return StatusCode(200,await _conversationInterface.GetConversationByChatSessionId(chatSessionId,question,10)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
}
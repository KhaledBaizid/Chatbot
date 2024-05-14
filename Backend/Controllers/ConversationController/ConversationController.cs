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
    public async Task<ActionResult<Chat_session>> GetConversationByChatSessionIdAsync(int chatSessionId,string question)
    {
        try
        {
            return StatusCode(200,await _conversationInterface.GetConversationByChatSessionIdAsync(chatSessionId,question,30)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
    [EnableCors]
    [HttpGet]
    [Route("ByFeedbackAndByDate")]
   
    public async Task<ActionResult<List<Conversation>>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate, string feedback)
    {
        try
        {
            return StatusCode(200,await _conversationInterface.GetConversationsByFeedbackAndByDateAsync(startDate,endDate,feedback)); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return   StatusCode(500, e.Message);
        }
    }
   
}
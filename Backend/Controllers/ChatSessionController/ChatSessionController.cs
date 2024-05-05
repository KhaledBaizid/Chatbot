using Backend.DataAccessObjects.ChatSessionDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared;

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
    [HttpPost]
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
    [EnableCors]
    [HttpGet]
    public async Task<ActionResult<Chat_session>> getChatSessionById(int chatSessionId)
    {
        try
        {
            return StatusCode(200,await _chatSessionInterface.getChatSessionById(chatSessionId)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
    [EnableCors]
    [HttpGet]
    [Route("ChatSessionsByDate")]
    public async Task<ActionResult<List<Chat_session>>> GetChatSessionsByDate(DateTime startDate, DateTime endDate)
    {
        try
        {
            return StatusCode(200,await _chatSessionInterface.GetChatSessionsByDate(startDate,endDate)); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return   StatusCode(500, e.Message);
        }
    }
}
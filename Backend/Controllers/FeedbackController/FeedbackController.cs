using Backend.DataAccessObjects.FeedbackDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers.FeedbackController;

[ApiController]
[Route("[controller]")]
public class FeedbackController : ControllerBase
{
   private readonly IFeedBackInterface _feedBackInterface;

   public FeedbackController(IFeedBackInterface feedBackInterface)
   {
      _feedBackInterface = feedBackInterface;
   }

   [EnableCors]
   [HttpPut]
   public async Task<ActionResult<string>> GiveFeedbackAsync(int conversationId, string feedback)
   {
      try
      {
         return StatusCode(200,await _feedBackInterface.GiveFeedbackAsync(conversationId,feedback)); 
      }
      catch (Exception e)
      {
         return   StatusCode(500, e.Message);
      }
   }
   
   [EnableCors]
   [HttpGet]
   
   public async Task<ActionResult<List<Conversation>>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate, string feedback)
   {
      try
      {
         return StatusCode(200,await _feedBackInterface.GetConversationsByFeedbackAndByDateAsync(startDate,endDate,feedback)); 
      }
      catch (Exception e)
      {
         Console.WriteLine(e.Message);
         return   StatusCode(500, e.Message);
      }
   }
}
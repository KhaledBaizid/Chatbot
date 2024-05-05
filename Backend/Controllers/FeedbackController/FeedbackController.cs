using Backend.DataAccessObjects.FeedbackDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
   public async Task<ActionResult<string>> giveFeedback(int conversationId, string feedback)
   {
      try
      {
         return StatusCode(200,await _feedBackInterface.giveFeedback(conversationId,feedback)); 
      }
      catch (Exception e)
      {
         return   StatusCode(500, e.Message);
      }
   }
}
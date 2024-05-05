using Backend.EFCData;
using Shared;

namespace Backend.DataAccessObjects.FeedbackDAO;

public class FeedbackImplementation: IFeedBackInterface
{
    private readonly DataContext _systemContext;

    public FeedbackImplementation(DataContext systemContext)
    {
        _systemContext = systemContext;
    }

    public async Task<string> giveFeedback(int conversationId,string feedback)
    {
        var conversation = await _systemContext.Conversations.FindAsync(conversationId);
        if (conversation != null) conversation.Feedback = feedback;
        await _systemContext.SaveChangesAsync();
       if (FeedbackEnum.Positive.ToString().Equals(feedback))
        {
            return FeedbackMessages.PositiveMessage;
        }
        else if (FeedbackEnum.Negative.ToString().Equals(feedback))
        {
            return FeedbackMessages.NegativeMessage;
        }

        return "";
            
    }
}
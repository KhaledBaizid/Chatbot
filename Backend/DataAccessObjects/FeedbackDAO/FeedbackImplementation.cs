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

    public Task<List<Conversation>> GetConversationsByFeedbackAndByDate(DateTime startDate, DateTime endDate, string feedback)
    {
        try
        {
            var startDateToUniversalTime = startDate.ToUniversalTime();
            var endDateToUniversalTime = endDate.ToUniversalTime();
            if (startDateToUniversalTime > endDateToUniversalTime)
            {
                throw new Exception("Start date cannot be greater than end date");
            }
            var conversations = _systemContext.Conversations.Where(c => c.Feedback == feedback && c.ConversationTime >= startDateToUniversalTime && c.ConversationTime <= endDateToUniversalTime).ToList();
            return Task.FromResult(conversations);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        throw new NotImplementedException();
    }
}
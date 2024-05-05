using Shared;

namespace Backend.DataAccessObjects.FeedbackDAO;

public interface IFeedBackInterface
{
    public Task<string> giveFeedback(int conversationId,string feedback);
    
    public Task<List<Conversation>> GetConversationsByFeedbackAndByDate(DateTime startDate, DateTime endDate, string feedback);
}
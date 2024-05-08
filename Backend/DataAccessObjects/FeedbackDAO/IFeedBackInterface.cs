using Shared;

namespace Backend.DataAccessObjects.FeedbackDAO;

public interface IFeedBackInterface
{
    public Task<string> GiveFeedbackAsync(int conversationId,string feedback);
    
    public Task<List<Conversation>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate, string feedback);
}
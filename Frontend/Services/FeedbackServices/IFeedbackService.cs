using Shared;

namespace Frontend.Services.FeedbackServices;

public interface IFeedbackService
{
    
    public Task<string> GiveFeedbackAsync(int? conversationId, FeedbackEnum feedback);
    public Task<List<Conversation>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate,
        string feedback);
    
}
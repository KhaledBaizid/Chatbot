using Shared;

namespace Backend.DataAccessObjects.ConversationDAO;

public interface IConversationInterface
{
    public Task<Chat_session> GetConversationByChatSessionIdAsync(int chatSessionId,string question,int timeOutSeconds);
    public Task<List<Conversation>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate, string feedback);

}
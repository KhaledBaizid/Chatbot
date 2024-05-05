using Shared;

namespace Backend.DataAccessObjects.ChatSessionDAO;

public interface IChatSessionInterface
{
    public Task<int> StartChatSessionAsync();
    public Task<Chat_session> getChatSessionById(int chatSessionId);
    public Task<List<Chat_session>> GetChatSessionsByDate(DateTime startDate, DateTime endDate);
}
using Shared;

namespace Frontend.Services.ChatSessionServices;

public interface IChatSessionService
{
    public Task<int> StartChatSessionAsync();
    public Task<Chat_session> getChatSessionById(int chatSessionId);
    
    public Task<List<Chat_session>> GetChatSessionsByDate(DateTime startDate, DateTime endDate);
}
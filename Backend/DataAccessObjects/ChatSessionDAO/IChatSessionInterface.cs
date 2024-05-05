using Shared;

namespace Backend.DataAccessObjects.ChatSessionDAO;

public interface IChatSessionInterface
{
    public Task<int> StartChatSessionAsync();
    public Task<Chat_session> getChatSessionById(int chatSessionId);
}
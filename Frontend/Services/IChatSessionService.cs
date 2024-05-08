using Shared;

namespace Frontend.Services;

public interface IChatSessionService
{
    public Task<int> StartChatSessionAsync();
    public Task<Chat_session> getChatSessionById(int chatSessionId);
}
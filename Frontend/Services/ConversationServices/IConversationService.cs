using Shared;

namespace Frontend.Services.ConversationServices;

public interface IConversationService
{
    public Task<Chat_session> GetConversationByChatSessionId(int chatSessionId, string question);

}
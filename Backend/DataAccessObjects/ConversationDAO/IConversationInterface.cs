using Shared;

namespace Backend.DataAccessObjects.ConversationDAO;

public interface IConversationInterface
{
    public Task<Chat_session> GetConversationByChatSessionId(int chatSessionId,string question,int timeOutSeconds);
}
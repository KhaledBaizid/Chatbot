namespace Backend.DataAccessObjects.ChatSessionDAO;

public interface IChatSessionInterface
{
    public Task<int> StartChatSessionAsync();
}
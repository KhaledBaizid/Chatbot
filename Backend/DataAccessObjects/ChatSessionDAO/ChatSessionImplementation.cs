using Backend.EFCData;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.DataAccessObjects.ChatSessionDAO;

public class ChatSessionImplementation : IChatSessionInterface
{
    private readonly DataContext _systemContext;

    public ChatSessionImplementation(DataContext systemContext)
    {
        _systemContext = systemContext;
    }

    public async Task<int> StartChatSessionAsync()
    {
        try
        {    // var conversations = await _systemContext.Chat_sessions.Include(c=>c.Conversations).ToListAsync();
            var chatSession = new Shared.Chat_session
            {
                ChatTime = DateTime.Now.ToUniversalTime()
            };
            await _systemContext.Chat_sessions.AddAsync(chatSession);
            await _systemContext.SaveChangesAsync();
            return chatSession.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        throw new NotImplementedException();
    }

    public async Task<Chat_session> getChatSessionById(int chatSessionId)
    {
        try
        {
            var chatSession =  await _systemContext.Chat_sessions.Include(c => c.Conversations).OrderByDescending(c=>c.Id).FirstAsync(c => c.Id == chatSessionId);
            return chatSession;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
}
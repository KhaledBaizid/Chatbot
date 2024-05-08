using Backend.EFCData;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.DataAccessObjects.ChatSessionDAO;

public class ChatSessionDAO : IChatSessionInterface
{
    private readonly DataContext _systemContext;

    public ChatSessionDAO(DataContext systemContext)
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

    public async Task<Chat_session> GetChatSessionByIdAsync(int chatSessionId)
    {
        try
        {
            var chatSession =  await _systemContext.Chat_sessions.Include(c => c.Conversations.OrderBy(con=>con.Id)).FirstAsync(c => c.Id == chatSessionId);
            return chatSession;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }

    public Task<List<Chat_session>> GetChatSessionsByDate(DateTime startDate, DateTime endDate)
    {
        try
        {
           var startDateToUniversalTime= startDate.ToUniversalTime();
           var endDateToUniversalTime = endDate.ToUniversalTime();
            if(startDateToUniversalTime > endDateToUniversalTime)
            {
                throw new Exception("Start date cannot be greater than end date");
            }
            var chatSessions = _systemContext.Chat_sessions.Include(c => c.Conversations.OrderBy(con=>con.Id))
                .Where(c => c.ChatTime.Date >= startDateToUniversalTime && c.ChatTime.Date <= endDateToUniversalTime && c.Conversations.Count>0)
                .ToListAsync();
            return chatSessions;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
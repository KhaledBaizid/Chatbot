using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.EFCData;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class ChatHistoryTests
{
    [Test]
    public async Task ReturnErrorMessage_WhenGetChatSessionByDate_WithInvalidDate()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new ChatSessionDAO(context);
        
        // Act
        var exception = Assert.ThrowsAsync<Exception>(async () => 
        {
            await service.GetChatSessionsByDate(DateTime.Now, DateTime.Now.AddDays(-1));
        });
        
        // Assert
        Assert.AreEqual("Start date cannot be greater than end date", exception.Message);

        
        await context.Database.EnsureDeletedAsync();
    }
    
   
    [Test]
    public async Task ReturnChatHistory_WhenGetChatSessionByDate_WithValidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new ChatSessionDAO(context);
    
        // Create some mock chat sessions in the database
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var chatSessions = new List<Chat_session>
        {
            new Chat_session { ChatTime = startDate },
            new Chat_session { ChatTime = endDate }
        };
        await context.Chat_sessions.AddRangeAsync(chatSessions);
        await context.SaveChangesAsync();
        
        var sd = startDate.AddHours(-1);
        // Act
        var result = await service.GetChatSessionsByDate(startDate, endDate);
    
        // Assert
        Assert.IsNotNull(result);
        
    
        await context.Database.EnsureDeletedAsync();
    }

}
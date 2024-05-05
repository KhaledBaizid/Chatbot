using Backend.Controllers.ChatSessionController;
using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.EFCData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared;

namespace TestProject;

public class ChatSessionTests
{
    [Test]
    public async Task ReturnChatSessionId_WhenStartSession_WithNoParameter()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new ChatSessionImplementation(context);
        
        // Act
        var result = await service.StartChatSessionAsync();
        var result2 = await service.StartChatSessionAsync();
        
        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(result2, Is.EqualTo(2));
        await context.Database.EnsureDeletedAsync();
        
    }
    
   
}
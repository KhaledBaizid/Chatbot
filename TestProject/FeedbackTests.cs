using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.DataAccessObjects.FeedbackDAO;
using Backend.EFCData;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class FeedbackTests
{
    [Test]
    public async Task ReturnThanksMessage_WhenGiveFeedback_WithPositive()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
        var service2 = new ChatSessionImplementation(context);
        var chatSessionId = await service2.StartChatSessionAsync();
        var conversation = new Conversation
        { ChatSessionId = chatSessionId,
            Question = "",
            Answer = "",
            Feedback = FeedbackEnum.Neutral.ToString()
        };
        await context.Conversations.AddAsync(conversation);
        await context.SaveChangesAsync();
        // Act
        
        var result = await service.giveFeedback(1, "Positive");
       
        
        // Assert
        Assert.That(result, Is.EqualTo("Thank you for your positive feedback!"));
       
        await context.Database.EnsureDeletedAsync();
        
    }
    
    [Test]
    public async Task ReturnSorryMessage_WhenGiveFeedback_WithNegative()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
        var service2 = new ChatSessionImplementation(context);
        var chatSessionId = await service2.StartChatSessionAsync();
        var conversation = new Conversation
        { ChatSessionId = chatSessionId,
            Question = "",
            Answer = "",
            Feedback = FeedbackEnum.Neutral.ToString()
        };
        await context.Conversations.AddAsync(conversation);
        await context.SaveChangesAsync();
        // Act
        
        var result = await service.giveFeedback(1, "Negative");
       
        
        // Assert
        Assert.That(result, Is.EqualTo("We're sorry to hear about your negative feedback."));
       
        await context.Database.EnsureDeletedAsync();
        
    }
}
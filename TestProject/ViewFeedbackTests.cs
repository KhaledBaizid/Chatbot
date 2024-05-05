using Backend.DataAccessObjects.ConversationDAO;
using Backend.DataAccessObjects.FeedbackDAO;
using Backend.EFCData;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class ViewFeedbackTests
{
    
    [Test]
    public async Task ReturnPositiveFeedbacks_WhenGetConversationsByFeedbackAndByDate_WithValidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
    
        // Create some mock conversations in the database
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var positiveFeedback = "Positive"; // Assuming "positive" is a valid feedback value
        var conversations = new List<Conversation>
        {
            new Conversation {Question = "",Answer = "",Feedback = positiveFeedback, ConversationTime = startDate }
            
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var result = await service.GetConversationsByFeedbackAndByDate(startDate, endDate, positiveFeedback);
    
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count); 
    
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ThrowExceptionWithErrorMessage_WhenGetConversationsByPositiveFeedbackAndByDate_WithInvalidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
      
        var startDate = DateTime.Now.AddDays(1); // Start date greater than end date
        var endDate = DateTime.Now;
        var positiveFeedback = "Positive"; 
        var conversations = new List<Conversation>
        {
            new Conversation { Question = "", Answer = "", Feedback = positiveFeedback, ConversationTime = startDate }
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var exception =   Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.GetConversationsByFeedbackAndByDate(startDate, endDate, positiveFeedback);
        });
        
        // Assert
        Assert.AreEqual("Start date cannot be greater than end date", exception.Message);
        await context.Database.EnsureDeletedAsync();
    }

    [Test]
    public async Task ReturnNegativeFeedbacks_WhenGetConversationsByFeedbackAndByDate_WithValidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
    
        // Create some mock conversations in the database
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var negativeFeedback = "Negative"; // Assuming "negative" is a valid feedback value
        
        var conversations = new List<Conversation>
        {
            new Conversation {Question = "",Answer = "",Feedback = negativeFeedback, ConversationTime = startDate }
            
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var result = await service.GetConversationsByFeedbackAndByDate(startDate, endDate,negativeFeedback );
    
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count); 
    
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ThrowExceptionWithErrorMessage_WhenGetConversationsByNegativeFeedbackAndByDate_WithInvalidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
      
        var startDate = DateTime.Now.AddDays(1); // Start date greater than end date
        var endDate = DateTime.Now;
        var negativeFeedback = "Positive"; 
        var conversations = new List<Conversation>
        {
            new Conversation { Question = "", Answer = "", Feedback = negativeFeedback, ConversationTime = startDate }
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var exception =   Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.GetConversationsByFeedbackAndByDate(startDate, endDate, negativeFeedback);
        });
        
        // Assert
        Assert.AreEqual("Start date cannot be greater than end date", exception.Message);
        await context.Database.EnsureDeletedAsync();
    }
    
     [Test]
    public async Task ReturnNeutralFeedbacks_WhenGetConversationsByFeedbackAndByDate_WithValidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
    
        // Create some mock conversations in the database
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var neutralFeedback = "Negative"; // Assuming "negative" is a valid feedback value
        
        var conversations = new List<Conversation>
        {
            new Conversation {Question = "",Answer = "",Feedback = neutralFeedback, ConversationTime = startDate }
            
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var result = await service.GetConversationsByFeedbackAndByDate(startDate, endDate,neutralFeedback );
    
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count); 
    
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ThrowExceptionWithErrorMessage_WhenGetConversationsByNeutralFeedbackAndByDate_WithInvalidDateRange()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new FeedbackImplementation(context);
      
        var startDate = DateTime.Now.AddDays(1); // Start date greater than end date
        var endDate = DateTime.Now;
        var neutralFeedback = "Positive"; 
        var conversations = new List<Conversation>
        {
            new Conversation { Question = "", Answer = "", Feedback = neutralFeedback, ConversationTime = startDate }
        };
        await context.Conversations.AddRangeAsync(conversations);
        await context.SaveChangesAsync();
    
        // Act
        var exception =   Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.GetConversationsByFeedbackAndByDate(startDate, endDate, neutralFeedback);
        });
        
        // Assert
        Assert.AreEqual("Start date cannot be greater than end date", exception.Message);
        await context.Database.EnsureDeletedAsync();
    }

}
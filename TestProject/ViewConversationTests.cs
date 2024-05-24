using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.DataAccessObjects.ConversationDAO;
using Backend.DataAccessObjects.PdfDAO;
using Backend.EFCData;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class ViewConversationTests
{
    const string apiKey = "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";
    [Test]
    public async Task ReturnAnswer_WhenGetAnswer_WithinTimeout()
    {
       
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        //////////////////////////////////////////////
        
        var servicePDF = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/howto.pdf";
        await servicePDF.AddPDFAsync(url,1);
        
        var serviceChat = new ChatSessionDAO(context);
        var resultChat = await serviceChat.StartChatSessionAsync();
        /////////////////////////////////////////////
        var service = new ConversationDAO(context,new EmbeddingProvider(apiKey),new LlmChainProvider(new EmbeddingProvider(apiKey)));
        var timeOutSeconds = 10;
     
        // Act
        
        var result = await service.GetConversationByChatSessionIdAsync(resultChat, "how to avoid Avoid nominalizations ?", timeOutSeconds);
        var errormessage = result.Conversations[0].Answer;
        
        // Assert
        Assert.That(errormessage, Is.Not.EqualTo("Getting answer took too long. Please try to ask again."));
        Assert.That(errormessage, Is.Not.EqualTo("I am sorry, i can not find the answer to your question."));
        await context.Database.EnsureDeletedAsync();
        
    }
    
    [Test]
    public async Task ReturnErrorMessage_WhenGetAnswer_TakesMoreThanTimeout()
    {

        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        //////////////////////////////////////////////
        
        var servicePDF = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/howto.pdf";
        await servicePDF.AddPDFAsync(url,1);
        
        var serviceChat = new ChatSessionDAO(context);
        var resultChat = await serviceChat.StartChatSessionAsync();
        /////////////////////////////////////////////
        var service = new ConversationDAO(context,new EmbeddingProvider(apiKey),new LlmChainProvider(new EmbeddingProvider(apiKey)));
        var timeOutSeconds = 1;
     
        // Act
        
        var result = await service.GetConversationByChatSessionIdAsync(resultChat, "how to avoid Avoid nominalizations ?", timeOutSeconds);
        var errormessage = result.Conversations[0].Answer;
        
        // Assert
        Assert.That(errormessage, Is.EqualTo("Getting answer took too long. Please try to ask again."));
      
        await context.Database.EnsureDeletedAsync();
        
    }
    
    [Test]
    public async Task ReturnErrorMessage_WhenGetAnswer_CanNotFindAnswerFromPDFFiles()
    {
        
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        //////////////////////////////////////////////
        
        var servicePDF = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/howto.pdf";
        await servicePDF.AddPDFAsync(url,1);
        
        var serviceChat = new ChatSessionDAO(context);
        var resultChat = await serviceChat.StartChatSessionAsync();
        /////////////////////////////////////////////
        var service = new ConversationDAO(context,new EmbeddingProvider(apiKey),new LlmChainProvider(new EmbeddingProvider(apiKey)));
        var timeOutSeconds = 10;
     
        // Act
        
        var result = await service.GetConversationByChatSessionIdAsync(resultChat, "How old is Khaled?", timeOutSeconds);
        var errormessage = result.Conversations[0].Answer;
        
        // Assert
     
        Assert.That(errormessage, Is.EqualTo("I am sorry, i can not find the answer to your question."));
        await context.Database.EnsureDeletedAsync();
        
    }
}
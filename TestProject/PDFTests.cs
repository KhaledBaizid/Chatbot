using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.AuthenticationDAO;
using Backend.DataAccessObjects.PdfDAO;
using Backend.EFCData;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class PDFTests
{
  const string apiKey = "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";
    
    [Test]
    public async Task ShouldGetConfirmationMessage_WithAddingPDF_WhenTheUrlISValidAndHasValidPDFExtension()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new PDFDAO(context,new EmbeddingProvider(apiKey));
       
        var url = "https://www.plainenglish.co.uk/files/formsguide.pdf";
        
        var adminService = new CredentialsDAO(context);
        var admin = new Admin
        {
            Mail = "khaled@chatbot.com",
            Password = "123456"
        };
        await adminService.CreateAdminAccountAsync(admin);
        await context.SaveChangesAsync();
        // Act
        var result = await service.AddPDFAsync(url,1);
        var isAdded = await service.IsPDFExistAsync(url);
        
        // Assert
        Assert.That(isAdded,Is.True);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("pdf is added successfully"));
        await context.Database.EnsureDeletedAsync();
    }

    [Test]
    public async Task ShouldGetErrorMessage_WhenAddingPDF_WithTheUrlHasWrongPDFExtension()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/formsguide.txt";
        
        var adminService = new CredentialsDAO(context);
        var admin = new Admin
        {
            Mail = "khaled@chatbot.com",
            Password = "123456"
        };
        await adminService.CreateAdminAccountAsync(admin);
        await context.SaveChangesAsync();
        // Act
        var result = await service.AddPDFAsync(url,1);
        var isAdded = await service.IsPDFExistAsync(url);
        // Assert
        Assert.That(isAdded,Is.False);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("The url does not have a pdf extension, please provide a valid pdf url with valid extension"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetErrorMessage_WithAddingPDF_WhenTheUrlIsNotValidAndHasValidPDFExtension()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/formsguide123.pdf";
        
        var adminService = new CredentialsDAO(context);
        var admin = new Admin
        {
            Mail = "khaled@chatbot.com",
            Password = "123456"
        };
        await adminService.CreateAdminAccountAsync(admin);
        await context.SaveChangesAsync();
        // Act
        var result = await service.AddPDFAsync(url,1);
        var isAdded = await service.IsPDFExistAsync(url);
        
        // Assert
        Assert.That(isAdded,Is.False);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("url is not valid"));
        await context.Database.EnsureDeletedAsync();
    }

 
    [Test]
    public async Task ShouldGetConfirmationMessage_WhenDeletingPDF_WithAValidExistingURL()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/formsguide.pdf";
        var adminService = new CredentialsDAO(context);
        var admin = new Admin
        {
            Mail = "khaled@chatbot.com",
            Password = "123456"
        };
        await adminService.CreateAdminAccountAsync(admin);
        await context.SaveChangesAsync();
        await service.AddPDFAsync(url,1);
        // Act
     var  result = await service.DeletePDFAsync(url);
        var isExisting = await service.IsPDFExistAsync(url);;
        
        // Assert
        Assert.That(isExisting,Is.False);
        Assert.That(result, Is.EqualTo("pdf is deleted successfully"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetErrorMessage_WhenDeletingPDF_WithNoValidExistingURL()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        var service = new PDFDAO(context,new EmbeddingProvider(apiKey));
        var url = "https://www.plainenglish.co.uk/files/formsguide.pdf";
        var notExistingUrl = "https://www.plainenglish.co.uk/files/formsguide1.pdf";
        
        var adminService = new CredentialsDAO(context);
        var admin = new Admin
        {
            Mail = "khaled@chatbot.com",
            Password = "123456"
        };
        await adminService.CreateAdminAccountAsync(admin);
        await context.SaveChangesAsync();
        await service.AddPDFAsync(url,1);
        // Act
        var  result = await service.DeletePDFAsync(notExistingUrl);
        var isExisting = await service.IsPDFExistAsync(url);;
        
        // Assert
        Assert.That(isExisting,Is.True);
        Assert.That(result, Is.EqualTo("URL does not exist"));
        await context.Database.EnsureDeletedAsync();
    }
    
    
    
}
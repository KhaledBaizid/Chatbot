using Backend.DataAccessObjects.LoginDAO;
using Backend.DataAccessObjects.PdfDAO;
using Backend.EFCData;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class PDFServiceTests
{

    
   // public IEmbeddingProvider EmbeddingProvider => _embeddingProvider;

    // public PDFServiceTests(IEmbeddingProvider embeddingProvider, IPromptProvider promptProvider)
    // {
    //     _embeddingProvider = embeddingProvider;
    //     _promptProvider = promptProvider;
    // }

    [Test]
    public async Task ShouldGetConfirmationMessage_WithAddingPDF_WhenTheUrlHasValidPDFExtension()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
       // await context.PDFs.AddAsync(new PDF {  Url="C:\\Users\\baizi\\Desktop\\Boyum\\Khaled.pdf", AdminId = 1 }); 
       // await context.SaveChangesAsync();
        var service = new PDFImplementation(context,new EmbeddingProvide("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"),new PromptProvider(new EmbeddingProvide("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")));
    
        // Act
        var result = await service.AddPDFAsync("https://www.plainenglish.co.uk/files/formsguide.pdf");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("pdf is added successfully"));
        await context.Database.EnsureDeletedAsync();
    }

    [Test]
    public async Task ShouldGetErrorMessage_WhenAddingPDF_WithTheUrlHasWrongPDFExtension()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        // await context.PDFs.AddAsync(new PDF {  Url="C:\\Users\\baizi\\Desktop\\Boyum\\Khaled.pdf", AdminId = 1 }); 
        // await context.SaveChangesAsync();
        var service = new PDFImplementation(context,new EmbeddingProvide("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"),new PromptProvider(new EmbeddingProvide("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")));
    
        // Act
        var result = await service.AddPDFAsync("test.txt");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("The url does not have a pdf extension, please provide a valid pdf url with valid extension"));
        await context.Database.EnsureDeletedAsync();
    }

    
}
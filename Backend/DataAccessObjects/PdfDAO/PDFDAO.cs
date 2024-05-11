using Backend.EFCData;
using Backend.Services;
using LangChain.Chains.LLM;
using LangChain.Prompts;
using LangChain.Schema;
using LangChain.TextSplitters;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Shared;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;

namespace Backend.DataAccessObjects.PdfDAO;

public class PDFDAO : IPDFInterface
{
    private readonly DataContext _systemContext;
    private readonly IEmbeddingProvider _embeddingProvider;
    public PDFDAO(DataContext systemContext, IEmbeddingProvider embeddingProvider)
    {
        _systemContext = systemContext;
        _embeddingProvider = embeddingProvider;
    }

    public async Task<string> AddPDFAsync(string url, int adminId)
    {
        try
        {    
            // Check if the URL has a PDF extension
            if (!IsUrlHasPdfExtension(url))
                return "The url does not have a pdf extension, please provide a valid pdf url with valid extension";
           
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            // check if the URL is valid      
            if (!response.IsSuccessStatusCode)
                return ("url is not valid");
            // save the pdf in the database
            return await ProcessPdfAsync(response, url, adminId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Please try again later");
            
        }
       
    }

    public async  Task<bool> IsPDFExistAsync(string url)
    {
        var isExisting = await _systemContext.PDFs.FirstOrDefaultAsync(p => p.Url == url);
        return isExisting != null;
    }

    public async Task<string> DeletePDFAsync(string url)
    {
        try
        {
            var pdf = await _systemContext.PDFs.FirstOrDefaultAsync(p => p.Url == url);
            if (pdf == null) return "URL does not exist";
            _systemContext.PDFs.Remove(pdf);
            await _systemContext.SaveChangesAsync();
            return "pdf is deleted successfully";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private bool IsUrlHasPdfExtension(string url)
    {
        if (url.Length < 4) return false;
        // Take the last four characters of the URL
        var lastFourCharacters = url[^4..];

        // Check if the last four characters contain ".pdf" extension
        return lastFourCharacters.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<string> ProcessPdfAsync(HttpResponseMessage response, string url, int adminId )
    {
        try
        {
            await DeletePDFAsync(url);
            var bytes = await response.Content.ReadAsStreamAsync();
            using PdfDocument document = PdfDocument.Open(bytes);
            await _systemContext.PDFs.AddAsync(new PDF { AdminId = adminId , Url = url });
            await _systemContext.SaveChangesAsync();
            var pdfFound = await _systemContext.PDFs.FirstAsync(p => p.Url == url);
            var pdfId = pdfFound.Id;
            int pageCount = document.NumberOfPages;

            for (int i = 1; i <= pageCount; i++)
            {
                Page page = document.GetPage(i);
                string text = "";
                text = page.Text;
                var r_splitter = new RecursiveCharacterTextSplitter(["\n\n", "\n", " ", ""], 1000, 80);
                var spl = r_splitter.SplitText(text);

                foreach (var splitText in spl)
                {
                    var v = await _embeddingProvider.GetModel().EmbedQueryAsync(splitText);
                    var chunk = new Chunks
                    {
                        PDFId = pdfId,
                        Embedding = new Vector(v),
                        Text = splitText
                    };
                    await _systemContext.Chunks.AddAsync(chunk);
                    await _systemContext.SaveChangesAsync();
                }
            }

            return "pdf is added successfully";
        }
        catch (Exception e)
        {
            return "Something went wrong while processing the pdf. Please try again later.";
            
        }
    }
}
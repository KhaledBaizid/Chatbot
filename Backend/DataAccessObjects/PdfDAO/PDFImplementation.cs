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

public class PDFImplementation : IPDFInterface
{
    private readonly DataContext _systemContext;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IPromptProvider _promptProvider;

    public PDFImplementation(DataContext systemContext, IEmbeddingProvider embeddingProvider, IPromptProvider promptProvider)
    {
        _systemContext = systemContext;
        _embeddingProvider = embeddingProvider;
        _promptProvider = promptProvider;
    }

    public async Task<string> AddPDFAsync(string url)
    {
        try
        {
            if (!UrlHasPdfExtension(url))
                return "The url does not have a pdf extension, please provide a valid pdf url with valid extension";
            //////////////////////
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    /* handle the error */
                }

                var bytes = await response.Content.ReadAsStreamAsync();
                //////////////  
                using PdfDocument document = PdfDocument.Open(bytes);
                await _systemContext.PDFs.AddAsync(new PDF { AdminId = 1, Url = url });
                await _systemContext.SaveChangesAsync();
                var pdfFound = await _systemContext.PDFs.FirstAsync(p => p.Url == url);
                var pdfId = pdfFound.Id;
                int pageCount = document.NumberOfPages;

                for (int i = 1; i <= pageCount; i++)
                {
                    Page page = document.GetPage(i);
                    string text = "";
                    text = page.Text;
                    Console.WriteLine(text);
                    Console.WriteLine("******************************************");
                    var r_splitter = new RecursiveCharacterTextSplitter(["\n\n", "\n", " ", ""], 1000, 80);
                    var spl = r_splitter.SplitText(text);

                    foreach (var splitText in spl)
                    {
                        Console.WriteLine(splitText);
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        throw new NotImplementedException();
    }

    public async  Task<bool> IsPDFExistAsync(string url)
    {
        var isExisting = await _systemContext.PDFs.FirstOrDefaultAsync(p => p.Url == url);
        if(isExisting != null)
        {
            return true;
        }

        return false;

    }

    public async Task<string> DeletePDFAsync(string url)
    {
        try
        {
            var pdf = await _systemContext.PDFs.FirstOrDefaultAsync(p => p.Url == url);
            if (pdf != null)
            {
                _systemContext.PDFs.Remove(pdf);
                await _systemContext.SaveChangesAsync();
                return "pdf is deleted successfully";
            }
            return "pdf is not found";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public bool UrlHasPdfExtension(string url)
    {
        if (url.Length >= 4)
        {
            // Take the last four characters of the URL
            string lastFourCharacters = url.Substring(url.Length - 4);

            // Check if the last four characters contain ".pdf" extension
            return lastFourCharacters.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }
    
        return false;
    }
}
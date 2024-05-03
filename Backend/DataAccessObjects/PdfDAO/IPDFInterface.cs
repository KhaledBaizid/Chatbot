namespace Backend.DataAccessObjects.PdfDAO;

public interface IPDFInterface
{
    public Task<string> AddPDFAsync(string url);
}
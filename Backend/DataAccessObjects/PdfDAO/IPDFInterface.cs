using Shared;

namespace Backend.DataAccessObjects.PdfDAO;

public interface IPDFInterface
{
    public Task<string> AddPDFAsync(string url,int adminId);
    
    public Task<string> DeletePDFAsync(string url);
    
    public Task<bool> IsPDFExistAsync(string url);
}
using UploadFile.Models;

namespace Frontend.Services;

public interface IUploadFileService
{
    public Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream);
    public Task<bool> DeleteFileToBlobAsync(string strFileName);
    public Task<List<FileUploadViewModel>> GetAllFilesFromBlobAsync();
}
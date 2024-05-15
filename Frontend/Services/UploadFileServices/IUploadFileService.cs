using UploadFile.Models;

namespace Frontend.Services.UploadFileServices;

public interface IUploadFileService
{
    public Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream);
    public Task<bool> DeleteFileToBlobAsync(string strFileName);
    public Task<List<FileUploadViewModel>> GetAllFilesFromBlobAsync();
    public Task<string> SendFileUrl(string fileUrl,int adminId);
    public Task<string> DeleteFileUrl(string fileUrl);
}
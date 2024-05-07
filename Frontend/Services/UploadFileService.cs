using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using UploadFile.Models;

namespace Frontend.Services;

public class UploadFileService : IUploadFileService
{
   
     private readonly IConfiguration _configuration;
    private readonly ILogger<UploadFileService> _logger;
    private readonly string blobContainerName = "pdfiles";
    private string connectionString;

    public UploadFileService(IConfiguration configuration, ILogger<UploadFileService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _configuration.GetConnectionString(
            "DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net");
    }

    public async Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream)
    {
        try
        {
            var container = new BlobContainerClient(
                "DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net",
                blobContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contecntType });
            var urlString = blob.Uri.ToString();
            Console.WriteLine(urlString);
            return urlString;

        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<bool> DeleteFileToBlobAsync(string strFileName)
    {
        try
        {
            var container = new BlobContainerClient(
                "DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net",
                blobContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<List<FileUploadViewModel>> GetAllFilesFromBlobAsync()
    {
        try
        {
            var container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net", blobContainerName);
            var blobs = container.GetBlobsAsync();
            var files = new List<FileUploadViewModel>();
            
            await foreach (var blobItem in blobs)
            {
                files.Add(new FileUploadViewModel
                {
                    FileName = blobItem.Name,
                    FileStorageUrl = container.GetBlobClient(blobItem.Name).Uri.ToString(),
                    ContentType = blobItem.Properties.ContentType
                });
            }
            return files;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}
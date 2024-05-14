using System.Net.Http.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using UploadFile.Models;

namespace Frontend.Services.UploadFileServices;

public class UploadFileService : IUploadFileService
{
    private readonly ILogger<UploadFileService> _logger;
    private readonly string blobContainerName = "pdfiles";
    private readonly string connectionString =
        "DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net";

    private readonly HttpClient httpClient;


    public UploadFileService( ILogger<UploadFileService> logger, HttpClient httpClient)
    {
        _logger = logger;
        this.httpClient = httpClient;
    }

    public async Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream)
    {
        try
        {
            var container = new BlobContainerClient(connectionString, blobContainerName);

            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            var DeleteIfExistsAsync = await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
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
            var container = new BlobContainerClient(connectionString, blobContainerName);
            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            var urlString = blob.Uri.ToString();
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
            var container = new BlobContainerClient(connectionString, blobContainerName);
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

    public async Task<string> SendFileUrl(string fileUrl, int adminId)
    {
        var endpointUrl = $"{httpClient.BaseAddress}PDF/?url={fileUrl}&adminId={adminId}";
        var response = await httpClient.PostAsJsonAsync(endpointUrl, new { });

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return errorContent;
        }
    }

    public async Task<string> DeleteFileUrl(string fileUrl)
    {
        var response = await httpClient.DeleteAsync($"/PDF?url={fileUrl}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Failed to delete file: {response.StatusCode}");
        }
    }
}
using System.Net.Http.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using UploadFile.Models;

namespace Frontend.Services.UploadFileServices;

public class UploadFileService : IUploadFileService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UploadFileService> _logger;
    private readonly string blobContainerName = "pdfiles";
    private string connectionString;

    private readonly HttpClient httpClient;


    public UploadFileService(IConfiguration configuration, ILogger<UploadFileService> logger, HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        this.httpClient = httpClient;
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
            //var createResponse = await container.CreateIfNotExistsAsync();
            //if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
           var DeleteIfExistsAsync = await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contecntType });
            var urlString = blob.Uri.ToString();
            // var response = await SendFileUrl(urlString);
            // Console.WriteLine("response for send utl request is: " + response);
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
            // two lines below are not needed, 
            // var createResponse = await container.CreateIfNotExistsAsync();
            // if (createResponse != null && createResponse.GetRawResponse().Status == 201)
           
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            var urlString = blob.Uri.ToString();
              await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            // var response = await DeleteFileUrl(urlString);
            
            // Console.WriteLine("response for delete utl request is: " + response);


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
            var container = new BlobContainerClient(
                "DefaultEndpointsProtocol=https;AccountName=sepfiles;AccountKey=+TcC9h5fVZIAF6whSo95aTEe7MD/jBUtVI5CTMN4wo4iyIJQbu3j3J1uU/xMV0HtMTllbV04Jb17+AStM7KVcg==;EndpointSuffix=core.windows.net",
                blobContainerName);
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

    public async Task<string> SendFileUrl(string fileUrl,int adminId)
    {  
        var endpointUrl = $"{httpClient.BaseAddress}PDF/?url={fileUrl}&adminId={adminId}";
        var response = await httpClient.PostAsJsonAsync(endpointUrl, new { });
      //  var response = await httpClient.GetStringAsync($"/PDF?url={fileUrl}&adminId={adminId}");
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
       // return response;
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
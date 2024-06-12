using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RedMango_api.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobServiceClient) {
            _blobClient = blobServiceClient;
        }

        public async Task<string> GetBlob(string blobName, string ContainerName) 
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            return blobClient.Uri.AbsoluteUri;

        }
        public async Task<bool> DeleteBlob(string blobName, string ContainerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
        }
        public async Task<string> UploadBlob(string blobName, string ContainerName, IFormFile file)
        {

            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
            if(result != null)
            {
                return await GetBlob(blobName, ContainerName);
            }
            return "";
        }

    }
}

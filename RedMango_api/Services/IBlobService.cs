namespace RedMango_api.Services
{
    public interface IBlobService
    {
        Task<string> GetBlob(string blobName, string ContainerName);
        Task<bool> DeleteBlob(string blobName, string ContainerName);
        Task<string> UploadBlob(string blobName, string ContainerName, IFormFile file);
    }
}

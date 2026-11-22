using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ArticleProject.Api.Application.Services
{
    public class AzureBlobService : IBaseSingletonService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("AzureBlobStorage").Get<AzureBlobSettingsModel>();
            string connectionString = $"DefaultEndpointsProtocol=https;AccountName={settings.AccountName};AccountKey={settings.AccountKey};EndpointSuffix=core.windows.net";

            _containerClient = new BlobContainerClient(connectionString, settings.ContainerName);
            _containerClient.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BP.Domain.Repository;
using BP.Shared.Types;

namespace BP.Infrastructure.Repositories;

public class AzureBlobFileRepository(BlobContainerClient blobContainer) : IFileUploadRepository
{
    public async Task<string> UploadAsync(FileUpload file)
    {
        await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None).ConfigureAwait(false);

        var blobName = $"products/{file.SkuId}/{file.ImageId}{file.Extension}";
        var blobClient = blobContainer.GetBlobClient(blobName);

        await blobClient.UploadAsync(file.Content, new BlobHttpHeaders
        {
            ContentType = file.ContentType,
           
        }).ConfigureAwait(false);

        return blobName;
    }

    public async Task<FileDownload?> DownloadAsync(string blobName)
    {
        var blobClient = blobContainer.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync().ConfigureAwait(false))
        {
            var response = await blobClient.DownloadContentAsync().ConfigureAwait(false);
            var contentType = response.Value.Details.ContentType ?? "application/octet-stream";
            return new FileDownload
            {
                Content = response.Value.Content.ToStream(),
                ContentType = contentType
            };
        }

        return null;
    }

    public async Task<bool> DeleteAsync(string blobName)
    {
        var blobClient = blobContainer.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync().ConfigureAwait(false);
        return response.Value;
    }
}

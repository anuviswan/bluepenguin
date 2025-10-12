using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BP.Domain.Repository;
using BP.Shared.Types;

namespace BP.Infrastructure.Repositories;

public class AzureBlobFileRepository(BlobContainerClient blobContainer) : IFileUploadRepository
{
    public async Task<string> UploadAsync(FileUpload file)
    {
        await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = file.FileName;
        var blobClient = blobContainer.GetBlobClient(blobName);

        await blobClient.UploadAsync(file.Content, new BlobHttpHeaders
        {
            ContentType = file.ContentType
        });

        return blobClient.Uri.ToString();
    }

    public async Task<FileDownload?> DownloadAsync(string blobName)
    {
        var blobClient = blobContainer.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadContentAsync();
            var contentType = response.Value.Details.ContentType ?? "application/octet-stream";
            return new FileDownload
            {
                Content = response.Value.Content.ToStream(),
                ContentType = contentType
            };
        }

        return null;
    }
}

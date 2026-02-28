using BP.Shared.Types;

namespace BP.Domain.Repository;

public interface IFileUploadRepository
{
    Task<string> UploadAsync(FileUpload file);
    Task<FileDownload?> DownloadAsync(string blobName);
    Task<bool> DeleteAsync(string blobName);
    Task<string?> GetBlobUrlAsync(string blobName);
    Task<string?> GetBlobSasUrlAsync(string blobName, int expirationMinutes = 60);
}

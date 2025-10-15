using BP.Shared.Types;

namespace BP.Application.Interfaces.Services;

public interface IProductImageService
{
    Task<string> UploadAsync(FileUpload file, bool isPrimary = false);
    Task<FileDownload?> DownloadAsync(string blobName);
}

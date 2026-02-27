using BP.Application.Interfaces.Services;
using BP.Domain.Repository;
using BP.Shared.Types;

namespace BP.Application.Services;

public class FileUploadService(IFileUploadRepository fileUploadRepository) : IFileUploadService
{
    public async Task<string> UploadAsync(FileUpload file)
    {
        return await fileUploadRepository.UploadAsync(file).ConfigureAwait(false);
    }

    public async Task<FileDownload?> DownloadByBlobNameAsync(string blobName)
    {
        return await fileUploadRepository.DownloadAsync(blobName).ConfigureAwait(false);
    }

    public async Task<bool> DeleteByBlobNameAsync(string blobName)
    {
        return await fileUploadRepository.DeleteAsync(blobName).ConfigureAwait(false);
    }

    public async Task<string?> GetBlobUrlAsync(string blobName)
    {
        return await fileUploadRepository.GetBlobUrlAsync(blobName).ConfigureAwait(false);
    }
}

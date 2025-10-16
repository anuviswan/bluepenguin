using BP.Shared.Types;

namespace BP.Application.Interfaces.Services;

public interface IProductImageService
{
    Task<string> UploadAsync(FileUpload file, bool isPrimary = false);
    Task<FileDownload?> DownloadByImageIdAsync(string skuId,string imageId);
    Task<IEnumerable<FileDownload>> DownloadBySkuIdAsync(string skuId);
    Task<IEnumerable<string>> GetImageIdsForSkuId(string skuId);
}

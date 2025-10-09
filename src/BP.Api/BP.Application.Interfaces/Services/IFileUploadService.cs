using BP.Shared.Types;

namespace BP.Application.Interfaces.Services;

public interface IFileUploadService
{
    Task<string> UploadAsync(FileUpload file);
}

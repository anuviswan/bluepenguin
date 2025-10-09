using BP.Shared.Types;

namespace BP.Domain.Repository;

public interface IFileUploadRepository
{
    Task<string> UploadAsync(FileUpload file);
}

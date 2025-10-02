using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface ISkuGeneratorService
{
    Task<string> GetSkuCode(string categoryCode, string materialCode, string[] featureCodes, string collectionCode, int yearCode);
}

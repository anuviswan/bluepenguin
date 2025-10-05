using BP.Application.Interfaces.Services;

namespace BP.Application.Services;

public class SkuGeneratorService(IProductService productService) : ISkuGeneratorService
{
    private IProductService ProductService => productService;
    public async Task<string> GetSkuCode(string categoryCode,string materialCode, string[] featureCodes, string collectionCode, int yearCode)
    {
        var lastGeneratedCollectionSequenceCode = await ProductService.GetItemCountForCollection(collectionCode, yearCode);
        var newCollectionSequenceCode = lastGeneratedCollectionSequenceCode  + 1;
        var skuCode = $"{categoryCode}{materialCode}-{string.Join('-', featureCodes)}-{collectionCode}{yearCode}{newCollectionSequenceCode:D3}";
        return skuCode;
    }
}

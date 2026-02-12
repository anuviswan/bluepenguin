namespace BP.Api.Contracts;

public record AddFeatureRequest(string FeatureId, string FeatureName,string? symbolic);

public record AddCollectionRequest(string CollectionId, string CollectionName);

using System;
using System.Collections.Generic;
using System.Text;

namespace BP.Application.Interfaces.Types
{
    public record ImageSimilarityMatch(
        string SkuId,
        string ImageId,
        string BlobName,
        double Similarity
    );
}

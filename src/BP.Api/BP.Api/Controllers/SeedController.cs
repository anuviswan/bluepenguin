using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using BP.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BP.Api.Controllers;

public class SeedController(IProductController productController, IProductImageService productImageService): Controller
{
    private IProductController ProductController => productController;
    private IProductImageService ProductImageService => productImageService;

    [AllowAnonymous]
    [HttpPost]
    [Route("seed/execute")]
    public async Task<IActionResult> ExecuteSeed()
    {
        await SeedProducts();
        return Ok();
    }

    private async Task SeedProducts()
    {
        var seeds = new List<CreateProductRequest>
        {
            new() {
                Name = "Onam Floral Ring",
                Price = 199.99,
                Category = Category.RI.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = [Feature.FL.ToString()],
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 1
            },
            new() {
                Name = "Christmas Mirror Pendant",
                Price = 249.5,
                Category = Category.PD.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = [Feature.MR.ToString(), Feature.PL.ToString()],
                CollectionCode = Collection.CMS.ToString(),
                YearCode = 2023,
                SequenceCode = 1
            },
            new() {
                Name = "Ocean Sea Elements Necklace",
                Price = 299.0,
                Category = Category.NK.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = [Feature.SE.ToString(), Feature.ST.ToString()],
                CollectionCode = Collection.OCN.ToString(),
                YearCode = 2024,
                SequenceCode = 2
            },
            new() {
                Name = "Nature Embedded Bracelet",
                Price = 149.75,
                Category = Category.BR.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { Feature.EM.ToString() },
                CollectionCode = Collection.NAT.ToString(),
                YearCode = 2022,
                SequenceCode = 1
            },
            new() {
                Name = "Traditional Glitter Earrings",
                Price = 129.0,
                Category = Category.ER.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = [Feature.GL.ToString(), Feature.SC.ToString()],
                CollectionCode = Collection.TRD.ToString(),
                YearCode = 2021,
                SequenceCode = 3
            },
            new() {
                Name = "Spotlight Multi-color Ring",
                Price = 219.99,
                Category = Category.RI.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = [Feature.MC.ToString(), Feature.FD.ToString()],
                CollectionCode = Collection.SLT.ToString(),
                YearCode = 2024,
                SequenceCode = 4
            },
            new() {
                Name = "Signature Pendant Metallic",
                Price = 329.0,
                Category = Category.PD.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = [Feature.MT.ToString(), Feature.PL.ToString()],
                CollectionCode = Collection.SGN.ToString(),
                YearCode = 2020,
                SequenceCode = 2
            },
            new() {
                Name = "Vintage Framed Necklace",
                Price = 279.5,
                Category = Category.NK.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = [Feature.FR.ToString(), Feature.PS.ToString()],
                CollectionCode = Collection.VIN.ToString(),
                YearCode = 2019,
                SequenceCode = 5
            },
            new() {
                Name = "Onam Custom Charm Bracelet",
                Price = 159.0,
                Category = Category.BR.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = [Feature.CH.ToString(), Feature.PR.ToString()],
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 6
            },
            new() {
                Name = "Spotlight Waves Earrings",
                Price = 139.99,
                Category = Category.ER.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = [Feature.WV.ToString(), Feature.IN.ToString()],
                CollectionCode = Collection.SLT.ToString(),
                YearCode = 2023,
                SequenceCode = 2
            }
        };

        foreach (var seed in seeds)
        {
            try
            {
                var res = await ProductController.CreateProduct(seed);
                string? sku = null;
                if (res is ObjectResult or)
                {
                    sku = or.Value as string;
                }


                if (!string.IsNullOrWhiteSpace(sku))
                {
                    // generate a dummy 200x300 PNG using ImageSharp
                    using var image = new Image<Rgba32>(200, 300);
                    image.Mutate(ctx => ctx.BackgroundColor(new Rgba32(211,211,211)));

                    await using var ms = new MemoryStream();
                    await image.SaveAsPngAsync(ms);
                    ms.Position = 0;

                    var fileUpload = new FileUpload
                    {
                        SkuId = sku,
                        ImageId = Guid.NewGuid().ToString(),
                        ContentType = "image/png",
                        Content = new MemoryStream(ms.ToArray()),
                        Extension = ".png"
                    };
                    fileUpload.Content.Position = 0;

                    try
                    {
                        var url = await ProductImageService.UploadAsync(fileUpload, true);
                    }
                    catch 
                    {
                        throw;
                        // Decide what to do if image upload fails
                    }
                }
            }
            catch 
            {
                throw;
                // Decide what to do if product creation fails
            }
        }
    }

}

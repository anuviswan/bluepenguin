using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BP.Api.Contracts;
using BP.Application.Interfaces.SkuAttributes;
using Microsoft.AspNetCore.Authorization;

namespace BP.Api.Controllers;

public class SeedController(IProductController productController): Controller
{
    private IProductController ProductController => productController;

    [AllowAnonymous]
    [HttpPost]
    [Route("seed/execute")]
    public async Task<IActionResult> ExecuteSeed()
    {
        var result = await SeedProducts();
        return Ok(result);
    }

    private async Task<object?> SeedProducts()
    {
        var seeds = new List<CreateProductRequest>
        {
            new CreateProductRequest
            {
                Name = "Onam Floral Ring",
                Price = 199.99,
                Category = Category.RI.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { Feature.FL.ToString() },
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                Name = "Christmas Mirror Pendant",
                Price = 249.5,
                Category = Category.PD.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { Feature.MR.ToString(), Feature.PL.ToString() },
                CollectionCode = Collection.CMS.ToString(),
                YearCode = 2023,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                Name = "Ocean Sea Elements Necklace",
                Price = 299.0,
                Category = Category.NK.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { Feature.SE.ToString(), Feature.ST.ToString() },
                CollectionCode = Collection.OCN.ToString(),
                YearCode = 2024,
                SequenceCode = 2
            },
            new CreateProductRequest
            {
                Name = "Nature Embedded Bracelet",
                Price = 149.75,
                Category = Category.BR.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { Feature.EM.ToString() },
                CollectionCode = Collection.NAT.ToString(),
                YearCode = 2022,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                Name = "Traditional Glitter Earrings",
                Price = 129.0,
                Category = Category.ER.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { Feature.GL.ToString(), Feature.SC.ToString() },
                CollectionCode = Collection.TRD.ToString(),
                YearCode = 2021,
                SequenceCode = 3
            },
            new CreateProductRequest
            {
                Name = "Spotlight Multi-color Ring",
                Price = 219.99,
                Category = Category.RI.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { Feature.MC.ToString(), Feature.FD.ToString() },
                CollectionCode = Collection.SLT.ToString(),
                YearCode = 2024,
                SequenceCode = 4
            },
            new CreateProductRequest
            {
                Name = "Signature Pendant Metallic",
                Price = 329.0,
                Category = Category.PD.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { Feature.MT.ToString(), Feature.PL.ToString() },
                CollectionCode = Collection.SGN.ToString(),
                YearCode = 2020,
                SequenceCode = 2
            },
            new CreateProductRequest
            {
                Name = "Vintage Framed Necklace",
                Price = 279.5,
                Category = Category.NK.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { Feature.FR.ToString(), Feature.PS.ToString() },
                CollectionCode = Collection.VIN.ToString(),
                YearCode = 2019,
                SequenceCode = 5
            },
            new CreateProductRequest
            {
                Name = "Onam Custom Charm Bracelet",
                Price = 159.0,
                Category = Category.BR.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { Feature.CH.ToString(), Feature.PR.ToString() },
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 6
            },
            new CreateProductRequest
            {
                Name = "Spotlight Waves Earrings",
                Price = 139.99,
                Category = Category.ER.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { Feature.WV.ToString(), Feature.IN.ToString() },
                CollectionCode = Collection.SLT.ToString(),
                YearCode = 2023,
                SequenceCode = 2
            }
        };

        var created = new List<object?>();
        foreach (var seed in seeds)
        {
            try
            {
                var res = await ProductController.CreateProduct(seed);
                if (res is ObjectResult or)
                {
                    created.Add(or.Value);
                }
                else
                {
                    created.Add(null);
                }
            }
            catch (Exception ex)
            {
                created.Add(ex.Message);
            }
        }

        return created;
    }

}

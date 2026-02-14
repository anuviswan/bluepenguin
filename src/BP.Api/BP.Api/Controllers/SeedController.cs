using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using BP.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

using Azure.Storage.Blobs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SeedController(IProductController productController, 
    IFeatureService featureService,
    ICollectionService collectionService,
    IProductImageService productImageService, 
    IWebHostEnvironment env, 
    BP.Domain.Repository.IProductRepository productRepository, 
    BP.Domain.Repository.IProductImageRepository productImageRepository, 
    BP.Domain.Repository.IMetaDataRepository metaDataRepository,
    BlobContainerClient blobContainer) : Controller
{
    private IProductController ProductController => productController;
    private IFeatureService FeatureService => featureService;
    private ICollectionService CollectionService => collectionService;
    private IProductImageService ProductImageService => productImageService;
    private IWebHostEnvironment Env => env;
    private BP.Domain.Repository.IProductRepository ProductRepository => productRepository;
    private BP.Domain.Repository.IProductImageRepository ProductImageRepository => productImageRepository;
    private BlobContainerClient BlobContainer => blobContainer;

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteSeed()
    {
        await SeedCollections();
        await SeedFeatures();
        await SeedProducts();
        return Ok();
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearSeed()
    {
        try
        {
            int deletedProducts = 0;
            int deletedImages = 0;

            // Instead of fetching and deleting each row individually (may miss untracked items),
            // delete all image metadata rows, then attempt to delete all blobs in the container,
            // then delete all product rows. This avoids relying on GetAll for complete state.

            await metaDataRepository.DeleteAllAsync();

            // Delete all product image metadata rows
            await ProductImageRepository.DeleteAllAsync();

            // Delete all blobs under the container (best-effort)
            await foreach (var blobItem in BlobContainer.GetBlobsAsync())
            {
                try
                {
                    var blob = BlobContainer.GetBlobClient(blobItem.Name);
                    await blob.DeleteIfExistsAsync();
                    deletedImages++;
                }
                catch
                {
                    // ignore per-blob failures
                }
            }

            // Delete all product rows
            await ProductRepository.DeleteAllAsync();

            // We don't have exact counts for products; return best-effort image count
            return Ok(new { deletedProducts = -1, deletedImages });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private record CollectionSeed(string Code, string Title);

    private List<CollectionSeed> Collections => new List<CollectionSeed>
    {
        new("ONM", "Onam Collection"),
        new("CMS", "Christmas Collection"),
        new("OCN", "Ocean Collection"),
        new("NAT", "Nature Collection"),
        new("TRD", "Traditional Collection"),
        new("SLT", "Spotlight Collection"),
        new("SGN", "Signature Collection"),
        new("VIN", "Vintage Collection")
    };

    private record FeatureSeed(string Code,  string Name, string symbolicText);
    private List<FeatureSeed> Features => new List<FeatureSeed>
    {
        new("FL", "Floral Inclusion", "Joy, creativity"),
        new("EM", "Embedded Object", "Personal meaning"),
        new("MR", "Mirror Work", "Reflection, brilliance"),
        new("SE", "Sea Elements", "Calm, flow"),
        new("ST", "Stones", "Strength, grounding"),
        new("PR", "Personal Elements", "Identity, connection"),
        new("GL", "Glitter", "Sparkle, celebration"),
        new("FR", "Framed Finish", "Structure, emphasis"),
        new("CH", "Metal Charm", "Strength, protection"),
        new("MC", "Multi Color", "Vibrance, diversity"),
        new("SC", "Single Color", "Simplicity, focus"),
        new("MT", "Metalic Bead", "Strength, grounding"),
        new("FD", "Flower Bead", "Joy, creativity"),
        new("CD", "Crackle Bead", "Transformation, balance"),
        new("PL", "Pendant Large", "Bold presence"),
        new("PS", "Pendant Small", "Subtle elegance"),
        new("WV", "Waves", "Movement, rhythm"),
        new("AG", "Artificial Grass", "Nature-inspired"),
        new("IN", "Invisible", "Minimal presence"),
        new("RS", "Rhine Stone", "Confidence, sparkle"),
        new("CE", "CatEyeBead", "Protection, focus"),
        new("GG", "Gungru", "Energy, movement"),
        new("FS", "Frameless", "Freedom, openness"),
        new("BL", "Pendant Bail", "Connection, support"),
        new("JB", "Jelly Bead", "Playfulness"),
        new("MF", "Matt Finish Bead", "Calm, grounding"),
        new("AP", "Artificial Pearl", "Peace, elegance"),
        new("ML", "Mona Lisa Pendant", "Confidence, expression"),
        new("GP", "Glass Pearl Bead", "Harmony, soothing"),
        new("CR", "Crystal Rondelle Bead", "Clarity, intention"),
        new("CG", "Crysal Glass Bead", "Clarity, positivity"),
        new("AB", "Agate Bead", "Grounding, stability"),
        new("TG", "Crystal Tyre Glass Bead", "Protection, resilience"),
        new("MH", "Multi Howlite", "Calm, patience")
    };

    private async Task SeedCollections()
    {
        foreach (var collection in Collections)
        {
            try
            {
                await CollectionService.Add(collection.Code, collection.Title);
            }
            catch
            {
                // decide what to do if collection creation fails; for now, ignore and continue
            }
        }
    }
    private async Task SeedFeatures()
    {
        foreach (var feature in Features)
        {
            try
            {
                await FeatureService.Add(feature.Code,feature.Name, feature.symbolicText);
            }
            catch
            {
                // decide what to do if feature creation fails; for now, ignore and continue
            }
        }
    }
    private async Task SeedProducts()
    {
        var seeds = new List<CreateProductRequest>
        {
            new CreateProductRequest
            {
                ProductName = "Onam Floral Ring",
                Description = "Celebrate the spirit of Onam with this exquisite floral ring, featuring vibrant blooms and intricate craftsmanship that captures the essence of the festival.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 199.99,
                CategoryCode = Category.RI.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { "FL" },
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                ProductName = "Christmas Mirror Pendant",
                Description = "Add a touch of festive sparkle to your holiday ensemble with this Christmas mirror pendant, reflecting the joy and warmth of the season in its shimmering design.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 249.5,
                CategoryCode = Category.PD.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { "MR", "PL"},
                CollectionCode = Collection.CMS.ToString(),
                YearCode = 2023,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                ProductName = "Ocean Sea Elements Necklace",
                Description = "Dive into the beauty of the ocean with this sea elements necklace, featuring a blend of shells, stones, and marine-inspired charms that evoke the tranquility and allure of the sea.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 299.0,
                CategoryCode = Category.NK.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { "SE", "ST"},
                CollectionCode = Collection.OCN.ToString(),
                YearCode = 2024,
                SequenceCode = 2
            },
            new CreateProductRequest
            {
                ProductName = "Nature Embedded Bracelet",
                Description = "Embrace the beauty of nature with this embedded bracelet, showcasing real flowers and natural elements encased in resin, creating a wearable piece of art that celebrates the wonders of the outdoors.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 149.75,
                CategoryCode = Category.BR.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { "EM" },
                CollectionCode = Collection.NAT.ToString(),
                YearCode = 2022,
                SequenceCode = 1
            },
            new CreateProductRequest
            {
                ProductName = "Traditional Glitter Earrings",
                Description = "Add a touch of sparkle to your traditional attire with these glitter earrings, featuring a dazzling design that combines classic elegance with a modern twist, perfect for festive occasions and celebrations.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 129.0,
                CategoryCode = Category.ER.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { "GL", "SC"},
                CollectionCode = Collection.TRD.ToString(),
                YearCode = 2021,
                SequenceCode = 3
            },
            new CreateProductRequest
            {
                ProductName = "Spotlight Multi-color Ring",
                Description = "Make a bold statement with this spotlight multi-color ring, featuring a vibrant array of stones that catch the light from every angle, creating a dazzling display of color and brilliance on your finger.",
                Price = 219.99,
                CategoryCode = Category.RI.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { "MC", "FD"},
                CollectionCode = Collection.SLT.ToString(),
                YearCode = 2024,
                SequenceCode = 4
            },
            new CreateProductRequest
            {
                ProductName = "Signature Pendant Metallic",
                Description = "Elevate your style with this signature pendant metallic, featuring a sleek and modern design adorned with metallic beads that add a touch of sophistication and shine to any outfit.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 329.0,
                CategoryCode = Category.PD.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { "MT", "PL"},
                CollectionCode = Collection.SGN.ToString(),
                YearCode = 2020,
                SequenceCode = 2
            },
            new CreateProductRequest
            {
                ProductName = "Vintage Framed Necklace",
                Description = "Step back in time with this vintage framed necklace, featuring an elegant pendant encased in a delicate frame, evoking the charm and sophistication of bygone eras for a timeless accessory.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 279.5,
                CategoryCode = Category.NK.ToString(),
                Material = Material.CY.ToString(),
                FeatureCodes = new[] { "FR", "PS" },
                CollectionCode = Collection.VIN.ToString(),
                YearCode = 2019,
                SequenceCode = 5
            },
            new CreateProductRequest
            {
                ProductName = "Onam Custom Charm Bracelet",
                Description = "Celebrate Onam with a custom charm bracelet, featuring a collection of meaningful charms that represent the spirit of the festival, allowing you to create a personalized piece that tells your unique story.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 159.0,
                CategoryCode = Category.BR.ToString(),
                Material = Material.BD.ToString(),
                FeatureCodes = new[] { "CH", "PR"},
                CollectionCode = Collection.ONM.ToString(),
                YearCode = 2024,
                SequenceCode = 6
            },
            new CreateProductRequest
            {
                ProductName = "Spotlight Waves Earrings",
                Description = "Make a splash with these spotlight waves earrings, featuring a dynamic design that captures the movement and energy of ocean waves, creating a stunning accessory that adds a touch of drama to your look.",
                Specifications = new[] { "Available in sizes 6-10", "Adjustable band for a comfortable fit" },
                ProductCareInstructions = new[] { "Avoid contact with water and chemicals", "Store in a dry place" },
                Price = 139.99,
                CategoryCode = Category.ER.ToString(),
                Material = Material.RS.ToString(),
                FeatureCodes = new[] { "WV", "IN" },
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
                    // Use a static image file stored under the API project's images folder
                    var imagesFolder = Path.Combine(Env.ContentRootPath, "images");
                    Directory.CreateDirectory(imagesFolder);
                    var imagePath = Path.Combine(imagesFolder, "seed.png");
                    var base64Path = Path.Combine(imagesFolder, "seed.png.base64");

                    if (!System.IO.File.Exists(imagePath))
                    {
                        if (System.IO.File.Exists(base64Path))
                        {
                            var b64 = await System.IO.File.ReadAllTextAsync(base64Path);
                            try
                            {
                                var bytes = Convert.FromBase64String(b64);
                                await System.IO.File.WriteAllBytesAsync(imagePath, bytes);
                            }
                            catch
                            {
                                // ignore failures; fallthrough
                            }
                        }
                        else
                        {
                            // fallback: write a minimal 1x1 PNG from embedded base64
                            var minimal = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII=";
                            var bytes = Convert.FromBase64String(minimal);
                            await System.IO.File.WriteAllBytesAsync(imagePath, bytes);
                        }
                    }

                    if (System.IO.File.Exists(imagePath))
                    {
                        var imgBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                        var fileUpload = new FileUpload
                        {
                            SkuId = sku,
                            ImageId = Guid.NewGuid().ToString(),
                            ContentType = "image/png",
                            Content = new MemoryStream(imgBytes),
                            Extension = ".png"
                        };

                        try
                        {
                            var url = await ProductImageService.UploadAsync(fileUpload, true);
                        }
                        catch
                        {
                            // decide what to do if upload fails
                        }
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

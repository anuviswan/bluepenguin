using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using BP.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

using Azure.Storage.Blobs;

public class SeedController(IProductController productController, IProductImageService productImageService, IWebHostEnvironment env, BP.Domain.Repository.IProductRepository productRepository, BP.Domain.Repository.IProductImageRepository productImageRepository, BlobContainerClient blobContainer) : Controller
{
    private IProductController ProductController => productController;
    private IProductImageService ProductImageService => productImageService;
    private IWebHostEnvironment Env => env;
    private BP.Domain.Repository.IProductRepository ProductRepository => productRepository;
    private BP.Domain.Repository.IProductImageRepository ProductImageRepository => productImageRepository;
    private BlobContainerClient BlobContainer => blobContainer;

    [AllowAnonymous]
    [HttpPost]
    [Route("seed/execute")]
    public async Task<IActionResult> ExecuteSeed()
    {
        await SeedProducts();
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("seed/clear")]
    public async Task<IActionResult> ClearSeed()
    {
        try
        {
            var allProducts = await ProductRepository.GetAll();
            var prodList = allProducts?.ToList() ?? new List<BP.Domain.Entities.ProductEntity>();
            int deletedProducts = 0;
            int deletedImages = 0;

            // Instead of fetching and deleting each row individually (may miss untracked items),
            // delete all image metadata rows, then attempt to delete all blobs in the container,
            // then delete all product rows. This avoids relying on GetAll for complete state.

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

    private async Task SeedProducts()
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

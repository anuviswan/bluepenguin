using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Types;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading;

namespace BP.PriceTracker.ViewModels;

public partial class SearchResultViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly INavigationCacheService _cacheService;
    private readonly ILogger<SearchResultViewModel> _logger;

    public SearchResultViewModel(IProductService productService, INavigationCacheService cacheService, ILogger<SearchResultViewModel> logger)
    {
        _productService = productService;
        _cacheService = cacheService;
        _logger = logger;
        Products = new ObservableCollection<ProductDto>();
    }

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private ObservableCollection<ProductDto> products = new();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            await LoadProductDetailsAsync();
            await LoadImagesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading search results");
            await Snackbar.Make("Unable to retrieve search results", async () => await LoadDataAsync(), "Retry", TimeSpan.FromSeconds(5)).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadProductDetailsAsync()
    {
        // Read selected filters from navigation cache
        var selectedCategories = _cacheService.Get<IEnumerable<TagItemEntry>>("SelectedCategories")?.Select(t => t.Id) ?? Enumerable.Empty<string>();
        var selectedMaterials = _cacheService.Get<IEnumerable<TagItemEntry>>("SelectedMaterials")?.Select(t => t.Id) ?? Enumerable.Empty<string>();
        var selectedCollections = _cacheService.Get<IEnumerable<TagItemEntry>>("SelectedCollections")?.Select(t => t.Id) ?? Enumerable.Empty<string>();
        var selectedFeatures = _cacheService.Get<IEnumerable<TagItemEntry>>("SelectedFeatures")?.Select(t => t.Id) ?? Enumerable.Empty<string>();
        var selectedYears = _cacheService.Get<IEnumerable<TagItemEntry>>("SelectedYears")?.Select(t => t.Id) ?? Enumerable.Empty<string>();

        var filters = new SearchFilter
        {
            Categories = selectedCategories.ToArray(),
            Materials = selectedMaterials.ToArray(),
            Collections = selectedCollections.ToArray(),
            Features = selectedFeatures.ToArray(),
            YearCodes = selectedYears.ToArray()
        };

        var results = await _productService.SearchProductsAsync(filters);

        Products = new ObservableCollection<ProductDto>(results);
    }

    private async Task LoadImagesAsync()
    {
        // Limit concurrency
        var sem = new SemaphoreSlim(4);
        var tasks = Products.Select(async product =>
        {
            await sem.WaitAsync();
            try
            {
                var imageDataUrl = await _productService.GetPrimaryImageForSkuAsync(product.SKU);
                if (!string.IsNullOrWhiteSpace(imageDataUrl))
                {
                    // update product in-place on UI thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var index = Products.IndexOf(product);
                        if (index >= 0)
                        {
                            Products[index] = product with { PrimaryImageUrl = imageDataUrl };
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to load image for {Sku}", product.SKU);
            }
            finally
            {
                sem.Release();
            }
        }).ToList();

        await Task.WhenAll(tasks);
    }
}

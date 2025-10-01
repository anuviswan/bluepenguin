using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class MaterialsViewModel(IProductService productService, INavigationCacheService cacheService, ILogger<MaterialsViewModel> logger) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TagItemEntry> tags = [];

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var materials = await productService.GetMaterialsAsync();
            Tags = new ObservableCollection<TagItemEntry>(materials.Select(c => new TagItemEntry(c.Name, c.Id, false)));
        }
        catch (Exception e)
        {
            IsBusy = false;
            logger.LogError("Failed to load materials {0}", e.Message);
            await Snackbar.Make("Unable to retrieve materials", async () => await LoadDataAsync(), "Retry", new TimeSpan(0, 0, 5)).Show();

        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedMaterials", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.FeatureView);
    }
}

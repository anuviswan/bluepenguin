using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class FeaturesViewModel(IProductService productService, INavigationCacheService cacheService, ILogger<FeaturesViewModel> logger) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TagItemEntry> tags = [];

    [ObservableProperty]
    private bool isBusy;


    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if(IsBusy)
            return;
        try
        {
            IsBusy = true;
            var features = await productService.GetFeaturesAsync();
            Tags = new ObservableCollection<TagItemEntry>(features.Select(c => new TagItemEntry(c.Name, c.Id, false)));
        }
        catch (Exception e)
        {
            IsBusy = false;
            logger.LogError("Failed to load features {0}", e.Message);
            await Snackbar.Make("Unable to retrieve features", async () => await LoadDataAsync(), "Retry", new TimeSpan(0, 0, 5)).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedFeatures", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.CollectionView);
    }

    [RelayCommand]
    private Task Clear()
    {
        cacheService.Remove("SelectedFeatures");
        for (int i = 0; i < Tags.Count; i++)
        {
            var tag = Tags[i];
            Tags[i] = tag with { IsSelected = false };
        }
        OnPropertyChanged(nameof(Tags));
        return Task.CompletedTask;
    }
}

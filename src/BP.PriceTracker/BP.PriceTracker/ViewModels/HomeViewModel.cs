using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class HomeViewModel(IProductService productService,INavigationCacheService cacheService, ILogger<HomeViewModel> logger): ObservableObject
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
            var categories = await productService.GetCategoriesAsync();
            Tags = new ObservableCollection<TagItemEntry>(categories.Select(c => new TagItemEntry(c.Name, c.Id, false)));
        }
        catch (Exception e)
        {
            IsBusy = false;
            logger.LogError("Failed to load categories {0}", e.Message);
            await Snackbar.Make("Unable to retrieve categories", async ()=> await LoadDataAsync(), "Retry", new TimeSpan(0,0,5)).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedCategories", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.MaterialView);
    }
}

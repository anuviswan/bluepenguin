using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class YearsViewModel(INavigationCacheService cacheService, ILogger<YearsViewModel> logger):ObservableObject
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
            var collections = Enumerable.Range(2023, (DateTime.Today.Year - 2023) + 1).Select(x=> new TagItemEntry(x.ToString(), x.ToString(), false)).ToList();
            Tags = new ObservableCollection<TagItemEntry>(collections);
        }
        catch (Exception e)
        {
            IsBusy = false;
            logger.LogError("Failed to load Year List {0}", e.Message);
            await Snackbar.Make("Unable to retrieve Year list", async () => await LoadDataAsync(), "Retry", new TimeSpan(0, 0, 5)).Show();
        }
        finally
        {
            IsBusy = false;
        }

    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedYears", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.SearchListView);
    }
}

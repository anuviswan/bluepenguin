using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class HomeViewModel(IProductService productService,INavigationCacheService cacheService, ILogger<HomeViewModel> logger): ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TagItemEntry> tags = new ();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var categories = await productService.GetCategoriesAsync();
        Tags = new ObservableCollection<TagItemEntry>(categories.Select(c => new TagItemEntry(c.Name, c.Id, false)));
    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedCategories", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.MaterialView);
    }
}

using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class MaterialsViewModel(IProductService productService, INavigationCacheService cacheService, ILogger<MaterialsViewModel> logger) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TagItemEntry> tags = new();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var materials = await productService.GetMaterialsAsync();
        Tags = new ObservableCollection<TagItemEntry>(materials.Select(c => new TagItemEntry(c.Name, c.Id, false)));
    }

    [RelayCommand]
    private async Task MoveNext()
    {
        cacheService.Add<IEnumerable<TagItemEntry>>("SelectedMaterials", Tags.Where(t => t.IsSelected));
        await Shell.Current.GoToAsync(Constants.Routes.FeatureView);
    }
}

using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.PriceTracker.ViewModels;

public partial class MaterialsViewModel(IProductService productService, INavigationCacheService cacheService, ILogger<HomeViewModel> logger) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TagItemEntry> tags = new();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var materials = await productService.GetMaterialsAsync();
        Tags = new ObservableCollection<TagItemEntry>(materials.Select(c => new TagItemEntry(c.Name, c.Id, false)));
    }
}

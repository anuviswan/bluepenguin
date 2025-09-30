using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public partial class HomeViewModel: ObservableObject
{
    public ObservableCollection<TagItemEntry> Tags { get; set; } = [
        new TagItemEntry("Electronics 1", false),
        new TagItemEntry("Electronics 2", false),
        ];

    [RelayCommand]
    private Task LoadDataAsync()
    {
        // TODO : Call your data loading logic here
        return Task.CompletedTask;
    }

}

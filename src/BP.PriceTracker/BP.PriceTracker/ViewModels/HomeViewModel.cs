using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BP.PriceTracker.ViewModels;

public class HomeViewModel: ObservableObject
{
    public ObservableCollection<TagItemEntry> Tags { get; set; } = [
        new TagItemEntry("Electronics 1", false),
        new TagItemEntry("Electronics 2", false),
        ];

}
public record TagItemEntry(string Title, bool IsSelected);

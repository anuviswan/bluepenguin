using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BP.PriceTracker.ViewModels;

public partial class SearchResultViewModel:ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            // Load Data from Api   

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }
}

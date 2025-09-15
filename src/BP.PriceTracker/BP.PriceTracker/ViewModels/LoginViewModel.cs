using CommunityToolkit.Mvvm.ComponentModel;

namespace BP.PriceTracker.ViewModels;

public partial class LoginViewModel: ObservableObject
{
    [ObservableProperty]
    private string passKey;
}

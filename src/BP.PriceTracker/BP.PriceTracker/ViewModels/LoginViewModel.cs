using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Extensions.Logging;

namespace BP.PriceTracker.ViewModels;

public partial class LoginViewModel(ILogger<LoginViewModel> logger, IUserService userService) : ObservableObject
{

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteLoginCommand))]
    private string? passKey;

    [RelayCommand(CanExecute = nameof(CanExecuteLogin))]
    private async Task ExecuteLogin()
    {
        var response = await userService.ValidateUser(PassKey);

        if (response?.IsAuthenticated == true)
        {

        }
        else
        {
            await Snackbar.Make("Invalid PassKey").Show();
        }

    }

    public bool CanExecuteLogin()
    {
        return !string.IsNullOrWhiteSpace(PassKey);
    }
}

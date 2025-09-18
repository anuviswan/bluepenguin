using BP.PriceTracker.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BP.PriceTracker.ViewModels;

public partial class LoginViewModel(ILogger<LoginViewModel> logger, IUserService userService) : ObservableObject
{

    [ObservableProperty]
    private string passKey;

    [RelayCommand(CanExecute = nameof(CanExecuteLogin))]
    private void ExecuteLoginCommand()
    {
        userService.ValidateUser(PassKey);
    }

    public bool CanExecuteLogin()
    {
        return !string.IsNullOrWhiteSpace(PassKey);
    }
}

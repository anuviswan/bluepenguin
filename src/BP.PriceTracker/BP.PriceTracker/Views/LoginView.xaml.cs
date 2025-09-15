using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}
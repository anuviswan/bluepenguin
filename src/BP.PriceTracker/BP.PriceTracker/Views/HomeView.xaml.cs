using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class HomeView : ContentPage
{
	public HomeView(HomeViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}
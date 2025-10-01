using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class FeaturesView : ContentPage
{
	public FeaturesView(FeaturesViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}
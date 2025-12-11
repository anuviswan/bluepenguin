using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class YearsView : ContentPage
{
	public YearsView(YearsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}
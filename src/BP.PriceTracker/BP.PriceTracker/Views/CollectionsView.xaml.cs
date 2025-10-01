using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class CollectionsView : ContentPage
{
	public CollectionsView(CollectionsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}
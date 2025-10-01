using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class MaterialsView : ContentPage
{
	public MaterialsView(MaterialsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }
}
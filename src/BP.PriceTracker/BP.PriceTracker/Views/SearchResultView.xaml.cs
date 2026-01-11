using BP.PriceTracker.ViewModels;

namespace BP.PriceTracker.Views;

public partial class SearchResultView : ContentPage
{
	public SearchResultView(SearchResultViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}
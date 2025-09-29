namespace BP.PriceTracker.Controls.Controls;

public partial class TagItem : ContentView
{
	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(TagItem), string.Empty);

	public static readonly BindableProperty IsSelectedProperty =
		BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TagItem), false);



    public string Text 
	{ 
		get => (string)GetValue(TextProperty); 
		set => SetValue(TextProperty, value); 
	}

    public bool IsSelected 
	{ 
		get => (bool)GetValue(IsSelectedProperty); 
		set => SetValue(IsSelectedProperty, value); 
	}

    public TagItem()
	{
		InitializeComponent();
    }

	private void OnTagTapped(object sender, EventArgs e)
	{
		IsSelected = !IsSelected;
    }

    private void OnTagTapped(object sender, TappedEventArgs e)
    {

    }
}
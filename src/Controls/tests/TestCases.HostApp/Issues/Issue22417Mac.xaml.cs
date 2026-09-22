using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 22417, "[Mac] Size is not correctly assigned in the CarouselView", PlatformAffected.MacCatalyst)]
	public partial class Issue22417Mac : ContentPage
	{
		ObservableCollection<Issue22000Model> _exampleItems = new();

		public Issue22417Mac()
		{
			InitializeComponent();

			_exampleItems.Add(new Issue22000Model("First", "First CarouselView item - should fill width", Colors.Red));
			_exampleItems.Add(new Issue22000Model("Second", "Second CarouselView item - should fill width", Colors.LightBlue));
			_exampleItems.Add(new Issue22000Model("Third", "Third CarouselView item - should fill width", Colors.Pink));
			_exampleItems.Add(new Issue22000Model("Fourth", "Fourth CarouselView item - should fill width", Colors.GreenYellow));
			_exampleItems.Add(new Issue22000Model("Fifth", "Fifth CarouselView item - should fill width", Colors.Purple));

			TestCarouselView.ItemsSource = _exampleItems;
			
			// Update size info when layout changes
			TestCarouselView.SizeChanged += OnCarouselViewSizeChanged;
		}

		void OnAddItemClicked(object sender, EventArgs e)
		{
			_exampleItems.Add(new Issue22000Model("Sixth", "Sixth CarouselView item - should fill width", Colors.CornflowerBlue));
			TestCarouselView.ScrollTo(5, animate: false);
		}
		
		void OnCarouselViewSizeChanged(object? sender, EventArgs e)
		{
			var width = TestCarouselView.Width;
			var height = TestCarouselView.Height;
			SizeInfoLabel.Text = $"Size: {width:F1} x {height:F1}";
		}
	}
}
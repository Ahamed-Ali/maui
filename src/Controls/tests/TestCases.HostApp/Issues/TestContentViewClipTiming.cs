using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 999998, "Test ContentView clip timing in Border", PlatformAffected.UWP)]
	public class TestContentViewClipTiming : ContentPage
	{
		public TestContentViewClipTiming()
		{
			var stackLayout = new StackLayout();

			var border = new Border
			{
				HeightRequest = 200,
				BackgroundColor = Colors.Green
			};

			var customView = new CustomViewWithImmedateClip
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "CustomView"
			};

			var label = new Label
			{
				Text = "immediate clip",
				TextColor = Colors.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			customView.Content = label;
			border.Content = customView;
			stackLayout.Children.Add(border);

			Content = stackLayout;
		}

		public class CustomViewWithImmedateClip : ContentView
		{
			public CustomViewWithImmedateClip()
			{
				this.BackgroundColor = Colors.Red;
				// Set the clip immediately in constructor
				this.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, 100, 100));
			}
		}
	}
}
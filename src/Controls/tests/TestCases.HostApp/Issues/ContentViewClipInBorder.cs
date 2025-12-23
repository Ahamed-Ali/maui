using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 3, "ContentView clip not being applied when inside Border on Windows", PlatformAffected.UWP)]
	public class ContentViewClipInBorder : ContentPage
	{
		public ContentViewClipInBorder()
		{
			var border = new Border
			{
				HeightRequest = 200,
				BackgroundColor = Colors.Green,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			var customView = new CustomContentView
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "CustomContentView"
			};

			border.Content = customView;
			Content = border;
		}

		private class CustomContentView : ContentView
		{
			public CustomContentView()
			{
				BackgroundColor = Colors.Red;
				Content = new Label
				{
					Text = "Clipped Content",
					TextColor = Colors.White,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};

				SizeChanged += OnSizeChanged;
			}

			private void OnSizeChanged(object sender, EventArgs e)
			{
				if (Width > 0 && Height > 0)
				{
					Clip = new RoundRectangleGeometry(6, new Rect(0, 0, Width, Height));
				}
			}
		}
	}
}
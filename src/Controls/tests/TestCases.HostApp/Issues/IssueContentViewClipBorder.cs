using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 999999, "ContentView clip is not updated when wrapping inside the Border in Windows", PlatformAffected.UWP)]
	public class IssueContentViewClipBorder : ContentPage
	{
		public IssueContentViewClipBorder()
		{
			var stackLayout = new StackLayout();

			var border = new Border
			{
				HeightRequest = 200,
				BackgroundColor = Colors.Green
			};

			var customView = new CustomView
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "CustomView"
			};

			var label = new Label
			{
				Text = "label",
				TextColor = Colors.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			customView.Content = label;
			border.Content = customView;
			stackLayout.Children.Add(border);

			Content = stackLayout;
		}

		public class CustomView : ContentView
		{
			public CustomView()
			{
				this.BackgroundColor = Colors.Red;
				this.SizeChanged += CustomView_SizeChanged;
			}

			private void CustomView_SizeChanged(object sender, EventArgs e)
			{
				if (this.Width > 0 && this.Height > 0)
				{
					this.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, this.Width, this.Height));
				}
			}
		}
	}
}
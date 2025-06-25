using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 999997, "Compare ContentView clip with and without Border", PlatformAffected.UWP)]
	public class CompareContentViewClip : ContentPage
	{
		public CompareContentViewClip()
		{
			var stackLayout = new StackLayout();

			// Test 1: ContentView with clip NOT inside border
			var label1 = new Label { Text = "ContentView with clip (no border):", FontAttributes = FontAttributes.Bold };
			stackLayout.Children.Add(label1);

			var customView1 = new CustomView
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "CustomViewNoBorder"
			};

			var label1Content = new Label
			{
				Text = "no border",
				TextColor = Colors.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			customView1.Content = label1Content;
			stackLayout.Children.Add(customView1);

			// Test 2: ContentView with clip INSIDE border
			var label2 = new Label { Text = "ContentView with clip inside border:", FontAttributes = FontAttributes.Bold };
			stackLayout.Children.Add(label2);

			var border = new Border
			{
				HeightRequest = 200,
				BackgroundColor = Colors.Green
			};

			var customView2 = new CustomView
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "CustomViewInBorder"
			};

			var label2Content = new Label
			{
				Text = "in border",
				TextColor = Colors.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			customView2.Content = label2Content;
			border.Content = customView2;
			stackLayout.Children.Add(border);

			Content = stackLayout;
		}

		public class CustomView : ContentView
		{
			public CustomView()
			{
				this.BackgroundColor = Colors.Red;
				// Set clip immediately to test if basic clipping works
				this.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, 100, 100));
				this.SizeChanged += CustomView_SizeChanged;
			}

			private void CustomView_SizeChanged(object sender, EventArgs e)
			{
				if (this.Width > 0 && this.Height > 0)
				{
					// Update clip with actual size
					this.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, this.Width, this.Height));
				}
			}
		}
	}
}
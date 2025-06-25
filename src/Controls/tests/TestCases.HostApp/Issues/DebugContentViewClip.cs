using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 999996, "Debug ContentView clip behavior", PlatformAffected.UWP)]
	public class DebugContentViewClip : ContentPage
	{
		public DebugContentViewClip()
		{
			var stackLayout = new StackLayout();

			var border = new Border
			{
				HeightRequest = 200,
				BackgroundColor = Colors.Green
			};

			var customView = new DebugCustomView
			{
				HeightRequest = 100,
				WidthRequest = 100,
				AutomationId = "DebugCustomView"
			};

			var label = new Label
			{
				Text = "debug",
				TextColor = Colors.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			customView.Content = label;
			border.Content = customView;
			stackLayout.Children.Add(border);

			Content = stackLayout;
		}

		public class DebugCustomView : ContentView
		{
			public DebugCustomView()
			{
				this.BackgroundColor = Colors.Red;
				Debug.WriteLine($"[DEBUG] DebugCustomView constructor - Handler: {this.Handler}");
				this.SizeChanged += DebugCustomView_SizeChanged;
				this.HandlerChanged += DebugCustomView_HandlerChanged;
			}

			private void DebugCustomView_HandlerChanged(object sender, EventArgs e)
			{
				Debug.WriteLine($"[DEBUG] HandlerChanged - Handler: {this.Handler}");
				if (this.Handler != null)
				{
					Debug.WriteLine($"[DEBUG] Handler type: {this.Handler.GetType().Name}");
#if WINDOWS
					if (this.Handler is Microsoft.Maui.Handlers.ViewHandler<Microsoft.Maui.IContentView, Microsoft.Maui.Platform.ContentPanel> viewHandler)
					{
						Debug.WriteLine($"[DEBUG] HasContainer: {viewHandler.HasContainer}");
						Debug.WriteLine($"[DEBUG] ContainerView: {viewHandler.ContainerView}");
						Debug.WriteLine($"[DEBUG] PlatformView: {viewHandler.PlatformView}");
					}
#endif
				}
			}

			private void DebugCustomView_SizeChanged(object sender, EventArgs e)
			{
				Debug.WriteLine($"[DEBUG] SizeChanged - Size: {this.Width}x{this.Height}");
				if (this.Width > 0 && this.Height > 0)
				{
					Debug.WriteLine($"[DEBUG] Setting clip...");
					this.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, this.Width, this.Height));
					Debug.WriteLine($"[DEBUG] Clip set - Clip: {this.Clip}");
					
#if WINDOWS
					if (this.Handler is Microsoft.Maui.Handlers.ViewHandler<Microsoft.Maui.IContentView, Microsoft.Maui.Platform.ContentPanel> viewHandler)
					{
						Debug.WriteLine($"[DEBUG] After clip - HasContainer: {viewHandler.HasContainer}");
						Debug.WriteLine($"[DEBUG] After clip - ContainerView: {viewHandler.ContainerView}");
					}
#endif
				}
			}
		}
	}
}
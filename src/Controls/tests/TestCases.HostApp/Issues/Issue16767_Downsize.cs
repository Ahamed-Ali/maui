using System.Reflection;
using Maui.Controls.Sample.Issues;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Storage;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Controls.TestCases.HostApp.Issues;

[Issue(IssueTracker.Github, 28725, "Downsize function in W2DImage class", PlatformAffected.UWP)]

public class Issue16767_DownSize : TestContentPage
{
	GraphicsView downSizeGraphicsView;
	Issue16767_DownsizeDrawable downSizeDrawable;

	protected override void Init()
	{
		var rootLayout = new VerticalStackLayout();

		downSizeGraphicsView = new GraphicsView()
		{
			BackgroundColor = Colors.Red,
			HeightRequest = 400,
			WidthRequest = 400
		};

		Label descriptionLabel = new Label()
		{
			Text = "The test should pass if the image is displayed and resized correctly. If the image is not displayed or not downsized correctly, the test has failed.",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			AutomationId = "descriptionLabel"
		};

		downSizeDrawable = new Issue16767_DownsizeDrawable();
		downSizeGraphicsView.Drawable = downSizeDrawable;
		rootLayout.Add(downSizeGraphicsView);

		rootLayout.Add(descriptionLabel);
		Content = rootLayout;
		
		// Pre-load the image asynchronously to avoid threading issues in Draw method
		_ = LoadImageAsync();
	}
	
	async Task LoadImageAsync()
	{
		try
		{
			// Try the PlatformImageLoadingService approach used in working DeviceTests
			var service = new PlatformImageLoadingService();
			using var stream = await FileSystem.OpenAppPackageFileAsync("royals.png");
			
			System.Diagnostics.Debug.WriteLine($"Original stream length: {stream.Length}");
			System.Diagnostics.Debug.WriteLine("About to call service.FromStream");
			
			var image = service.FromStream(stream);
			if (image != null && image.Width > 0 && image.Height > 0)
			{
				System.Diagnostics.Debug.WriteLine($"Successfully loaded image: {image.Width}x{image.Height}");
				downSizeDrawable.SetImage(image);
				return;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("service.FromStream returned null or invalid image, trying fallback");
				throw new InvalidOperationException("Failed to load image from FileSystem");
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Primary load failed: {ex.Message}, trying fallback");
			// Fallback to embedded resource approach
			var assembly = GetType().GetTypeInfo().Assembly;
			using var stream = assembly.GetManifestResourceStream("Controls.TestCases.HostApp.Resources.Images.royals.png");
			if (stream != null)
			{
				System.Diagnostics.Debug.WriteLine($"Fallback stream length: {stream.Length}");
				var service = new PlatformImageLoadingService();
				var image = service.FromStream(stream);
				if (image != null && image.Width > 0 && image.Height > 0)
				{
					System.Diagnostics.Debug.WriteLine($"Fallback successfully loaded image: {image.Width}x{image.Height}");
					downSizeDrawable.SetImage(image);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Fallback also failed to load image");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Embedded resource stream is null");
			}
		}
	}
}

public class Issue16767_DownsizeDrawable : IDrawable
{
	IImage _image;
	
	internal void SetImage(IImage image)
	{
		_image = image;
	}

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		if (_image is not null)
		{
			float spacing = 20;
			float currentY = 0;

			canvas.FontColor = Colors.Black;
			canvas.FontSize = 16;

			// Label before first image
			canvas.DrawString("Downsize (100, 200)", 0, currentY, dirtyRect.Width, 30, HorizontalAlignment.Left, VerticalAlignment.Top);
			currentY += 30;

			var downsized1 = _image.Downsize(100, 200);
			canvas.SetFillImage(downsized1);
			canvas.FillRectangle(0, currentY, 240, downsized1.Height);
			currentY += downsized1.Height + spacing;

			// Label before second image
			canvas.DrawString("Downsize (100)", 0, currentY, dirtyRect.Width, 30, HorizontalAlignment.Left, VerticalAlignment.Top);
			currentY += 30;

			var downsized2 = _image.Downsize(100);
			canvas.SetFillImage(downsized2);
			canvas.FillRectangle(0, currentY, 240, downsized2.Height);
		}
	}
}

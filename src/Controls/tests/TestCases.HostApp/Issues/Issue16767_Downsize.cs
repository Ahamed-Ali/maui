using System.Reflection;
using Maui.Controls.Sample.Issues;
using Microsoft.Maui.Graphics.Platform;
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
	}
}

public class Issue16767_DownsizeDrawable : IDrawable
{
	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		IImage image = null;
		
		try
		{
			// Primary: Use FileSystem for MauiAsset (more reliable in net10.0)
			var task = FileSystem.OpenAppPackageFileAsync("royals.png");
			task.Wait();
			using (var stream = task.Result)
			{
				// Copy to MemoryStream to ensure stream is fully buffered before decoding
				using var memoryStream = new MemoryStream();
				stream.CopyTo(memoryStream);
				memoryStream.Position = 0;
				
				// Add defensive try-catch around the actual PlatformImage.FromStream call
				try
				{
					image = PlatformImage.FromStream(memoryStream);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"PlatformImage.FromStream failed with MauiAsset: {ex.Message}");
					throw; // Re-throw to trigger fallback
				}
			}
		}
		catch
		{
			// Fallback: Original embedded resource approach with enhanced error handling
			try
			{
				var assembly = GetType().GetTypeInfo().Assembly;
				using (var stream = assembly.GetManifestResourceStream("Controls.TestCases.HostApp.Resources.Images.royals.png"))
				{
					if (stream != null)
					{
						// Copy to MemoryStream for embedded resource too
						using var memoryStream = new MemoryStream();
						stream.CopyTo(memoryStream);
						memoryStream.Position = 0;
						
						try
						{
							image = PlatformImage.FromStream(memoryStream);
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.WriteLine($"PlatformImage.FromStream failed with embedded resource: {ex.Message}");
							// Don't re-throw, just leave image as null
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Fallback embedded resource loading failed: {ex.Message}");
			}
		}

		if (image is not null)
		{
			try
			{
				float spacing = 20;
				float currentY = 0;

				canvas.FontColor = Colors.Black;
				canvas.FontSize = 16;

				// Label before first image
				canvas.DrawString("Downsize (100, 200)", 0, currentY, dirtyRect.Width, 30, HorizontalAlignment.Left, VerticalAlignment.Top);
				currentY += 30;

				var downsized1 = image.Downsize(100, 200);
				canvas.SetFillImage(downsized1);
				canvas.FillRectangle(0, currentY, 240, downsized1.Height);
				currentY += downsized1.Height + spacing;

				// Label before second image
				canvas.DrawString("Downsize (100)", 0, currentY, dirtyRect.Width, 30, HorizontalAlignment.Left, VerticalAlignment.Top);
				currentY += 30;

				var downsized2 = image.Downsize(100);
				canvas.SetFillImage(downsized2);
				canvas.FillRectangle(0, currentY, 240, downsized2.Height);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Image processing failed: {ex.Message}");
			}
		}
	}
}

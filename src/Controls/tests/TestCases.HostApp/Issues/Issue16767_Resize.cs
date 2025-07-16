using System.Reflection;
using Maui.Controls.Sample.Issues;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Controls.TestCases.HostApp.Issues;

[Issue(IssueTracker.Github, 16767, "Resize function in W2DImage class", PlatformAffected.UWP)]

public class Issue16767_Resize : TestContentPage
{
	GraphicsView ReSizeGraphicsView;
	Issue16767_ResizeDrawable resizeDrawable;

	RadioButton resizeModeFit;
	RadioButton resizeModeBleed;
	RadioButton resizeModeStretch;
	protected override void Init()
	{

		var rootLayout = new VerticalStackLayout();

		ReSizeGraphicsView = new GraphicsView()
		{
			HeightRequest = 300,
			WidthRequest = 300
		};
		resizeDrawable = new Issue16767_ResizeDrawable();
		ReSizeGraphicsView.Drawable = resizeDrawable;
		rootLayout.Add(CreateResizeModeSelection());
		rootLayout.Add(ReSizeGraphicsView);
		Content = rootLayout;
	}

	HorizontalStackLayout CreateResizeModeSelection()
	{
		var horizontalStackLayout = new HorizontalStackLayout()
		{
			Spacing = 10,
		};

		resizeModeFit = CreateRadioButton("Fit", "ResizeModeSelection", true, "ResizeModeFit");
		resizeModeBleed = CreateRadioButton("Bleed", "ResizeModeSelection", false, "ResizeModeBleed");
		resizeModeStretch = CreateRadioButton("Stretch", "ResizeModeSelection", false, "ResizeModeStretch");

		resizeModeFit.CheckedChanged += OnResizeModeChanged;
		resizeModeBleed.CheckedChanged += OnResizeModeChanged;
		resizeModeStretch.CheckedChanged += OnResizeModeChanged;

		horizontalStackLayout.Children.Add(resizeModeFit);
		horizontalStackLayout.Children.Add(resizeModeBleed);
		horizontalStackLayout.Children.Add(resizeModeStretch);

		return horizontalStackLayout;
	}

	RadioButton CreateRadioButton(string text, string groupName, bool isChecked, string automationID)
	{
		return new RadioButton
		{
			Content = text,
			GroupName = groupName,
			IsChecked = isChecked,
			FontSize = 11,
			AutomationId = automationID
		};
	}

	void OnResizeModeChanged(object sender, CheckedChangedEventArgs e)
	{
		if (resizeModeFit.IsChecked)
		{
			resizeDrawable.SetResizeMode(ResizeMode.Fit);
			ReSizeGraphicsView.Invalidate();
		}
		else if (resizeModeBleed.IsChecked)
		{
			resizeDrawable.SetResizeMode(ResizeMode.Bleed);
			ReSizeGraphicsView.Invalidate();
		}
		else if (resizeModeStretch.IsChecked)
		{
			resizeDrawable.SetResizeMode(ResizeMode.Stretch);
			ReSizeGraphicsView.Invalidate();
		}
	}
}

public class Issue16767_ResizeDrawable : IDrawable
{
	ResizeMode _resizeMode;

	internal void SetResizeMode(ResizeMode resizeMode)
	{
		_resizeMode = resizeMode;
	}

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
				var resizedImage = image.Resize(100, 200, _resizeMode);
				canvas.SetFillImage(resizedImage);
				canvas.FillRectangle(0, 0, 200, resizedImage.Height);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Image processing failed: {ex.Message}");
			}
		}
	}
}

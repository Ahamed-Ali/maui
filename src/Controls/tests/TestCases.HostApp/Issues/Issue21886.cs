using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Storage;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Controls.TestCases.HostApp.Issues;

[Issue(IssueTracker.Github, 21886, "The original image remains undisposed even after setting disposeOriginal to true in the Resize and Downsize methods", PlatformAffected.Android | PlatformAffected.iOS)]
public class Issue21886 : ContentPage
{
	Label _originalImageStatusLabel;

	public Issue21886()
	{
		_originalImageStatusLabel = new Label
		{
			AutomationId = "OriginalImageStatusLabel",
			Text = "Status of Original Image Disposal"
		};

		VerticalStackLayout stackLayout = new VerticalStackLayout
		{
			Children =
			{
				CreateButton("Resize", OnResize),
				CreateButton("DownSize", OnDownSize),
				_originalImageStatusLabel,
			}
		};

		Content = new ScrollView { Content = stackLayout };
	}

	Button CreateButton(string text, EventHandler handler)
	{
		Button button = new Button
		{
			AutomationId = $"Issue21886{text}Btn",
			Text = text,
			HorizontalOptions = LayoutOptions.Fill
		};

		button.Clicked += handler;
		return button;
	}

	async Task<IImage> LoadImageAsync()
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
				return image;
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
					return image;
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
			
			return null;
		}
	}

	async void OnResize(object sender, EventArgs e)
	{
		var image = await LoadImageAsync();
		var res = image.Resize(10, 10, ResizeMode.Fit, true);

		UpdateStatusLabels(res, image, "Resize");
	}

	async void OnDownSize(object sender, EventArgs e)
	{
		var image = await LoadImageAsync();
		var res = image.Downsize(10, 10, true);

		UpdateStatusLabels(res, image, "Downsize");
	}

	void UpdateStatusLabels(IImage resultImage, IImage originalImage, string operation)
	{
		_originalImageStatusLabel.Text = TryAccessImage(originalImage)
			? "Success"
			: originalImage.Width == 0 && originalImage.Height == 0 ? "Success" : "Failure";
	}

	bool TryAccessImage(IImage image)
	{
		try
		{
			var _ = image.Width;
			return false;
		}
		catch (ObjectDisposedException)
		{
			return true;
		}
	}
}
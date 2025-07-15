using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Storage;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace Controls.TestCases.HostApp.Issues;

[Issue(IssueTracker.Github, 30006, "The downsized image continues to retain its original dimensions", PlatformAffected.iOS)]
public class Issue30006 : ContentPage
{
	Label _convertedImageStatusLabel;
	public Issue30006()
	{
		_convertedImageStatusLabel = new Label
		{
			AutomationId = "ConvertedImage",
			Text = "Result Image Status: "
		};

		VerticalStackLayout verticalStackLayout = new VerticalStackLayout
		{
			Padding = new Thickness(20),
			Spacing = 10,
			Children =
			{
				CreateButton("DownSize", OnDownSize),
				_convertedImageStatusLabel,
			}
		};

		Content = new ScrollView { Content = verticalStackLayout };
	}

	Button CreateButton(string text, EventHandler handler)
	{
		Button button = new Button
		{
			AutomationId = $"Issue30006DownSizeBtn",
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

	async void OnDownSize(object sender, EventArgs e)
	{
		var image = await LoadImageAsync();
		var res = image.Downsize(10);

		UpdateStatusLabels(res);
	}

	void UpdateStatusLabels(IImage resultImage)
	{
		_convertedImageStatusLabel.Text = TryAccessImage(resultImage)
		? "Success"
		: "Failure";
	}

	bool TryAccessImage(IImage downsizedImage)
	{
		if (Math.Round(downsizedImage.Width) == 10 && Math.Round(downsizedImage.Height) == 8)
		{
			return true;
		}

		return false;
	}
}
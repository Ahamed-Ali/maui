using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
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
			// Use FileSystem.OpenAppPackageFileAsync for MauiAsset approach
			using var stream = await FileSystem.OpenAppPackageFileAsync("royals.png");
			// Copy to MemoryStream to ensure stream is properly buffered for marshal methods
			using var memoryStream = new MemoryStream();
			await stream.CopyToAsync(memoryStream);
			memoryStream.Position = 0;
			return PlatformImage.FromStream(memoryStream);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to load image: {ex.Message}");
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
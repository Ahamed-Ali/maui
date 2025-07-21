using System;
using System.IO;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 19, "Android: Updating Image.Source with ImageSource.FromStream() is broken in Release builds", PlatformAffected.Android)]
	public partial class Issue19 : ContentPage
	{
		public Issue19()
		{
			InitializeComponent();
		}

		private void OnLoadImageClicked(object sender, EventArgs e)
		{
			try
			{
				StatusLabel.Text = "Loading image from stream...";

				// Create a simple red square image as byte array (Base64 encoded PNG)
				var imageBytes = Convert.FromBase64String(
					"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg=="
				);

				// Create an image source from the stream
				var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
				
				// Set the image source
				TestImage.Source = imageSource;
				
				StatusLabel.Text = "Image loaded successfully from stream";
			}
			catch (Exception ex)
			{
				StatusLabel.Text = $"Error loading image: {ex.Message}";
			}
		}
	}
}
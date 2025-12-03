#nullable enable
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using Microsoft.Extensions.Logging;

namespace Microsoft.Maui
{
	public partial class StreamImageSourceService
	{
		public override async Task<IImageSourceServiceResult?> LoadDrawableAsync(IImageSource imageSource, ImageView imageView, CancellationToken cancellationToken = default)
		{
			var streamImageSource = (IStreamImageSource)imageSource;

			if (!streamImageSource.IsEmpty)
			{
				Stream? stream = null;
				try
				{
					stream = await streamImageSource.GetStreamAsync(cancellationToken);
					// Convert stream to byte array to avoid AndroidMarshalMethod InputStreamAdapter issues in release builds
					byte[] buffer;
					int length;
					using (var memoryStream = new MemoryStream())
					{
						await stream.CopyToAsync(memoryStream, cancellationToken);
						buffer = memoryStream.GetBuffer();
						length = (int)memoryStream.Length;
					}

					// Decode the byte array into a Bitmap
					var bitmap = global::Android.Graphics.BitmapFactory.DecodeByteArray(buffer, 0, length);

					// Set the bitmap to the ImageView
					imageView.SetImageBitmap(bitmap);

					// Convert Bitmap to Drawable
					var drawable = new Android.Graphics.Drawables.BitmapDrawable(imageView.Resources, bitmap);

					// Create a result object
					var result = new ImageSourceServiceResult(drawable);

					stream?.Dispose();

					return result;
				}
				catch (Exception ex)
				{
					Logger?.LogWarning(ex, "Unable to load image stream.");
					throw;
				}
				finally
				{
					if (stream != null)
						GC.KeepAlive(stream);
				}
			}

			return null;
		}

		public override async Task<IImageSourceServiceResult<Drawable>?> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default)
		{
			var streamImageSource = (IStreamImageSource)imageSource;

			if (!streamImageSource.IsEmpty)
			{
				Stream? stream = null;

				try
				{
					stream = await streamImageSource.GetStreamAsync(cancellationToken).ConfigureAwait(false);

					// Convert stream to byte array to avoid AndroidMarshalMethod InputStreamAdapter issues in release builds
					byte[] buffer;
					int length;
					using (var memoryStream = new MemoryStream())
					{
						await stream.CopyToAsync(memoryStream, cancellationToken);
						buffer = memoryStream.GetBuffer();
						length = (int)memoryStream.Length;
					}

					// Decode the byte array into a Bitmap
					var bitmap = global::Android.Graphics.BitmapFactory.DecodeByteArray(buffer, 0, length);

					// Convert Bitmap to Drawable
					var drawable = new Android.Graphics.Drawables.BitmapDrawable(context.Resources, bitmap);

					// Create a result object
					var result = new ImageSourceServiceResult(drawable);

					stream?.Dispose();

					return result;
				}
				catch (Exception ex)
				{
					Logger?.LogWarning(ex, "Unable to load image stream.");
					throw;
				}
				finally
				{
					if (stream != null)
						GC.KeepAlive(stream);
				}
			}

			return null;
		}
	}
}
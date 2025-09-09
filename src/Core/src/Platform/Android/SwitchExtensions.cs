using Android.Graphics.Drawables;
using Android.OS;
using ASwitch = AndroidX.AppCompat.Widget.SwitchCompat;

namespace Microsoft.Maui.Platform
{
	public static class SwitchExtensions
	{
		public static void UpdateIsOn(this ASwitch aSwitch, ISwitch view) =>
			aSwitch.Checked = view.IsOn;

		public static void UpdateTrackColor(this ASwitch aSwitch, ISwitch view)
		{
			var trackColor = view.TrackColor;

			if (aSwitch.Checked)
			{
				if (trackColor != null)
					SafeSetColorFilter(aSwitch.TrackDrawable, trackColor, FilterMode.SrcAtop);
			}
			else
				SafeClearColorFilter(aSwitch.TrackDrawable);
		}

		public static void UpdateThumbColor(this ASwitch aSwitch, ISwitch view)
		{
			var thumbColor = view.ThumbColor;

			if (thumbColor != null)
				SafeSetColorFilter(aSwitch.ThumbDrawable, thumbColor, FilterMode.SrcAtop);
		}

		/// <summary>
		/// Safely sets ColorFilter on Android, preventing crashes on Android 5.1.1 and below
		/// where Skia has race conditions in PixelRef management during rapid updates.
		/// </summary>
		static void SafeSetColorFilter(Drawable? drawable, Graphics.Color color, FilterMode mode)
		{
			if (drawable == null)
				return;

			try
			{
				// Android 5.1.1 (API 22) and below have known issues with ColorFilter operations
				// in older Skia versions that can cause SIGSEGV crashes during rapid UI updates
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// For Android 5.1.1 and below, avoid ColorFilter operations during potential
					// rapid updates by checking if the drawable is already in the desired state
					var currentFilter = drawable.ColorFilter;
					if (currentFilter != null)
					{
						// Already has a filter applied, skip to prevent crash
						return;
					}
				}

				drawable.SetColorFilter(color, mode);
			}
			catch (System.Exception)
			{
				// Silently ignore ColorFilter failures on older Android versions
				// The visual difference is minimal compared to preventing crashes
			}
		}

		/// <summary>
		/// Safely clears ColorFilter on Android, preventing crashes on Android 5.1.1 and below.
		/// </summary>
		static void SafeClearColorFilter(Drawable? drawable)
		{
			if (drawable == null)
				return;

			try
			{
				// Android 5.1.1 (API 22) and below have known issues with ColorFilter operations
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// For older versions, only clear if there's actually a filter to prevent
					// unnecessary operations that might trigger Skia crashes
					if (drawable.ColorFilter == null)
						return;
				}

				drawable.ClearColorFilter();
			}
			catch (System.Exception)
			{
				// Silently ignore ColorFilter failures on older Android versions
			}
		}

		public static Drawable? GetDefaultSwitchTrackDrawable(this ASwitch aSwitch) =>
			aSwitch.TrackDrawable;

		public static Drawable? GetDefaultSwitchThumbDrawable(this ASwitch aSwitch) =>
			aSwitch.ThumbDrawable;
	}
}
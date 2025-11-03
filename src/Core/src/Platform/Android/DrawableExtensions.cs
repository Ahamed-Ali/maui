using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using AColor = Android.Graphics.Color;
using AColorFilter = Android.Graphics.ColorFilter;
using ADrawable = Android.Graphics.Drawables.Drawable;
using ADrawableCompat = AndroidX.Core.Graphics.Drawable.DrawableCompat;

namespace Microsoft.Maui.Platform
{
	public static class DrawableExtensions
	{
		[System.Runtime.Versioning.SupportedOSPlatform("android29.0")]
		public static BlendMode? GetFilterMode(FilterMode mode)
		{
			switch (mode)
			{
				case FilterMode.SrcIn:
					return BlendMode.SrcIn;
				case FilterMode.Multiply:
					return BlendMode.Multiply;
				case FilterMode.SrcAtop:
					return BlendMode.SrcAtop;
			}

			throw new Exception("Invalid Mode");
		}

		public static AColorFilter? GetColorFilter(this ADrawable drawable)
		{
			if (drawable == null)
				return null;

			return ADrawableCompat.GetColorFilter(drawable);
		}

		public static void SetColorFilter(this ADrawable drawable, AColorFilter? colorFilter)
		{
			if (drawable == null)
				return;

			try
			{
				// Android 5.1.1 (API 22) and below have known race conditions in Skia
				// ColorFilter operations that can cause SIGSEGV crashes during rapid updates
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// For older versions, check current state to avoid unnecessary operations
					var currentFilter = ADrawableCompat.GetColorFilter(drawable);
					if ((currentFilter == null && colorFilter == null) || 
						(currentFilter != null && colorFilter != null))
					{
						// Already in desired state, skip to prevent crash
						return;
					}
				}

				if (colorFilter == null)
					ADrawableCompat.ClearColorFilter(drawable);
				else
					drawable.SetColorFilter(colorFilter);
			}
			catch (System.Exception)
			{
				// Silently ignore ColorFilter failures on older Android versions
			}
		}

		public static void SetColorFilter(this ADrawable drawable, Graphics.Color color, FilterMode mode)
		{
			if (drawable == null)
				return;

			try
			{
				// Android 5.1.1 (API 22) and below have known race conditions in Skia
				// ColorFilter operations that can cause SIGSEGV crashes during rapid updates
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// For older versions, avoid redundant operations
					var currentFilter = ADrawableCompat.GetColorFilter(drawable);
					if (color == null && currentFilter == null)
					{
						// Already cleared, skip
						return;
					}
					if (color != null && currentFilter != null)
					{
						// Already has a filter, skip to prevent crash
						return;
					}
				}

				if (color != null)
					drawable.SetColorFilter(color.ToPlatform(), mode);
			}
			catch (System.Exception)
			{
				// Silently ignore ColorFilter failures on older Android versions
			}
		}

		public static void SetColorFilter(this ADrawable drawable, AColor color, FilterMode mode)
		{
			if (drawable is not null)
			{
				try
				{
					// Android 5.1.1 (API 22) and below have known race conditions in Skia
					// ColorFilter operations that can cause SIGSEGV crashes during rapid updates
					if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
					{
						// For older versions, avoid redundant operations
						var currentFilter = ADrawableCompat.GetColorFilter(drawable);
						if (currentFilter != null)
						{
							// Already has a filter, skip to prevent crash
							return;
						}
					}

					PlatformInterop.SetColorFilter(drawable, color, (int)mode);
				}
				catch (System.Exception)
				{
					// Silently ignore ColorFilter failures on older Android versions
				}
			}
		}

		public static void ClearColorFilter(this ADrawable drawable)
		{
			if (drawable == null)
				return;

			try
			{
				// Android 5.1.1 (API 22) and below have known race conditions in Skia
				// ColorFilter operations that can cause SIGSEGV crashes during rapid updates
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// For older versions, only clear if there's actually a filter
					var currentFilter = ADrawableCompat.GetColorFilter(drawable);
					if (currentFilter == null)
					{
						// No filter to clear, skip
						return;
					}
				}

				ADrawableCompat.ClearColorFilter(drawable);
			}
			catch (System.Exception)
			{
				// Silently ignore ColorFilter failures on older Android versions
			}
		}

		internal static IAnimatable? AsAnimatable(this ADrawable? drawable)
		{
			if (drawable is null)
				return null;

			if (drawable is IAnimatable animatable)
				return animatable;

			if (PlatformInterop.GetAnimatable(drawable) is IAnimatable javaAnimatable)
				return javaAnimatable;

			return null;
		}
	}
}
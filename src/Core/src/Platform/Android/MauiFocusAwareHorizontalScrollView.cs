using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Microsoft.Maui.Platform
{
	/// <summary>
	/// Extensions for HorizontalScrollView to handle focus behavior during scroll gestures
	/// </summary>
	public static class HorizontalScrollViewExtensions
	{
		// Use unique hash codes as tag keys to avoid conflicts
		private static readonly int TouchStartXTag = "MauiFocusFix_TouchStartX".GetHashCode(StringComparison.Ordinal);
		private static readonly int TouchStartYTag = "MauiFocusFix_TouchStartY".GetHashCode(StringComparison.Ordinal);
		private static readonly int IsScrollingTag = "MauiFocusFix_IsScrolling".GetHashCode(StringComparison.Ordinal);

		/// <summary>
		/// Prevents child views from gaining focus during scroll gestures in a HorizontalScrollView.
		/// This addresses the issue where Entry controls gain focus when swiping in a HorizontalScrollView.
		/// Call this method once after creating and configuring your HorizontalScrollView.
		/// </summary>
		/// <param name="horizontalScrollView">The HorizontalScrollView to apply the fix to</param>
		public static void PreventFocusDuringScroll(this HorizontalScrollView horizontalScrollView)
		{
			if (horizontalScrollView == null)
				return;

			// Remove any existing listeners to avoid duplicate registration
			horizontalScrollView.Touch -= OnHorizontalScrollViewTouch;
			
			// Add the touch listener
			horizontalScrollView.Touch += OnHorizontalScrollViewTouch;
		}

		private static void OnHorizontalScrollViewTouch(object? sender, View.TouchEventArgs e)
		{
			if (sender is not HorizontalScrollView scrollView || e.Event == null)
			{
				e.Handled = false;
				return;
			}

			var ev = e.Event;

			// Handle scroll gestures to prevent unwanted focus changes
			switch (ev.Action)
			{
				case MotionEventActions.Down:
					// Store initial touch position for gesture detection
					scrollView.SetTag(TouchStartXTag, ev.GetX());
					scrollView.SetTag(TouchStartYTag, ev.GetY());
					scrollView.SetTag(IsScrollingTag, false);
					break;

				case MotionEventActions.Move:
					// Check if this is a scroll gesture
					var startX = scrollView.GetTag(TouchStartXTag) as Java.Lang.Float;
					var startY = scrollView.GetTag(TouchStartYTag) as Java.Lang.Float;
					
					if (startX != null && startY != null)
					{
						var deltaX = Math.Abs(ev.GetX() - startX.FloatValue());
						var deltaY = Math.Abs(ev.GetY() - startY.FloatValue());
						
						// Use system touch slop for consistent behavior
						var touchSlop = scrollView.Context != null ? 
							ViewConfiguration.Get(scrollView.Context)?.ScaledTouchSlop ?? 10 : 10;
						
						// If horizontal movement is greater than vertical and exceeds touch slop, it's a scroll
						if (deltaX > deltaY && deltaX > touchSlop)
						{
							scrollView.SetTag(IsScrollingTag, true);
							
							// Clear any focus from child views during scroll
							ClearChildFocus(scrollView);
						}
					}
					break;

				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
					// Reset scroll state
					scrollView.SetTag(IsScrollingTag, false);
					break;
			}

			e.Handled = false; // Allow normal event processing to continue
		}

		private static void ClearChildFocus(ViewGroup viewGroup)
		{
			for (int i = 0; i < viewGroup.ChildCount; i++)
			{
				var child = viewGroup.GetChildAt(i);
				if (child != null && child.HasFocus)
				{
					child.ClearFocus();
				}

				// Recursively clear focus in nested ViewGroups
				if (child is ViewGroup childGroup)
				{
					ClearChildFocus(childGroup);
				}
			}
		}
	}
}
# HorizontalScrollView Focus Fix

This fix addresses the issue where Entry controls inside a native Android HorizontalScrollView gain focus during swipe gestures instead of direct taps.

## Problem

When using a custom handler with a native Android HorizontalScrollView containing Entry controls:
- Swiping anywhere in the scroll view incorrectly causes Entry to gain focus
- Direct tapping the Entry may not cause it to gain focus as expected

## Solution

Use the `PreventFocusDuringScroll()` extension method provided in `Microsoft.Maui.Platform.HorizontalScrollViewExtensions`.

## Usage

```csharp
// In your custom handler for Android
protected override HorizontalScrollView CreatePlatformView()
{
    var context = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!;
    var scrollView = new HorizontalScrollView(context);

    if (VirtualView.Content is not null)
    {
        var viewHandler = VirtualView.Content.ToHandler(MauiContext!);
        var platformView = viewHandler.PlatformView as Android.Views.View;
        scrollView.AddView(platformView);
    }

    // Apply the focus fix
    scrollView.PreventFocusDuringScroll();

    return scrollView;
}
```

## How it works

The extension method:
1. Monitors touch events on the HorizontalScrollView
2. Detects when a touch gesture is a horizontal scroll vs a direct tap
3. Clears focus from child views during scroll gestures
4. Allows normal focus behavior for direct taps

## Requirements

- .NET MAUI (this fix is included in the Core package)
- Android platform
- Custom handlers using native Android HorizontalScrollView

## Example Complete Handler

```csharp
#if ANDROID
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif

namespace YourApp
{
    public class InteractiveScrollViewExt : View
    {
        public InteractiveScrollViewExt(View child)
        {
            Content = child;
        }

        public View Content { get; }
    }

#if ANDROID
    public class InteractiveScrollViewExtHandler() : ViewHandler(ViewMapper)
    {
        protected override HorizontalScrollView CreatePlatformView()
        {
            var context = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!;
            var scrollView = new HorizontalScrollView(context);

            if (VirtualView.Content is not null)
            {
                var viewHandler = VirtualView.Content.ToHandler(MauiContext!);
                var platformView = viewHandler.PlatformView as Android.Views.View;
                scrollView.AddView(platformView);
            }

            // Apply the focus fix to prevent Entry focus during swipe
            scrollView.PreventFocusDuringScroll();

            return scrollView;
        }
    }
#endif
}
```
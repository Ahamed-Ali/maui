using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{
	public partial class MenuFlyoutSeparatorHandler
	{
		protected override MenuFlyoutSeparator CreatePlatformElement()
		{
			return new MenuFlyoutSeparator();
		}

		protected override void DisconnectHandler(MenuFlyoutSeparator PlatformView)
		{
			base.DisconnectHandler(PlatformView);
			//PlatformView.Tapped -= OnTapped;
		}

		protected override void ConnectHandler(MenuFlyoutSeparator PlatformView)
		{
			base.ConnectHandler(PlatformView);
			//PlatformView.Tapped += OnTapped;
		}

		//void OnTapped(object sender, UI.Xaml.Input.TappedRoutedEventArgs e)
		//{
		//	VirtualView.Clicked();
		//}

		//public static void MapText(IMenuFlyoutSubItemHandler handler, IMenuFlyoutSubItem view)
		//{
		//	handler.PlatformView.Text = view.Text;
		//}

		//public static void MapIsEnabled(IMenuFlyoutSubItemHandler handler, IMenuFlyoutSubItem view) =>
		//	handler.PlatformView.UpdateIsEnabled(view.IsEnabled);

		//public static void MapSource(IMenuFlyoutSubItemHandler handler, IMenuFlyoutSubItem view)
		//{
		//	handler.PlatformView.Icon =
		//		view.Source?.ToIconSource(handler.MauiContext!)?.CreateIconElement();
		//}

		public override void SetVirtualView(IElement view)
		{
			base.SetVirtualView(view);
		}
	}
}

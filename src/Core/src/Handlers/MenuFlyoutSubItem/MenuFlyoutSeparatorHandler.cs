using System;
using System.Collections.Generic;
using System.Text;
#if IOS || MACCATALYST
using PlatformView = UIKit.UIMenu;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.MenuFlyoutSeparator;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class MenuFlyoutSeparatorHandler : ElementHandler<IMenuFlyoutSeparator, PlatformView>, IMenuFlyoutSeparatorHandler
	{
		public static IPropertyMapper<IMenuFlyoutSeparator, IMenuFlyoutSeparatorHandler> Mapper = new PropertyMapper<IMenuFlyoutSeparator, IMenuFlyoutSeparatorHandler>(ElementMapper)
		{
#if WINDOWS
			//[nameof(IMenuFlyoutSubItem.Text)] = MapText,
			//[nameof(IMenuFlyoutSubItem.Source)] = MapSource
#endif
		};

		public static CommandMapper<IMenuFlyoutSeparator, IMenuFlyoutSeparatorHandler> CommandMapper = new(ElementCommandMapper)
		{
		};

#if IOS
		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
#endif
		public MenuFlyoutSeparatorHandler() : this(Mapper, CommandMapper)
		{

		}

#if IOS
		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
#endif
		public MenuFlyoutSeparatorHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
		{

		}

#if IOS
		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
#endif
		IMenuFlyoutSeparator IMenuFlyoutSeparatorHandler.VirtualView => VirtualView;
#if IOS
		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
#endif
		PlatformView IMenuFlyoutSeparatorHandler.PlatformView => PlatformView;
#if IOS
		[System.Runtime.Versioning.SupportedOSPlatform("ios13.0")]
#endif
		private protected override void OnDisconnectHandler(object platformView)
		{
			base.OnDisconnectHandler(platformView);
		}
	}
}

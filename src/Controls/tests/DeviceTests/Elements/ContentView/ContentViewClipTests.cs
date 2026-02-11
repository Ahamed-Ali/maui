using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.ContentView)]
	public partial class ContentViewClipTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<Label, LabelHandler>();
					handlers.AddHandler<IContentView, ContentViewHandler>();
					handlers.AddHandler<IBorderView, BorderHandler>();
				});
			});
		}

		[Fact]
		public async Task ContentViewWithClipShouldCreateContainer()
		{
			SetupBuilder();

			await InvokeOnMainThreadAsync(() =>
			{
				var contentView = new ContentView();
				var handler = CreateHandler<ContentViewHandler>(contentView);

				// Initially no clip, should not have container
				Assert.False(handler.HasContainer);

				// Set clip, should create container
				contentView.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, 100, 100));
				
				// The container should be created now
				Assert.True(handler.HasContainer);
				
#if WINDOWS
				// On Windows, the container should be a WrapperView
				Assert.True(handler.ContainerView is WrapperView);
#endif
			});
		}

		[Fact]
		public async Task ContentViewWithClipInBorderShouldWork()
		{
			SetupBuilder();

			await InvokeOnMainThreadAsync(() =>
			{
				var border = new Border();
				var borderHandler = CreateHandler<BorderHandler>(border);

				var contentView = new ContentView
				{
					BackgroundColor = Colors.Red
				};

				// Set clip immediately
				contentView.Clip = new RoundRectangleGeometry(6, new Rect(0, 0, 100, 100));

				var label = new Label { Text = "Test" };
				contentView.Content = label;
				border.Content = contentView;

				// Verify the contentView handler was created
				Assert.NotNull(contentView.Handler);
				var contentHandler = contentView.Handler as ContentViewHandler;
				Assert.NotNull(contentHandler);

				// Verify container was created for clip
				Assert.True(contentHandler.HasContainer);

#if WINDOWS
				// On Windows, verify container is WrapperView
				Assert.True(contentHandler.ContainerView is WrapperView);
				
				// The border's content panel should contain the wrapper view
				var borderPlatformView = borderHandler.PlatformView as ContentPanel;
				Assert.NotNull(borderPlatformView);
				
				// The content should be the wrapper view
				Assert.True(borderPlatformView.Content is WrapperView);
#endif
			});
		}
	}
}
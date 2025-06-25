using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests.Handlers.ContentView
{
	[Category(TestCategory.ContentView)]
	public partial class ContentViewClipTests : CoreHandlerTestBase<ContentViewHandler, ContentViewStub>
	{
		[Fact, Category(TestCategory.Clip)]
		public async Task ContentViewHandlesClipProperty()
		{
			// Create a ContentView with content
			var contentView = new ContentViewStub();
			var label = new LabelStub { Text = "Test Content" };
			contentView.PresentedContent = label;

			// Set clip geometry (similar to the issue description)
			var clipGeometry = new RoundRectangleGeometryStub(new Point(50, 50), 25, new Rect(0, 0, 100, 100));
			contentView.Clip = clipGeometry;

			var contentViewHandler = await CreateHandlerAsync(contentView);

			await InvokeOnMainThreadAsync(() =>
			{
				// Verify the ContentView platform view exists and has the clip set
				var platformView = contentViewHandler.PlatformView;
				Assert.NotNull(platformView);

				// Check that the clip was applied
				var contentViewClip = platformView.Clip;
				Assert.NotNull(contentViewClip);
				Assert.NotNull(contentViewClip.Shape);
			});
		}

		[Fact, Category(TestCategory.Clip)]
		public async Task ContentViewClipCanBeUpdatedDynamically()
		{
			// Create a ContentView with content
			var contentView = new ContentViewStub();
			var label = new LabelStub { Text = "Test Content" };
			contentView.PresentedContent = label;

			var contentViewHandler = await CreateHandlerAsync(contentView);

			await InvokeOnMainThreadAsync(() =>
			{
				var platformView = contentViewHandler.PlatformView;
				Assert.NotNull(platformView);

				// Initially no clip
				Assert.Null(platformView.Clip);

				// Set clip dynamically (like in the issue description)
				var clipGeometry = new RoundRectangleGeometryStub(new Point(50, 50), 25, new Rect(0, 0, 100, 100));
				contentView.Clip = clipGeometry;
				contentViewHandler.UpdateValue(nameof(IView.Clip));

				// Check that the clip was applied
				var contentViewClip = platformView.Clip;
				Assert.NotNull(contentViewClip);
				Assert.NotNull(contentViewClip.Shape);

				// Remove clip
				contentView.Clip = null;
				contentViewHandler.UpdateValue(nameof(IView.Clip));

				// Check that the clip was removed
				Assert.Null(platformView.Clip);
			});
		}
	}

	// Helper stub for RoundRectangleGeometry used in the issue
	public class RoundRectangleGeometryStub : IShape
	{
		public RoundRectangleGeometryStub(Point center, double cornerRadius, Rect rect)
		{
			Center = center;
			CornerRadius = cornerRadius;
			Rect = rect;
		}

		public Point Center { get; }
		public double CornerRadius { get; }
		public Rect Rect { get; }

		public PathF PathForBounds(Rect rect)
		{
			var path = new PathF();
			path.AppendRoundedRectangle((float)Rect.X, (float)Rect.Y, (float)Rect.Width, (float)Rect.Height, (float)CornerRadius);
			return path;
		}
	}
}
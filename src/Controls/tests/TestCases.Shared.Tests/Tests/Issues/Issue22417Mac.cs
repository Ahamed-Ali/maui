#if TEST_FAILS_ON_CATALYST 
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue22417Mac : _IssuesUITest
	{
		public Issue22417Mac(TestDevice device) : base(device) { }

		public override string Issue => "[Mac] Size is not correctly assigned in the CarouselView";

		[Test]
		[Category(UITestCategories.CarouselView)]
		[FailsOnMacCatalystWhenRunningOnXamarinUITest("CarouselView sizing issue on Mac Catalyst https://github.com/dotnet/maui/issues/15")]
		public async Task CarouselViewItemsHaveCorrectWidthOnMac()
		{
			App.WaitForElement("WaitForStubControl");
			
			// Wait for the CarouselView to load
			App.WaitForElement("TestCarouselView");
			
			// Check if we can see the item content (indicates proper sizing)
			App.WaitForElement("ItemTitle");
			App.WaitForElement("ItemDescription");

			// Verify the size info shows reasonable dimensions
			var sizeLabel = App.WaitForElement("SizeInfoLabel");
			Assert.IsNotNull(sizeLabel);
			
			// The text should show actual dimensions, not "N/A" and not "0 x 0"
			var sizeText = sizeLabel.GetText();
			Assert.IsFalse(sizeText.Contains("N/A"), "CarouselView should have valid size");
			Assert.IsFalse(sizeText.Contains("0.0 x"), "CarouselView width should not be zero");

			// Add an item to ensure the sizing works correctly when items are added
			App.Tap("AddItemButton");

			await Task.Delay(500);

			// Verify the sizing is still correct after adding an item
			sizeLabel = App.WaitForElement("SizeInfoLabel");
			sizeText = sizeLabel.GetText();
			Assert.IsFalse(sizeText.Contains("0.0 x"), "CarouselView width should remain non-zero after adding items");
			
			VerifyScreenshot();
		}
	}
}
#endif
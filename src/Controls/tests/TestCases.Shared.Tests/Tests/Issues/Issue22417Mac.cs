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

			// Add an item to ensure the sizing works correctly when items are added
			App.Tap("AddItemButton");

			await Task.Delay(500);

			// Verify the sizing is still correct after adding an item
			VerifyScreenshot();
		}
	}
}
#endif
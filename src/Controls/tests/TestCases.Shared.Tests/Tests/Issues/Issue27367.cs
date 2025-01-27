#if TEST_FAILS_ON_CATALYST && TEST_FAILS_ON_WINDOWS
// Using SwipeRightToLeft leads to exception of type 'OpenQA.Selenium.WebDriverException'. "Only pointer type 'mouse' is supported on Mac"
//Unable to capture a screenshot with the opened swipe, as SwipeRightToLeft does not fully open the SwipeItems and instead closes them.
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue27367 : _IssuesUITest
	{
		const string SwipeViewId = "SwipeView";
		public Issue27367(TestDevice device) : base(device) { }

		public override string Issue => "[Android] Right SwipeView items are not visible in the SwipeView";

		[Test]
		[Category(UITestCategories.SwipeView)]
		public void TextInRightSwipeItemIsVisibleOnRightSwipe()
		{
			App.WaitForElement(SwipeViewId);
			App.SwipeRightToLeft(SwipeViewId);
			VerifyScreenshot();
		}
	}
}
#endif
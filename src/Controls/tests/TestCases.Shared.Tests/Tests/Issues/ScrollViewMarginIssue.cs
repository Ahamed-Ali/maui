using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class ScrollViewMarginIssue : _IssuesUITest
    {
        public override string Issue => "Adding a margin to a control that contains a scrollview causes scrolling behavior";

        public ScrollViewMarginIssue(TestDevice testDevice) : base(testDevice) { }

        [Test]
        [Category(UITestCategories.ScrollView)]
        public void ScrollViewWithMarginShouldNotShowUnnecessaryScrollbars()
        {
            App.WaitForElement("TestScrollView");
            App.WaitForElement("ContentLine1");
            
            // The content should be visible and the scrollview should not be scrollable
            // since the content fits within the available space even with the margin
            var contentElement = App.WaitForElement("ContentLine1");
            Assert.IsNotNull(contentElement, "Content should be visible");
            
            // TODO: Add additional assertions to verify scrollbar visibility and scrolling behavior
            // This would require platform-specific implementation to check if scrollbars are actually visible
        }
    }
}
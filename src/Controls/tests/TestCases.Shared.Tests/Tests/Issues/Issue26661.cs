using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;
public class Issue26661 : _IssuesUITest
{
	public Issue26661(TestDevice device) : base(device) { }

	public override string Issue => "Default SelectedTab Text Color is not applied properly";

	[Test]
	[Category(UITestCategories.TabbedPage)]
	public void DefaultSelectedTabTextColorShouldApplyProperly()
	{
		App.WaitForElement("Tab1");
		VerifyScreenshot();
	}
}
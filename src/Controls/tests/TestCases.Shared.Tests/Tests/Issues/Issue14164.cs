#if TEST_FAILS_ON_IOS && TEST_FAILS_ON_CATALYST // Content of the border disappears when apply clip to it see https://github.com/dotnet/maui/issues/14164
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue14164 : _IssuesUITest
{
	public Issue14164(TestDevice device) : base(device) { }

	public override string Issue => "[Windows] ContentView clip is not updated when wrapping inside the Border";

	[Test]
	[Category(UITestCategories.Border)]
	public void ClipShouldApplyProperlyToTheContentOfBorder()
	{
		App.WaitForElement("BorderContentLabel");
		VerifyScreenshot();
	}
}
#endif
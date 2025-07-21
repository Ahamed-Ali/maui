using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue19 : _IssuesUITest
{
	public Issue19(TestDevice device) : base(device) { }

	public override string Issue => "Android: Updating Image.Source with ImageSource.FromStream() is broken in Release builds";

	[Test]
	[Category(UITestCategories.Image)]
	public void ImageFromStreamLoadsInReleaseBuilds()
	{
		App.WaitForElement("LoadImageButton");
		App.Click("LoadImageButton");
		
		// Wait for the image to load and verify status
		App.WaitForElement("StatusLabel");
		var statusText = App.GetText("StatusLabel");
		
		// The test passes if the image loads successfully without error
		Assert.That(statusText, Is.EqualTo("Image loaded successfully from stream"));
		
		// Verify the image is visible
		App.WaitForElement("TestImage");
		
		// Verify the screenshot to ensure image is actually displayed
		VerifyScreenshot();
	}
}
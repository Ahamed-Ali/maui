﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue20535 : _IssuesUITest
{
	public Issue20535(TestDevice device) : base(device) { }

	public override string Issue => "Re-enable/move TrackColorInitializesCorrectly/TrackColorUpdatesCorrectly to Appium";

	[Test]
	[Category(UITestCategories.Switch)]
	public void SwitchTrackColorTest()
	{
		App.WaitForElement("WaitForStubControl");

		// 1. Update the state of the Switch by updating the OnColor property.
		// In this way, we validate the initial value but also the update.
		App.Tap("UpdateOnColorSwitch");
		App.WaitForElement("Update OnColor"); // Wait for the Thumb animation to complete

		// 2. Verify the result.
		VerifyScreenshot();
	}
}
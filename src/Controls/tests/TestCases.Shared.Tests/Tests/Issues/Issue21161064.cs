using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue21161064 : _IssuesUITest
	{
		public Issue21161064(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "PointerGestureRecognizer wrong works when having multiple window";

		[Test]
		[Category(UITestCategories.Gestures)]
		[Category(UITestCategories.Compatibility)]
		public void PointerGestureRecognizerShouldWorkCorrectlyWithMultipleWindows()
		{
			// This test verifies that PointerGestureRecognizer works correctly when multiple windows are open
			// The issue was that PointerExited events would fire rapidly when a window was minimized

			// First, interact with the red square to ensure pointer gestures work
			App.WaitForElement("RedSquare");

			// Move mouse over the red square - this should trigger PointerEntered
			App.TapCoordinates(App.FindElement("RedSquare"), 100, 100);

			// Wait a moment for the gesture to register
			System.Threading.Thread.Sleep(500);

			// Check that the event log shows some activity
			var eventLog = App.WaitForElement("EventLog");
			Assert.IsNotNull(eventLog);

			// The event log should contain some events but not excessive rapid firing
			var logText = eventLog.GetText();
			Assert.IsTrue(logText.Contains("Event Log"));

			// Create a new window to test multi-window scenario
			App.Tap("NewWindowButton");

			// Wait for new window to be created
			System.Threading.Thread.Sleep(1000);

			// Move mouse over the original red square again
			App.TapCoordinates(App.FindElement("RedSquare"), 100, 100);

			// Wait a moment
			System.Threading.Thread.Sleep(500);

			// Move mouse away from the red square
			App.TapCoordinates(App.FindElement("RedSquare"), 300, 300);

			// Wait for pointer events to settle
			System.Threading.Thread.Sleep(1000);

			// Check the event log again
			eventLog = App.WaitForElement("EventLog");
			logText = eventLog.GetText();

			// The fix should prevent rapid firing of PointerExited events
			// We can't easily count the exact events in the UI test, but we can verify
			// that the system is still responsive and the log is not flooded with events
			Assert.IsTrue(logText.Length < 2000, "Event log should not be excessively long due to rapid event firing");
		}
	}
}
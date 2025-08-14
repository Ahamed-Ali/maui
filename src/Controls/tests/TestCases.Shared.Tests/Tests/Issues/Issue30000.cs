using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue30000 : _IssuesUITest
{
	public Issue30000(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "SIGSEGV on Android 5.1.1 with CollectionView, RecyclerView and ColorFilter operations";

	[Test]
	[Category(UITestCategories.CollectionView)]
	[Category(UITestCategories.Switch)]
	public void CollectionViewWithSwitchControlsShouldNotCrashOnRapidUpdates()
	{
		App.WaitForElement("StartTestButton");
		
		// Start the rapid update test that would previously crash on Android 5.1.1
		// This test exercises both ColorFilter operations AND RecyclerView memory management
		App.Tap("StartTestButton");
		
		// Wait for the test to run - if it crashes, the test will fail
		App.WaitForElement("StopTestButton");
		
		// Let the rapid updates run for a few seconds to trigger the crash scenario
		// On Android 5.1.1, this would previously cause SIGSEGV in RecyclerView and ColorFilter operations
		System.Threading.Thread.Sleep(5000);
		
		// Verify the app is still responsive by checking we can interact with switches
		App.WaitForElement("TestCollectionView");
		
		// Try to tap some switches to ensure ColorFilter operations still work
		var switches = App.FindElements("Switch1");
		if (switches?.Count > 0)
		{
			App.Tap("Switch1");
		}
		
		// Stop the test
		App.Tap("StopTestButton");
		
		// If we reach this point without crashing, the fix is working
		App.WaitForElement("StartTestButton");
	}

	[Test]
	[Category(UITestCategories.CollectionView)]
	[Category(UITestCategories.Switch)]
	public void SwitchColorFilterOperationsShouldBeSafeOnOlderAndroid()
	{
		App.WaitForElement("TestCollectionView");
		
		// Test individual switch interactions to ensure ColorFilter operations are safe
		var switches1 = App.FindElements("Switch1");
		var switches2 = App.FindElements("Switch2"); 
		var switches3 = App.FindElements("Switch3");
		
		// Rapid switch toggling should not crash even on Android 5.1.1
		if (switches1?.Count > 0)
		{
			for (int i = 0; i < 10; i++)
			{
				App.Tap("Switch1");
				System.Threading.Thread.Sleep(50); // Quick succession
			}
		}
		
		if (switches2?.Count > 0)
		{
			for (int i = 0; i < 10; i++)
			{
				App.Tap("Switch2");
				System.Threading.Thread.Sleep(50);
			}
		}
		
		if (switches3?.Count > 0)
		{
			for (int i = 0; i < 10; i++)
			{
				App.Tap("Switch3");
				System.Threading.Thread.Sleep(50);
			}
		}
		
		// If we reach here without crashing, ColorFilter operations are safe
		App.WaitForElement("StartTestButton");
	}
}
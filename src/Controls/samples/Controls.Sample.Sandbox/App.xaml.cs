namespace Maui.Controls.Sample;

public partial class App : Application
{
	[Obsolete("Use newMethod instead", false)]
	public App(TabbedPageViewModel tabbedPage)
	{
		InitializeComponent();
		MainPage = new MainPage(tabbedPage);
	}

	// protected override Window CreateWindow(IActivationState? activationState)
	// {
	// 	// To test shell scenarios, change this to true
	// 	bool useShell = false;

	// 	if (!useShell)
	// 	{
	// 		return new Window(new NavigationPage(new MainPage()));
	// 	}
	// 	else
	// 	{
	// 		return new Window(new SandboxShell());
	// 	}
	// }
}

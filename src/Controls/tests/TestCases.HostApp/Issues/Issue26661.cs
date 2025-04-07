namespace Maui.Controls.Sample.Issues;
[Issue(IssueTracker.Github, 26661, "Default SelectedTab Text Color is not applied properly", PlatformAffected.Android)]
public class Issue26661 : TabbedPage
{
    public Issue26661()
    {
        // Set the UnselectedTabColor property , it should be applied only to the unselected tab.
        UnselectedTabColor = Colors.Red;

        // Add tabs (pages)
        Children.Add(new Issue26661Tab1());
        Children.Add(new Issue26661Tab2());
        Children.Add(new Issue26661Tab3());
    }
}

public class Issue26661Tab1 : ContentPage
{
    public Issue26661Tab1()
    {
        Title = "Tab 1";
        var verticalStackLayout = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Children = {
                    new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Tab 1",
                            AutomationId = "Tab1"
                        },
                    }

        };
        Content = verticalStackLayout;
    }
}

public class Issue26661Tab2 : ContentPage
{
    public Issue26661Tab2()
    {
        Title = "Tab 2";
        var verticalStackLayout = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Children = {
                    new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Tab 2"
                        }
                    }

        };
        Content = verticalStackLayout;
    }
}
public class Issue26661Tab3 : ContentPage
{
    public Issue26661Tab3()
    {
        Title = "Tab 3";
        var verticalStackLayout = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Children = {
                    new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Tab 3"
                        }
                    }

        };
        Content = verticalStackLayout;
    }
}
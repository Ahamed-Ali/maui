using System;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.Github, 25000, "Adding a margin to a control that contains a scrollview causes scrolling behavior", PlatformAffected.Android)]
    public partial class ScrollViewMarginIssue : TestContentPage
    {
        public ScrollViewMarginIssue()
        {
            InitializeComponent();
        }

        protected override void Init()
        {
            // Test content is defined in XAML
        }
    }
}
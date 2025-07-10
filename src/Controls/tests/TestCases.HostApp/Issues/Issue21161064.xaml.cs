using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.Github, 21161064, "PointerGestureRecognizer wrong works when having multiple window", PlatformAffected.UWP)]
    public partial class Issue21161064 : ContentPage
    {
        private int _pointerExitedCount = 0;
        private int _pointerEnteredCount = 0;
        private DateTime _lastEventTime = DateTime.Now;
        private List<string> _eventLog = new List<string>();

        public Issue21161064()
        {
            InitializeComponent();
        }

        private void NewWindowButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.OpenWindow(new Window(new Issue21161064()));
        }

        private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
        {
            _pointerEnteredCount++;
            TheBorder.BackgroundColor = Colors.Lime;
            UpdateEventLog($"PointerEntered #{_pointerEnteredCount}");
        }

        private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
        {
            _pointerExitedCount++;
            TheBorder.BackgroundColor = Colors.Red;
            UpdateEventLog($"PointerExited #{_pointerExitedCount}");
        }

        private void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
        {
            var pt = e.GetPosition(TheBorder).GetValueOrDefault();
            AbsoluteLayout.SetLayoutBounds(Ellipse, new(pt.X - 8, pt.Y - 8, 16, 16));
        }

        private void UpdateEventLog(string eventDescription)
        {
            var currentTime = DateTime.Now;
            var timeSinceLastEvent = currentTime - _lastEventTime;
            _lastEventTime = currentTime;

            var logEntry = $"{eventDescription} (Time: {currentTime:HH:mm:ss.fff}, Delta: {timeSinceLastEvent.TotalMilliseconds:F0}ms)";
            _eventLog.Add(logEntry);

            // Keep only the last 10 events
            if (_eventLog.Count > 10)
            {
                _eventLog.RemoveAt(0);
            }

            // Update the UI with the latest event log
            EventLog.Text = $"Event Log:\n{string.Join("\n", _eventLog)}";
        }
    }
}
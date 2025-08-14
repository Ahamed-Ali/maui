using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 30000, "SIGSEGV on Android 5.1.1 with CollectionView, RecyclerView and ColorFilter operations", PlatformAffected.Android)]
public partial class Issue30000 : ContentPage
{
	private readonly Issue30000ViewModel _viewModel;
	private System.Timers.Timer? _updateTimer;
	private Random _random;

	public Issue30000()
	{
		InitializeComponent();
		_viewModel = new Issue30000ViewModel();
		BindingContext = _viewModel;
		_random = new Random();
	}

	private void OnStartTestClicked(object sender, EventArgs e)
	{
		StartTestButton.IsEnabled = false;
		StopTestButton.IsEnabled = true;
		StatusLabel.Text = "Rapid updates running - Testing RecyclerView and ColorFilter crash prevention";

		// Start rapid updates that would previously cause SIGSEGV on Android 5.1.1
		// This test reproduces crashes from both ColorFilter operations AND RecyclerView memory management
		_updateTimer = new System.Timers.Timer(100); // Very rapid updates
		_updateTimer.Elapsed += OnTimerElapsed;
		_updateTimer.Start();
	}

	private void OnStopTestClicked(object sender, EventArgs e)
	{
		_updateTimer?.Stop();
		_updateTimer?.Dispose();
		_updateTimer = null;

		StartTestButton.IsEnabled = true;
		StopTestButton.IsEnabled = false;
		StatusLabel.Text = "Test stopped - No crashes detected (fix working)";
	}

	private void OnTimerElapsed(object sender, ElapsedEventArgs e)
	{
		// Perform rapid updates on the UI thread that would trigger the crash
		MainThread.BeginInvokeOnMainThread(() =>
		{
			// Rapidly update switch states and colors - this triggers ColorFilter operations
			// and RecyclerView adapter notifications that cause memory race conditions
			var randomIndex = _random.Next(_viewModel.Items.Count);
			var item = _viewModel.Items[randomIndex];
			
			// Toggle switches rapidly to trigger ColorFilter updates in SwitchExtensions
			item.Switch1 = !item.Switch1;
			item.Switch2 = !item.Switch2; 
			item.Switch3 = !item.Switch3;

			// Also update the collection to trigger CollectionView/RecyclerView updates
			// This triggers SafeClearRecycledViewPool and SafeSwapAdapter calls
			if (_random.Next(10) == 0) // Occasionally shuffle
			{
				var temp = _viewModel.Items[0];
				_viewModel.Items.RemoveAt(0);
				_viewModel.Items.Add(temp);
			}

			// Update status
			StatusLabel.Text = $"Rapid updates running - Updated item {randomIndex} - No crashes detected";
		});
	}

	protected override void OnDisappearing()
	{
		OnStopTestClicked(null, null);
		base.OnDisappearing();
	}
}

public class Issue30000ViewModel : INotifyPropertyChanged
{
	public ObservableCollection<Issue30000Item> Items { get; set; }

	public Issue30000ViewModel()
	{
		Items = new ObservableCollection<Issue30000Item>();
		
		// Create test items with switches - similar to the crash reproduction case
		for (int i = 0; i < 50; i++)
		{
			Items.Add(new Issue30000Item 
			{ 
				Name = $"Test Item {i + 1}",
				Switch1 = i % 2 == 0,
				Switch2 = i % 3 == 0,
				Switch3 = i % 5 == 0
			});
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class Issue30000Item : INotifyPropertyChanged
{
	private string _name;
	private bool _switch1;
	private bool _switch2;
	private bool _switch3;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	public bool Switch1
	{
		get => _switch1;
		set
		{
			_switch1 = value;
			OnPropertyChanged();
		}
	}

	public bool Switch2
	{
		get => _switch2;
		set
		{
			_switch2 = value;
			OnPropertyChanged();
		}
	}

	public bool Switch3
	{
		get => _switch3;
		set
		{
			_switch3 = value;
			OnPropertyChanged();
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
#nullable disable
using Android.OS;
using AndroidX.RecyclerView.Widget;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	internal class AdapterNotifier : ICollectionChangedNotifier
	{
		readonly RecyclerView.Adapter _adapter;

		public AdapterNotifier(RecyclerView.Adapter adapter)
		{
			_adapter = adapter;
		}

		public void NotifyDataSetChanged()
		{
			if (IsValidAdapter())
				_adapter.NotifyDataSetChanged();
		}

		public void NotifyItemChanged(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
				SafeNotifyItemChanged(startIndex);
		}

		public void NotifyItemInserted(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemInserted(startIndex);
			}
		}

		public void NotifyItemMoved(IItemsViewSource source, int fromPosition, int toPosition)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemMoved(fromPosition, toPosition);
			}
		}

		public void NotifyItemRangeChanged(IItemsViewSource source, int start, int end)
		{
			if (IsValidAdapter())
				_adapter.NotifyItemRangeChanged(start, end);
		}

		public void NotifyItemRangeInserted(IItemsViewSource source, int startIndex, int count)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRangeInserted(startIndex, count);
			}
		}

		public void NotifyItemRangeRemoved(IItemsViewSource source, int startIndex, int count)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRangeRemoved(startIndex, count);
			}
		}

		public void NotifyItemRemoved(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRemoved(startIndex);
			}
		}

		internal bool IsValidAdapter()
		{
			if (_adapter == null || _adapter.IsDisposed())
				return false;

			return true;
		}

		/// <summary>
		/// Safely notifies the adapter that an item has changed.
		/// On Android 5.1.1 and below, adds protection against race conditions with individual item updates
		/// during rapid collection operations.
		/// </summary>
		void SafeNotifyItemChanged(int index)
		{
			try
			{
				// Android 5.1.1 (API 22) and below have race conditions when individual item
				// property changes happen simultaneously with collection structure changes
				if (Build.VERSION.SdkInt <= BuildVersionCodes.LollipopMr1)
				{
					// On older versions, use the safer but less efficient NotifyDataSetChanged
					// when individual items are being updated rapidly to avoid SIGSEGV crashes
					_adapter.NotifyDataSetChanged();
				}
				else
				{
					_adapter.NotifyItemChanged(index);
				}
			}
			catch (System.Exception)
			{
				// Silently ignore individual item notification failures on older Android versions
			}
		}
	}
}

#nullable disable
using System;
using System.Collections;
using System.Collections.Specialized;
using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Handlers.Items2
{
	internal class LoopObservableItemsSource2 : Items.ObservableItemsSource, Items.ILoopItemsViewSource
	{
		int _section = 0;

		public LoopObservableItemsSource2(IEnumerable itemSource, UICollectionViewController collectionViewController, bool loop, int group = -1) : base(itemSource, collectionViewController, group)
		{
			Loop = loop;
		}

		public bool Loop { get; set; }

		protected override NSIndexPath[] CreateIndexesFrom(int startIndex, int count)
		{
			if (ItemCount == 0)
			{
				startIndex = 0;
			}
			if (!Loop)
			{
				return base.CreateIndexesFrom(startIndex, count);
			}

			// When Loop=true, we add 2 extra items to the index paths to create a "fake loop" effect.
			// This works by adding one copy of the last item at the beginning of the collection
			// and one copy of the first item at the end, enabling smooth circular navigation.
			return IndexPathHelpers.GenerateIndexPathRange(_section, startIndex, count + 2);
		}

		private protected override bool ShouldReload(NotifyCollectionChangedEventArgs args)
		{
			if (args.Action == NotifyCollectionChangedAction.Reset)
			{
				return true;
			}
			if (args.Action == NotifyCollectionChangedAction.Add)
			{
				if (ItemCount == 0)
				{
					return true;
				}
			}
			if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				if (ItemCount < 1)
				{
					return true;
				}
			}
			return base.ShouldReload(args);
		}

		//We are going to add 2 items since we are inserting 1 item at the beginning and 1 item at the end
		public int LoopCount
		{
			get
			{
				var newCount = ItemCount;
				if (Loop && newCount > 0)
				{
					newCount = ItemCount + 2;
				}
				return newCount;
			}
		}

	}
}

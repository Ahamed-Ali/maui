using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.StyleSheets;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class MenuFlyoutSeparator : MenuFlyoutItem, IMenuFlyoutSeparator
	{
		ReadOnlyCastingList<Element, IMenuElement> _logicalChildren;
		readonly ObservableCollection<IMenuElement> _menus = new ObservableCollection<IMenuElement>();

		internal override IReadOnlyList<Element> LogicalChildrenInternal =>
			_logicalChildren ??= new ReadOnlyCastingList<Element, IMenuElement>(_menus);

	}
}

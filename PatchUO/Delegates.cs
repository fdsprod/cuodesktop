using System;
using System.Collections.Generic;
using System.Text;

namespace PatchUO
{
	public delegate void ItemAddedHandler(object sender, ItemAddedEventArgs args);
	public delegate void ItemRemovedHandler(object sender, ItemRemovedEventArgs args);
	public delegate void ItemsClearedHandler(object sender, ItemsClearedEventArgs args);
	public delegate void ItemInsertedHandler(object sender, ItemInsertedEventArgs args);
	public delegate void ItemValueChangedHandler(object sender, ItemValueChangedEventArgs args);
}

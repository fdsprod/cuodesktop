using System;
using System.Collections.Generic;
using System.Text;

namespace PatchUO
{
	public class ItemAddedEventArgs : EventArgs
	{
		private object _item;
		private int _index;

		public object Item { get { return _item; } }
		public int Index { get { return _index; } }

		public ItemAddedEventArgs(object item, int index)
			: base()
		{
			_item = item;
			_index = index;
		}
	}

	public class ItemInsertedEventArgs : EventArgs
	{
		private object _item;
		private int _index;

		public object Item { get { return _item; } }
		public int Index { get { return _index; } }

		public ItemInsertedEventArgs(object item, int index)
			: base()
		{
			_item = item;
			_index = index;
		}
	}

	public class ItemRemovedEventArgs : EventArgs
	{
		private object _item;
		private int _index;

		public object Item { get { return _item; } }
		public int Index { get { return _index; } }

		public ItemRemovedEventArgs(object item, int index)
			: base()
		{
			_item = item;
			_index = index;
		}
	}

	public class ItemValueChangedEventArgs : EventArgs
	{
		private object _oldItem;
		private object _newItem;
		private int _index;

		public object OldItem { get { return _oldItem; } }
		public object NewItem { get { return _newItem; } }
		public int Index { get { return _index; } }

		public ItemValueChangedEventArgs(object oldValue, object newValue, int index)
			: base()
		{
			_oldItem = oldValue;
			_newItem = newValue;
			_index = index;
		}
	}

	public class ItemsClearedEventArgs : EventArgs
	{
		private int _numOfItemsCleared;

		public int NumberOfItemsCleared { get { return _numOfItemsCleared; } }

		public ItemsClearedEventArgs(int numOfItemsCleared)
			: base()
		{
			_numOfItemsCleared = numOfItemsCleared;
		}
	}
}

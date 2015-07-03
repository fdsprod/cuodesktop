using System;
using System.Collections.Generic;
using System.Text;

namespace PatchUO
{
	public class SubscriberList<T> : IList<T>
	{
		public static implicit operator List<T>(SubscriberList<T> sList) 
		{ 
			return sList.ToGenericList(); 
		}

		public static implicit operator SubscriberList<T>( List<T> list )
		{
			return new SubscriberList<T>(list);
		}

		public event ItemAddedHandler ItemAdded;
		public event ItemRemovedHandler ItemRemoved;
		public event ItemsClearedHandler ItemsCleared;
		public event ItemInsertedHandler ItemInserted;
		public event ItemValueChangedHandler ItemValueChanged;

		private List<T> _list;

		public SubscriberList()
		{
			_list = new List<T>();
		}

		public SubscriberList(List<T> list)
		{
			_list = new List<T>(list);
		}

		public List<T> ToGenericList()
		{
			return _list;
		}

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);

			if (ItemInserted != null)
				ItemInserted(this, new ItemInsertedEventArgs(item, index));
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);

			if (ItemRemoved != null)
				ItemRemoved(this, new ItemRemovedEventArgs(_list[index], index));
		}

		public T this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				if (ItemValueChanged != null && value.Equals(_list[index]))
					ItemValueChanged(this, new ItemValueChangedEventArgs(_list[index], value, index));

				_list[index] = value;
			}
		}

		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			_list.Add(item);

			if (ItemAdded != null)
				ItemAdded(this, new ItemAddedEventArgs(item, _list.IndexOf(item)));
		}

		public void Clear()
		{
			if (ItemsCleared != null)
				ItemsCleared(this, new ItemsClearedEventArgs(_list.Count));

			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			if (ItemRemoved != null)
				ItemRemoved(this, new ItemRemovedEventArgs(item, _list.IndexOf(item)));

			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator(); ;
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop.PatchLib
{
	public class ProgressChangeEventArgs : EventArgs
	{
		private int _percent;
		private long _current;
		private long _total;

		public int Percent { get { return _percent; } }
		public long Current { get { return _current; } }
		public long Total { get { return _total; } }

		public ProgressChangeEventArgs(int percent, long current, long total)
			: base()
		{
			_percent = percent;
			_current = current;
			_total = total;
		}
	}

	public class OperationCompleteArgs : EventArgs
	{
		public OperationCompleteArgs() : base() { }
	}

	public class StatusChangeEventArgs : EventArgs
	{
		private string _status;

		public string Status { get { return _status; } }

		public StatusChangeEventArgs(string status)
			: base()
		{
			_status = status;
		}
	}
}

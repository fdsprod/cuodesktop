using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop.PatchLib
{
	public delegate void ProgressChangeHandler(object sender, ProgressChangeEventArgs args);
	public delegate void OperationCompleteHandler(object sender, OperationCompleteArgs args);
	public delegate void StatusChangeHandler(object sender, StatusChangeEventArgs args);
}

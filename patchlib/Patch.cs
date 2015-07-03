using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop.PatchLib
{
	public struct Patch
	{
		public int FileID;
		public int BlockID;
		public int Extra;
		public int Length;
		public byte[] Data;
	}
}

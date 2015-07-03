using System;
using System.Collections.Generic;
using System.IO;

namespace CUODesktop
{
	public class Paths
	{
		internal static void EnsureDirectory(string dir)
		{
			if( !Directory.Exists(dir) )
				Directory.CreateDirectory(dir);
		}

		internal static string ApplicationPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
	}
}

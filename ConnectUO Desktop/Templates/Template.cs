using System;
using System.Collections.Generic;
using System.IO;

namespace CUODesktop
{

	public class Template
	{
		private string _name;
		private string _rootDir;
		public string RootDirectory { get { return _rootDir; } }
		public string Name { get { return _name; } }

		public Template(string rootDir, string name)
		{

			_name = name;
			_rootDir = rootDir;
		}

		public string GetPage(string pageName)
		{
			StreamReader reader = new StreamReader(Path.Combine(_rootDir, pageName));
			string data = reader.ReadToEnd();

			reader.Close();
			reader.Dispose();

			return data;
		}

		public bool ContainsFile(string request)
		{
			string[] split = request.Split('?');

			if (split.Length < 1)
				return false;

			char[] invalidChars = Path.GetInvalidPathChars();

			for (int i = 0; i < invalidChars.Length; i++)
				if (split[0].Contains(invalidChars[i].ToString()))
					return false;

			return File.Exists((Path.Combine(_rootDir, split[0])));
		}
	}
}

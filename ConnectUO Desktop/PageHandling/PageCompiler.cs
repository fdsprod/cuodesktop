using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace CUODesktop
{
	public class PageCompiler
	{
		private class HTMLSlice
		{
			private string _slice;
			private bool _compile;

			public string Slice { get { return _slice; } }
			public bool Compile { get { return _compile; } }

			public HTMLSlice(string slice, bool compile)
			{
				_slice = slice;
				_compile = compile;
			}
		}

		/// <summary>
		/// Compiles the page given into HTML to be sent to the web browser
		/// </summary>
		/// <param name="pagepath"></param>
		/// <param name="getData"></param>
		/// <returns></returns>
		public static string CompileCode(string pagepath, string getData)
		{
			StreamReader reader = new StreamReader(pagepath);
			string page = reader.ReadToEnd().Trim();
			reader.Close();

			List<int> codeIndexes = new List<int>();
			int nextIndex = -1;

			while ((nextIndex = page.IndexOf("<%", nextIndex + 1)) != -1)
				codeIndexes.Add(nextIndex);

			bool complete = false;
			int index = 0;

			List<HTMLSlice> slices = new List<HTMLSlice>();

			while (!complete)
			{
				if (page.Substring(index).StartsWith("<%"))
				{
					string slice = page.Substring(index);
					int end = slice.IndexOf("%>") + 2;

					if (end == -1)
						break;

					slice = slice.Substring(0, end);
					slice = slice.Replace("%>", "");
					slice = slice.Replace("<%", "");

					index += end;

					slice = slice.Replace("\r", "");
					slices.Add(new HTMLSlice(slice, true));
				}
				else
				{
					string slice = page.Substring(index);
					int end = slice.IndexOf("<%");

					if (end == -1)
					{
						index = page.Length;
					}
					else
					{
						slice = slice.Substring(0, end);
						index += end;
					}

					slice = slice.Replace("\r", "");
					slice = slice.Replace("\n", "");
					slice = slice.Replace("\t", "");
					slices.Add(new HTMLSlice(slice, false));
				}

				if (index == page.Length)
					complete = true;
			}

			StringBuilder builder = new StringBuilder();

			reader = new StreamReader(Path.Combine(Templates.CurrentTemplate.RootDirectory, "code.cs"));
			string main = reader.ReadToEnd();
			reader.Close();

			for (int i = 0; i < slices.Count; i++)
			{
				if (!slices[i].Compile)
				{
                    string slice = slices[i].Slice.Replace( "\"", "\\\"");
                    builder.AppendFormat("\t\t\tEcho(\"{0}\");", slice);
				}
				else
				{
					builder.Append(@slices[i].Slice);
				}
			}

            string code = main.Replace("{0}", builder.ToString()); 

			return CodeCompiler.Compile(code, getData);
		}

		private string _page;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="page"></param>
		public PageCompiler(string page)
		{
			_page = page;	
		}
		
		private static void Replace(ref string text, string find, string replace)
		{
			text = text.Replace(find, replace);
		}
	}
}

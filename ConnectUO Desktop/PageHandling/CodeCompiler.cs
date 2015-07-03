using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace CUODesktop
{
	public class CodeCompiler
	{
		/// <summary>
		/// Compiles and runs the specified code and returns the output
		/// </summary>
		/// <param name="code"></param>
		/// <param name="getData"></param>
		/// <returns></returns>
		public static string Compile( string code, string getData )
		{
			StringBuilder builder = new StringBuilder();
			CSharpCodeProvider provider = new CSharpCodeProvider();
			ICodeCompiler compiler = provider.CreateCompiler(); 
			CompilerParameters param = new CompilerParameters();

			param.GenerateExecutable = false;
			param.GenerateInMemory = true;
			param.IncludeDebugInformation = true;

			param.MainClass = "CUOCompiledCode.CUOCompiledPage";

			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				param.ReferencedAssemblies.Add(asm.Location);

			CompilerResults res = compiler.CompileAssemblyFromSource(param, code);

			if (res.Errors.Count > 0)
			{
				List<int> lines = new List<int>();

				builder.AppendLine("Page compile failed for the following reasons:\n<font color=\"#cc0000\"><b>");

				foreach (CompilerError err in res.Errors)
				{
					string error = err.ToString();
					int index = error.IndexOf('(');

					if (index >= 0)
					{
						error = error.Substring(index);
						int comma = error.IndexOf(',');
						if (comma > 0)
						{
							string line = error.Substring(1, comma - 1);
							int num;

							if (int.TryParse(line, out num))
								lines.Add(num);
						}
						error = "Line:" + error;
					}

					builder.AppendLine(error);	
				}

				builder.Append("</b></font><br><br>");
				builder.Append("CODE:<br>");

				string[] codeLines = code.Split(new string[] { "\n" }, StringSplitOptions.None);

				for (int i = 0; i < codeLines.Length; i++)
				{
					bool errorLine = lines.Contains(i + 1);

					if (errorLine)
						builder.Append("<font color=\"#cc0000\"><b>");

					int lineNum = i + 1;
					builder.Append(lineNum.ToString("0000 | ") + codeLines[i].Replace("\n", "").Replace("<", "&lt;").Replace(">", "&gt;") + "<br>");
				
					if (errorLine)
						builder.Append("</b></font>");
				}

				builder = builder.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                builder = builder.Replace("\n", "<br>");
			}
			else
			{
				Assembly assembly = res.CompiledAssembly;				
				List<MethodInfo> invoke = new List<MethodInfo>();
				Type[] types = assembly.GetTypes();

				for (int i = 0; i < types.Length; ++i)
				{
					MethodInfo m = types[i].GetMethod("Output", BindingFlags.Static | BindingFlags.Public);

					if (m != null)
						invoke.Add(m);
				}

                try
                {
                    for (int i = 0; i < invoke.Count; ++i)
                        builder.Append((string)invoke[i].Invoke(null, new object[] { new string[] { getData } }));

                }
                catch (Exception e)
                {
                    Version ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                    builder.AppendLine("Crash Report");
                    builder.AppendLine("===================");
                    builder.AppendLine();
                    builder.AppendFormat("Version {0}.{1}, Build {2}.{3}\n", ver.Major, ver.Minor, ver.Build, ver.Revision);
                    builder.AppendFormat("Operating System: {0}\n", Environment.OSVersion);
                    builder.AppendFormat(".NET Framework: {0}\n", Environment.Version);
                    builder.AppendFormat("Processor Count: {0}\n", Environment.ProcessorCount);
                    builder.AppendFormat("User Name: {0}\n", Environment.UserName);
                    builder.AppendFormat("Machine Name: {0}\n", Environment.MachineName);
                    builder.AppendFormat("Time: {0}\n", DateTime.Now);
                    builder.AppendLine();
                    builder.AppendLine("Exception:");
                    builder.AppendLine(e.ToString());
                    builder.Replace("\n", "<br>");
                }

				invoke.Clear();
			}

			string outputString = builder.ToString();

			outputString = outputString.Replace("%>", "");
			outputString = outputString.Replace("<%", "");

			return outputString;
		}
	}
}
using System;
using System.IO;
using System.Text;

namespace CUODesktop
{
	public class Trace
	{
		/// <summary>
		/// Writes the the callstack and other important information from the given Exception to a crashlog
		/// </summary>
		/// <param name="e"></param>
		public static void HandleException(Exception e)
		{
			try
			{
				string timeStamp = GetTimeStamp();
				string fileName = String.Format("crashlog {0}.log", timeStamp);

				string root = System.IO.Directory.GetCurrentDirectory();
				string filePath = Path.Combine(root, fileName);

				StringBuilder sb = new StringBuilder();
				Version ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

				sb.AppendLine("Crash Report");
				sb.AppendLine("===================");
				sb.AppendLine();
				sb.AppendFormat("Version {0}.{1}, Build {2}.{3}\n", ver.Major, ver.Minor, ver.Build, ver.Revision);
				sb.AppendFormat("Operating System: {0}\n", Environment.OSVersion);
				sb.AppendFormat(".NET Framework: {0}\n", Environment.Version);
				sb.AppendFormat("Processor Count: {0}\n", Environment.ProcessorCount);
				sb.AppendFormat("User Name: {0}\n", Environment.UserName);
				sb.AppendFormat("Machine Name: {0}\n", Environment.MachineName);
				sb.AppendFormat("Time: {0}\n", DateTime.Now);
				sb.AppendLine();
				sb.AppendLine("Exception:");
				sb.AppendLine(e.ToString());
				sb.AppendLine();

				using( StreamWriter op = new StreamWriter(filePath, true) )
				{
					op.Write(sb.ToString());
					op.Close();
				}
#if(DEBUG)
	System.Windows.Forms.MessageBox.Show( sb.ToString(), "Exception:" );
#endif
			}
			catch { }
#if(!DEBUG)

			System.Windows.Forms.MessageBox.Show( "A crashlog has been created in " + Core.BaseDirectory + "\n\nPlease post the log at www.runuo.com/issues/", "Error" );
			MainForm.Instance.Invoke((System.Windows.Forms.MethodInvoker)delegate { MainForm.Instance.Close(); });
#endif
		}

		private static string GetTimeStamp()
		{
			DateTime now = DateTime.Now;

			return String.Format( "{0}-{1}-{2}",
					now.Day,
					now.Month,
					now.Year
				);
		}
	}
}

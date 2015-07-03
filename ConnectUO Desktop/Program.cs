using System;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CUODesktop
{
	public static class Core
	{
		private static string _startAddress;
		private static string _baseDirectory;
		private static string _exePath;
		private static string _dataDirectory;
		private static string _templatesDirectory;
		private static Assembly _assembly;
		private static Process _process;
        private static WebServer _server;

		/// <summary>
		/// Gets the start url address of the server
		/// </summary>
		public static string StartAddress
		{
			get
			{
				return _startAddress;
			}
		}

		/// <summary>
		/// Gets the base directory of the ConnectUO
		/// </summary>
		public static string BaseDirectory 
		{ 
			get 
			{
				if (_baseDirectory == null)
				{
					try
					{
						_baseDirectory = ExePath;

						if (_baseDirectory.Length > 0)
							_baseDirectory = Path.GetDirectoryName(_baseDirectory);
					}
					catch
					{
						_baseDirectory = "";
					}
				}

				return _baseDirectory;
			} 
		}

		/// <summary>
		/// Gets the exe path to cuodesktop.exe
		/// </summary>
		public static string ExePath 
		{
			get
			{
				if (_exePath == null)
					_exePath = _assembly.Location;

				return _exePath;
			}
		}

		/// <summary>
		/// Gets the path to the data directory for ConnectUO
		/// </summary>
		public static string DataDirectory
		{ 
			get 
			{
				if (_dataDirectory == null)
					_dataDirectory = Path.Combine(BaseDirectory, "data");

				return _dataDirectory; 
			} 
		}

		/// <summary>
		/// Gets the path to the templates directory for ConnectUO
		/// </summary>
		public static string TemplatesDirectory
		{
			get
			{
				if (_templatesDirectory == null)
					_templatesDirectory = Path.Combine(DataDirectory, "templates");

				return _templatesDirectory;
			}
		}

		/// <summary>
		/// Gets the local server.
		/// </summary>
		public static WebServer Server { get { return _server; } }

		[STAThread]
		static void Main(string[] args)
		{
			_process = Process.GetCurrentProcess();
			_assembly = Assembly.GetEntryAssembly();

			if( args.Length > 0 )
				_startAddress = "http://localhost.:1980/" + args[0].Substring(11);
			else
				_startAddress = "http://localhost.:1980/home.html?page=1";

			string mutexName = "ConnectUO Desktop";

			using( Mutex instanceMutex = new Mutex(false, mutexName) )
			{
				if( instanceMutex.WaitOne(1, true) == false )
				{
					Process.Start(_startAddress);
					return;
				}

				_server = new WebServer();
				
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
				
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.HandleException((Exception)e.ExceptionObject);
		}
	}
}
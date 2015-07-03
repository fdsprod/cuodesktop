using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CUODesktop.PatchLib;
using Microsoft.Win32;

namespace CUODesktop
{
	public class Utility
	{
		/// <summary>
		/// META Redirect header for directing the browser to another page, refresh time is 0 seconds
		/// </summary>
        public static string META_REDIRECT = "<html><head><META http-equiv=\"refresh\" content=\"0;URL={0}\"></head></html>";
        /// <summary>
        /// META Redirect header for redirecting when the serverlist is busy updating, refresh time is 5 seconds
        /// </summary>
		public static string SERVLISTBUSY_REDIRECT = "<html><head><META http-equiv=\"refresh\" content=\"5;URL={0}\"></head><body>Serverlist is busy, will try again in 5 seconds...</body></html>";
		
		#region _sortForm
		private static string[] _sortForm = new string[] 
        {
			"<form id=\"form1\" name=\"form1\" method=\"get\"  style=\"display:inline\" action=\"http://localhost.:1980/home.html%GETDATA%\">",
			"      <select name=\"sortby\">",
			"        <option value=\"default\">Default</option>",
			"        <option value=\"Rank\">Rank</option>",
			"        <option value=\"CurOnline\">Online Count</option>",
			"        <option value=\"MaxOnline\">Online Peak</option>",
			"        <option value=\"AvgOnline\">Online Avg</option>",
			"        <option value=\"Votes\">Votes</option>",
			"        <option value=\"TotalVotes\">Total Votes</option>",
			"        <option value=\"Name\">Name</option>",
			"        <option value=\"Description\">Description</option>",
		    "      </select>",
			"      <input type=\"submit\" value=\"Sort\" />",
            "    </form>"	
	    };
		#endregion

		/// <summary>
		/// HTML form used for submitting sorting
		/// </summary>
		public static string SortForm
		{
			get
			{
				string s = "";
				for (int i = 0; i < _sortForm.Length; i++)
					s += _sortForm[i];
				return s;
			}
		}	

		/// <summary>
		/// Opens the browser to the specified URL
		/// </summary>
		/// <param name="url"></param>
		public static void OpenUrl(string url)
        {
			MainForm.Instance.WebBrowser.Navigate(url);
        }

		/// <summary>
		/// Ensures a directory exists, if it does not, it creates it
		/// </summary>
		/// <param name="dir"></param>
		public static void EnsureDirectory(string dir)
		{
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
		}

		/// <summary>
		/// Detects the path to start Razor, returns "Not Found" if Razor is not installed
		/// </summary>
		/// <returns></returns>
		public static string DetectRazor()
		{
			try
			{
				using( RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Razor") )
				{
					if( key == null )
						return "Not Installed";

					string v = Path.Combine(key.GetValue("InstallDir") as string, "Razor.exe");

					if( v == null || v.Length <= 0 )
						return "Not Installed";

					if( !File.Exists(v) )
						return "Not Installed";

					return v;
				}
			}
			catch( Exception e )
			{
				Trace.HandleException(e);
				return "Not Installed";
			}
		}

		/// <summary>
		/// Detects the path to the Ultima Online client, returns "Not Found" if Razor is not installed
		/// </summary>
		/// <param name="subName"></param>
		/// <returns></returns>
		public static string DetectClient(string subName)
		{
			try
			{
				string regkey = ( IntPtr.Size == 8 ) ? String.Format(@"SOFTWARE\Wow6432Node\Origin Worlds Online\{0}\1.0", subName) : String.Format(@"SOFTWARE\Origin Worlds Online\{0}\1.0", subName);
				using( RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey) )
				{
					if( key == null )
						return "Not Installed";

					string v = key.GetValue("ExePath") as string;

					if( v == null || v.Length <= 0 )
						return "Not Installed";

					if( !File.Exists(v) )
						return "Not Installed";

					return v;
				}
			}
			catch( Exception e )
			{
				Trace.HandleException(e);
				return "Not Installed";
			}
		}

		/// <summary>
		/// Starts the Ultima Online client for the specified server
		/// </summary>
		/// <param name="id"></param>
		/// <param name="custom"></param>
		public static unsafe void Play(string id, string type, Socket socket)
		{
			IEntry entry =  ServerList.GetServerById(id);
						
			if( entry == null )
			{
				System.Windows.Forms.MessageBox.Show("The server information requested was not found", "Invalid Element ID");
				Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/home.html?page=1"), ref socket);	
				return;
			}

			try
			{
				string client = AppSettings.Current.Get<string>("ClientPath");
				string razor = AppSettings.Current.Get<bool>("LoadRazor") ? AppSettings.Current.Get<string>("RazorPath") : "Not Found";

				if( client == "Not Found" )
				{
					System.Windows.Forms.MessageBox.Show("You do not appear to have Ultima Online installed.", "Ultima Online Not Found");
					Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/home.html?page=1"), ref socket);	
					return;
				}

				if (type == "public")
					Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/home.html?page=1"), ref socket);
				else
					Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);	

				byte[] param = new byte[257];

				MemoryStream memStream = new MemoryStream(param);
				BinaryWriter writer = new BinaryWriter(memStream);

				if( razor != "Not Found" )
					writer.Write((byte)0); // when razor is enabled it will remove the encryption for us
				else
					writer.Write((byte)Convert.ToByte(entry.RemoveEncryption));

				string patch;
				string path = BuildPatchList(entry, out patch);

				writer.Write(path.ToCharArray());

				writer.Close();
				memStream.Close();

				UInt32 pid;
				LoaderError err;

				string encPath = Path.Combine(Core.BaseDirectory, "EncPatcher.dll");

				fixed( byte* para_ptr = param )
					err = (LoaderError)Load(client, encPath, "Attach", para_ptr, param.Length, out pid);

				if( err == LoaderError.SUCCESS )
				{

					if( pid != 0 && razor != "Not Found" && entry.AllowRazor )
					{
						string opts;
						if( entry.RemoveEncryption )
							opts = "--clientenc";
						else
							opts = "--clientenc --serverenc";

						System.Diagnostics.Process.Start(razor, String.Format("{0} --pid {1}", opts, pid));

						if (MainForm.Instance.Visible)
							MainForm.Instance.Invoke((MethodInvoker)delegate { MainForm.Instance.WindowState = FormWindowState.Minimized; });
					}
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(String.Format("Loader.dll ERROR: {0} (pid = {1})", err, pid));
				}
			}
			catch( Exception e )
			{
				System.Windows.Forms.MessageBox.Show(e.ToString());
				Trace.HandleException(e);
			}			
		}

		[DllImport("Loader.dll", CallingConvention = CallingConvention.StdCall)]
		private static unsafe extern UInt32 Load(string clientPath, string dllPath, string funcName, byte* ptr, Int32 dataSize, out UInt32 pid_ptr);

		private static string BuildPatchList(IEntry entry, out string patch)
		{
			string file = "fileoverrides.cuo";
			patch = string.Empty;

			string name = Uri.EscapeDataString(entry.Name);
			string path = Path.Combine(AppSettings.Current.Get<string>("PatchingPath"), Path.Combine("Servers", name));

			if( !Directory.Exists(path) )
				Directory.CreateDirectory(path);

			CreateLogin(path, entry);

			if( !String.IsNullOrEmpty(entry.PatchUrl) )
				patch = GetPatchList(path, entry);

			DirectoryInfo dir = new DirectoryInfo(path);
			FileInfo[] files = dir.GetFiles();

			using( StreamWriter sw = new StreamWriter(Path.Combine(path, file), false) )
			{
				for( int i = 0; i < files.Length; i++ )
				{
					if( files[i].Name == file || files[i].Name.ToLower() == "patch.rar" )
						continue;

					sw.WriteLine(String.Format("{0}={1}", files[i].Name, files[i].FullName));
				}

				sw.Close();
			}

			return Path.Combine(path, file);
		}

		private static string GetPatchList(string path, IEntry entry)
		{
			if( Path.GetExtension(entry.PatchUrl) != ".txt" )
				return "";

			WebClient client = new WebClient();
			try
			{
				client.DownloadFile(new Uri(entry.PatchUrl), Path.Combine(path, "patchlist.txt"));
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("There was an error while downloading the patchlist\nPlay at your own risk.", "Error");
				return "";
			}

			StreamReader reader = new StreamReader(Path.Combine(path, "patchlist.txt"));

			List<string> patches = new List<string>();
			string patch = string.Empty;
			while( ( patch = reader.ReadLine() ) != null )
				patches.Add(patch);

			reader.Close();

			PatchForm form = new PatchForm();
			form.PatchUpdateURL = entry.UpdateUrl;
			form.PatchList = patches;
			form.PatchFolder = path;
			form.ShowDialog();

			return form.PatchFile;
		}

		private static void CreateLogin(string path, IEntry entry)
		{
			string file = Path.Combine(path, "login.cfg");

			StreamWriter writer = new StreamWriter(file);
			writer.WriteLine(";Loginservers for " + entry.Name);
			writer.WriteLine(";Editing this file will cause issues with login and support");
			writer.WriteLine(";ConnectUO generated entry");
			writer.WriteLine("LoginServer=" + entry.HostAddress + "," + entry.Port.ToString());
			writer.Close();
		}

		/// <summary>
		/// Shortens the string to the specified length, adds ... to the end of the string
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Shorten(string str, int length)
		{
			if( str.Length < length )
				return str;

			str = str.Substring(0, length - 3);
			str += "...";

			return str;
		}

		internal static string ParseUrl(string url, string key)
		{
			string[] split = url.Split('&');
			string value = string.Empty;

			for (int i = 0; i < split.Length; i++)
				if (split[i].Contains(key))
				{
					split = split[i].Split('=');
					value = split[1];
					break;
				}

			return value;
		}

		/// <summary>
		/// Sorts HTTP GET data into a Dictionary
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static Dictionary<string, string> RetrieveGetData(string[] args)
		{
			Dictionary<string, string> getData = new Dictionary<string, string>();

			if (args.Length < 1)
				return getData;

			string[] split = args[0].Split('&');

			for (int i = 0; i < split.Length; i++)
			{
				string[] keyValuePair = split[i].Split('=');

				if (keyValuePair.Length != 2)
					continue;

                if( !getData.ContainsKey(keyValuePair[0]) )
				    getData.Add(keyValuePair[0], keyValuePair[1]);
			}

			return getData;
		}
	}

	/// <summary>
	/// Client load status'
	/// </summary>
	public enum LoaderError
	{
		SUCCESS = 0,
		NO_OPEN_EXE,
		NO_READ_EXE_DATA,
		NO_RUN_EXE,
		NO_ALLOC_MEM,
		NO_WRITE,
		NO_VPROTECT,
		NO_READ,
	};
}

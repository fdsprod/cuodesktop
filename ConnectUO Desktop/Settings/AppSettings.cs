using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CUODesktop
{
	public class AppSettings : Settings
	{
		public static AppSettings Current;
		
		public override string FilePath
		{
			get
			{
				return Path.Combine(Core.BaseDirectory, "cuodesktop.conf");
			}
		}

		public static void Initialize()
		{
			Current = new AppSettings(File.Exists(Path.Combine(Core.BaseDirectory, "cuodesktop.conf")));
		}

		public AppSettings(bool loadFromFile)
			: base()
		{
			SetDefaults();

			if (loadFromFile)
				Load(FilePath);
		}

		public override void Load(string filename)
		{
			base.Load(filename);
			Set<string>("ProxyHostPassword", SimpleCrypt(Get<string>("ProxyHostPassword")));
		}

		public override void Save()
		{
			string prev = Get<string>("ProxyHostPassword");
			Set<string>("ProxyHostPassword", SimpleCrypt(prev));

			base.Save();

			Set<string>("ProxyHostPassword", SimpleCrypt(prev));
		}

		private string SimpleCrypt(string prev)
		{
			char[] chars = prev.ToCharArray();

			for (int i = 0; i < chars.Length; i++)
				chars[i] = (char)((byte)chars[i] ^ 0xAA);

			StringBuilder builder = new StringBuilder();
			builder.Append(chars);

			return builder.ToString();
		}

		public override void SetDefaults()
		{
			bool prevState = AllowSaving;

			AllowSaving = false;
			Set<int>("AddLocalOpacity", 100);
			Set<int>("ConfigOpacity", 100);
			Set<int>("BrowserOpacity", 100);
			Set<int>("AboutOpacity", 100);
			Set<int>("SplashOpacity", 100);
			Set<bool>("MinimizeToTray", true);
			Set<string>("PatchingPath", Core.BaseDirectory);
			Set<string>("ClientPath", Utility.DetectClient("Ultima Online"));
			Set<string>("RazorPath", Utility.DetectRazor());
			Set<string>("Template", "release");
			Set<bool>("LoadRazor", Get<string>("RazorPath") == "Not Found");
			Set<int>("ServersPerPage", 25);
			Set<long>("NextUpdateTime", DateTime.MinValue.Ticks);
			Set<bool>("OpenBrowserOnStartup", true);
			Set<bool>("ProxyEnabled", false);
			Set<string>("ProxyHostAddress", "");
			Set<int>("ProxyHostPort", 0);
			Set<string>("ProxyHostUsername", "");
			Set<string>("ProxyHostPassword", "");
			Set<uint>("VersioningId", 0);
			AllowSaving = prevState;
		}
	}
}

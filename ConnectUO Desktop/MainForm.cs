using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace CUODesktop
{
	public partial class MainForm : Form
	{
		private Thread _checkUpdatesThread;
		private bool _checkingForUpdates;

		private static MainForm _instance;
		public static MainForm Instance { get { return _instance; } }

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if( Visible )
				Hide();

			base.OnPaintBackground(e);
		}

		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Left )
				//Program.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/home?page=1"), ref socket);
				Utility.OpenUrl("http://localhost.:1980/home.html?page=1");
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			notifyIcon.Visible = false;
			Invalidate();

			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			_instance = this;

			MainForm.Instance.notifyIcon.ShowBalloonTip(10, "Serverlist", "Loading please wait...", ToolTipIcon.Info);
			ServerList.LoadServerList();
			MainForm.Instance.notifyIcon.ShowBalloonTip(10, "Serverlist", "Complete", ToolTipIcon.Info);

			if( DateTime.Now > Properties.Settings.Default.NextUpdateTime )
				CheckForUpdates();

			Properties.Settings.Default.SettingChanging += new System.Configuration.SettingChangingEventHandler(Default_SettingChanging);

			if( Properties.Settings.Default.RazorPath == "Not Installed" )
				Properties.Settings.Default.RazorPath = Utility.DetectRazor();

			if( Properties.Settings.Default.ClientPath == "Not Installed" )
				Properties.Settings.Default.ClientPath = Utility.DetectClient("Ultima Online");

			if( Properties.Settings.Default.ClientPath == "Not Installed" )
				Properties.Settings.Default.ClientPath = Utility.DetectClient("Ultima Online Third Dawn");

			if( Properties.Settings.Default.OpenBrowserOnStartup )
				Utility.OpenUrl(Core.StartAddress);
		}

		private void InternalCheckForUpdates()
		{
			try
			{
				WebClient client = new WebClient();
				byte[] buffer = client.DownloadData(new Uri("http://www.connectuo.com/cuodesktop_latestver.txt"));

				string version = Encoding.ASCII.GetString(buffer);
				Version latest = new Version(version);
				Version current = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

				if( current.CompareTo(latest) == -1 && MessageBox.Show("A newer version of ConnectUO Desktop is available!\nDo you wish to update?", "New Version", MessageBoxButtons.YesNo) == DialogResult.Yes )
				{
                    System.Diagnostics.Process.Start(Path.Combine(Core.BaseDirectory, "Updater.exe"));
					Properties.Settings.Default.NextUpdateTime = (DateTime)(DateTime.Now + TimeSpan.FromDays(1));
					Close();
				}
			}
			catch
			{

			}

			_checkUpdatesThread.Join();
		}

		internal void CheckForUpdates()
		{
			if (_checkUpdatesThread != null && (_checkUpdatesThread.ThreadState == ThreadState.Running || _checkUpdatesThread.ThreadState == ThreadState.Background))
				return;

			if (_checkUpdatesThread != null)
				_checkUpdatesThread.Abort();

			_checkUpdatesThread = new Thread(InternalCheckForUpdates);
			_checkUpdatesThread.Start();
		}

		private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		public void addLocalServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddLocalServerDialog d = new AddLocalServerDialog();
			bool editmode = (sender != null && sender is object[]);
			d.EditMode = editmode;
			CustomEntry en = null;

			if( editmode )
			{
				string request = (string)( (object[])sender )[0];
				Socket socket = (Socket)( (object[])sender )[1];

				en = (CustomEntry)ServerList.GetServerById(Utility.ParseUrl(request, "id"));

				if( en == null )
				{
					MessageBox.Show("The requested server entry was invalid or did not exist");
					Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
					//Utility.OpenUrl("http://localhost.:1980/favorites");
					return;
				}

				d.ServerName = en.Name;
				d.ServerDescription = en.Description;
				d.ServerAddress = en.HostAddress ;
				d.ServerPort = en.Port.ToString();
				d.ServerUpdateURL = en.UpdateUrl;
				d.ServerPatchURL = en.PatchUrl;
				d.RemoveEnc = en.RemoveEncryption;

				Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
			}
			else
				Utility.OpenUrl("http://localhost.:1980/favorites.html");

			if( d.ShowDialog() == DialogResult.OK )
			{
				if( !editmode )
					en = new CustomEntry();

				en.Name = d.ServerName;
				en.Description = d.ServerDescription;
				en.HostAddress = d.ServerAddress;
				en.Port = int.Parse(d.ServerPort);
				en.UpdateUrl = d.ServerUpdateURL;
				en.PatchUrl = d.ServerPatchURL;
				en.RemoveEncryption = d.RemoveEnc;

				Favorites.AddCustom(en); 
			}
		}

		public void configurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfigDialog c = new ConfigDialog();
			c.ShowDialog();
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckForUpdates();
		}

		private void publicListToolStripMenuItem_Click(object sender, EventArgs e)
		{
            Utility.OpenUrl("http://localhost.:1980/home.html?page=1");
		}

		private void favoritesToolStripMenuItem_Click(object sender, EventArgs e)
		{
            Utility.OpenUrl("http://localhost.:1980/favorites.html");
		}

		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
            Utility.OpenUrl("http://localhost.:1980/help.html");
		}

		private void updateServerlistToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( !ServerList.Updating )
				ServerList.Update();
		}
	}
}
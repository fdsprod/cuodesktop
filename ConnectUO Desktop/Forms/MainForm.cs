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

using Microsoft.Win32;

namespace CUODesktop
{
    public partial class MainForm : Form
    {
        private int _current = -1;
        private int _max = -1;
        private bool _newPage = false;
        private bool _goingBack = false;
        private bool _goingForward = false;
        private Thread _checkUpdatesThread;

        private static MainForm _instance;
        public static MainForm Instance { get { return _instance; } set { _instance = new MainForm(); } }

        public MainForm()
        {
            InitializeComponent();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
			if (e.Button == MouseButtons.Left)
			{
				Show();
				WindowState = FormWindowState.Normal;
				notifyIcon.Visible = false;
			}
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Visible = false;
            Invalidate();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			if (AppSettings.Current.Get<bool>("MinimizeToTray") && WindowState == FormWindowState.Minimized)
			{
				notifyIcon.Visible = true;
				Hide();
			}

			if (WindowState == FormWindowState.Normal)
			{
				foreach (System.Windows.Forms.Control c in Controls)
					c.Refresh();
			}
		}

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

		private double GetOpacity(string setting)
		{
			int settingsOp = AppSettings.Current.Get<int>(setting);
			double op = (double)settingsOp * .01;

			if (op < .20)
				op = .20;

			if (op > 1.0)
				op = 1.0;

			return op;
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_instance == null)
                _instance = this;
			
			Opacity = GetOpacity("BrowserOpacity");

            Text = String.Format("ConnectUO Desktop - v.{0}.{1}.{3}.{2}", Core.Version.Major, Core.Version.Minor, Core.Version.Revision, Core.Version.Build);

            if (DateTime.Now.Ticks > AppSettings.Current.Get<long>("NextUpdateTime"))
                CheckForUpdates();

            if (AppSettings.Current.Get<string>("RazorPath") == "Not Installed")
                AppSettings.Current.Set<string>("RazorPath", Utility.DetectRazor());

            if (AppSettings.Current.Get<string>("ClientPath") == "Not Installed")
                AppSettings.Current.Set<string>("ClientPath", Utility.DetectClient("Ultima Online"));

            if (AppSettings.Current.Get<string>("ClientPath") == "Not Installed")
                AppSettings.Current.Set<string>("ClientPath", Utility.DetectClient("Ultima Online Third Dawn"));

            SplashScreen screen = new SplashScreen();
			screen.Opacity = GetOpacity("SplashOpacity");
            screen.ShowDialog();

            FixAddressBar();

            WebBrowser.DocumentTitleChanged += new EventHandler(OnDocumentTitleChanged);
            WebBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(OnProgressChanged);
            WebBrowser.StatusTextChanged += new EventHandler(OnStatusTextChanged);

            Utility.OpenUrl("http://localhost.:1980/home.html?page=1");
        }

		private static uint GetVersionId()
		{
			uint id = 0;
			string regkey = @"SOFTWARE\ConnectUO Desktop";
			using (RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey))
			{
				if (key.GetValue("VersionId") == null)
					return SetVersionId();
				else
					id = (uint)key.GetValue("VersionId");

				key.Close();
			}

			return id;
		}

		private static uint SetVersionId()
		{
			Random rand = new Random();
			uint id = (uint)(rand.Next(0, int.MaxValue) * 2);
			SetVersionId(id);

			return id;
		}

		private static void SetVersionId(uint id)
		{
			string regkey = @"SOFTWARE\ConnectUO Desktop";
			using (RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey))
			{
				key.SetValue("VersionId", id);
				key.Close();
			}
		}

        #region WebBrowser
        private void backBtn_Click(object sender, EventArgs e)
        {
            _goingBack = true;
            _newPage = false;
            _current--;

            WebBrowser.GoBack();
        }

        private void forwardBtn_Click(object sender, EventArgs e)
        {
            _goingForward = true;
            _newPage = false;
            _current++;
            WebBrowser.GoForward();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            WebBrowser.Refresh();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (WebBrowser.IsBusy)
                WebBrowser.Stop();
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            WebBrowser.Navigate("http://localhost.:1980/home.html?page=1");
        }

        private void goBtn_Click(object sender, EventArgs e)
        {
            WebBrowser.Navigate(addressBar.Text);
        }

        private void addressBar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                goBtn_Click(sender, e);
        }

        private void OnProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.CurrentProgress > 0 && e.MaximumProgress > 0)
            {
                int percent = (int)((e.CurrentProgress * 100) / e.MaximumProgress);

                if (percent > 100)
                    percent = 100;

                if (percent < 0)
                    percent = 0;

                if (percent != progressBar.Value)
                    progressBar.Value = percent;
            }
        }

        private void OnStatusTextChanged(object sender, EventArgs e)
        {
            statusLbl.Text = String.Format("Status: {0}", WebBrowser.StatusText);
        }

        private void OnDocumentTitleChanged(object sender, EventArgs e)
        {
            Text = String.Format("ConnectUO Desktop - v.{0}.{1}.{3}.{2} | {4}", Core.Version.Major, Core.Version.Minor, Core.Version.Revision, Core.Version.Build, WebBrowser.DocumentTitle);
        }

        private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            progressBar.Value = 0;
        }

        private void OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            addressBar.Text = e.Url.ToString();
            stopBtn.Enabled = true;

            if (!_goingBack && !_goingForward)
                _current = ++_max;

            backBtn.Enabled = _current != 0;
            forwardBtn.Enabled = _current != _max;

            _goingForward = false;
            _goingBack = false;
            _newPage = false;
        }

        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            stopBtn.Enabled = false;
        }
        #endregion

        #region Check For Updates
        private void InternalCheckForUpdates()
        {
            try
            {				
                WebClient client = new WebClient();
				byte[] buffer = client.DownloadData(new Uri(String.Format("http://www.connectuo.com/version.php?id={0}&version={1}", GetVersionId(), Core.Version.ToString())));

                string version = Encoding.ASCII.GetString(buffer);
                Version latest = new Version(version);
                Version current = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                if (current.CompareTo(latest) == -1 && MessageBox.Show("A newer version of ConnectUO Desktop is available!\nDo you wish to update?", "New Version", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(Path.Combine(Core.BaseDirectory, "Updater.exe"));
                    AppSettings.Current.Set<long>("NextUpdateTime", ((DateTime)(DateTime.Now + TimeSpan.FromDays(1))).Ticks);
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
        #endregion

        public void addLocalServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLocalServerDialog d = new AddLocalServerDialog();
			d.Opacity = GetOpacity("AddLocalOpacity");

            bool editmode = (sender != null && sender is object[]);
            d.EditMode = editmode;
            CustomEntry en = null;

            if (editmode)
            {
                string request = (string)((object[])sender)[0];
                Socket socket = (Socket)((object[])sender)[1];

                en = (CustomEntry)ServerList.GetServerById(Utility.ParseUrl(request, "id"));

                if (en == null)
                {
                    MessageBox.Show("The requested server entry was invalid or did not exist");
                    Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
                    //Utility.OpenUrl("http://localhost.:1980/favorites");
                    return;
                }

                d.ServerName = en.Name;
                d.ServerDescription = en.Description;
                d.ServerAddress = en.HostAddress;
                d.ServerPort = en.Port.ToString();
                d.ServerUpdateURL = en.UpdateUrl;
                d.ServerPatchURL = en.PatchUrl;
                d.RemoveEnc = en.RemoveEncryption;

                Core.Server.SendToBrowser(String.Format(Utility.META_REDIRECT, "http://localhost.:1980/favorites.html"), ref socket);
            }
            
            if (d.ShowDialog() == DialogResult.OK)
            {
                if (!editmode)
                    en = new CustomEntry();

                en.Name = d.ServerName;
                en.Description = d.ServerDescription;
                en.HostAddress = d.ServerAddress;
                en.Port = int.Parse(d.ServerPort);
                en.UpdateUrl = d.ServerUpdateURL;
                en.PatchUrl = d.ServerPatchURL;
                en.RemoveEncryption = d.RemoveEnc;

                Favorites.AddCustom(en);
				Utility.OpenUrl("http://localhost.:1980/favorites.html");
            }
        }

        public void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigDialog c = new ConfigDialog();
			c.Opacity = GetOpacity("ConfigOpacity");

			double currentOp = Opacity;

			if (c.ShowDialog() == DialogResult.OK)
			{
				AppSettings.Current.SettingsTable = c.Settings.SettingsTable;
				AppSettings.Current.Save();

				Templates.LoadTemplate(AppSettings.Current.Get<string>("Template"));

				if (ConfigDialog.GetLoadWithWindows() != c.LoadWithWindows)
				{
					string regkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

					using (RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey))
					{
						if (c.LoadWithWindows)
							key.SetValue("cuodesktop", Core.ExePath);
						else
							key.DeleteValue("cuodesktop", false);
					}
				}

				Opacity = GetOpacity("BrowserOpacity");
				Refresh();
			}
			else
				Opacity = currentOp;
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
			SplashScreen screen = new SplashScreen();
			screen.Opacity = GetOpacity("SplashOpacity");
            screen.ShowDialog(this);
        }

        protected override void OnRegionChanged(EventArgs e)
        {
            base.OnRegionChanged(e);
            FixAddressBar();
        }

        private void FixAddressBar()
        {
            int offset = 60;

            for (int i = 0; i < toolStrip1.Items.Count; i++)
            {
                if (toolStrip1.Items[i] is ToolStripButton)
                    offset += toolStrip1.Items[i].Width;
            }

            int width = Width - offset;

            addressBar.Size = new Size(width, addressBar.Height);
            Invalidate();
        }

        private void MainForm_ClientSizeChanged(object sender, EventArgs e)
        {
            FixAddressBar();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
			about.Opacity = GetOpacity("AboutOpacity");
            about.ShowDialog(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace CUODesktop
{
	public partial class ConfigDialog : Form
	{
		private AppSettings _settings;
		private bool _loadWithWindows;

		public AppSettings Settings { get { return _settings; } }
		public bool LoadWithWindows { get { return _loadWithWindows; } }

		public ConfigDialog()
		{
			DialogResult = DialogResult.Cancel;
			InitializeComponent();
		}

		private void ConfigDialog2_Load(object sender, EventArgs e)
		{
			_settings = new AppSettings(false);
			_settings.Set<uint>("VersioningId", AppSettings.Current.Get<uint>("VersioningId"));
			
			_settings.AllowSaving = false;
				
			addLocalTrackBar.Value = AppSettings.Current.Get<int>("AddLocalOpacity");
			splashTrackBar.Value = AppSettings.Current.Get<int>("SplashOpacity");
			browserTrackBar.Value = AppSettings.Current.Get<int>("BrowserOpacity");
			aboutTrackBar.Value = AppSettings.Current.Get<int>("AboutOpacity");
			configTrackBar.Value = AppSettings.Current.Get<int>("ConfigOpacity");

			clientPathBox.Text = AppSettings.Current.Get<string>("ClientPath");
			patchPathBox.Text = AppSettings.Current.Get<string>("PatchingPath");
			razorPathBox.Text = AppSettings.Current.Get<string>("RazorPath");
			loadRazorChkBox.Checked = AppSettings.Current.Get<bool>("LoadRazor");
			perPageBox.Text = AppSettings.Current.Get<int>("ServersPerPage").ToString(); ;
			templateBox.Text = AppSettings.Current.Get<string>("Template");
			openBrowserBox.Checked = AppSettings.Current.Get<bool>("OpenBrowserOnStartup");
			minimizeToTrayBox.Checked = AppSettings.Current.Get<bool>("MinimizeToTray");
			loadWithWindowsBox.Checked = GetLoadWithWindows();

			proxyEnabledChkBox.Checked = AppSettings.Current.Get<bool>("ProxyEnabled");
			proxyHostBox.Text = AppSettings.Current.Get<string>("ProxyHostAddress");
			proxyPortBox.Text = AppSettings.Current.Get<int>("ProxyHostPort").ToString();
			proxyUsernameBox.Text = AppSettings.Current.Get<string>("ProxyUsername");
			proxyPasswordBox.Text = AppSettings.Current.Get<string>("ProxyPassword");

			proxyGroup.Enabled = proxyEnabledChkBox.Checked;

			List<string> keys = new List<string>(Templates.LoadedTemplates.Keys);
			for (int i = 0; i < keys.Count; i++)
				templateBox.Items.Add(Templates.LoadedTemplates[keys[i]].Name);
		}

		private void SetDefaults()
		{			
			_settings.SetDefaults();
			_settings.Set<uint>("VersioningId", AppSettings.Current.Get<uint>("VersioningId"));
			_settings.AllowSaving = false;

			clientPathBox.Text = _settings.Get<string>("ClientPath");
			patchPathBox.Text = _settings.Get<string>("PatchingPath");
			razorPathBox.Text = _settings.Get<string>("RazorPath");
			loadRazorChkBox.Checked = _settings.Get<bool>("LoadRazor");
			perPageBox.Text = _settings.Get<int>("ServersPerPage").ToString(); ;
			templateBox.Text = _settings.Get<string>("Template");
			openBrowserBox.Checked = _settings.Get<bool>("OpenBrowserOnStartup");
			minimizeToTrayBox.Checked = _settings.Get<bool>("MinimizeToTray");
			loadWithWindowsBox.Checked = GetLoadWithWindows();
			addLocalTrackBar.Value = _settings.Get<int>("AddLocalOpacity");
			splashTrackBar.Value = _settings.Get<int>("SplashOpacity");
			browserTrackBar.Value = _settings.Get<int>("BrowserOpacity");
			aboutTrackBar.Value = _settings.Get<int>("AboutOpacity");
			configTrackBar.Value = _settings.Get<int>("ConfigOpacity");

			proxyEnabledChkBox.Checked = _settings.Get<bool>("ProxyEnabled");
			proxyHostBox.Text = _settings.Get<string>("ProxyHostAddress");
			proxyPortBox.Text = _settings.Get<int>("ProxyHostPort").ToString();
			proxyUsernameBox.Text = _settings.Get<string>("ProxyUsername");
			proxyPasswordBox.Text = _settings.Get<string>("ProxyPassword");
		}

		private void clientPathBox_TextChanged(object sender, EventArgs e)
		{
			_settings.Set<string>("ClientPath", clientPathBox.Text);
		}

		private void patchPathBox_TextChanged(object sender, EventArgs e)
		{
			_settings.Set<string>("PatchingPath", patchPathBox.Text);
		}

		private void razorPathBox_TextChanged(object sender, EventArgs e)
		{
			_settings.Set<string>("RazorPath", razorPathBox.Text);
		}		

		private void loadRazorChkBox_CheckedChanged(object sender, EventArgs e)
		{
			_settings.Set<bool>("LoadRazor", loadRazorChkBox.Checked);
		}

		private void perPageBox_TextChanged(object sender, EventArgs e)
		{
			int perPage;

			if (int.TryParse(perPageBox.Text, out perPage))
				_settings.Set<int>("ServersPerPage", perPage);
		}

		private void templateBox_TextChanged(object sender, EventArgs e)
		{
			if (Templates.IsValidTemplateName(templateBox.Text))
				_settings.Set<string>("Template", templateBox.Text);
		}

		private void openBrowserBox_CheckedChanged(object sender, EventArgs e)
		{
			_settings.Set<bool>("OpenBrowserOnStartup", openBrowserBox.Checked);
		}

		private void minimizeToTrayBox_CheckedChanged(object sender, EventArgs e)
		{
			_settings.Set<bool>("MinimizeToTray", minimizeToTrayBox.Checked);			
		}

		private void loadWithWindowsBox_CheckedChanged(object sender, EventArgs e)
		{
			_loadWithWindows = loadWithWindowsBox.Checked;
		}

		private void detect2dBtn_Click(object sender, EventArgs e)
		{
			clientPathBox.Text = Utility.DetectClient("Ultima Online");
		}

		private void detect3dBtn_Click(object sender, EventArgs e)
		{
			clientPathBox.Text = Utility.DetectClient("Ultima Online Third Dawn");
		}

		private void browseClientBtn_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Please find the client you wish to launch";
			openFileDialog.FileName = "client.exe";
			openFileDialog.Filter = "UO Client | client.exe";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
				clientPathBox.Text = openFileDialog.FileName;
		}

		private void detectRazorBtn_Click(object sender, EventArgs e)
		{
			razorPathBox.Text = Utility.DetectRazor();
		}

		private void browseRazorBtn_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Please find the razor.exe you wish to launch";
			openFileDialog.FileName = "razor.exe";
			openFileDialog.Filter = "Razor | razor.exe";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
				razorPathBox.Text = openFileDialog.FileName;
		}

		private void defaultsBtn_Click(object sender, EventArgs e)
		{
			SetDefaults();
		}

		private void okBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		public static bool GetLoadWithWindows()
		{
			bool load = true;

			try
			{
				string regkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
				using (RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey))
				{
					if (key.GetValue("cuodesktop") == null)
						load = false;

					key.Close();
				}
			}
			catch
			{
				load = false;
			}

			return load;
		}		

		private void addLocalTrackBar_Scroll(object sender, EventArgs e)
		{
			_settings.Set<int>("AddLocalOpacity", addLocalTrackBar.Value);
		}

		private void configTrackBar_Scroll(object sender, EventArgs e)
		{
			double op =(double)configTrackBar.Value * .01;

			if( op < .20 )
				op = .20;

			Opacity = op;
			_settings.Set<int>("ConfigOpacity", configTrackBar.Value);
		}

		private void browserTrackBar_Scroll(object sender, EventArgs e)
		{
			double op = (double)browserTrackBar.Value * .01;

			if (op < .20)
				op = .20;

			MainForm.Instance.Opacity = op;
			_settings.Set<int>("BrowserOpacity", browserTrackBar.Value);
		}

		private void aboutTrackBar_Scroll(object sender, EventArgs e)
		{
			_settings.Set<int>("AboutOpacity", aboutTrackBar.Value);
		}

		private void splashTrackBar_Scroll(object sender, EventArgs e)
		{
			_settings.Set<int>("SplashOpacity", splashTrackBar.Value);
		}

		private void pathBtn_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
				patchPathBox.Text = folderBrowserDialog.SelectedPath;
		}

		private void proxyEnabledChkBox_CheckedChanged(object sender, EventArgs e)
		{
			_settings.Set<bool>("ProxyEnabled", proxyEnabledChkBox.Checked);
			proxyGroup.Enabled = proxyEnabledChkBox.Checked;
		}

		private void proxyHostBox_TextChanged(object sender, EventArgs e)
		{
			_settings.Set<string>("ProxyHostAddress", proxyHostBox.Text);
		}

		private void proxyPortBox_TextChanged(object sender, EventArgs e)
		{
			int port;

			if (int.TryParse(proxyPortBox.Text, out port))
				_settings.Set<int>("ProxyHostPort", port);
		}

		private void proxyUsernameBox_TextChanged(object sender, EventArgs e)
		{
			_settings.Set<string>("ProxyUsername", proxyUsernameBox.Text);
		}

		private void proxyPasswordBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			_settings.Set<string>("ProxyPassword", proxyPasswordBox.Text);
		}
	}
}
/*			proxyEnabledChkBox.CanFocus = AppSettings.Current.Get<bool>("ProxyEnabled");
			proxyHostBox.Text = AppSettings.Current.Get<string>("ProxyHostAddress");
			proxyPortBox.Text = AppSettings.Current.Get<int>("ProxyHostPort");
			proxyUsernameBox.Text = AppSettings.Current.Get<string>("ProxyUsername");
			proxyPasswordBox.Text = AppSettings.Current.Get<string>("ProxyPassword");*/
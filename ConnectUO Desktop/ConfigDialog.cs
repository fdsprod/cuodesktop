using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Microsoft.Win32;

namespace CUODesktop
{
    public partial class ConfigDialog : Form
    {
        private bool _fromSettings = true;

        private string _ClientPath;
        private string _RazorPath;

        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void ConfigDialog_Load( object sender, EventArgs e )
        {
            //BackgroundImage = SkinManager.Background != null ? SkinManager.Background :
                //Properties.Resources.Background;
                        
            DisplaySettings();            
        }

        private void DisplaySettings()
        {
            if( _fromSettings )
            {
                _fromSettings = false;
				_ClientPath = Properties.Settings.Default.ClientPath;
                _RazorPath = Properties.Settings.Default.RazorPath;

                clientLbl.Text = Utility.Shorten( "Client Path: " + _ClientPath, 60 );
                razorLbl.Text = Utility.Shorten( "Razor Path: " + _RazorPath, 60 );

				templateBox.Text = Properties.Settings.Default.Template;

				List<Template> templates = new List<Template>(Templates.LoadedTemplates.Values);
				for( int i = 0; i < templates.Count; i++ )
					templateBox.Items.Add(templates[i].Name);

				loadWithWindowsBox.Checked = GetLoadWithWindows();
				openBrowserBox.Checked = Properties.Settings.Default.OpenBrowserOnStartup;

				perPageBox.Text = Properties.Settings.Default.ServersPerPage.ToString();

                loadRazorChkBox.Checked = Properties.Settings.Default.LoadRazor;                
            }
            else
            {
                clientLbl.Text = Utility.Shorten( "Client Path: " + _ClientPath, 60 );
                razorLbl.Text = Utility.Shorten( "Razor Path: " + _RazorPath, 60 );
            }
        }

		private void SetLoadWithWindows(bool load)
		{
			try
			{
				string regkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
				using( RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey) )
				{
					if( load )
						key.SetValue("cuodesktop", Core.ExePath);
					else
						key.DeleteValue("cuodesktop", false);

					key.Close();
				}
			}
			catch
			{
			}
		}

		private bool GetLoadWithWindows()
		{
			bool load = true;
			try
			{
				string regkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
				using( RegistryKey key = Registry.LocalMachine.CreateSubKey(regkey) )
				{
					if( key.GetValue("cuodesktop") == null )
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

        private void cancelBtn_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void okBtn_Click( object sender, EventArgs e )
        {
            Properties.Settings.Default.ClientPath = _ClientPath;
            Properties.Settings.Default.RazorPath = _RazorPath;
			Properties.Settings.Default.LoadRazor = loadRazorChkBox.Checked;
			Properties.Settings.Default.Template = templateBox.Text;
			Templates.LoadTemplate(Properties.Settings.Default.Template);
			Properties.Settings.Default.OpenBrowserOnStartup = openBrowserBox.Checked;

			int perPage;

			if( int.TryParse(perPageBox.Text, out perPage) )
				Properties.Settings.Default.ServersPerPage = perPage;

			SetLoadWithWindows(loadWithWindowsBox.Checked);

            Properties.Settings.Default.Save();
            Close();
        }

        private void detect2dBtn_Click( object sender, EventArgs e )
        {
            _ClientPath = Utility.DetectClient( "Ultima Online" );
            DisplaySettings();
        }

        private void detect3dBtn_Click( object sender, EventArgs e )
        {
			_ClientPath = Utility.DetectClient("Ultima Online Third Dawn");
            DisplaySettings();
        }

        private void browseClientBtn_Click( object sender, EventArgs e )
        {
            openFileDialog.Title = "Please find the client you wish to launch";
            openFileDialog.FileName = "client.exe";
            openFileDialog.Filter = "UO Client | client.exe";

            DialogResult res = openFileDialog.ShowDialog();

            if( res == DialogResult.OK )
            {
                _ClientPath = openFileDialog.FileName;
                DisplaySettings();
            }
        }

        private void detectRazorBtn_Click( object sender, EventArgs e )
        {
			_RazorPath = Utility.DetectRazor();
            DisplaySettings();
        }

        private void browseRazorBtn_Click( object sender, EventArgs e )
        {
            openFileDialog.Title = "Please find the razor.exe you wish to launch";
            openFileDialog.FileName = "razor.exe";
            openFileDialog.Filter = "Razor | razor.exe";

            DialogResult res = openFileDialog.ShowDialog();

            if( res == DialogResult.OK )
            {
                _RazorPath = openFileDialog.FileName;
                loadRazorChkBox.Checked = true;
                DisplaySettings();
            }         
        }
    }
}
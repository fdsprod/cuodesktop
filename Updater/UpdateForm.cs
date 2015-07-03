using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace Updater
{
    public partial class UpdateForm : Form
    {
        private WebClient _changeLogClient;
        private WebClient _updateClient;
        private System.Timers.Timer _closeTimer;
        private bool _cancelled = false;
		private int _closeTime = 30;

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
			Process[] processes;
			if (IsConnectUORunning(out processes))
			{
				if (DialogResult.Yes == MessageBox.Show("ConnectUO is currently running, do you wish to stop this process and allow it to update?", "ConnectUO is Running", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
				{
					for (int i = 0; i < processes.Length; i++)
						processes[i].Kill();
				}
				else
					Close();
			}

			System.Threading.Thread.Sleep(100);

            InitializeClients();
            DownloadFiles();
        }

		private bool IsConnectUORunning(out Process[] processes)
		{
			processes = Process.GetProcessesByName("CUODesktop.exe");
			return processes.Length > 0;
		}

        private void DownloadFiles()
        {
            changeLogBox.Text = "(Retrieving changelog...)";
            _changeLogClient.DownloadDataAsync(new Uri("http://www.connectuo.com/release/cuo_changelog.txt"));
            _updateClient.DownloadFileAsync(new Uri("http://www.connectuo.com/release/CUODesktop_Latest_Update.zip"), Path.Combine(Directory.GetCurrentDirectory(), "update.zip"));
        }

        private void InitializeClients()
        {
            _changeLogClient = new WebClient();
            _changeLogClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(OnDownloadDataComplete);

            _updateClient = new WebClient();
            _updateClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
			_updateClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);
        }

		private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            statusLbl.Text = String.Format("Downloading...{0:0,0}/{1:0,0} bytes", e.BytesReceived, e.TotalBytesToReceive);
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                statusLbl.Text = "Operation cancelled by user.";
                _cancelled = true;
            }

            if (e.Error != null)
            {
				if( MessageBox.Show("Download Error!", "An error occurred while trying to download the current update.\nThis may be due to a issue with your internet connection, or the server is down.\nPlease try again later.\n\nClick Ok to start ConnectUO Desktop without updating.", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK )
					UpdateComplete();
				else
					base.Close();
            }

            ExtractFiles();
        }

        private void ExtractFiles()
        {
            if (!_cancelled)
            {
				statusLbl.Text = "Extracting files...";
                try
                {
					using( ZipInputStream s = new ZipInputStream(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "update.zip"))) )
					{
						ZipEntry theEntry;
						while( ( theEntry = s.GetNextEntry() ) != null )
						{
							string directoryName = Path.GetDirectoryName(theEntry.Name);
							string fileName = Path.GetFileName(theEntry.Name);

							// create directory
							if( directoryName.Length > 0 )
								Directory.CreateDirectory(directoryName);
							
							if( fileName != String.Empty && !fileName.Contains("Update"))
							{
								using( FileStream streamWriter = File.Create(theEntry.Name) )
								{

									int size = 2048;
									byte[] data = new byte[2048];
									while( true )
									{
										size = s.Read(data, 0, data.Length);
										if( size > 0 )
										{
											streamWriter.Write(data, 0, size);
										}
										else
										{
											streamWriter.Close();
											break;
										}
									}
								}
							}
						}
					}
                }
                catch( Exception e )
                {
					MessageBox.Show("An error occurred while trying to extract the current update.\n\n" + e.ToString(), "Extraction Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

			UpdateComplete();
        }

        private void UpdateComplete()
		{
			statusLbl.Text = "Click close to start ConnectUO Desktop.";

			_closeTimer = new System.Timers.Timer(1000);
			_closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTick);
			_closeTimer.Start();
        }

		private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			_closeTime--;

			if( _closeTime < 1 )
				StartConnectUO();
			else
				Invoke((MethodInvoker)delegate { cancelBtn.Text = "Close (" + _closeTime.ToString() + ")"; });
		}

        private void OnDownloadDataComplete(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            
            if (e.Error != null)
                changeLogBox.Text = "Error downloading changelog.";
            else
            {
                string changelog = Encoding.ASCII.GetString(e.Result, 0, e.Result.Length);
                string[] updateInfo = changelog.Split(new string[] { "\n" }, StringSplitOptions.None);
                changeLogBox.Lines = updateInfo;
            }
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_changeLogClient != null && _changeLogClient.IsBusy)
            {
                _changeLogClient.CancelAsync();
                _changeLogClient.Dispose();
            }

            if (_updateClient != null && _updateClient.IsBusy)
            {
                _updateClient.CancelAsync();
                _updateClient.Dispose();
            }

            if (_closeTimer != null)
                _closeTimer.Dispose();
        }

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			if( _closeTimer == null )
			{
				if( _updateClient != null )
					_updateClient.CancelAsync();

				if( _changeLogClient != null )
					_changeLogClient.CancelAsync();

				UpdateComplete();
			}
			else
				StartConnectUO();
		}

		private void StartConnectUO()
		{
			_closeTimer.Stop();
			System.Diagnostics.Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "CUODesktop.exe"));
			Close();
		}
    }
}
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;

namespace CUODesktop
{
	public class ServerListUpdater
	{
		private SplashScreen _splashScreen;
		private Thread _thread;
		private WebClient _client;
		private bool _isBusy;
		private static string gzFile = Path.Combine(Core.BaseDirectory, "list.xml.gz");
		private static string listFile = Path.Combine(Core.BaseDirectory, "list.xml");
		private static string tmpFile = Path.Combine(Core.BaseDirectory, "list.xml.tmp");

		private ProxyInfo _proxyInfo;

		public event EventHandler UpdateComplete;

		public bool IsBusy { get { return _isBusy; } }

		public ServerListUpdater(SplashScreen splashScreen)
		{			
			_splashScreen = splashScreen;
			_splashScreen.CloseRequestEvent += new EventHandler(OnCloseRequested);

			_client = new WebClient();
			_client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(OnDownloadFileCompleted);
			_client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);

			_proxyInfo = new ProxyInfo(
				AppSettings.Current.Get<string>("ProxyHostAddress"),
				AppSettings.Current.Get<int>("ProxyHostPort"),
				AppSettings.Current.Get<string>("ProxyUsername"),
				AppSettings.Current.Get<string>("ProxyPassword")
			);
		}

		private void OnCloseRequested(object sender, EventArgs args)
		{	
			if (_client.IsBusy)
				_client.CancelAsync();

			EndUpdate();
		}

		public void Update()
		{
			if (_thread == null )
				_thread = new Thread(InternalUpdate);

			if (_thread.ThreadState == ThreadState.Background || _thread.ThreadState == ThreadState.Running)
				return;

			if( _thread != null )
				_thread = new Thread(InternalUpdate);
			
			_thread.Start();					
		}

		private void InternalUpdate()
		{
			_isBusy = true;

			try
			{
				if (File.Exists(gzFile))
					File.Delete(gzFile);
			}
			catch { }

			if (AppSettings.Current.Get<bool>("ProxyEnabled") && !String.IsNullOrEmpty(_proxyInfo.Server))
			{
				WebProxy proxy = null;
				bool success = true;
				try
				{
					proxy = new WebProxy(_proxyInfo.Server, _proxyInfo.Port);
					proxy.Credentials = new NetworkCredential(_proxyInfo.Username, _proxyInfo.Password);
					proxy.UseDefaultCredentials = false;
				}
				catch { }

				if( success )
					_client.Proxy = proxy;
				else
					_client.Proxy = null;
			}
			else
				_client.Proxy = null;


			_client.DownloadFileAsync(new Uri("http://www.connectuo.com/list.xml.gz"), gzFile);		
		}

		private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			_splashScreen.Progress = e.ProgressPercentage;
			_splashScreen.Status = String.Format("Downloading {0}/{1}", e.BytesReceived.ToString("0,0"), e.TotalBytesToReceive.ToString("0,0"));
		}

		private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			_splashScreen.Progress = 100;
			_splashScreen.Status = "Download Complete";

			if (e.Error != null)
				MessageBox.Show("An error occurred while trying to download the serverlist.\nPlease try check your internet connection and try again.", "ConnectUO Desktop");
			else if(!e.Cancelled)
			{
				if (ExtractList())
					CleanUp();
				else
					MessageBox.Show("An error occurred while trying to extract the serverlist.\nPlease quit all applications that could be accessing the files in the ConnectUO Desktop directory and try again.", "ConnectUO Desktop");
			}

			EndUpdate();
		}

		private void EndUpdate()
		{
			_splashScreen.Status = "Loading the serverlist...";
			ServerList.LoadServerList();

			_isBusy = false;

			if (UpdateComplete != null)
				UpdateComplete(this, new EventArgs());

			if (_splashScreen.Visible)							
				_splashScreen.Invoke((MethodInvoker)delegate { _splashScreen.Close(); });

			_thread.Abort();
		}

		private void CleanUp()
		{
			try
			{
				if (File.Exists(listFile))
					File.Delete(listFile);

				File.Move(tmpFile, listFile);

				File.Delete(tmpFile);
				File.Delete(gzFile);
			}
			catch
			{

			}
			finally
			{
				if (File.Exists(gzFile))
					File.Delete(gzFile);

				if (File.Exists(tmpFile))
					File.Delete(tmpFile);
			}
		}

		private bool ExtractList()
		{
			try
			{
				_splashScreen.Status = "Extracting...";

				byte[] buffer = new byte[0xFF];

				using (Stream s = new GZipInputStream(File.OpenRead(gzFile)))
				{
					using (FileStream fs = File.Create(tmpFile))
					{
						StreamUtils.Copy(s, fs, buffer);

						fs.Close();
						s.Close();
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}

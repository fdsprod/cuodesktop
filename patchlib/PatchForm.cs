using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace CUODesktop.PatchLib
{
	public partial class PatchForm : Form
	{
		private List<string> _patchList;
		private bool _oldPatchingStyle = true;
		private Thread _updateInfoThread;
		private string _patchFolder;
		private string _patchUpdateURL;
		private int _index;
		private System.Timers.Timer _closeTimer;
		private List<string> _patchFiles; 
		private bool _needsPatching = false;
		private int _closeTime = 30;
		private Thread _patchThread;
		private string _patchFile;
		private Patcher _patcher;
		private List<FileInfo> _allPatchFiles;

		public string PatchFile
		{
			get { return _patchFile; } 
		}

		public string PatchUpdateURL
		{
			get { return _patchUpdateURL; }
			set { _patchUpdateURL = value; }
		}

		public List<string> PatchList
		{
			get { return _patchList; }
			set { _patchList = value; }
		}

		public string PatchFolder
		{
			get { return _patchFolder; }
			set { _patchFolder = value; }
		}

		public PatchForm()
		{
			InitializeComponent();
		}

		private void PatchForm_Load(object sender, EventArgs e)
		{
			_patchFiles = new List<string>();

			if( _patchList == null || _patchList.Count == 0 )
			{
				MessageBox.Show("Unable to find any patches to download, Please try again later", "No Patches Found");
				OnPatchingComplete(null, new OperationCompleteArgs());
			}

			_updateInfoThread = new Thread(GetUpdateInfo);
			_updateInfoThread.Start();

			_patchThread = new Thread(GetNextPatch);
			_patchThread.Start();
		}

		private void GetUpdateInfo()
		{
			try
			{
				WebClient client = new WebClient();
				byte[] data = client.DownloadData(_patchUpdateURL);

				string update = Encoding.ASCII.GetString(data);

				string[] updateInfo = update.Split(new string[] { "\n" }, StringSplitOptions.None);

				if( this.Visible)
					Invoke((MethodInvoker)delegate { updateBox.Lines = updateInfo; });
			}
			catch
			{
				if (this.Visible)
					Invoke((MethodInvoker)delegate { updateBox.Text = "Failed to retrieve update information"; });
			}

			_updateInfoThread.Join();
		}

		private void GetNextPatch()
		{
			if( _index == _patchList.Count )
				ExtractFiles();
			else
			{
				string[] patchAndCrc = _patchList[_index].Split('|');

				if( patchAndCrc.Length < 2 )
				{
					MessageBox.Show("Patch listing on line " + ( (int)( _index + 1 ) ).ToString() + " is invalid, Aborting...");
					_index++;
					GetNextPatch();
				}

				string patch = patchAndCrc[0];
				string scrc = patchAndCrc[1];

				string fileName = Path.GetFileName(patch);
				uint crc;

				if( !uint.TryParse(scrc, out crc) )
				{
					MessageBox.Show("Patch CRC on line " + ( (int)( _index + 1 ) ).ToString() + " is invalid, Aborting...");
					OnPatchingComplete(null, new OperationCompleteArgs());
				}

				if( File.Exists(Path.Combine(_patchFolder, fileName)) )
				{
					CRC32 crc32 = new CRC32();
					FileStream fs = new FileStream(Path.Combine(_patchFolder, fileName), FileMode.Open);
					uint fileCRC = crc32.GetCrc32(fs);
					fs.Close();


					if( fileCRC != crc )
					{
						_needsPatching = true;
						DownloadFile(patch, Path.Combine(_patchFolder, fileName));
					}
					else
					{
						_index++;
						GetNextPatch();
					}
				}
				else
				{
					_needsPatching = true;
					DownloadFile(patch, Path.Combine(_patchFolder, fileName));		
				}
			}
		}

		private void DownloadFile(string patch, string downloadTo)
		{
			WebClient client = new WebClient();
			client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnPercentChanged);
			client.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadComplete);
			client.DownloadFileAsync(new Uri(patch), downloadTo, downloadTo);
		}

		private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
		{
			_index++;
			_patchFiles.Add((string)e.UserState);

			GetNextPatch();
		}

		private void OnPercentChanged(object sender, DownloadProgressChangedEventArgs e)
		{
            if (e.BytesReceived != 0 && this.Visible)
				Invoke((MethodInvoker)delegate { progressBar.Value = e.ProgressPercentage; });

			OnStatusChange(null, new StatusChangeEventArgs(String.Format("Downloading...{0:0,0}/{1:0,0} bytes", e.BytesReceived, e.TotalBytesToReceive)));
		}

				
		private void ExtractFiles()
		{			
			if (_needsPatching)
			{
				DirectoryInfo patchFolder = new DirectoryInfo(_patchFolder);
				FileInfo[] zipFiles = patchFolder.GetFiles("*.zip");
				FileInfo[] rarFiles = patchFolder.GetFiles("*.rar");

				for (int i = 0; i < zipFiles.Length; i++)
				{
					ZipInputStream s = null;
					FileStream streamWriter = null;
					bool fail = false;
					try
					{
						OnStatusChange(null, new StatusChangeEventArgs("Extracting " + zipFiles[i].Name + "..."));

						using (s = new ZipInputStream(File.OpenRead(zipFiles[i].FullName)))
						{
							ZipEntry entry;
							while ((entry = s.GetNextEntry()) != null)
							{
								string fileName = Path.GetFileName(entry.Name);

								if (fileName != String.Empty)
								{
									if (!File.Exists(Path.Combine(_patchFolder, fileName)) || (entry.Size != new FileInfo(Path.Combine(_patchFolder, fileName)).Length))
										using (streamWriter = File.Create(Path.Combine(_patchFolder, entry.Name)))
										{
											int size = 2048;
											byte[] data = new byte[2048];
											while (true)
											{
												size = s.Read(data, 0, data.Length);
												if (size > 0)
												{
													streamWriter.Write(data, 0, size);
												}
												else
												{
													break;
												}
											}
										}
								}
							}
						}
					}
					catch (Exception e)
					{
						MessageBox.Show("An error occured while trying to extract " + zipFiles[i].Name + ". This patch will not be applied and you should inform the server admin about this issue", "Extraction Error");
						fail = true;
					}
					finally
					{
						if (s != null)
							s.Close();

						if (streamWriter != null)
							streamWriter.Close();

						if (fail)
							File.Delete(zipFiles[i].FullName);
					}
				}

				for (int i = 0; i < rarFiles.Length; i++)
				{

					Unrar rar = null;
					bool fail = false;
					try
					{
						OnStatusChange(null, new StatusChangeEventArgs("Extracting " + rarFiles[i].Name + "..."));

						rar = new Unrar(rarFiles[i].FullName);
						rar.DestinationPath = _patchFolder;
						rar.ExtractionProgress += new ExtractionProgressHandler(rar_ExtractionProgress);
						rar.Open();

						while (rar.ReadHeader())
						{
							if (!File.Exists(Path.Combine(_patchFolder, rar.CurrentFile.FileName)) || new FileInfo(Path.Combine(_patchFolder, rar.CurrentFile.FileName)).Length != rar.CurrentFile.UnpackedSize)
								rar.Extract();
						}

						rar.Close();
					}
					catch (Exception e)
					{
						MessageBox.Show("While trying to extract " + rarFiles[i].Name + " the following Exception was thrown.\n\n" + e.Message + "\n\nThis patch will not be applied and you should inform the server admin about this issue", "Extraction Error");
						fail = true;
					}
					finally
					{
						if (rar != null)
							rar.Close();

						if (fail)
							File.Delete(rarFiles[i].FullName);
					}
				}

				ApplyPatches(patchFolder);
			}
			else if (!CheckFileHash())
			{
				OnStatusChange(null, new StatusChangeEventArgs("Hash check failed, re-applying patches... "));

				_needsPatching = true;
				ExtractFiles();
			}
			else
				Invoke((MethodInvoker)delegate { Close(); });
		}

		private bool CheckFileHash()
		{

			OnStatusChange(null, new StatusChangeEventArgs("Checking file hash... "));
			string hashFile = Path.Combine(_patchFolder, "hash.chk");

			if (!File.Exists(hashFile))
				return false;

			BinaryReader reader = new BinaryReader(File.OpenRead(hashFile));
			long hash = reader.ReadInt64();
			reader.Close();

			long hashCheck = 0;
			DirectoryInfo dir = new DirectoryInfo(_patchFolder);
			FileInfo[] muls = dir.GetFiles("*.mul");
			FileInfo[] idxs = dir.GetFiles("*.idx");

			for (int i = 0; i < muls.Length; i++)
				hashCheck += muls[i].LastWriteTime.GetHashCode();

			for (int i = 0; i < idxs.Length; i++)
				hashCheck += idxs[i].LastWriteTime.GetHashCode();

			return hash == hashCheck;
		}

		private void rar_ExtractionProgress(object sender, ExtractionProgressEventArgs e)
		{
			OnPatchProgressChange(null, new ProgressChangeEventArgs((int)e.PercentComplete, 0, 0));
		}

		private void ApplyPatches(DirectoryInfo patchFolder)
		{
			OnStatusChange(null, new StatusChangeEventArgs("Gathering Patch data..."));

			FileInfo[] muoFiles = patchFolder.GetFiles("*.muo");
			FileInfo[] uopFiles = patchFolder.GetFiles("*.uop");
			FileInfo[] mulFiles = patchFolder.GetFiles("verdata.mul");

			_allPatchFiles = new List<FileInfo>();
			_allPatchFiles.AddRange(muoFiles);
			_allPatchFiles.AddRange(uopFiles);

			for( int i = 0; i < mulFiles.Length; i++ )
					_allPatchFiles.Add(mulFiles[i]);

			if( !CrcCheck(_allPatchFiles) )
			{
				List<Patch> patches = new List<Patch>();
				for( int i = 0; i < _allPatchFiles.Count; i++ )
				{
					PatchReader reader = new PatchReader(File.OpenRead(_allPatchFiles[i].FullName), PatchReader.ExtensionToPatchFileType(_allPatchFiles[i].FullName));
					patches.AddRange(reader.ReadPatches());
					reader.Close();
				}

				_patcher = new Patcher(patches.ToArray(), patchFolder.FullName, _oldPatchingStyle);//Switch argument 2 to false for new patching system.
				_patcher.ProgressChange += new ProgressChangeHandler(OnPatchProgressChange);
				_patcher.PatchingComplete += new OperationCompleteHandler(OnPatchingComplete);
				_patcher.StatusChange += new StatusChangeHandler(OnStatusChange);
				_patcher.WritePatches();
			}
			else
				OnPatchingComplete(null, new OperationCompleteArgs());
		}

		private bool CrcCheck(List<FileInfo> _allPatchFiles)
		{
			CRC32 crc32 = new CRC32();

			ulong hash = 0;
			for( int i = 0; i < _allPatchFiles.Count; i++ )
			{
				FileStream fs = new FileStream(_allPatchFiles[i].FullName, FileMode.Open);
				hash += (ulong)crc32.GetCrc32(fs);
				fs.Close();				
			}

			_patchFile = Path.Combine(_patchFolder, hash.ToString() + ".patch");
			
			return File.Exists(_patchFile);
		}

        private void OnStatusChange(object sender, StatusChangeEventArgs args)
        {
			if (this.Visible)
				statusLbl.Invoke((MethodInvoker)delegate { statusLbl.Text = args.Status; });
        }
        
		private void OnPatchingComplete( object sender, OperationCompleteArgs args )
		{
			if (!_oldPatchingStyle && _needsPatching)
			{
				DirectoryInfo dir = new DirectoryInfo(_patchFolder);
				FileInfo[] patches = dir.GetFiles("*.patch");

				for (int i = 0; i < patches.Length; i++)
					if( patches[i].FullName != _patcher.CombinedPatchFile)
						File.Delete(patches[i].FullName);

				string tempFile = _patcher.CombinedPatchFile;
				CRC32 crc32 = new CRC32();

				ulong hash = 0;
				for( int i = 0; i < _allPatchFiles.Count; i++ )
					hash += (ulong)crc32.GetCrc32(File.Open(_allPatchFiles[i].FullName, FileMode.Open));

				_patchFile = Path.Combine(_patchFolder, hash.ToString() + ".patch");
				File.Move(tempFile, _patchFile);
			}
			else
			{
				DirectoryInfo dir = new DirectoryInfo(_patchFolder);
				FileInfo[] patch = dir.GetFiles("*.patch");

				if (patch.Length >= 1)
					_patchFile = patch[0].FullName;
			}

			if (this.Visible)
				Invoke((MethodInvoker)delegate
				{
					cancelBtn.Text = "Close";
					_closeTimer = new System.Timers.Timer(1000);
					_closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(closeTimer_Elapsed);
					_closeTimer.Start();
				});
		}

		private void closeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_closeTime--;

			if( _closeTime < 1 )
			{
				_closeTimer.Stop();

				if (this.Visible)
					Invoke((MethodInvoker)delegate { Close(); });
			}
			else
                OnStatusChange(null, new  StatusChangeEventArgs("Patching complete, click Close to start UO. (Closing in " + _closeTime.ToString() + " seconds)"));
		}

		private void OnPatchProgressChange(object sender, ProgressChangeEventArgs args)
		{
			if (this.Visible)
				Invoke((MethodInvoker)delegate { progressBar.Value = args.Percent; });
		}

		private void PatchForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if( _closeTimer != null )
				_closeTimer.Stop();
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
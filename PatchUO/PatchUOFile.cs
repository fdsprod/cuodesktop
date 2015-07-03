using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class PatchUOFile
	{
		private List<Patch> _patches;
		private List<PatchGroup> _groups;
		private string _path;

		public List<Patch> Patches { get { return _patches; } set { _patches = value; } }
		public List<PatchGroup> Groups { get { return _groups; } set { _groups = value; } }

		public PatchUOFile()
		{
			_path = string.Empty;
			_patches = new List<Patch>();
			MakeDefaultGroup();
		}

		public PatchUOFile( string path, bool load )
		{
			_path = path;
			_patches = new List<Patch>();
			MakeDefaultGroup();

			if( ( load && File.Exists(path) ) && (!LoadFromFile(path) ) )
				throw new FileLoadException(path + " is not a valid PatchUO file.");
		}

		private void MakeDefaultGroup()
		{
			_groups = new List<PatchGroup>();
			_groups.Add(new PatchGroup("All", _patches));
		}

		public bool LoadFromFile(string path)
		{
			PatchUOFileReader reader = new PatchUOFileReader(File.OpenRead(path));

			if( !reader.IsValidFile )
			{
				reader.Close();
				return false;
			}
			else
			{
				int ver = reader.ReadInt32();
				switch( ver )
				{
					case 0:
					{
						_patches = reader.ReadPatches();
						break;
					}
				}
				reader.Close();
				return true;
			}
		}

		public void SaveFile()
		{
			if( _path == string.Empty )
			{
				SaveFileDialog dialog = new SaveFileDialog();
				if( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
					_path = dialog.FileName;
				else
					return;
			}

			PatchUOFileWriter writer = new PatchUOFileWriter(File.Create(_path));
			writer.WritePatches(_patches);
			writer.Close();
		}

		public void SaveFile(string file)
		{
			_path = file;
			PatchUOFileWriter writer = new PatchUOFileWriter(File.Create(_path));
			writer.Write((int)0);
			writer.WritePatches(_patches);
			writer.Close();
		}

		internal void Import(string path)
		{
			PatchReader reader = new PatchReader(File.OpenRead(path));
			string ext = Path.GetExtension(path);

			if( ext == ".uop" )
			{
				if( reader.ReadInt32() != PatchReader.UOPHeader )
				{
					MessageBox.Show("Invalid UOP file, Aborting", "Invalid File");
					return;
				}

				int count = reader.ReadInt32();
				reader.ReadInt32();//UNKNOWN FIELD

				for( int i = 0; i < count; i++ )
				{
					Patch p = reader.ReadUOPPatch();

					if( IsValid(p.FileID) && !ContainsPatch(p) )
						_patches.Add(p);
				}
			}
			else if ( ext == ".muo" )
			{
				if( reader.ReadInt32() != PatchReader.MUOHeader )
				{
					MessageBox.Show("Invalid MUO file, Aborting", "Invalid File");
					return;
				}

				string[] data = reader.ReadMUOHeaderData();
				int count = reader.ReadInt32();

				for( int i = 0; i < count; i++ )
				{
					Patch p = reader.ReadMUOPatch();

					if( IsValid(p.FileID) && !ContainsPatch(p) )
						_patches.Add(p);
				}
			}
			else if( ext == ".puo" )
			{
				if( reader.ReadInt32() != PatchReader.UOPHeader )
				{
					MessageBox.Show("Invalid UOP file, Aborting", "Invalid File");
					return;
				}
				reader.Close();

				PatchUOFileReader puoReader = new PatchUOFileReader(File.OpenRead(path));

				List<Patch> patches = puoReader.ReadPatches();
				for( int i = 0; i < patches.Count; i++ )
				{
					Patch p = patches[i];
					
					if( IsValid(p.FileID) && !ContainsPatch(p) )
						_patches.Add(p);
				}				
				
				puoReader.Close();
			}
			else if( ext == ".mul" )
			{
				try
				{
					int count = reader.ReadInt32();
					
					for( int i = 0; i < count; i++ )
					{
						Patch p = reader.ReadVerdataPatch();

						if( IsValid(p.FileID) && !ContainsPatch(p) )
							_patches.Add(p);
					}
				}
				catch
				{
					MessageBox.Show("Invalid or corrupt verdata file.", "Error");
				}
			}
			else
				MessageBox.Show("That file extension is invalid.", "Not Supported");

			if( reader != null )
				reader.Close();
		}

		private bool IsValid(int id)
		{
			return ( id > 0 && id < 0x00000013 );
		}

		private bool ContainsPatch(Patch p)
		{
			bool found = false;

			for( int i = 0; i < _patches.Count; i++ )
			{
				if( p.FileID.CompareTo(_patches[i].FileID) != 0 && p.Extra.CompareTo(_patches[i].Extra) != 0 &&
					p.BlockID.CompareTo(_patches[i].BlockID) != 0 && p.Length.CompareTo(_patches[i].Length) != 0 )
				{
					bool hadBreak = false;
					for( int a = 0; a < p.Data.Length; a++ )
						if( p.Data[a] != _patches[i].Data[a] )
						{
							hadBreak = true;
							break;
						}

					if( !hadBreak )
					{
						found = true;
						break;
					}
				}
			}

			return found;
		}

		internal void Export(string path, List<PatchGroup> groups)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}

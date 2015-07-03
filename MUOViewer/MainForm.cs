using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MUOViewer
{
	public enum FileID
	{
		Map0_mul = 0x00000000,
		StaIdx0_mul = 0x00000001,
		Statics0_mul = 0x00000002,
		ArtIdx_mul = 0x00000003,
		Art_mul = 0x00000004,
		Anim_idx = 0x00000005,
		Anim_mul = 0x00000006,
		SoundIdx_mul = 0x00000007,
		Sound_mul = 0x00000008,
		TexIdx_mul = 0x00000009,
		TexMaps_mul = 0x0000000A,
		GumpIdx_mul = 0x0000000B,
		GumpArt_mul = 0x0000000C,
		Multi_idx = 0x0000000D,
		Multi_mul = 0x0000000E,
		Skills_idx = 0x0000000F,
		Skills_mul = 0x00000010,
		TileData_mul = 0x00000011,
		AnimData_mul = 0x00000012,
		Hues_mul = 0x00000013,
	}

	public enum ExtendedFileID : int//Fucking UOG had to go and fuck up the MUO
	{
		Map0_mul = 0x00000040,
		StaIdx0_mul = 0x00000041,
		Statics0_mul = 0x00000042,
		ArtIdx_mul = 0x00000043,
		Art_mul = 0x00000044,
		Anim_idx = 0x00000045,
		Anim_mul = 0x00000046,
		SoundIdx_mul = 0x00000047,
		Sound_mul = 0x00000048,
		TexIdx_mul = 0x00000049,
		TexMaps_mul = 0x0000004A,
		GumpIdx_mul = 0x0000004B,
		GumpArt_mul = 0x0000004C,
		Multi_idx = 0x0000004D,
		Multi_mul = 0x0000004E,
	}

	public partial class MainForm : Form
	{
		private PatchFile _patchFile;
		private bool _updateScreen = true;
		private Animation _anim;

		public PatchFile patchFile { get { return _patchFile; } set { _patchFile = value;} }

		private void UpdateForm()
		{
			upDown.Maximum = _patchFile.patchCount - 1;
			nameLbl.Text = "Name: " + _patchFile.name;
			authLbl.Text = "Author: " + _patchFile.author;
			descLbl.Text = "Description: " + _patchFile.desc;
			patchesLbl.Text = "Patches: " + _patchFile.patchCount.ToString("0,0");
			Patch p = _patchFile.patches[(int)upDown.Value];

			UpdateImage(p);		

			if( p.data != null )
				textBox.Lines = FormatString(p.data, p.data.Length);

			fileidLbl.Text = "FileID: " + p.fileID.ToString("X2");
			blockidLbl.Text = "BlockID: " + p.blockID.ToString("X2");
			extraLbl.Text = "Extra: " + p.extra.ToString("X2");
			lengthLbl.Text = "Length: " + p.length.ToString();
			Invalidate();	
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Select a file to open";
			openFileDialog.CheckFileExists = false;
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				BinaryReader reader = new BinaryReader(File.OpenRead(openFileDialog.FileName));

				_patchFile = new PatchFile();
				int read = reader.ReadInt32();
				if( read != 0x504f554d )
				{
					MessageBox.Show("Invalid MUO file");
					return;
				}

				reader.ReadInt32();

				string temp = "";
				byte b = 0;
				while( ( b = reader.ReadByte() ) != 0 )
					temp = temp + ( (char)b );

				_patchFile.name = temp;
				temp = "";
				while( ( b = reader.ReadByte() ) != 0 )
				{
					temp = temp + ( (char)b );
				}
				_patchFile.desc = temp;
				temp = "";
				while( ( b = reader.ReadByte() ) != 0 )
				{
					temp = temp + ( (char)b );
				}
				_patchFile.author = temp;
				int count = reader.ReadInt32();
				_patchFile.patchCount = count;
				_patchFile.patches = new Patch[count];
				for( int i = 0; i < count; i++ )
				{
					Patch patch = new Patch();
					patch.fileID = reader.ReadInt32();
					patch.blockID = reader.ReadInt32();
					patch.extra = reader.ReadInt32();
					patch.length = reader.ReadInt32();
					if (patch.length > 0)
						patch.data = reader.ReadBytes(patch.length);
					else
					{
						patch.length = 0;
						patch.data = new byte[0];
					}
					_patchFile.patches[i] = patch;
				}

				if( reader.BaseStream.Length != reader.BaseStream.Position )
					MessageBox.Show("More Data!");

				UpdateForm(); 
			}
		}

		private string[] FormatString(byte[] buffer, int length)
		{
			List<string> output = new List<string>();

			output.Add("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
			output.Add("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

			int byteIndex = 0;
			int whole = length >> 4;
			int rem = length & 0xF;

			for( int i = 0; i < whole; ++i, byteIndex += 16 )
			{
				StringBuilder bytes = new StringBuilder(49);
				StringBuilder chars = new StringBuilder(16);

				for( int j = 0; j < 16; ++j )
				{
					int c = buffer[( i * 16 ) + j];
					bytes.Append(c.ToString("X2"));

					if( j != 7 )
						bytes.Append(' ');
					else
						bytes.Append("  ");

					if( c >= 0x20 && c < 0x80 )
						chars.Append((char)c);
					else
						chars.Append('.');
				}

				output.Add(byteIndex.ToString("X4") + "   " + bytes.ToString() + "  " + chars.ToString());
			}

			if( rem != 0 )
			{
				StringBuilder bytes = new StringBuilder(49);
				StringBuilder chars = new StringBuilder(rem);

				for( int j = 0; j < 16; ++j )
				{
					if( j < rem )
					{
						int c = buffer[j];
						bytes.Append(c.ToString("X2"));

						if( j != 7 )
							bytes.Append(' ');
						else
							bytes.Append("  ");

						if( c >= 0x20 && c < 0x80 )
							chars.Append((char)c);
						else
							chars.Append('.');
					}
					else
						bytes.Append("   ");
				}

				output.Add(byteIndex.ToString("X4") + "   " + bytes.ToString() + "  " + chars.ToString());
			}

			string[] realOutput = new string[output.Count];

			for( int i = 0; i < realOutput.Length; i++ )
				realOutput[i] = output[i].ToString();		

			return realOutput;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}		
		
		private byte[] GetData(byte[] p, out int length)
		{
			int sec = (int)secUpDown.Value;
			if( p.Length / 512 < secUpDown.Value && p.Length % 512 == 0 )
				sec = p.Length / 512;

			if (sec < 0)
			{
				secUpDown.Maximum = 0;
				_updateScreen = false;
				secUpDown.Value = 0;
				_updateScreen = true;
				sec = 0;
			}
			else
			{
				secUpDown.Maximum = p.Length % 512 == 0 ? p.Length / 512 - 1 : p.Length / 512;
				_updateScreen = false;
				secUpDown.Value = sec;
				_updateScreen = true;
			}

			length = Math.Min(512, p.Length - ( sec * 512 ));

			byte[] data = new byte[512];

			for( int i = 0; i < length; i++ )
				data[i] = p[i + sec * 512];

			return data;
		}

		private void upDown_ValueChanged(object sender, EventArgs e)
		{
			Patch p = _patchFile.patches[(int)upDown.Value];
			int length;
			UpdateImage(p);		

			textBox.Lines = FormatString(GetData(p.data, out length), length);
			fileidLbl.Text = "FileID: " + p.fileID.ToString("X2");
			blockidLbl.Text = "BlockID: " + p.blockID.ToString("X2");
			extraLbl.Text = "Extra: " + p.extra.ToString("X2");
			lengthLbl.Text = "Length: " + p.length.ToString();
			Invalidate();
		}

		private void UpdateImage(Patch p)
		{
			if( _anim != null )
				_anim.Stop();

			switch( p.fileID )
			{
				case (int)FileID.GumpArt_mul:
				{
					pictureBox1.BackgroundImage = GetGump(p);
					break;
				}
			case (int)FileID.Art_mul:
				{
					if (p.blockID > 0x3FFF)
						pictureBox1.BackgroundImage = LoadStatic(new MemoryStream(p.data));
					else
						pictureBox1.BackgroundImage = LoadLand(new MemoryStream(p.data));

					break;
				}
			case (int)FileID.Anim_mul:
				{
					_anim = new Animation(pictureBox1, p);					
					break;
				}
			case (int)ExtendedFileID.GumpArt_mul:
				{
					pictureBox1.BackgroundImage = GetGump(p);
					break;
				}
			case (int)ExtendedFileID.Art_mul:
				{
					try
					{
						pictureBox1.BackgroundImage = LoadLand(new MemoryStream(p.data));
					}
					catch
					{
						pictureBox1.BackgroundImage = LoadStatic(new MemoryStream(p.data));
					}
					break;
				}
			case (int)ExtendedFileID.Anim_mul:
				{
					_anim = new Animation(pictureBox1, p);
					break;
				}
				default:
				{
					pictureBox1.BackgroundImage = null;
					break;
				}
			}
		}

		private void secUpDown_ValueChanged(object sender, EventArgs e)
		{
			if( !_updateScreen )
				return;

			int length;

			Patch p = _patchFile.patches[(int)upDown.Value];
			textBox.Lines = FormatString(GetData(p.data, out length), length);
		}

		public unsafe static Bitmap GetGump(Patch p)
		{
			int length = p.length;
			int extra = p.extra;			

			int width = ( extra >> 16 ) & 0xFFFF;
			int height = extra & 0xFFFF;

			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
			MemoryStream ms = new MemoryStream(p.data);
			BinaryReader bin = new BinaryReader(ms);

			int[] lookups = new int[height];
			int start = (int)bin.BaseStream.Position;

			for( int i = 0; i < height; ++i )
				lookups[i] = start + ( bin.ReadInt32() * 4 );

			ushort* line = (ushort*)bd.Scan0;
			int delta = bd.Stride >> 1;

			for( int y = 0; y < height; ++y, line += delta )
			{
				bin.BaseStream.Seek(lookups[y], SeekOrigin.Begin);

				ushort* cur = line;
				ushort* end = line + bd.Width;

				while( cur < end )
				{
					ushort color = bin.ReadUInt16();
					ushort* next = cur + bin.ReadUInt16();

					if( color == 0 )
					{
						cur = next;
					}
					else
					{
						color ^= 0x8000;

						while( cur < next )
							*cur++ = color;
					}
				}
			}

			bmp.UnlockBits(bd);
			bin.Close();

			return bmp;
		}

		private static unsafe Bitmap LoadStatic(Stream stream)
		{
			BinaryReader bin = new BinaryReader(stream);

			bin.ReadInt32();
			int width = bin.ReadInt16();
			int height = bin.ReadInt16();

			if( width <= 0 || height <= 0 )
				return null;

			int[] lookups = new int[height];

			int start = (int)bin.BaseStream.Position + ( height * 2 );

			for( int i = 0; i < height; ++i )
				lookups[i] = (int)( start + ( bin.ReadUInt16() * 2 ) );

			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

			ushort* line = (ushort*)bd.Scan0;
			int delta = bd.Stride >> 1;

			for( int y = 0; y < height; ++y, line += delta )
			{
				bin.BaseStream.Seek(lookups[y], SeekOrigin.Begin);

				ushort* cur = line;
				ushort* end;

				int xOffset, xRun;

				while( ( ( xOffset = bin.ReadUInt16() ) + ( xRun = bin.ReadUInt16() ) ) != 0 )
				{
					cur += xOffset;
					end = cur + xRun;

					while( cur < end )
						*cur++ = (ushort)( bin.ReadUInt16() ^ 0x8000 );
				}
			}

			bmp.UnlockBits(bd);

			return bmp;
		}

		private static unsafe Bitmap LoadLand(Stream stream)
		{
			Bitmap bmp = new Bitmap(44, 44, PixelFormat.Format16bppArgb1555);
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 44, 44), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
			BinaryReader bin = new BinaryReader(stream);

			int xOffset = 21;
			int xRun = 2;

			ushort* line = (ushort*)bd.Scan0;
			int delta = bd.Stride >> 1;

			for( int y = 0; y < 22; ++y, --xOffset, xRun += 2, line += delta )
			{
				ushort* cur = line + xOffset;
				ushort* end = cur + xRun;

				while( cur < end )
					*cur++ = (ushort)( bin.ReadUInt16() | 0x8000 );
			}

			xOffset = 0;
			xRun = 44;

			for( int y = 0; y < 22; ++y, ++xOffset, xRun -= 2, line += delta )
			{
				ushort* cur = line + xOffset;
				ushort* end = cur + xRun;

				while( cur < end )
					*cur++ = (ushort)( bin.ReadUInt16() | 0x8000 );
			}

			bmp.UnlockBits(bd);

			return bmp;
		}
	}

	public struct Patch
	{
		public int fileID;
		public int blockID;
		public int extra;
		public int length;
		public byte[] data;
	}

	public struct PatchFile
	{
		public string name;
		public string desc;
		public string author;
		public int patchCount;
		public Patch[] patches;
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class ImageCompiler
	{
		public static unsafe Bitmap LoadStatic(Stream stream)
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

		public static unsafe Bitmap LoadLand(Stream stream)
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

		public unsafe static Bitmap GetGump(Patch p)
		{
			int length = p.Length;
			int extra = p.Extra;

			int width = ( extra >> 16 ) & 0xFFFF;
			int height = extra & 0xFFFF;

			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
			MemoryStream ms = new MemoryStream(p.Data);
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

		internal static Image GetImage(Patch patch)
		{
			switch( (FileID)patch.FileID )
			{
				case FileID.GumpArt_mul:
				{
					return GetGump(patch);
				}
				case FileID.Art_mul:
				{
					try
					{
						return LoadLand(new MemoryStream(patch.Data));
					}
					catch
					{
						return LoadStatic(new MemoryStream(patch.Data));
					}
				}
				case FileID.Anim_mul:
				{
					return Animation.GetFirstFrame(patch);							
				}
				default:
				{
					return null;
				}
			}
		}
	}
}

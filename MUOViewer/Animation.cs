using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Timers;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MUOViewer
{
	public class Animation
	{
		private PictureBox _image;
		private Patch _patch;
		private Frame[] _frames;
		private int _frameIndex;
		private System.Timers.Timer _timer;

		public Animation(PictureBox image, Patch patch)
		{
			_image = image;
			_patch = patch;
			_frames = GetAnimation();
			_timer = new System.Timers.Timer(100);
			_timer.Elapsed += new ElapsedEventHandler(OnTick);
			_timer.Start();
		}

		public void Stop()
		{
			_timer.Stop();
		}

		private void OnTick(object sender, ElapsedEventArgs e)
		{
			if( _frames == null || _frames.Length < 1 )
				return;

			_image.Invoke((MethodInvoker)delegate { _image.BackgroundImage = _frames[_frameIndex].Bitmap; });
			_frameIndex++;

			if( _frameIndex > _frames.Length - 1 )
				_frameIndex = 0;
		}

		private Frame[] GetAnimation()
		{
			if (_patch.length < 1)
				return null;

			MemoryStream ms = new MemoryStream(_patch.data);
			BinaryReader bin = new BinaryReader(ms);

			if (_patch.length < 1)
				return null;

			ushort[] palette = new ushort[0x100];

			for( int i = 0; i < 0x100; ++i )
				palette[i] = (ushort)( bin.ReadUInt16() ^ 0x8000 );

			int start = (int)bin.BaseStream.Position;
			int frameCount = bin.ReadInt32();

			int[] lookups = new int[frameCount];

			for( int i = 0; i < frameCount; ++i )
				lookups[i] = start + bin.ReadInt32();

			Frame[] frames = new Frame[frameCount];

			for( int i = 0; i < frameCount; ++i )
			{
				bin.BaseStream.Seek(lookups[i], SeekOrigin.Begin);
				frames[i] = new Frame(palette, bin, false);
			}

			return frames;
		}
	}

	public class Frame
	{
		private Point m_Center;
		private Bitmap m_Bitmap;

		public Point Center { get { return m_Center; } }
		public Bitmap Bitmap { get { return m_Bitmap; } }

		private const int DoubleXor = ( 0x200 << 22 ) | ( 0x200 << 12 );

		public static readonly Frame Empty = new Frame();
		public static readonly Frame[] EmptyFrames = new Frame[1] { Empty };

		private Frame()
		{
			m_Bitmap = new Bitmap(1, 1);
		}

		public unsafe Frame(ushort[] palette, BinaryReader bin, bool flip)
		{
			int xCenter = bin.ReadInt16();
			int yCenter = bin.ReadInt16();

			int width = bin.ReadUInt16();
			int height = bin.ReadUInt16();

			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

			ushort* line = (ushort*)bd.Scan0;
			int delta = bd.Stride >> 1;

			int header;

			int xBase = xCenter - 0x200;
			int yBase = ( yCenter + height ) - 0x200;

			if( !flip )
			{
				line += xBase;
				line += ( yBase * delta );

				while( ( header = bin.ReadInt32() ) != 0x7FFF7FFF )
				{
					header ^= DoubleXor;

					ushort* cur = line + ( ( ( ( header >> 12 ) & 0x3FF ) * delta ) + ( ( header >> 22 ) & 0x3FF ) );
					ushort* end = cur + ( header & 0xFFF );

					while( cur < end )
						*cur++ = palette[bin.ReadByte()];
				}
			}
			else
			{
				line -= xBase - width + 1;
				line += ( yBase * delta );

				while( ( header = bin.ReadInt32() ) != 0x7FFF7FFF )
				{
					header ^= DoubleXor;

					ushort* cur = line + ( ( ( ( header >> 12 ) & 0x3FF ) * delta ) - ( ( header >> 22 ) & 0x3FF ) );
					ushort* end = cur - ( header & 0xFFF );

					while( cur > end )
						*cur-- = palette[bin.ReadByte()];
				}

				xCenter = width - xCenter;
			}

			bmp.UnlockBits(bd);

			m_Center = new Point(xCenter, yCenter);
			m_Bitmap = bmp;
		}
	}
}

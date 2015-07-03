using System;
using System.Collections.Generic;
using System.IO;

namespace CUODesktop.PatchLib
{
	public class PatchWriter : BinaryWriter
	{
		public static void CreateMUO(string path, Patch[] patches, ProgressChangeHandler progressDelegate)
		{
			PatchWriter writer = new PatchWriter(File.Create(path), PatchFileType.MUO);

			writer.WriteMUOHeader();
			writer.WriteMUOMetaData(new string[] { "MUO", "Created with PatchLib", "Jeff Boulanger" });
			writer.Write((int)patches.Length);

			for( int i = 0; i < patches.Length; i++ )
			{
				writer.WriteMUOPatch(patches[i]);

				if( i != 0 && progressDelegate != null )
					progressDelegate(writer, new ProgressChangeEventArgs(((100 * i) / patches.Length), i, patches.Length));			
			}

			writer.Close();
		}

		public static void CreateMUO(string path, Patch[] patches)
		{
			CreateMUO(path, patches, null);	
		}

		public static void CreateUOP(string path, Patch[] patches, ProgressChangeHandler progressDelegate)
		{
			PatchWriter writer = new PatchWriter(File.Create(path), PatchFileType.UOP);

			writer.WriteUOPHeader();
			writer.Write(patches.Length);
			writer.Write((int)0);//Unknown

			for( int i = 0; i < patches.Length; i++ )
			{
				writer.WriteUOPPatch(patches[i]);
				
				if( i != 0 && progressDelegate != null )
					progressDelegate(writer, new ProgressChangeEventArgs(((100 * i) / patches.Length), i, patches.Length));
			}

			writer.Close();
		}

		public static void CreateUOP(string path, Patch[] patches)
		{
			CreateUOP(path, patches, null );
		}

		private PatchFileType _patchFileType;

		public PatchWriter(Stream stream, PatchFileType patchFileType)
			: base(stream)
		{
			if( patchFileType == PatchFileType.Verdata )
				throw new Exception("This file format is not supported");

			_patchFileType = patchFileType;
		}

		public void WriteMUOMetaData(string[] metaData)
		{
			for( int i = 0; i < metaData.Length; i++ )
			{
				char[] chars = metaData[i].ToCharArray();

				for( int c = 0; c < chars.Length; c++ )
					Write(chars[c]);

				Write((byte)0);
			}
		}

		public void WriteMUOHeader()
		{
			Write(PatchReader.MUOHeader);
		}

		public void WriteUOPHeader()
		{
			Write(PatchReader.UOPHeader);
		}

		public void WritePatch(Patch patch)
		{
			switch( _patchFileType )
			{
				case PatchFileType.MUO:
				{
					WriteMUOPatch(patch);
					break;
				}
				case PatchFileType.UOP:
				{
					WriteUOPPatch(patch);
					break;
				}
			}
		}

		public void WriteMUOPatch(Patch patch)
		{
			Write((int)patch.FileID);
			Write((int)patch.BlockID);
			Write((int)patch.Extra);
			Write((int)patch.Length);
			Write(patch.Data);
		}

		public void WriteUOPPatch(Patch patch)
		{
			Write((byte)patch.FileID);
			Write((int)patch.BlockID);
			Write((int)patch.Length);
			Write((int)patch.Extra);
			Write(patch.Data);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class PatchUOFileReader : PatchReader
	{
		public const int PATCHUO_HEADER = 0x50554F23;
		private int _fileHeader;

		public bool IsValidFile { get { return PATCHUO_HEADER == _fileHeader; } }

		public PatchUOFileReader(Stream stream)
			: base(stream)
		{
			_fileHeader = ReadInt32();
		}

		public List<Patch> ReadPatches()
		{
			int count = ReadInt32();

			List<Patch> patches = new List<Patch>();

			for( int i = 0; i < count; i++ )
				patches.Add(ReadMUOPatch());

			return patches;
		}
	}
}

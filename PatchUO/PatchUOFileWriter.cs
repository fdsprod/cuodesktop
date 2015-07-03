using System;
using System.Collections.Generic;
using System.IO;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class PatchUOFileWriter : BinaryWriter
	{
		public PatchUOFileWriter(Stream stream)
			: base(stream)
		{
			base.Write(PatchUOFileReader.PATCHUO_HEADER);
		}

		public void WritePatch(Patch patch)
		{
			Write(patch.FileID);
			Write(patch.BlockID);
			Write(patch.Extra);
			Write(patch.Length);
			Write(patch.Data);
		}

		public void WritePatches(List<Patch> patches)
		{
			Write(patches.Count);

			for( int i = 0; i < patches.Count; i++ )
				WritePatch(patches[i]);
		}
	}
}

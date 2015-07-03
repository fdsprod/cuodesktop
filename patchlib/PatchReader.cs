using System;
using System.Collections.Generic;
using System.IO;

namespace CUODesktop.PatchLib
{
    public enum PatchFileType
    {
        MUO,
        UOP,
        Verdata,
    }

	public class PatchReader : BinaryReader
	{
        public static PatchFileType ExtensionToPatchFileType(string ext)
        {
            ext = Path.GetExtension(ext);

            switch (ext)
            {
                case ".muo": return PatchFileType.MUO;
                case ".uop": return PatchFileType.UOP;
                case ".mul": return PatchFileType.Verdata;
                default: throw new Exception("Invalid file extension");
            }
        }

		public const int UOPHeader = 0x04504F55;
		public const int MUOHeader = 0x504f554d;

        private PatchFileType _patchFileType;

		public PatchReader(Stream stream, PatchFileType patchFileType) : base(stream)
		{
            _patchFileType = patchFileType;
        }

		public Patch ReadMUOPatch()
		{
			Patch p = new Patch();
			p.FileID = ReadInt32();
			p.BlockID = ReadInt32();
			p.Extra = ReadInt32();
			p.Length = ReadInt32();
			if (p.Length >= 0)
				p.Data = ReadBytes(p.Length);
			else
				p.Data = new byte[0];
			return p;
		}

		public Patch ReadUOPPatch()
		{
			Patch p = new Patch();
			p.FileID = (int)ReadByte();
			p.BlockID = ReadInt32();
			p.Length = ReadInt32();
			p.Extra = ReadInt32();
			p.Data = ReadBytes(p.Length);
			return p;
		}

		public Patch ReadVerdataPatch()
		{
			Patch p = new Patch();
			p.FileID = ReadInt32();
			p.BlockID = ReadInt32();

			int position = ReadInt32();

			p.Length = ReadInt32();
			p.Extra = ReadInt32();

			int streamPos = (int)BaseStream.Position;
			BaseStream.Seek(position, SeekOrigin.Begin);

			p.Data = ReadBytes(p.Length);

			BaseStream.Seek(streamPos, SeekOrigin.Begin);
			return p;
		}

		public string[] ReadMUOHeaderData()
		{
			ReadInt32();
			string[] headerInfo = new string[3];

			for( int i = 0; i < 3; i++ )
			{
				byte bChar;
				while( ( bChar = ReadByte() ) != 0 )
					headerInfo[i] += ( (char)bChar );
			}

			return headerInfo;
		}

		public List<Patch> ReadPatches()
		{
            List<Patch> patches = new List<Patch>();

            switch (_patchFileType)
            {
                case PatchFileType.MUO:
                    {
                        int header = ReadInt32();

                        if (header != MUOHeader)
                            throw new Exception("The current file is not a valid MUO file");

						ReadMUOHeaderData();
                        int count = ReadInt32();

                        for (int i = 0; i < count; i++)
                            patches.Add(ReadMUOPatch());

                        break;
                    }
                case PatchFileType.UOP:
                    {
                       int header = ReadInt32();

                        if (header != UOPHeader)
                            throw new Exception("The current file is not a valid UOP file");

                        int count = ReadInt32();
                        int unknown = ReadInt32();

                        for (int i = 0; i < count; i++)
                            patches.Add(ReadUOPPatch());

                        break;
                    }
                case PatchFileType.Verdata:
                    {
                        int count = ReadInt32();

                        for (int i = 0; i < count; i++)
                            patches.Add(ReadVerdataPatch());

                        break;
                    }
            }

			return patches;
		}
	}
}

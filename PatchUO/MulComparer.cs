using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class MulComparer
	{
		private List<Patch> _patches;

		private string _pathOneMul;
		private string _pathTwoMul;
		private string _pathOneIdx;
		private string _pathTwoIdx;
		private bool _usesIdx;
		private FileID _fileId;
		private Thread _thread;

		public event ProgressChangeHandler ProgressChange;
		public event OperationCompleteHandler OperationComplete;
		public event StatusChangeHandler StatusChanged;

		public MulComparer(string pathOneMul, string pathTwoMul, FileID id)
			: this(pathOneMul, pathTwoMul, "", "", false, id) { }

		public MulComparer(string pathOneMul, string pathTwoMul, string pathOneIdx, string pathTwoIdx, bool usesIdx, FileID id)
		{
			if (!File.Exists(pathOneMul) && !Directory.Exists(pathOneMul))
				throw new ArgumentException(pathOneMul + " does not exist");

			if (!File.Exists(pathTwoMul) && !Directory.Exists(pathTwoMul))
				throw new ArgumentException(pathTwoMul + " does not exist");

			if (usesIdx)
			{
				if (!File.Exists(pathOneIdx) && !Directory.Exists(pathOneIdx))
					throw new ArgumentException(pathOneIdx + " does not exist");

				if (!File.Exists(pathTwoIdx) && !Directory.Exists(pathTwoIdx))
					throw new ArgumentException(pathTwoIdx + " does not exist");
			}

			if (!File.Exists(pathOneMul))
				pathOneMul += Enum.GetName(typeof(FileID), id).Replace('_', '.');

			if (!File.Exists(pathOneIdx))
				pathTwoMul += Enum.GetName(typeof(FileID), id).Replace('_', '.');

			if (!File.Exists(pathOneIdx))
				pathOneIdx += Enum.GetName(typeof(FileID), id - 1).Replace('_', '.');

			if (!File.Exists(pathTwoIdx))
				pathTwoIdx += Enum.GetName(typeof(FileID), id - 1).Replace('_', '.');

			_pathOneMul = pathOneMul;
			_pathTwoMul = pathTwoMul;
			_pathOneIdx = pathOneIdx;
			_pathTwoIdx = pathTwoIdx;
			_usesIdx = usesIdx;
			_fileId = id;
			_patches = new List<Patch>();

			_thread = new Thread(InternalCompare);
		}

		public void Compare()
		{
			if (_thread.ThreadState != ThreadState.Running)
				_thread.Start();
		}

        private void InternalCompare()
		{
			if (_usesIdx)
			{
					byte[] buffOne = new byte[0xFFFF];
					byte[] buffTwo = new byte[0xFFFF];

					FileInfo infoOne = new FileInfo(_pathOneIdx);
					FileInfo infoTwo = new FileInfo(_pathTwoIdx);

					long lengthOne = infoOne.Length;
                    long lengthTwo = infoTwo.Length;

					int maxIndexes = (int)Math.Min(lengthOne / 12, lengthTwo / 12);

					FileIndex indexOne = new FileIndex(Path.GetDirectoryName(_pathOneMul), Path.GetFileName(_pathOneIdx), Path.GetFileName(_pathOneMul), maxIndexes);
					FileIndex indexTwo = new FileIndex(Path.GetDirectoryName(_pathTwoMul), Path.GetFileName(_pathTwoIdx), Path.GetFileName(_pathTwoMul), maxIndexes);

					for (int i = 0; i < maxIndexes; i++)
					{
						Stream streamOne = indexOne.Seek(i);
						Stream streamTwo = indexTwo.Seek(i);
						BinaryReader readerOne = null;
						BinaryReader readerTwo = null;

						if (streamOne == null && streamTwo == null)
							continue;

						if (streamOne != null)
							readerOne = new BinaryReader(streamOne);

						if (streamTwo != null)
							readerTwo = new BinaryReader(streamTwo);

						if (readerOne != null && readerTwo == null)
							CreatePatchFromIdxMul(readerOne, indexOne.Index[i]);
						else if (readerOne == null && readerTwo != null)
							CreatePatchFromIdxMul(readerTwo, indexTwo.Index[i]);
						else
						{
							Entry3D entryOne = indexOne.Index[i];
							Entry3D entryTwo = indexTwo.Index[i];

							if (entryOne.length != entryTwo.length)
							{
								CreatePatchFromIdxMul(readerOne, entryOne);
								CreatePatchFromIdxMul(readerTwo, entryTwo);
							}
							else
							{
								while (readerOne.BaseStream.Position != readerOne.BaseStream.Length)
								{
									int toRead = Math.Min(buffOne.Length, (int)(readerOne.BaseStream.Length - readerOne.BaseStream.Position));

									readerOne.Read(buffOne, 0, toRead);
									readerTwo.Read(buffTwo, 0, toRead);

									bool match = true;

									for (int a = 0; a < buffOne.Length; a++)
										if (buffOne[a] != buffTwo[a])
										{
											match = false;
											break;
										}

									if (!match)
									{
										CreatePatchFromIdxMul(readerOne, entryOne);
										CreatePatchFromIdxMul(readerTwo, entryTwo);
									}
								}
							}
						}
					}
				
			}
			else
			{

			}
		}

		private void CreatePatchFromIdxMul(BinaryReader reader, Entry3D entry)
		{
			reader.BaseStream.Seek(entry.lookup, SeekOrigin.Begin);

            Patch p = new Patch();
		}
	}
}

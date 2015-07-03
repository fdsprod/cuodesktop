using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using CUODesktop;
using PatchUO;

namespace CUODesktop.PatchLib
{
	public class Patcher
	{
		private Patch[] _patches;
		private string _patchToFolder;
		private string _UOPath;
		private bool _patchMuls;
		private string _combinedPatchFile;
		private Thread _thread;

		public string CombinedPatchFile { get { return _combinedPatchFile; } }
		public string PatchToFolder { get { return _patchToFolder; } }

		public Patcher(Patch[] patches, string patchToFolder, bool patchMuls)
		{
			if (File.Exists(patchToFolder))
				throw new Exception(patchToFolder + " is not a valid folder");

			if ((_UOPath = Client.DetectClient(Client.TwoDClient)) == Client.NotFound &&
				(_UOPath = Client.DetectClient(Client.ThreeDClient)) == Client.NotFound)
				throw new Exception("Ultima Online does not appear to be installed on this machine.");

			_UOPath = Path.GetDirectoryName(_UOPath);
			_patches = patches;
			_patchToFolder = patchToFolder;
			_patchMuls = patchMuls;

			if (_patchMuls)
				_thread = new Thread(InternalPatchMuls);
			else
				_thread = new Thread(InternalCombinePatches);
		}

		private void InternalCombinePatches()
		{
			if (ProgressChange != null)
				ProgressChange(this, new ProgressChangeEventArgs(0, 0, 0));

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Combining Patches...")));

			_combinedPatchFile = Path.Combine(_patchToFolder, "temp.patch");

			for (int i = 0; i < _patches.Length; i++)
			{
				if (_patches[i].FileID == (int)FileID.Anim_mul)
				{
					int id = _patches[i].BlockID;
					int fileIndex = BodyConverter.Convert(ref id);

					if (fileIndex == 1)
						continue;
					else if (fileIndex == 2)
						_patches[i].FileID = (int)FileID.Anim2_mul;
					else if (fileIndex == 3)
						_patches[i].FileID = (int)FileID.Anim3_mul;
					else if (fileIndex == 4)
						_patches[i].FileID = (int)FileID.Anim4_mul;
					else
						_patches[i].FileID = (int)FileID.Anim5_mul;
				}
			}

			PatchWriter.CreateMUO(_combinedPatchFile, _patches, ProgressChange);

			if (ProgressChange != null)
				ProgressChange(this, new ProgressChangeEventArgs(100, 0, 0));

			if (PatchingComplete != null)
				PatchingComplete(this, new OperationCompleteArgs());

			_thread.Join();
		}

		private void InternalPatchMuls()
		{
			if (!Directory.Exists((_patchToFolder)))
				Directory.CreateDirectory((_patchToFolder));

			if (ProgressChange != null)
				ProgressChange(this, new ProgressChangeEventArgs(0, 0, 0));

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating patch table...")));

			Dictionary<int, List<Patch>> typedPatchTable = CreateTable();

			List<int> keys = new List<int>(typedPatchTable.Keys);

			for (int i = 0; i < keys.Count; i++)
			{
				int key = keys[i];
				List<Patch> patches = typedPatchTable[keys[i]];

				if (StatusChange != null)
					StatusChange(this, new StatusChangeEventArgs(String.Format("Sorting...")));

				patches.Sort(new Comparison<Patch>(CompareTo));
				string id = Enum.GetName(typeof(FileID), (FileID)key);

				if (id == null && (id = Enum.GetName(typeof(ExtendedFileID), (ExtendedFileID)key)) == null)
					continue;

				switch (key)
				{
					case (int)FileID.Anim_mul:
						{
							PatchAnim(patches);
							break;
						}
					case (int)FileID.Art_mul:
						{
							PatchArt(patches);
							break;
						}
					case (int)FileID.AnimData_mul:
						{
							PatchAnimData(patches);
							break;
						}
					case (int)FileID.GumpArt_mul:
						{
							PatchGump(patches);
							break;
						}
					case (int)FileID.Hues_mul:
						{
							PatchHues(patches);
							break;
						}
					case (int)FileID.Map0_mul:
						{
							PatchMap0(patches);
							break;
						}
					case (int)FileID.Multi_mul:
						{
							PatchMultis(patches);
							break;
						}
					case (int)FileID.Skills_mul:
						{
							PatchSkills(patches);
							break;
						}
					case (int)FileID.Sound_mul:
						{
							PatchSounds(patches);
							break;
						}
					case (int)FileID.Statics0_mul:
						{
							PatchStatics(patches);
							break;
						}
					case (int)FileID.TexMaps_mul:
						{
							PatchTextures(patches);
							break;
						}
					case (int)FileID.TileData_mul:
						{
							PatchTileData(patches);
							break;
						}
					case (int)ExtendedFileID.Anim_mul:
						{
							PatchAnim(patches);
							break;
						}
					case (int)ExtendedFileID.Art_mul:
						{
							PatchArt(patches);
							break;
						}
					case (int)ExtendedFileID.GumpArt_mul:
						{
							PatchGump(patches);
							break;
						}
					case (int)ExtendedFileID.Map0_mul:
						{
							PatchMap0(patches);
							break;
						}
					case (int)ExtendedFileID.Multi_mul:
						{
							PatchMultis(patches);
							break;
						}
					case (int)ExtendedFileID.Sound_mul:
						{
							PatchSounds(patches);
							break;
						}
					case (int)ExtendedFileID.Statics0_mul:
						{
							PatchStatics(patches);
							break;
						}
					case (int)ExtendedFileID.TexMaps_mul:
						{
							PatchTextures(patches);
							break;
						}
					default: throw new Exception("Fuck, Fuck, Fuck, Fuck, Fuck, Fuck, Fuck, Fuck UOG");
				}
			}

			if (ProgressChange != null)
				ProgressChange(this, new ProgressChangeEventArgs(100, 0, 0));

			BuildCRCTable();

			if (PatchingComplete != null)
				PatchingComplete(this, new OperationCompleteArgs());

			_thread.Join();
		}

		private void BuildCRCTable()
		{
			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating file hash...")));

			string hashFile = Path.Combine(_patchToFolder, "hash.chk");
			long hash = 0;
			DirectoryInfo dir = new DirectoryInfo(_patchToFolder);
			FileInfo[] muls = dir.GetFiles("*.mul");
			FileInfo[] idxs = dir.GetFiles("*.idx");

			for (int i = 0; i < muls.Length; i++)
				hash += muls[i].LastWriteTime.GetHashCode();

			for (int i = 0; i < idxs.Length; i++)
				hash += idxs[i].LastWriteTime.GetHashCode();

			BinaryWriter bin = new BinaryWriter(File.Open(hashFile, FileMode.OpenOrCreate));
			bin.Write(hash);
			bin.Close();
		}

		public void WritePatches()
		{
			if (_thread.ThreadState != ThreadState.Running || _thread.ThreadState != ThreadState.Background)
				_thread.Start();
		}

		private string GetPath(string file)
		{
			if (File.Exists(Path.Combine(_patchToFolder, file)))
				return _patchToFolder;
			else
				return _UOPath;
		}

		private void PatchTileData(List<Patch> patches)
		{
			string tiledatamul = Path.Combine(GetPath("tiledata.mul"), "tiledata.mul");
			string output = Path.Combine(_patchToFolder, "tiledata.mul.tmp");

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating temp mul/idx file(s)...")));

			File.Copy(tiledatamul, output, true);
			BinaryWriter mul = new BinaryWriter(new FileStream(output, FileMode.Open));

			for (int p = 0; p < patches.Count; p++)
			{
				Patch patch = patches[p];
				if (patch.BlockID < 512)//1st 512 entries are Land Blocks
				{
					//each land block is 836 bytes
					mul.Seek(patch.BlockID * 836, SeekOrigin.Begin);
					mul.Write(patch.Data, 0, patch.Length);
				}
				else//static block
				{
					int offset = 428032;//offset in bytes of land blocks
					int index = patch.BlockID - 512;//index past land block offset
					int seekTo = offset + (1184 * index);//static block is 1184 bytes

					mul.Seek(seekTo, SeekOrigin.Begin);
					mul.Write(patch.Data, 0, patch.Length);
				}
			}

			mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			File.Copy(output, output.Substring(0, output.Length - 4), true);
		}

		private void PatchAnimData(List<Patch> patches)
		{
			string animdatamul = Path.Combine(GetPath("animdata.mul"), "animdata.mul");
			string output = Path.Combine(_patchToFolder, "animdata.mul.tmp");

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating temp mul/idx file(s)...")));

			File.Copy(animdatamul, output, true);
			BinaryWriter mul = new BinaryWriter(new FileStream(output, FileMode.Open));

			for (int p = 0; p < patches.Count; p++)
			{
				Patch patch = patches[p];
				mul.Seek(patch.BlockID * 548, SeekOrigin.Begin);
				mul.Write(patch.Data, 0, patch.Length);
			}

			mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			File.Copy(output, output.Substring(0, output.Length - 4), true);
		}

		private void PatchMap0(List<Patch> patches)
		{
			string mapmul = Path.Combine(GetPath("map0.mul"), "map0.mul");
			string output = Path.Combine(_patchToFolder, "map0.mul.tmp");

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating temp mul/idx file(s)...")));

			File.Copy(mapmul, output, true);
			BinaryWriter mul = new BinaryWriter(new FileStream(output, FileMode.Open));

			for (int p = 0; p < patches.Count; p++)
			{
				Patch patch = patches[p];
				mul.Seek(patch.BlockID * 196, SeekOrigin.Begin);
				mul.Write(patch.Data, 0, patch.Length);
			}

			mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			File.Copy(output, output.Substring(0, output.Length - 4), true);
		}

		private void PatchAnim(List<Patch> patches)
		{
			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating animation table...")));

			Dictionary<int, List<Patch>> animTable = new Dictionary<int, List<Patch>>();

			for (int i = 0; i < patches.Count; i++)
			{
				int id = _patches[i].BlockID;
				int key = 1;

				if (!animTable.ContainsKey(key))
					animTable.Add(key, new List<Patch>());

				animTable[key].Add(patches[i]);
			}

			List<int> keys = new List<int>(animTable.Keys);

			for (int i = 0; i < keys.Count; i++)
			{
				string fileNumber = keys[i] == 1 ? "" : keys[i].ToString();

				PatchFile(Path.Combine(GetPath("Anim" + fileNumber + ".idx"), "Anim" + fileNumber + ".idx"), Path.Combine(GetPath("Anim" + fileNumber + ".mul"), "Anim" + fileNumber + ".mul"),
				Path.Combine(_patchToFolder, "Anim" + fileNumber + ".idx.tmp"), Path.Combine(_patchToFolder, "Anim" + fileNumber + ".mul.tmp"), animTable[keys[i]]);
			}
		}

		private void PatchHues(List<Patch> patches)
		{
			string huesmul = Path.Combine(GetPath("hues.mul"), "hues.mul");
			string output = Path.Combine(_patchToFolder, "hues.mul.tmp");

			File.Copy(huesmul, output, true);
			BinaryWriter mul = new BinaryWriter(new FileStream(output, FileMode.Open));

			for (int p = 0; p < patches.Count; p++)
			{
				Patch patch = patches[p];
				mul.Seek(patch.BlockID * 708, SeekOrigin.Begin);
				mul.Write(patch.Data, 0, patch.Length);
			}

			mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			File.Copy(output, output.Substring(0, output.Length - 4), true);
		}

		private void PatchArt(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("Artidx.mul"), "Artidx.mul"), Path.Combine(GetPath("Art.mul"), "Art.mul"),
				Path.Combine(_patchToFolder, "Artidx.mul.tmp"), Path.Combine(_patchToFolder, "Art.mul.tmp"), patches);
		}

		private void PatchGump(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("Gumpidx.mul"), "Gumpidx.mul"), Path.Combine(GetPath("Gumpart.mul"), "Gumpart.mul"),
				Path.Combine(_patchToFolder, "Gumpidx.mul.tmp"), Path.Combine(_patchToFolder, "Gumpart.mul.tmp"), patches);
		}

		private void PatchMultis(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("Multi.idx"), "Multi.idx"), Path.Combine(GetPath("Multi.mul"), "Multi.mul"),
				Path.Combine(_patchToFolder, "Multi.idx.tmp"), Path.Combine(_patchToFolder, "Multi.mul.tmp"), patches);
		}

		private void PatchSkills(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("Skills.idx"), "Skills.idx"), Path.Combine(GetPath("skills.mul"), "skills.mul"),
				Path.Combine(_patchToFolder, "Skills.idx.tmp"), Path.Combine(_patchToFolder, "skills.mul.tmp"), patches);
		}

		private void PatchSounds(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("soundidx.mul"), "soundidx.mul"), Path.Combine(GetPath("sound.mul"), "sound.mul"),
				Path.Combine(_patchToFolder, "soundidx.mul.tmp"), Path.Combine(_patchToFolder, "sound.mul.tmp"), patches);
		}

		private void PatchStatics(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("StaIdx0.mul"), "StaIdx0.mul"), Path.Combine(GetPath("Statics0.mul"), "Statics0.mul"),
				Path.Combine(_patchToFolder, "StaIdx0.mul.tmp"), Path.Combine(_patchToFolder, "Statics0.mul.tmp"), patches);
		}

		private void PatchTextures(List<Patch> patches)
		{
			PatchFile(Path.Combine(GetPath("TexIdx.mul"), "TexIdx.mul"), Path.Combine(GetPath("TexMaps.mul"), "TexMaps.mul"),
				Path.Combine(_patchToFolder, "TexIdx.mul.tmp"), Path.Combine(_patchToFolder, "TexMaps.mul.tmp"), patches);
		}

		private void PatchFile(string idxPath, string mulPath, string newIdxPath, string newMulPath, List<Patch> patches)
		{
			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating temp mul/idx file(s)...")));

			File.Copy(idxPath, newIdxPath, true);
			File.Copy(mulPath, newMulPath, true);

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Applying {0} patches to {1}...", patches.Count, Path.GetFileName(newMulPath))));

			FileInfo idxFileInfo = new FileInfo(newIdxPath);
			FileIndex index = new FileIndex(Path.GetFileName(idxPath), Path.GetFileName(mulPath), (int)(idxFileInfo.Length / 12));

			BinaryWriter idx = new BinaryWriter(new FileStream(newIdxPath, FileMode.Open));
			BinaryWriter mul = new BinaryWriter(new FileStream(newMulPath, FileMode.Open));

			int oldPercent = 0;
			for (int p = 0; p < patches.Count; p++)
			{
				Patch patch = patches[p];
				int a = 0;
				if( patch.BlockID == 0xEEEE )
					a = 4;
				/*
				int pos;

				if (index[patch.BlockID].length > patch.Length)
					pos = index[patch.BlockID].lookup;		
				else
				*/	int	pos = Convert.ToInt32(mul.BaseStream.Length);	

				idx.Seek(patch.BlockID * 12, SeekOrigin.Begin);

				idx.Write(pos);
				idx.Write(patch.Length);
				idx.Write(patch.Extra);

				if (patch.Length >= 0)
				{
					mul.Seek(pos, SeekOrigin.Begin);
					mul.Write(patch.Data, 0, patch.Length);
				}

				if (p != 0 && ProgressChange != null)
				{
					int percent = (p * 100) / patches.Count;

					if (percent != oldPercent)
					{
						ProgressChange.Invoke(this, new ProgressChangeEventArgs(percent, p, patches.Count));
						oldPercent = percent;
					}
				}
			}

			index.Close();

			if (idx != null)
				idx.Close();
			if (mul != null)
				mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			if (File.Exists(newIdxPath.Substring(0, newIdxPath.Length - 4)))
				File.Delete(newIdxPath.Substring(0, newIdxPath.Length - 4));

			File.Move(newIdxPath, newIdxPath.Substring(0, newIdxPath.Length - 4));

			if (File.Exists(newMulPath.Substring(0, newMulPath.Length - 4)))
				File.Delete(newMulPath.Substring(0, newMulPath.Length - 4));

			File.Move(newMulPath, newMulPath.Substring(0, newMulPath.Length - 4));

			if (this.StatusChange != null)
				this.StatusChange(this, new StatusChangeEventArgs(string.Format("Cleaning up...", new object[0])));

			File.Delete(newIdxPath);
			File.Delete(newMulPath);

			#region Meh for now
			/*
			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Creating temp mul/idx file(s)...")));

			FileInfo info = new FileInfo(idxPath);
			FileIndex oldIndex = new FileIndex(Path.GetFileName(idxPath), Path.GetFileName(mulPath), (int)(info.Length / ((long)12)));

			BinaryWriter idx = new BinaryWriter(new FileStream(newIdxPath, FileMode.Create));
			idx.Seek((int)new FileInfo(idxPath).Length, SeekOrigin.Begin);

			BinaryWriter mul = new BinaryWriter(new FileStream(newMulPath, FileMode.Create));
			mul.Seek((int)new FileInfo(mulPath).Length, SeekOrigin.Begin);

			idx.Seek(0, SeekOrigin.Begin);
			mul.Seek(0, SeekOrigin.Begin);

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Applying {0} patches to {1}...", patches.Count, Path.GetFileName(newMulPath))));
			
			int patchIndex = 0;
			int oldPercent = 0;

			int offset = 0;

			for (int i = 0; i < oldIndex.IndexCount; i++)
			{
				if (patchIndex < patches.Count && patches[patchIndex].BlockID == i)//Write the patch
				{
					if (StatusChange != null)
						StatusChange(this, new StatusChangeEventArgs(String.Format("Applying patch id: {0}...", i)));

					Patch patch = patches[patchIndex];

					idx.Write((int)mul.BaseStream.Position);
					idx.Write(patch.Length);
					idx.Write(patch.Extra);

					if (patch.Length > 0)
						mul.Write(patch.Data, 0, patch.Length);

					patchIndex++;
				}
				else//Write the regular data
				{
					if (StatusChange != null && (i % 200 == 0))
						StatusChange(this, new StatusChangeEventArgs(String.Format("Writing index: {0}...", i)));

					idx.Write((int)mul.BaseStream.Position);
					idx.Write(oldIndex[i].length);
					idx.Write(oldIndex[i].extra);

					if (oldIndex[i].length > 0)
					{
						BinaryReader reader = new BinaryReader(oldIndex.Seek(i));
						mul.Write(reader.ReadBytes(oldIndex[i].length), 0, oldIndex[i].length);
					}
				}

				if (i != 0 && ProgressChange != null)
				{
					int percent = (i * 100) / oldIndex.IndexCount;

					if (percent != oldPercent)
					{
						ProgressChange(this, new ProgressChangeEventArgs(percent, i, oldIndex.IndexCount));
						oldPercent = percent;
					}
				}
			}

			//Patches out of idx range? Is this possible?
			if (patchIndex < patches.Count - 1)
			{
				if (StatusChange != null)
					StatusChange(this, new StatusChangeEventArgs(String.Format("Applying left over patches...")));

				int count = (patches.Count - 1) - patchIndex;
				int i = 0;
				while (i < count)
				{
					if (StatusChange != null)
						StatusChange(this, new StatusChangeEventArgs(String.Format("Applying patch id: {0}...", i)));
			
					Patch patch = patches[patchIndex];

					idx.Write((int)mul.BaseStream.Position);
					idx.Write(patch.Length);
					idx.Write(patch.Extra);

					if (patch.Length >= 0)
						mul.Write(patch.Data, 0, patch.Length);

					if (i != 0 && ProgressChange != null)
					{
						int percent = (i * 100) / count;

						if (percent != oldPercent)
						{
							ProgressChange(this, new ProgressChangeEventArgs(percent, i, count));
							oldPercent = percent;
						}
					}

					patchIndex++;
					i++;
				}
			}

			oldIndex.Close();

			if (idx != null)
				idx.Close();
			if (mul != null)
				mul.Close();

			if (StatusChange != null)
				StatusChange(this, new StatusChangeEventArgs(String.Format("Moving temp mul/idx file(s)...")));

			if (File.Exists(newIdxPath.Substring(0, newIdxPath.Length - 4)))
				File.Delete(newIdxPath.Substring(0, newIdxPath.Length - 4));

			File.Move(newIdxPath, newIdxPath.Substring(0, newIdxPath.Length - 4));

			if (File.Exists(newMulPath.Substring(0, newMulPath.Length - 4)))
				File.Delete(newMulPath.Substring(0, newMulPath.Length - 4));

			File.Move(newMulPath, newMulPath.Substring(0, newMulPath.Length - 4));

			if (this.StatusChange != null)
				this.StatusChange(this, new StatusChangeEventArgs(string.Format("Cleaning up...", new object[0])));

			File.Delete(newIdxPath);
			File.Delete(newMulPath);*/
			#endregion
		}

		private int CompareTo(Patch one, Patch two)
		{
			return one.BlockID.CompareTo(two.BlockID);
		}

		private Dictionary<int, List<Patch>> CreateTable()
		{
			Dictionary<int, List<Patch>> typedPatchTable = new Dictionary<int, List<Patch>>();

			for (int i = 0; i < _patches.Length; i++)
			{
				if (!typedPatchTable.ContainsKey(_patches[i].FileID))
					typedPatchTable.Add(_patches[i].FileID, new List<Patch>());

				typedPatchTable[_patches[i].FileID].Add(_patches[i]);
			}

			return typedPatchTable;
		}

		public event ProgressChangeHandler ProgressChange;
		public event OperationCompleteHandler PatchingComplete;
		public event StatusChangeHandler StatusChange;
	}
}
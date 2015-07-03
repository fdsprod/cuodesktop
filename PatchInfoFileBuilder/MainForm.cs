using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PatchlistBuilder
{
	public partial class MainForm : Form
	{
        private bool _needsSaving;

		public MainForm()
		{
			InitializeComponent();
		}

		private void saveInfoFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( listBox.Items.Count == 0 )
			{
				MessageBox.Show(this, "You have not yet added any patch files, there is nothing to save.");
				return;
			}

			saveFileDialog.Title = "Save the patch info file.";
			saveFileDialog.OverwritePrompt = true;
			saveFileDialog.Filter = "Txt File (*.txt)|*.txt";

			if( saveFileDialog.ShowDialog(this) == DialogResult.OK )
			{
				StreamWriter writer = new StreamWriter(File.Create(saveFileDialog.FileName));

				for( int i = 0; i < listBox.Items.Count; i++ )
					writer.WriteLine(((PatchlistFile)listBox.Items[i]).ToString());

				writer.Close();

                _needsSaving = false;
				MessageBox.Show(this, "Save successful");
			}
		}

		private void importPatchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Please select a compressed patch file";
			openFileDialog.CheckFileExists = false;
            openFileDialog.Filter = "Zip File (*.zip)|*.zip|Rar File (*.rar)|*.rar";

			if( openFileDialog.ShowDialog(this) == DialogResult.OK )
			{
				if( !File.Exists(openFileDialog.FileName) )
				{
					MessageBox.Show(this, "That file does not exist!");
					return;
				}

                CRC32 crc32 = new CRC32();
                crc32.PercentCompleteChange += new ProgressChangeHandler(OnPercentChange);
				FileStream stream = File.OpenRead(openFileDialog.FileName);
				uint crc = crc32.GetCrc32(stream);
				stream.Close();
                
				string location = string.Empty;
				PathInputForm form = new PathInputForm();
				form.PatchLocation = "http://";

				if( form.ShowDialog(this) == DialogResult.Cancel )
					return;

				if (!form.PatchLocation.Contains(Path.GetFileName(openFileDialog.FileName)))
				{
					if (!form.PatchLocation.EndsWith("/"))
						form.PatchLocation += "/";

					form.PatchLocation += Path.GetFileName(openFileDialog.FileName);
				}

                PatchlistFile file = new PatchlistFile(form.PatchLocation, crc, new FileInfo(openFileDialog.FileName).Length);

                if (!listBox.Items.Contains(file) && !DuplicateFileNames(file))
                {
                    _needsSaving = true;
                    listBox.Items.Add(file);
                }
			}
		}

   //     private void OnPercentChange( object sender, ProgressChangeEventArgs args)
   //     {
			//Invoke((MethodInvoker)delegate { progressBar.Value = args.Percent; });
   //     }

        private bool DuplicateFileNames(PatchlistFile file)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
                if (((PatchlistFile)listBox.Items[i]).FileName == file.FileName)
                    return true;

            return false;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (listBox.SelectedIndex == -1)
                return;

            listBox.Items.Remove(listBox.Items[listBox.SelectedIndex]);
            listBox.Invalidate();
		}

		private void openInfoFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Please select a patchlist file";
			openFileDialog.CheckFileExists = false;
			openFileDialog.Filter = "Patchlist (*.txt)|*.txt";

			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				string file = openFileDialog.FileName;

				if( !File.Exists(file) )
					return;

				if( Path.GetExtension(file) != ".txt")
					return;

				listBox.Items.Clear();
				StreamReader reader = new StreamReader(file);

				while( !reader.EndOfStream )
				{
					string line = reader.ReadLine();

					string[] split = line.Split('|');
					if (split.Length == 2)
						listBox.Items.Add(new PatchlistFile(split[0], uint.Parse(split[1]), 0));
					else if (split.Length == 3)
						listBox.Items.Add(new PatchlistFile(split[0], uint.Parse(split[1]), long.Parse(split[2])));
				}
			}
		}

        private void newPatchlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_needsSaving && MessageBox.Show("All unsaved data will be lost!\n\nAre you sure you want to start a new patchlist?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                listBox.Items.Clear();
			else
				listBox.Items.Clear();
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1)
				return;

			e.DrawBackground();

			if (((e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.Focus) != 0))
				e.Graphics.FillRectangle(Brushes.Gainsboro, e.Bounds);
			else
			{
				if (e.Index == 1 || e.Index % 2 != 0)
					e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
				else
					e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			}

			if (listBox.Items[e.Index] is PatchlistFile)
			{
				PatchlistFile p = (PatchlistFile)listBox.Items[e.Index];

				e.Graphics.DrawString("Patch CRC: " + p.CRC.ToString("X4"), Font, Brushes.Black, new PointF( 5, e.Bounds.Y + 6 ));
				e.Graphics.DrawString("Patch Size: " + ((p.Size == 0) ? "Unknown" : p.Size.ToString("0,0")), Font, Brushes.Black, new PointF(150, e.Bounds.Y + 6));
				e.Graphics.DrawString("Patch Url: " + p.Url, Font, Brushes.Black, new PointF(5, e.Bounds.Y + 24));
			}
        }

		private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight = 40;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void listBox_MouseClick(object sender, MouseEventArgs e)
		{
			for( int i = 0; i < listBox.Items.Count; i++ )
				if (listBox.GetItemRectangle(i).Contains(e.Location))
				{
					listBox.SetSelected(i, true);
					listBox.Invalidate();
					break;
				}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
		}
	}
}
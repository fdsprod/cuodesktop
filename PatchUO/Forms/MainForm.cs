using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public partial class MainForm : Form
	{
        private StatusForm _statusForm;
        public StatusForm StatusForm { get { return _statusForm; } set { _statusForm = value; } }

        private static MainForm _instance;
        public static MainForm Instance { get { return _instance; } set { _instance = value; } }

		public MainForm()
		{
			InitializeComponent();
		}

        private void OnLoad(object sender, EventArgs e)
        {
            _statusForm = new StatusForm();
            _statusForm.MdiParent = this;
            _statusForm.Show();

            UpdateStatus("PatchUO Initialized");
        }

        private void patchConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select the patch you wish to convert";
            openFileDialog.Filter = "MUO patch (*.muo)|*.muo|UOP patch (*.uop)|*.uop|Verdata patch (verdata.mul)|verdata.mul";
            openFileDialog.FileName = string.Empty;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateStatus("Loading " = openFileDialog.FileName + "...");
                PatchReader reader = new PatchReader(File.Open(openFileDialog.FileName, FileMode.Open), PatchReader.ExtensionToPatchFileType(Path.GetExtension(openFileDialog.FileName)));
            
                UpdateStatus("Reading patches, please wait... ");
                List<Patch> patches = reader.ReadPatches();


                UpdateStatus("Loaded " + patches.Count.ToString() + " patches into memory...");
                saveFileDialog.Title = "Select where you wish to save the patches to";
                saveFileDialog.Filter = "MUO patch (*.muo)|*.muo|UOP patch (*.uop)|*.uop|Verdata patch (verdata.mul)|verdata.mul";
                saveFileDialog.FileName = string.Empty;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PatchFileType type = PatchReader.ExtensionToPatchFileType(Path.GetExtension(saveFileDialog.FileName));

                    UpdateStatus("Saving patches, please wait... ");

                    PatchWriter writer = new PatchWriter(File.Open(saveFileDialog.FileName, FileMode.OpenOrCreate), type);
                    
                    switch (type)
                    {
                        case PatchFileType.MUO:
                            PatchWriter.CreateMUO(saveFileDialog.FileName, patches); break;
                        case PatchFileType.UOP:
                            PatchWriter.CreateUOP(saveFileDialog.FileName, patches); break;
                        case PatchFileType.Verdata:
                            break;
                    }

                    UpdateStatus("Patch conversion complete");
                    MessageBox.Show("Patch conversion complete", "Success");
                }
                else
                    MessageBox.Show("Patch conversion process aborted", "Aborted");

                if (reader != null)
                    reader.Close();

                patches = null;
            }
        }

        public void UpdateStatus(string status)
        {
            _statusForm.AddStatus(status);
            statusLabel.Text = "Status: " + status;
        }

        private void statusBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _statusForm.Show();
        }
	}
}

#region Old Reference Codes
/*
		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.LightSlateGray, pictureBox.Bounds);
			Rectangle rect = new Rectangle(( pictureBox.Width / 2 ) - 100, pictureBox.Height / 2 - 100, 200, 200);
			
			if( _drawRoom )
				DrawRoomView(e.Graphics, rect);
			else
				e.Graphics.FillRectangle(Brushes.Black, pictureBox.Bounds);

			if( _currentImage != null )
				e.Graphics.DrawImage(_currentImage, GetCenteredPoint(_currentImage.Size, pictureBox.Size));
		}

		private Point GetCenteredPoint(Size imageSize, Size frameSize)
		{
			int x = ( frameSize.Width / 2 ) - ( imageSize.Width / 2 );
			int y = ( frameSize.Height / 2 ) - ( imageSize.Height / 2 );
			return new Point(x, y);
		}	
		
		private void MainForm_Load(object sender, EventArgs e)
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
						
			PatchFile = new PatchUOFile();
			UpdateGroups();
		}

		private void UpdateGroups()
		{
			groupBox.BeginUpdate();
			for( int i = 0; i < PatchFile.Groups.Count; i++ )
				if( !groupBox.Items.Contains(PatchFile.Groups[i]) )
					groupBox.Items.Add(PatchFile.Groups[i]);
			groupBox.EndUpdate();

			groupBox.Invalidate();
		}

		private void newLibraryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( _notSaved && MessageBox.Show(this, "This will erased all unsaved data! Are you sure you want to continue?", 
				"Unsaved Data", MessageBoxButtons.YesNo,
				MessageBoxIcon.Exclamation) == DialogResult.No )
				return;

			_patchFile = new PatchUOFile();
			_notSaved = false;	
		}

		private void saveLibraryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( _patchFile == null || Patches.Count == 0 )
				return;

			saveFileDialog.Title = "Save PatchUO Library";
			saveFileDialog.Filter = "PatchUO files|*.puo|All Files|*.*";
			if( saveFileDialog.ShowDialog() == DialogResult.OK )
			{
				string title = Text;
				Text = title + " - Saving please wait...";
				string path = saveFileDialog.FileName;
				_patchFile.SaveFile(path);
				Text = title;
			}
		}

		private void openLibraryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( _notSaved && MessageBox.Show( this, "This will erased all unsaved data! Are you sure you want to continue?",
				"Unsaved Data", MessageBoxButtons.YesNo,
				MessageBoxIcon.Exclamation) == DialogResult.No)
				return;

			openFileDialog.Filter = "PatchUO files|*.puo|All Files|*.*";
			openFileDialog.Title = "Open PatchUO Library";
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				string title = Text;
				Text = title + " - Loading please wait...";
				_patchFile = new PatchUOFile(openFileDialog.FileName, true);
				
				listBox.BeginUpdate();
				listBox.Items.Clear();
				for( int i = 0; i < _patchFile.Patches.Count; i++ )
					listBox.Items.Add(_patchFile.Patches[i]);

				UpdateGroups();

				listBox.EndUpdate();

				_notSaved = false;
				Text = title;
			}
		}

		private void importPatchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Title = "Selected a MUO, UOP, or Verdata file";
			openFileDialog.Filter = "MUO files|*.muo|UOP files|*.uop|Verdata files|verdata.mul|All Files|*.*";

			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				string title = Text;
				Text = title + " - Importing please wait...";
				_patchFile.Import(openFileDialog.FileName);

				listBox.BeginUpdate();
				listBox.Items.Clear();
				for( int i = 0; i < _patchFile.Patches.Count; i++ )
						listBox.Items.Add(_patchFile.Patches[i]);

				UpdateGroups();

				listBox.EndUpdate();

				_notSaved = true;
				Text = title;
			}

		}

		private void exportPatchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ListBox.SelectedObjectCollection objects = groupBox.SelectedItems;
			List<PatchGroup> groups = new List<PatchGroup>();

			for( int i = 0; i < objects.Count; i++ )
				groups.Add((PatchGroup)objects[i]);

			if( _patchFile == null || groups.Count == 0 )
				return;

			saveFileDialog.Title = "Export group(s)";
			saveFileDialog.Filter = "MUO files|*.muo|UOP files|*.uop|Verdata files|verdata.mul";
			if( saveFileDialog.ShowDialog() == DialogResult.OK )
			{
				string title = Text;
				Text = title + " - Exporting please wait...";
				string path = saveFileDialog.FileName;
				_patchFile.Export(path, groups);
				Text = title;
			}
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void mulComparerToolStripMenuItem_Click(object sender, EventArgs e)
		{

			_notSaved = true;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			pictureBox.Invalidate();
		}

		private void DrawRoomView(Graphics g, Rectangle imgRect)
		{
			Point bottom = Point.Empty;
			Point top = Point.Empty;
			Point left = Point.Empty;
			Point right = Point.Empty;

			bottom.X = imgRect.X + ( imgRect.Width / 2 );
			bottom.Y = imgRect.Bottom;

			top.X = bottom.X;
			top.Y = bottom.Y - imgRect.Width;

			left.X = imgRect.X;
			left.Y = imgRect.Bottom - ( imgRect.Width / 2 );

			right.X = imgRect.Right;
			right.Y = left.Y;

			// top points
			Point top1 = Point.Empty;
			Point top2 = Point.Empty;
			Point top3 = Point.Empty;

			top1.Y = top2.Y = top3.Y = 0;
			top1.X = left.X;
			top2.X = top.X;
			top3.X = right.X;

			Pen blackPen = new Pen(Color.Black);
			Brush whiteBrush = new System.Drawing.Drawing2D.LinearGradientBrush(top, right, Color.DarkSlateBlue, Color.SkyBlue);

			// Draw base block
			Point[] baseBlock = new Point[] { top, right, bottom, left, top };
			g.FillPolygon(whiteBrush, baseBlock);

			// Fill
			Point[] leftArea = new Point[] { left, top1, top2, top, left };
			Point[] rightArea = new Point[] { top, top2, top3, right, top };

			Brush darkBrush = new System.Drawing.Drawing2D.LinearGradientBrush(left, top1, Color.MidnightBlue, Color.LightGray);
			Brush lightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(right, top3, Color.SteelBlue, Color.White);

			g.FillPolygon(darkBrush, leftArea);
			g.FillPolygon(lightBrush, rightArea);

			// Draw base
			g.DrawLines(blackPen, baseBlock);

			// Draw vertical lines
			g.DrawLine(blackPen, left, top1);
			g.DrawLine(blackPen, top, top2);
			g.DrawLine(blackPen, right, top3);

			// Draw top line
			g.DrawLine(blackPen, top1, top3);

			// Draw axes
			g.DrawLine(blackPen, right, GetIntersection(bottom, right));
			g.DrawLine(blackPen, right, GetIntersection(top, right));
			g.DrawLine(blackPen, left, GetIntersection(bottom, left));
			g.DrawLine(blackPen, left, GetIntersection(top, left));

			blackPen.Dispose();
			whiteBrush.Dispose();
			darkBrush.Dispose();
			lightBrush.Dispose();
		}

		private Point GetIntersection(Point p1, Point p2)
		{
			double a = ( (double)( p1.Y - p2.Y ) / (double)( p1.X - p2.X ) );
			double b = (double)p1.Y - ( a * (double)p1.X );

			if( p1.X < p2.X )
			{
				// Right side
				Point pRight = Point.Empty;

				pRight.X = Width;
				pRight.Y = (int)( (double)Width * a + b );

				if( pRight.Y < 0 )
				{
					pRight.Y = 0;
					pRight.X = (int)-( b / a );
				}

				return pRight;
			}
			else
			{
				Point pLeft = Point.Empty;

				if( b < 0 )
				{
					pLeft.Y = 0;
					pLeft.X = (int)( -b / a );
				}
				else
				{
					pLeft.Y = (int)b;
					pLeft.X = 0;
				}

				return pLeft;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if( _anim != null )
			{
				_anim.Stop();
				_anim = null;
			}
		}

		private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			if( listBox.Items.Count < 1 )
				return;

			object item = listBox.Items[e.Index];

			if( item == null || !( listBox.Items[e.Index] is Patch ) )
				return;

			Patch patch = (Patch)item;
			Image art = ImageCompiler.GetImage(patch);

			if( art == null )
			{
				e.ItemHeight = 1;
				return;
			}

			int width = art.Width;
			int height = art.Height;

			if( height > 24 )
				height = 24;

			e.ItemHeight = height + 5;
		}

		private void listBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if( e.Index < 0 )
				return;

			if( listBox.Items.Count < 1 )
				return;

			object item = listBox.Items[e.Index];

			if( item == null || !( listBox.Items[e.Index] is Patch ) )
				return;

			Patch patch = (Patch)item;
			Image art = ImageCompiler.GetImage(patch);

			if( art == null )
			{				
				return;
			}

			if( e.Index == 1 || e.Index % 2 != 0 )
			{
				if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
				else
					e.Graphics.FillRectangle(Brushes.GhostWhite, e.Bounds);
			}
			else
			{
				if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
				else
					e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			}

			Size size = art.Size;

			if( size.Width > 24 )
				size.Width = 24;

			if( size.Height > 24 )
				size.Height = 24;

			if( size.Height < 12 )
				size.Height = 12;

			Rectangle rect = new Rectangle(e.Bounds.Location, size);
			rect.Offset(5, 3);
			e.Graphics.DrawImage(art, rect);
			art.Dispose();

			e.Graphics.DrawString(String.Format("File ID: {0}", Enum.GetName(typeof(FileID), (FileID)patch.FileID).Replace('_', ',')), 
				Font, Brushes.Black, (float)e.Bounds.X + 50f, (float)e.Bounds.Y + 2);

			e.Graphics.DrawString(String.Format("Block ID: {0}", patch.BlockID.ToString("X4")),
				Font, Brushes.Black, (float)e.Bounds.X + 165f, (float)e.Bounds.Y + 2);

			e.Graphics.DrawString(String.Format("Size: {0:0,0} bytes", patch.Length),
				Font, Brushes.Black, (float)e.Bounds.X + 265f, (float)e.Bounds.Y + 2);
	
			e.Graphics.DrawRectangle(Pens.Gainsboro, e.Bounds);
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = listBox.SelectedIndex;
			int group = groupBox.SelectedIndex;

			if( group < 0 )
				group = 0;

			Patch patch = _patchFile.Groups[group].Patches[index];

			if( (FileID)patch.FileID == FileID.Anim_mul )
			{
				_drawRoom = true;

				if( _anim != null )
					_anim.Stop();

				_anim = new Animation(new DrawImageHandler(OnNextFrame), patch);
				_anim.Start();
			}
			else
			{
				_drawRoom = !( (FileID)patch.FileID == FileID.GumpArt_mul );
				
				if( _anim != null )
					_anim.Stop();

				_currentImage = ImageCompiler.GetImage(patch);
				pictureBox.Invalidate();
			}
		}

		private void OnNextFrame(Image image)
		{
			_currentImage = image;
			pictureBox.Invalidate();
		}

		private void groupBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox.SelectedObjectCollection objects = groupBox.SelectedItems;
			List<PatchGroup> groups = new List<PatchGroup>();

			for( int i = 0; i < objects.Count; i++ )
				groups.Add((PatchGroup)objects[i]);

			toolStripExportButton.Enabled = ( groups.Count >= 1 );
			exportPatchToolStripMenuItem.Enabled = ( groups.Count >= 1 );
			exportToFileToolStripMenuItem.Enabled = ( groups.Count >= 1 );				

			listBox.Items.Clear();
			listBox.BeginUpdate();

			for( int i = 0; i < groups.Count; i++ )
				for( int a = 0; a < groups[i].Patches.Count; a++ )
					listBox.Items.Add(groups[i].Patches[a]);

			listBox.EndUpdate();
		}

		private void groupBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight = 64;
			e.ItemWidth = 64;
		}

		private void groupBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if( groupBox.Items.Count < 1 )
				return;

			object item = groupBox.Items[e.Index];

			if( item == null || !( groupBox.Items[e.Index] is PatchGroup ) )
				return;

			PatchGroup group = (PatchGroup)item;
			Image art = group.GetGroupImage();

			if( art == null )
			{
				return;
			}

			if( e.Index == 1 || e.Index % 2 != 0 )
			{
				if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
				else
					e.Graphics.FillRectangle(Brushes.GhostWhite, e.Bounds);
			}
			else
			{
				if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds);
				else
					e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			}

			Rectangle rect = new Rectangle(new Point(e.Bounds.Location.X, ( e.Bounds.Location.Y + ( e.Bounds.Height / 2 ) - ( art.Height / 2 ) ) - 5), art.Size);
			rect.Offset(5, 3);
			e.Graphics.DrawImage(art, rect);
			art.Dispose();

			e.Graphics.DrawString(String.Format("Name: {0}", group.Name), Font, Brushes.Black, (float)e.Bounds.X + 50f, (float)e.Bounds.Y + 10);
			e.Graphics.DrawString(String.Format("Items: {0:0,0}", group.Patches.Count), Font, Brushes.Black, (float)e.Bounds.X + 50f, (float)e.Bounds.Y + 25);
			e.Graphics.DrawString(String.Format("Size: {0:0,0} bytes", group.GetSize()), Font, Brushes.Black, (float)e.Bounds.X + 50f, (float)e.Bounds.Y + 40);
		}

		private void listBox_HandleDroppedData(object data, DragEventArgs e)
		{
			if( data == null )
				return;
		}

		private void groupBox_HandleDroppedData(object data, DragEventArgs e)
		{
			if( data == null )
				return;

			Point mouse = groupBox.PointToClient(new Point(e.X, e.Y));

			if( !groupBox.Bounds.Contains(mouse) )
				return;

			if( data is Patch[] )
			{
				Patch[] patches = (Patch[])data;
				PatchGroup group = null;
				int index = 0;

				for( int i = 0; i < groupBox.Items.Count; i++ )
					if( groupBox.GetItemRectangle(i).Contains(mouse) )
					{
						index = i;
						group = (PatchGroup)groupBox.Items[i];
						break;
					}

				if( groupBox != null )
				{
					for( int i = 0; i < patches.Length; i++ )
						if( !group.Patches.Contains(patches[i]) )
							group.Patches.Add(patches[i]);

					groupBox.SetSelected(index, true);
					groupBox.Invalidate();
				}
			}
		}

		private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GroupNameDialog diag = new GroupNameDialog();
			diag.Text = "Group Name";

			if( diag.ShowDialog(this) == DialogResult.OK )
			{
				for( int i = 0; i < _patchFile.Groups.Count; i++ )
					if( _patchFile.Groups[i].Name == diag.GroupName )
					{
						MessageBox.Show("That name is already in use");
						addGroupToolStripMenuItem_Click(sender, e);
					}

				PatchGroup group = new PatchGroup(diag.GroupName);

				_patchFile.Groups.Add(group);
				groupBox.Items.Add(group);

				groupBox.Invalidate();
				_notSaved = true;
			}
		}

		private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( groupBox.SelectedItem == null )
				return;

			PatchGroup group = (PatchGroup)groupBox.SelectedItem;

			if( group.Name == "All" )
				return;

			if( MessageBox.Show("Are you sure you want to delete group: " + group.Name, "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes )
			{
				_patchFile.Groups.Remove(group);

				groupBox.Items.Remove(group);
				groupBox.SetSelected(0, true);
				groupBox.Invalidate();
			}
		}

		private void addToGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( groupBox.SelectedItem == null )
				return;

			ListBox.SelectedObjectCollection objects = listBox.SelectedItems;

			if( objects.Count < 1 )
				return;

			GroupNameDialog diag = new GroupNameDialog();
			diag.Text = "Name of group";

			if( diag.ShowDialog(this) == DialogResult.OK )
			{
				bool found = false;
				PatchGroup group = null;

				for( int i = 0; i < _patchFile.Groups.Count; i++ )
					if( _patchFile.Groups[i].Name == diag.GroupName )
					{
						found = true;
						group = _patchFile.Groups[i];
						break;
					}

				if( found )
				{
					List<Patch> patches = new List<Patch>();
					for( int i = 0; i < objects.Count; i++ )
						patches.Add((Patch)objects[i]);
					
					( (PatchGroup)groupBox.SelectedItem ).Patches.AddRange(patches);
					group.Patches.AddRange(patches);
					groupBox.Invalidate();

				}
				else
				{
					if( MessageBox.Show("This group does not exist, Would you like to create it?", "Group does not exist", MessageBoxButtons.YesNo) == DialogResult.Yes )
					{
						List<Patch> patches = new List<Patch>();
						
						for( int i = 0; i < objects.Count; i++ )
							patches.Add((Patch)objects[i]);

						group = new PatchGroup(diag.GroupName, patches);

						_patchFile.Groups.Add(group);
						groupBox.Items.Add(group);
						
						groupBox.SetSelected(0, true);
						groupBox.Invalidate();
					}
				}
			}
		}*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public delegate void DroppedDataHandler( object data, DragEventArgs e);

	public partial class DragDropListBox : ListBox
	{
		public event DroppedDataHandler HandleDroppedData;

		public DragDropListBox()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{			
				base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Right && MouseIsOverSelectedItem(e) && SelectedItems.Count > 0 )
				DoDragDrop(SelectedItems, DragDropEffects.Copy);
			else	
				base.OnMouseMove(e);
		}

		private bool MouseIsOverSelectedItem(MouseEventArgs e)
		{
			Point mouse = PointToClient(new Point(e.X, e.Y));

			for( int i = 0; i < Items.Count; i++ )
			{
				Rectangle rect = GetItemRectangle(i);

				if( rect.Contains(mouse) )
					return true;
			}

			return false;

		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			if( e.Data.GetDataPresent(typeof(SelectedObjectCollection)))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
				base.OnDragEnter(e);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			if( e.Data.GetDataPresent(typeof(SelectedObjectCollection)) )
			{
				object obj = e.Data.GetData(typeof(SelectedObjectCollection));

				if( obj != null )
				{
					SelectedObjectCollection col = (SelectedObjectCollection)obj;

					if( col.Count > 0 )
					{
						object data = null;

						if( col[0] is Patch )
						{
							Patch[] patches = new Patch[col.Count];

							for( int i = 0; i < col.Count; i++ )
								patches[i] = (Patch)col[i];

							data = patches;
						}
						else if( col[0] is PatchGroup )
						{
							PatchGroup[] groups = new PatchGroup[col.Count];

							for( int i = 0; i < col.Count; i++ )
								groups[i] = (PatchGroup)col[i];

							data = groups;
						}

						if( HandleDroppedData != null )
							HandleDroppedData(data, e);
					}					
				}						
			}			
			else
				base.OnDragDrop(e);
		}
	}
}

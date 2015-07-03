using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PatchlistBuilder
{
	public partial class PathInputForm : Form
	{
		public string PatchLocation { get { return textBox1.Text; } set { textBox1.Text = value; } }

		public PathInputForm()
		{
			DialogResult = DialogResult.Cancel;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PatchUO
{
	public partial class GroupNameDialog : Form
	{
		public string GroupName { get { return nameBox.Text; } set { nameBox.Text = value; } }
		public string FormTitle { get { return Text; } set { Text = value; } }

		public GroupNameDialog()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		private void addBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void GroupNameDialog_Load(object sender, EventArgs e)
		{

		}
	}
}
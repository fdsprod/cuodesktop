using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CUODesktop
{
    public partial class AddLocalServerDialog : Form
    {
        private bool _EditMode = false;
        public bool EditMode { get { return _EditMode; } set { _EditMode = value; } }

		public string ServerName { get { return nameBox.Text; } set { nameBox.Text = value; } }
		public string ServerDescription { get { return descBox.Text; } set { descBox.Text = value; } }
		public string ServerPort { get { return portBox.Text; } set { portBox.Text = value; } }
		public string ServerAddress { get { return addressBox.Text; } set { addressBox.Text = value; } }
		public string ServerPatchURL { get { return patchBox.Text; } set { patchBox.Text = value; } }
		public string ServerUpdateURL { get { return updateBox.Text; } set { updateBox.Text = value; } }
		public bool RemoveEnc { get { return patchChkBox.Checked; } set { patchChkBox.Checked = value; } }

        public AddLocalServerDialog()
        {
			DialogResult = DialogResult.Cancel;
            InitializeComponent();
        }

        private void AddLocalServerDialog_Load( object sender, EventArgs e )
        {
			if (_EditMode)
				addBtn.Text = "Edit";
			else
				addBtn.Text = "Add";

			this.BringToFront();
			this.Focus();
        }

        private void addBtn_Click( object sender, EventArgs e )
        {
            if( nameBox.Text == "" || nameBox.Text == null )                 
            {
                MessageBox.Show( "You must enter a Server Name.", "Invalid Entry" );
                return;
            }

            if( addressBox.Text == "" || addressBox.Text == null )
            {
                MessageBox.Show( "You must enter a Host Address.", "Invalid Entry" );
                return;
            }

            if( portBox.Text == "" || portBox.Text == null )
            {
                MessageBox.Show( "You must enter a Port Number.", "Invalid Entry" );
                return;
            }

            int port;
            if( !Int32.TryParse( portBox.Text, out port ) )
            {
                MessageBox.Show( "Invalid port address", "Invalid Entry" );
                return;
            }

			DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click( object sender, EventArgs e )
        {
            Close();
        }
    }
}
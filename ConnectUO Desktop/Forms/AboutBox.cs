using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace CUODesktop
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            label2.Text = String.Format("Version: {0}.{1}.{3}.{2}", Core.Version.Major, Core.Version.Minor, Core.Version.Revision, Core.Version.Build);
        }               
    }
}

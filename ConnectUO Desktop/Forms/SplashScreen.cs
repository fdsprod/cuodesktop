using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CUODesktop
{
	public partial class SplashScreen : Form
	{
		private ServerListUpdater _updater;

		public string Status 
		{ 
			get { return statusLbl.Text; } 
			set
			{
				if (this.Visible)
					statusLbl.Invoke((MethodInvoker)delegate { statusLbl.Text = String.Format("Status: {0}", value); });
			} 
		}

		public int Progress
		{
			get { return progressBar.Value; }
			set
			{
				if (this.Visible) progressBar.Invoke((MethodInvoker)delegate { progressBar.Value = value; });
			}
		}

		public event EventHandler CloseRequestEvent;

		public SplashScreen()
		{
			InitializeComponent();
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			if (CloseRequestEvent != null)
				CloseRequestEvent(this, new EventArgs());
		}

		private void SplashScreen_Load(object sender, EventArgs e)
		{
			if (MainForm.Instance.Visible)
				StartPosition = FormStartPosition.CenterParent;
			else
				StartPosition = FormStartPosition.CenterScreen;

			Activate();
			BringToFront();
			Focus();

			_updater = new ServerListUpdater(this);
			_updater.Update();
		}
	}
}
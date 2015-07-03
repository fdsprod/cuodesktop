namespace CUODesktop
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.updateServerlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.addLocalServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.updateServerlistToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
			this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.backBtn = new System.Windows.Forms.ToolStripButton();
			this.forwardBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.stopBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.homeBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.addressBar = new System.Windows.Forms.ToolStripComboBox();
			this.goBtn = new System.Windows.Forms.ToolStripButton();
			this.WebBrowser = new System.Windows.Forms.WebBrowser();
			this.addLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "ConnectUO Desktop";
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateServerlistToolStripMenuItem,
            this.toolStripSeparator2,
            this.addLocalServerToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.toolStripSeparator1,
            this.checkForUpdatesToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripSeparator3,
            this.quitToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(171, 154);
			// 
			// updateServerlistToolStripMenuItem
			// 
			this.updateServerlistToolStripMenuItem.Name = "updateServerlistToolStripMenuItem";
			this.updateServerlistToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.updateServerlistToolStripMenuItem.Text = "Update Serverlist";
			this.updateServerlistToolStripMenuItem.Click += new System.EventHandler(this.updateServerlistToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
			// 
			// addLocalServerToolStripMenuItem
			// 
			this.addLocalServerToolStripMenuItem.Name = "addLocalServerToolStripMenuItem";
			this.addLocalServerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.addLocalServerToolStripMenuItem.Text = "Add local server";
			this.addLocalServerToolStripMenuItem.Click += new System.EventHandler(this.addLocalServerToolStripMenuItem_Click);
			// 
			// configurationToolStripMenuItem
			// 
			this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
			this.configurationToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.configurationToolStripMenuItem.Text = "Configuration";
			this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
			this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.helpToolStripMenuItem.Text = "Help";
			this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(167, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.toolsToolStripMenuItem1,
            this.helpToolStripMenuItem2});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1084, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem1
			// 
			this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLocalToolStripMenuItem,
            this.newToolStripMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem1});
			this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
			this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem1.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "&Home";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem1
			// 
			this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
			this.exitToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem1.Text = "E&xit";
			this.exitToolStripMenuItem1.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem1
			// 
			this.toolsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.updateServerlistToolStripMenuItem1});
			this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
			this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem1.Text = "&Tools";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.optionsToolStripMenuItem.Text = "&Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
			// 
			// updateServerlistToolStripMenuItem1
			// 
			this.updateServerlistToolStripMenuItem1.Name = "updateServerlistToolStripMenuItem1";
			this.updateServerlistToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
			this.updateServerlistToolStripMenuItem1.Text = "Update Serverlist";
			this.updateServerlistToolStripMenuItem1.Click += new System.EventHandler(this.updateServerlistToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem2
			// 
			this.helpToolStripMenuItem2.Name = "helpToolStripMenuItem2";
			this.helpToolStripMenuItem2.Size = new System.Drawing.Size(52, 20);
			this.helpToolStripMenuItem2.Text = "&About";
			this.helpToolStripMenuItem2.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLbl,
            this.progressBar});
			this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip1.Location = new System.Drawing.Point(0, 710);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1084, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLbl
			// 
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(45, 17);
			this.statusLbl.Text = "Status: ";
			// 
			// progressBar
			// 
			this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(100, 16);
			// 
			// toolStrip1
			// 
			this.toolStrip1.AllowItemReorder = true;
			this.toolStrip1.CanOverflow = false;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backBtn,
            this.forwardBtn,
            this.toolStripSeparator4,
            this.refreshBtn,
            this.toolStripSeparator5,
            this.stopBtn,
            this.toolStripSeparator6,
            this.homeBtn,
            this.toolStripSeparator7,
            this.addressBar,
            this.goBtn});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1084, 32);
			this.toolStrip1.TabIndex = 4;
			// 
			// backBtn
			// 
			this.backBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.backBtn.Enabled = false;
			this.backBtn.Image = ((System.Drawing.Image)(resources.GetObject("backBtn.Image")));
			this.backBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.backBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.backBtn.Name = "backBtn";
			this.backBtn.Size = new System.Drawing.Size(28, 29);
			this.backBtn.Text = "Back";
			this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
			// 
			// forwardBtn
			// 
			this.forwardBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.forwardBtn.Enabled = false;
			this.forwardBtn.Image = ((System.Drawing.Image)(resources.GetObject("forwardBtn.Image")));
			this.forwardBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.forwardBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.forwardBtn.Name = "forwardBtn";
			this.forwardBtn.Size = new System.Drawing.Size(28, 29);
			this.forwardBtn.Text = "Forward";
			this.forwardBtn.Click += new System.EventHandler(this.forwardBtn_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
			// 
			// refreshBtn
			// 
			this.refreshBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
			this.refreshBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.refreshBtn.Name = "refreshBtn";
			this.refreshBtn.Size = new System.Drawing.Size(28, 29);
			this.refreshBtn.Text = "Refresh";
			this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 32);
			// 
			// stopBtn
			// 
			this.stopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stopBtn.Enabled = false;
			this.stopBtn.Image = ((System.Drawing.Image)(resources.GetObject("stopBtn.Image")));
			this.stopBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.stopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stopBtn.Name = "stopBtn";
			this.stopBtn.Size = new System.Drawing.Size(28, 29);
			this.stopBtn.Text = "Stop";
			this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 32);
			// 
			// homeBtn
			// 
			this.homeBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.homeBtn.Image = ((System.Drawing.Image)(resources.GetObject("homeBtn.Image")));
			this.homeBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.homeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.homeBtn.Name = "homeBtn";
			this.homeBtn.Size = new System.Drawing.Size(28, 29);
			this.homeBtn.Text = "Home";
			this.homeBtn.Click += new System.EventHandler(this.homeBtn_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 32);
			// 
			// addressBar
			// 
			this.addressBar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.addressBar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
			this.addressBar.Name = "addressBar";
			this.addressBar.Size = new System.Drawing.Size(121, 32);
			this.addressBar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.addressBar_KeyUp);
			// 
			// goBtn
			// 
			this.goBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.goBtn.Image = ((System.Drawing.Image)(resources.GetObject("goBtn.Image")));
			this.goBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.goBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.goBtn.Name = "goBtn";
			this.goBtn.Size = new System.Drawing.Size(23, 29);
			this.goBtn.Text = "Go";
			this.goBtn.Click += new System.EventHandler(this.goBtn_Click);
			// 
			// WebBrowser
			// 
			this.WebBrowser.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WebBrowser.Location = new System.Drawing.Point(0, 56);
			this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.WebBrowser.Name = "WebBrowser";
			this.WebBrowser.Size = new System.Drawing.Size(1084, 654);
			this.WebBrowser.TabIndex = 6;
			this.WebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.OnNavigated);
			this.WebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.OnNavigating);
			this.WebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.OnDocumentCompleted);
			// 
			// addLocalToolStripMenuItem
			// 
			this.addLocalToolStripMenuItem.Name = "addLocalToolStripMenuItem";
			this.addLocalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.addLocalToolStripMenuItem.Text = "Add Local";
			this.addLocalToolStripMenuItem.Click += new System.EventHandler(this.addLocalServerToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1084, 732);
			this.Controls.Add(this.WebBrowser);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "ConnectUO Desktop";
			this.ClientSizeChanged += new System.EventHandler(this.MainForm_ClientSizeChanged);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.contextMenuStrip.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		public System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ToolStripMenuItem addLocalServerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem updateServerlistToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLbl;
		private System.Windows.Forms.ToolStripProgressBar progressBar;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton backBtn;
		private System.Windows.Forms.ToolStripButton forwardBtn;
		private System.Windows.Forms.ToolStripButton refreshBtn;
		private System.Windows.Forms.ToolStripButton stopBtn;
		private System.Windows.Forms.ToolStripButton homeBtn;
        public System.Windows.Forms.WebBrowser WebBrowser;
		private System.Windows.Forms.ToolStripButton goBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateServerlistToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem2;
        private System.Windows.Forms.ToolStripComboBox addressBar;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addLocalToolStripMenuItem;
	}
}
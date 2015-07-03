namespace CUODesktop
{
    partial class ConfigDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.okBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.cGroupBox1 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.loadWithWindowsBox = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.templateBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.perPageBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.clientLbl = new System.Windows.Forms.Label();
			this.razorLbl = new System.Windows.Forms.Label();
			this.detect3dBtn = new System.Windows.Forms.Button();
			this.browseClientBtn = new System.Windows.Forms.Button();
			this.detect2dBtn = new System.Windows.Forms.Button();
			this.loadRazorChkBox = new System.Windows.Forms.CheckBox();
			this.detectRazorBtn = new System.Windows.Forms.Button();
			this.browseRazorBtn = new System.Windows.Forms.Button();
			this.openBrowserBox = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cGroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// colorDialog
			// 
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			this.colorDialog.SolidColorOnly = true;
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog";
			// 
			// okBtn
			// 
			this.okBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.okBtn.ForeColor = System.Drawing.Color.Black;
			this.okBtn.Location = new System.Drawing.Point(125, 245);
			this.okBtn.Name = "okBtn";
			this.okBtn.Size = new System.Drawing.Size(75, 23);
			this.okBtn.TabIndex = 12;
			this.okBtn.Text = "OK";
			this.okBtn.UseVisualStyleBackColor = true;
			this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
			// 
			// cancelBtn
			// 
			this.cancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.cancelBtn.ForeColor = System.Drawing.Color.Black;
			this.cancelBtn.Location = new System.Drawing.Point(206, 245);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 10;
			this.cancelBtn.Text = "Cancel";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
			// 
			// cGroupBox1
			// 
			this.cGroupBox1.BackColor = System.Drawing.Color.Transparent;
			this.cGroupBox1.Controls.Add(this.openBrowserBox);
			this.cGroupBox1.Controls.Add(this.label5);
			this.cGroupBox1.Controls.Add(this.label4);
			this.cGroupBox1.Controls.Add(this.loadWithWindowsBox);
			this.cGroupBox1.Controls.Add(this.label3);
			this.cGroupBox1.Controls.Add(this.templateBox);
			this.cGroupBox1.Controls.Add(this.label2);
			this.cGroupBox1.Controls.Add(this.perPageBox);
			this.cGroupBox1.Controls.Add(this.label1);
			this.cGroupBox1.Controls.Add(this.clientLbl);
			this.cGroupBox1.Controls.Add(this.razorLbl);
			this.cGroupBox1.Controls.Add(this.detect3dBtn);
			this.cGroupBox1.Controls.Add(this.browseClientBtn);
			this.cGroupBox1.Controls.Add(this.detect2dBtn);
			this.cGroupBox1.Controls.Add(this.loadRazorChkBox);
			this.cGroupBox1.Controls.Add(this.detectRazorBtn);
			this.cGroupBox1.Controls.Add(this.browseRazorBtn);
			this.cGroupBox1.Location = new System.Drawing.Point(12, 12);
			this.cGroupBox1.Name = "cGroupBox1";
			this.cGroupBox1.Size = new System.Drawing.Size(376, 227);
			this.cGroupBox1.TabIndex = 21;
			this.cGroupBox1.TabStop = false;
			this.cGroupBox1.Text = "ConnectUO Desktop Settings";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 106);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(148, 13);
			this.label4.TabIndex = 22;
			this.label4.Text = "Load Razor on Client Launch:";
			// 
			// loadWithWindowsBox
			// 
			this.loadWithWindowsBox.AutoSize = true;
			this.loadWithWindowsBox.Location = new System.Drawing.Point(160, 126);
			this.loadWithWindowsBox.Name = "loadWithWindowsBox";
			this.loadWithWindowsBox.Size = new System.Drawing.Size(15, 14);
			this.loadWithWindowsBox.TabIndex = 21;
			this.loadWithWindowsBox.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(53, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Load with windows:";
			// 
			// templateBox
			// 
			this.templateBox.FormattingEnabled = true;
			this.templateBox.Location = new System.Drawing.Point(160, 191);
			this.templateBox.Name = "templateBox";
			this.templateBox.Size = new System.Drawing.Size(145, 21);
			this.templateBox.TabIndex = 19;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(99, 194);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "Template:";
			// 
			// perPageBox
			// 
			this.perPageBox.Location = new System.Drawing.Point(160, 165);
			this.perPageBox.Name = "perPageBox";
			this.perPageBox.Size = new System.Drawing.Size(49, 20);
			this.perPageBox.TabIndex = 17;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(61, 168);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Servers Per Page:";
			// 
			// clientLbl
			// 
			this.clientLbl.AutoSize = true;
			this.clientLbl.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.clientLbl.ForeColor = System.Drawing.Color.Black;
			this.clientLbl.Location = new System.Drawing.Point(6, 16);
			this.clientLbl.Name = "clientLbl";
			this.clientLbl.Size = new System.Drawing.Size(74, 13);
			this.clientLbl.TabIndex = 9;
			this.clientLbl.Text = "Client Path:";
			// 
			// razorLbl
			// 
			this.razorLbl.AutoSize = true;
			this.razorLbl.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.razorLbl.ForeColor = System.Drawing.Color.Black;
			this.razorLbl.Location = new System.Drawing.Point(6, 58);
			this.razorLbl.Name = "razorLbl";
			this.razorLbl.Size = new System.Drawing.Size(74, 13);
			this.razorLbl.TabIndex = 13;
			this.razorLbl.Text = "Razor Path:";
			// 
			// detect3dBtn
			// 
			this.detect3dBtn.AutoSize = true;
			this.detect3dBtn.BackColor = System.Drawing.Color.Transparent;
			this.detect3dBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.detect3dBtn.ForeColor = System.Drawing.Color.Black;
			this.detect3dBtn.Location = new System.Drawing.Point(78, 32);
			this.detect3dBtn.Name = "detect3dBtn";
			this.detect3dBtn.Size = new System.Drawing.Size(66, 23);
			this.detect3dBtn.TabIndex = 11;
			this.detect3dBtn.Text = "Detect 3D";
			this.detect3dBtn.UseVisualStyleBackColor = false;
			this.detect3dBtn.Click += new System.EventHandler(this.detect3dBtn_Click);
			// 
			// browseClientBtn
			// 
			this.browseClientBtn.AutoSize = true;
			this.browseClientBtn.BackColor = System.Drawing.Color.Transparent;
			this.browseClientBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.browseClientBtn.ForeColor = System.Drawing.Color.Black;
			this.browseClientBtn.Location = new System.Drawing.Point(150, 32);
			this.browseClientBtn.Name = "browseClientBtn";
			this.browseClientBtn.Size = new System.Drawing.Size(61, 23);
			this.browseClientBtn.TabIndex = 12;
			this.browseClientBtn.Text = "Browse...";
			this.browseClientBtn.UseVisualStyleBackColor = false;
			this.browseClientBtn.Click += new System.EventHandler(this.browseClientBtn_Click);
			// 
			// detect2dBtn
			// 
			this.detect2dBtn.AutoSize = true;
			this.detect2dBtn.BackColor = System.Drawing.Color.Transparent;
			this.detect2dBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.detect2dBtn.ForeColor = System.Drawing.Color.Black;
			this.detect2dBtn.Location = new System.Drawing.Point(6, 32);
			this.detect2dBtn.Name = "detect2dBtn";
			this.detect2dBtn.Size = new System.Drawing.Size(66, 23);
			this.detect2dBtn.TabIndex = 10;
			this.detect2dBtn.Text = "Detect 2D";
			this.detect2dBtn.UseVisualStyleBackColor = false;
			this.detect2dBtn.Click += new System.EventHandler(this.detect2dBtn_Click);
			// 
			// loadRazorChkBox
			// 
			this.loadRazorChkBox.AutoSize = true;
			this.loadRazorChkBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ));
			this.loadRazorChkBox.ForeColor = System.Drawing.Color.Black;
			this.loadRazorChkBox.Location = new System.Drawing.Point(160, 106);
			this.loadRazorChkBox.Name = "loadRazorChkBox";
			this.loadRazorChkBox.Size = new System.Drawing.Size(15, 14);
			this.loadRazorChkBox.TabIndex = 10;
			this.loadRazorChkBox.UseVisualStyleBackColor = true;
			// 
			// detectRazorBtn
			// 
			this.detectRazorBtn.AutoSize = true;
			this.detectRazorBtn.BackColor = System.Drawing.Color.Transparent;
			this.detectRazorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.detectRazorBtn.ForeColor = System.Drawing.Color.Black;
			this.detectRazorBtn.Location = new System.Drawing.Point(6, 74);
			this.detectRazorBtn.Name = "detectRazorBtn";
			this.detectRazorBtn.Size = new System.Drawing.Size(80, 23);
			this.detectRazorBtn.TabIndex = 14;
			this.detectRazorBtn.Text = "Detect Razor";
			this.detectRazorBtn.UseVisualStyleBackColor = false;
			this.detectRazorBtn.Click += new System.EventHandler(this.detectRazorBtn_Click);
			// 
			// browseRazorBtn
			// 
			this.browseRazorBtn.AutoSize = true;
			this.browseRazorBtn.BackColor = System.Drawing.Color.Transparent;
			this.browseRazorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.browseRazorBtn.ForeColor = System.Drawing.Color.Black;
			this.browseRazorBtn.Location = new System.Drawing.Point(92, 74);
			this.browseRazorBtn.Name = "browseRazorBtn";
			this.browseRazorBtn.Size = new System.Drawing.Size(61, 23);
			this.browseRazorBtn.TabIndex = 15;
			this.browseRazorBtn.Text = "Browse...";
			this.browseRazorBtn.UseVisualStyleBackColor = false;
			this.browseRazorBtn.Click += new System.EventHandler(this.browseRazorBtn_Click);
			// 
			// openBrowserBox
			// 
			this.openBrowserBox.AutoSize = true;
			this.openBrowserBox.Location = new System.Drawing.Point(160, 146);
			this.openBrowserBox.Name = "openBrowserBox";
			this.openBrowserBox.Size = new System.Drawing.Size(15, 14);
			this.openBrowserBox.TabIndex = 24;
			this.openBrowserBox.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(23, 146);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(131, 13);
			this.label5.TabIndex = 23;
			this.label5.Text = "Open Browser On Startup:";
			// 
			// ConfigDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(403, 279);
			this.Controls.Add(this.cGroupBox1);
			this.Controls.Add(this.okBtn);
			this.Controls.Add(this.cancelBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ConfigDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ConfigDialog_Load);
			this.cGroupBox1.ResumeLayout(false);
			this.cGroupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button okBtn;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button browseClientBtn;
        private System.Windows.Forms.Label razorLbl;
		private System.Windows.Forms.Button detect3dBtn;
		private System.Windows.Forms.Button detect2dBtn;
		private System.Windows.Forms.Button detectRazorBtn;
        private System.Windows.Forms.Label clientLbl;
		private System.Windows.Forms.Button browseRazorBtn;
		private System.Windows.Forms.CheckBox loadRazorChkBox;
		private System.Windows.Forms.GroupBox cGroupBox1;
		private System.Windows.Forms.TextBox perPageBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox templateBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox loadWithWindowsBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox openBrowserBox;
		private System.Windows.Forms.Label label5;
    }
}
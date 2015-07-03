namespace MUOViewer
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
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.patchesLbl = new System.Windows.Forms.Label();
			this.authLbl = new System.Windows.Forms.Label();
			this.descLbl = new System.Windows.Forms.Label();
			this.nameLbl = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lengthLbl = new System.Windows.Forms.Label();
			this.extraLbl = new System.Windows.Forms.Label();
			this.blockidLbl = new System.Windows.Forms.Label();
			this.fileidLbl = new System.Windows.Forms.Label();
			this.textBox = new System.Windows.Forms.TextBox();
			this.upDown = new System.Windows.Forms.NumericUpDown();
			this.Patches = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.secUpDown = new System.Windows.Forms.NumericUpDown();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.menuStrip.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.upDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.secUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1070, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog";
			this.openFileDialog.Filter = "ModifyUO Patch Files (*.muo)|*.muo";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.patchesLbl);
			this.groupBox1.Controls.Add(this.authLbl);
			this.groupBox1.Controls.Add(this.descLbl);
			this.groupBox1.Controls.Add(this.nameLbl);
			this.groupBox1.Location = new System.Drawing.Point(12, 553);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(421, 99);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Information";
			// 
			// patchesLbl
			// 
			this.patchesLbl.AutoSize = true;
			this.patchesLbl.Location = new System.Drawing.Point(6, 72);
			this.patchesLbl.Name = "patchesLbl";
			this.patchesLbl.Size = new System.Drawing.Size(49, 13);
			this.patchesLbl.TabIndex = 3;
			this.patchesLbl.Text = "Patches:";
			// 
			// authLbl
			// 
			this.authLbl.AutoSize = true;
			this.authLbl.Location = new System.Drawing.Point(6, 52);
			this.authLbl.Name = "authLbl";
			this.authLbl.Size = new System.Drawing.Size(41, 13);
			this.authLbl.TabIndex = 2;
			this.authLbl.Text = "Author:";
			// 
			// descLbl
			// 
			this.descLbl.AutoSize = true;
			this.descLbl.Location = new System.Drawing.Point(6, 34);
			this.descLbl.Name = "descLbl";
			this.descLbl.Size = new System.Drawing.Size(63, 13);
			this.descLbl.TabIndex = 1;
			this.descLbl.Text = "Description:";
			// 
			// nameLbl
			// 
			this.nameLbl.AutoSize = true;
			this.nameLbl.Location = new System.Drawing.Point(6, 16);
			this.nameLbl.Name = "nameLbl";
			this.nameLbl.Size = new System.Drawing.Size(38, 13);
			this.nameLbl.TabIndex = 0;
			this.nameLbl.Text = "Name:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lengthLbl);
			this.groupBox2.Controls.Add(this.extraLbl);
			this.groupBox2.Controls.Add(this.blockidLbl);
			this.groupBox2.Controls.Add(this.fileidLbl);
			this.groupBox2.Location = new System.Drawing.Point(439, 553);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(219, 99);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Patch Information";
			// 
			// lengthLbl
			// 
			this.lengthLbl.AutoSize = true;
			this.lengthLbl.Location = new System.Drawing.Point(6, 72);
			this.lengthLbl.Name = "lengthLbl";
			this.lengthLbl.Size = new System.Drawing.Size(43, 13);
			this.lengthLbl.TabIndex = 3;
			this.lengthLbl.Text = "Length:";
			// 
			// extraLbl
			// 
			this.extraLbl.AutoSize = true;
			this.extraLbl.Location = new System.Drawing.Point(6, 54);
			this.extraLbl.Name = "extraLbl";
			this.extraLbl.Size = new System.Drawing.Size(34, 13);
			this.extraLbl.TabIndex = 2;
			this.extraLbl.Text = "Extra:";
			// 
			// blockidLbl
			// 
			this.blockidLbl.AutoSize = true;
			this.blockidLbl.Location = new System.Drawing.Point(6, 36);
			this.blockidLbl.Name = "blockidLbl";
			this.blockidLbl.Size = new System.Drawing.Size(48, 13);
			this.blockidLbl.TabIndex = 1;
			this.blockidLbl.Text = "BlockID:";
			// 
			// fileidLbl
			// 
			this.fileidLbl.AutoSize = true;
			this.fileidLbl.Location = new System.Drawing.Point(6, 18);
			this.fileidLbl.Name = "fileidLbl";
			this.fileidLbl.Size = new System.Drawing.Size(40, 13);
			this.fileidLbl.TabIndex = 0;
			this.fileidLbl.Text = "FileID: ";
			// 
			// textBox
			// 
			this.textBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox.Location = new System.Drawing.Point(12, 27);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.Size = new System.Drawing.Size(646, 494);
			this.textBox.TabIndex = 3;
			// 
			// upDown
			// 
			this.upDown.Location = new System.Drawing.Point(477, 527);
			this.upDown.Name = "upDown";
			this.upDown.Size = new System.Drawing.Size(61, 20);
			this.upDown.TabIndex = 4;
			this.upDown.ValueChanged += new System.EventHandler(this.upDown_ValueChanged);
			// 
			// Patches
			// 
			this.Patches.AutoSize = true;
			this.Patches.Location = new System.Drawing.Point(436, 529);
			this.Patches.Name = "Patches";
			this.Patches.Size = new System.Drawing.Size(38, 13);
			this.Patches.TabIndex = 5;
			this.Patches.Text = "Patch:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(550, 529);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Sector:";
			// 
			// secUpDown
			// 
			this.secUpDown.Location = new System.Drawing.Point(597, 527);
			this.secUpDown.Name = "secUpDown";
			this.secUpDown.Size = new System.Drawing.Size(61, 20);
			this.secUpDown.TabIndex = 6;
			this.secUpDown.ValueChanged += new System.EventHandler(this.secUpDown_ValueChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Location = new System.Drawing.Point(664, 27);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(401, 625);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1070, 664);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.secUpDown);
			this.Controls.Add(this.Patches);
			this.Controls.Add(this.upDown);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "MUO Viewer";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.upDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.secUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label authLbl;
		private System.Windows.Forms.Label descLbl;
		private System.Windows.Forms.Label nameLbl;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lengthLbl;
		private System.Windows.Forms.Label extraLbl;
		private System.Windows.Forms.Label blockidLbl;
		private System.Windows.Forms.Label fileidLbl;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.NumericUpDown upDown;
		private System.Windows.Forms.Label Patches;
		private System.Windows.Forms.Label patchesLbl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown secUpDown;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}


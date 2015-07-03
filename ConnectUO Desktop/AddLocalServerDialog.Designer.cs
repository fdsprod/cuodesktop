namespace CUODesktop
{
    partial class AddLocalServerDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddLocalServerDialog));
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.descBox = new System.Windows.Forms.TextBox();
			this.addressBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.portBox = new System.Windows.Forms.TextBox();
			this.patchChkBox = new System.Windows.Forms.CheckBox();
			this.addBtn = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.updateBox = new System.Windows.Forms.TextBox();
			this.patchBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.patchLbl = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(93, 12);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(226, 20);
			this.nameBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Server Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 119);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Host Address:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Description:";
			// 
			// descBox
			// 
			this.descBox.Location = new System.Drawing.Point(93, 38);
			this.descBox.Name = "descBox";
			this.descBox.Size = new System.Drawing.Size(226, 20);
			this.descBox.TabIndex = 4;
			// 
			// addressBox
			// 
			this.addressBox.Location = new System.Drawing.Point(93, 116);
			this.addressBox.Name = "addressBox";
			this.addressBox.Size = new System.Drawing.Size(118, 20);
			this.addressBox.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(217, 119);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Port:";
			// 
			// portBox
			// 
			this.portBox.Location = new System.Drawing.Point(247, 116);
			this.portBox.Name = "portBox";
			this.portBox.Size = new System.Drawing.Size(72, 20);
			this.portBox.TabIndex = 7;
			// 
			// patchChkBox
			// 
			this.patchChkBox.AutoSize = true;
			this.patchChkBox.Checked = true;
			this.patchChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.patchChkBox.Location = new System.Drawing.Point(93, 142);
			this.patchChkBox.Name = "patchChkBox";
			this.patchChkBox.Size = new System.Drawing.Size(234, 17);
			this.patchChkBox.TabIndex = 8;
			this.patchChkBox.Text = "Patch client encryption( this means remove )";
			this.patchChkBox.UseVisualStyleBackColor = true;
			// 
			// addBtn
			// 
			this.addBtn.Location = new System.Drawing.Point(69, 169);
			this.addBtn.Name = "addBtn";
			this.addBtn.Size = new System.Drawing.Size(75, 23);
			this.addBtn.TabIndex = 9;
			this.addBtn.Text = "Add";
			this.addBtn.UseVisualStyleBackColor = true;
			this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(189, 169);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// updateBox
			// 
			this.updateBox.Location = new System.Drawing.Point(93, 90);
			this.updateBox.Name = "updateBox";
			this.updateBox.Size = new System.Drawing.Size(226, 20);
			this.updateBox.TabIndex = 12;
			// 
			// patchBox
			// 
			this.patchBox.Location = new System.Drawing.Point(93, 64);
			this.patchBox.Name = "patchBox";
			this.patchBox.Size = new System.Drawing.Size(226, 20);
			this.patchBox.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(26, 93);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Update Url:";
			// 
			// patchLbl
			// 
			this.patchLbl.AutoSize = true;
			this.patchLbl.Location = new System.Drawing.Point(33, 67);
			this.patchLbl.Name = "patchLbl";
			this.patchLbl.Size = new System.Drawing.Size(54, 13);
			this.patchLbl.TabIndex = 13;
			this.patchLbl.Text = "Patch Url:";
			// 
			// AddLocalServerDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(338, 207);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.patchLbl);
			this.Controls.Add(this.updateBox);
			this.Controls.Add(this.patchBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.addBtn);
			this.Controls.Add(this.patchChkBox);
			this.Controls.Add(this.portBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.addressBox);
			this.Controls.Add(this.descBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nameBox);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "AddLocalServerDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AddLocalServerDialog";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.AddLocalServerDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox descBox;
        private System.Windows.Forms.TextBox addressBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portBox;
		private System.Windows.Forms.CheckBox patchChkBox;
		private System.Windows.Forms.Button addBtn;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox updateBox;
		private System.Windows.Forms.TextBox patchBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label patchLbl;
    }
}
namespace CUODesktop.PatchLib
{
	partial class PatchForm
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
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.statusLbl = new System.Windows.Forms.Label();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.updateBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 275);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(546, 10);
			this.progressBar.TabIndex = 0;
			// 
			// statusLbl
			// 
			this.statusLbl.AutoSize = true;
			this.statusLbl.Location = new System.Drawing.Point(12, 296);
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(78, 13);
			this.statusLbl.TabIndex = 1;
			this.statusLbl.Text = "Downloading...";
			// 
			// cancelBtn
			// 
			this.cancelBtn.Location = new System.Drawing.Point(483, 291);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 2;
			this.cancelBtn.Text = "Cancel";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
			// 
			// updateBox
			// 
			this.updateBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.updateBox.Location = new System.Drawing.Point(12, 12);
			this.updateBox.Multiline = true;
			this.updateBox.Name = "updateBox";
			this.updateBox.ReadOnly = true;
			this.updateBox.Size = new System.Drawing.Size(546, 257);
			this.updateBox.TabIndex = 3;
			this.updateBox.Text = "(Retrieving update info...)";
			// 
			// PatchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(570, 323);
			this.Controls.Add(this.updateBox);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.statusLbl);
			this.Controls.Add(this.progressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PatchForm";
			this.Text = "Patcher";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PatchForm_FormClosing);
			this.Load += new System.EventHandler(this.PatchForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label statusLbl;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.TextBox updateBox;
	}
}
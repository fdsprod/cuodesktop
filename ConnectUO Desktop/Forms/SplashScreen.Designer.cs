namespace CUODesktop
{
	partial class SplashScreen
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
			if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
			this.statusLbl = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.closeBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// statusLbl
			// 
			this.statusLbl.AutoSize = true;
			this.statusLbl.BackColor = System.Drawing.Color.Transparent;
			this.statusLbl.Location = new System.Drawing.Point(12, 140);
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(40, 13);
			this.statusLbl.TabIndex = 0;
			this.statusLbl.Text = "Status:";
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 114);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(296, 10);
			this.progressBar.TabIndex = 1;
			// 
			// closeBtn
			// 
			this.closeBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("closeBtn.BackgroundImage")));
			this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.closeBtn.Location = new System.Drawing.Point(302, 5);
			this.closeBtn.Name = "closeBtn";
			this.closeBtn.Size = new System.Drawing.Size(13, 13);
			this.closeBtn.TabIndex = 2;
			this.closeBtn.UseVisualStyleBackColor = true;
			this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
			// 
			// SplashScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(320, 160);
			this.Controls.Add(this.closeBtn);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.statusLbl);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.Opacity = 0.95;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.SplashScreen_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label statusLbl;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button closeBtn;
	}
}
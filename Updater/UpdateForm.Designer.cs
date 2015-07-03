namespace Updater
{
    partial class UpdateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
			this.changeLogBox = new System.Windows.Forms.TextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.statusLbl = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// changeLogBox
			// 
			this.changeLogBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.changeLogBox.Location = new System.Drawing.Point(12, 12);
			this.changeLogBox.Multiline = true;
			this.changeLogBox.Name = "changeLogBox";
			this.changeLogBox.ReadOnly = true;
			this.changeLogBox.Size = new System.Drawing.Size(537, 221);
			this.changeLogBox.TabIndex = 0;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 240);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(537, 10);
			this.progressBar.TabIndex = 1;
			// 
			// cancelBtn
			// 
			this.cancelBtn.Location = new System.Drawing.Point(474, 254);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 2;
			this.cancelBtn.Text = "Cancel";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
			// 
			// statusLbl
			// 
			this.statusLbl.AutoSize = true;
			this.statusLbl.Location = new System.Drawing.Point(9, 259);
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(78, 13);
			this.statusLbl.TabIndex = 3;
			this.statusLbl.Text = "Downloading...";
			// 
			// UpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(561, 286);
			this.Controls.Add(this.statusLbl);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.changeLogBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UpdateForm";
			this.Text = "Updating ConnectUO Desktop...";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateForm_FormClosing);
			this.Load += new System.EventHandler(this.UpdateForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox changeLogBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label statusLbl;
    }
}


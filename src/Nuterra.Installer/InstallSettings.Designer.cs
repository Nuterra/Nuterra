namespace Nuterra.Installer
{
	partial class InstallSettings
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rootDirectory = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.findGameDialog = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rootDirectory
			// 
			this.rootDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rootDirectory.Location = new System.Drawing.Point(6, 19);
			this.rootDirectory.Name = "rootDirectory";
			this.rootDirectory.Size = new System.Drawing.Size(325, 20);
			this.rootDirectory.TabIndex = 0;
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.Location = new System.Drawing.Point(337, 17);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(35, 23);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "...";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// findGameDialog
			// 
			this.findGameDialog.FileName = "TerraTechWin64.exe";
			this.findGameDialog.Filter = "TerraTech Windows (*.exe)|*.exe|TerraTech Linux (*.x86_64)|*.x86_64|TerraTech Mac" +
    "OS (*.app)|*.app|All Files (*.*)|*.*";
			this.findGameDialog.SupportMultiDottedExtensions = true;
			this.findGameDialog.Title = "Locate TerraTech executable";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rootDirectory);
			this.groupBox1.Controls.Add(this.browseButton);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(378, 50);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "TerraTech Install Directory";
			// 
			// InstallSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "InstallSettings";
			this.Size = new System.Drawing.Size(378, 50);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox rootDirectory;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.OpenFileDialog findGameDialog;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}

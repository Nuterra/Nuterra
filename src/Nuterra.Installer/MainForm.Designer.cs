namespace Nuterra.Installer
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
			this.installButton = new System.Windows.Forms.Button();
			this.installSettings = new Nuterra.Installer.InstallSettings();
			this.installConsole = new Nuterra.Installer.InstallConsole();
			this.SuspendLayout();
			// 
			// installButton
			// 
			this.installButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.installButton.Location = new System.Drawing.Point(528, 277);
			this.installButton.Name = "installButton";
			this.installButton.Size = new System.Drawing.Size(75, 23);
			this.installButton.TabIndex = 2;
			this.installButton.Text = "Install";
			this.installButton.UseVisualStyleBackColor = true;
			this.installButton.Click += new System.EventHandler(this.installButton_Click);
			// 
			// installSettings
			// 
			this.installSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.installSettings.Location = new System.Drawing.Point(12, 12);
			this.installSettings.Name = "installSettings";
			this.installSettings.Size = new System.Drawing.Size(591, 50);
			this.installSettings.TabIndex = 0;
			this.installSettings.TerraTechRoot = "";
			this.installSettings.TerraTechRootSelected += new System.Action<Nuterra.Installer.InstallSettings>(this.installerSettings_TerraTechRootSelected);
			// 
			// installConsole
			// 
			this.installConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.installConsole.Location = new System.Drawing.Point(12, 68);
			this.installConsole.Margin = new System.Windows.Forms.Padding(4);
			this.installConsole.Name = "installConsole";
			this.installConsole.Size = new System.Drawing.Size(591, 203);
			this.installConsole.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(615, 312);
			this.Controls.Add(this.installButton);
			this.Controls.Add(this.installSettings);
			this.Controls.Add(this.installConsole);
			this.MinimumSize = new System.Drawing.Size(400, 250);
			this.Name = "MainForm";
			this.Text = "Nuterra v0.4.2.1 Installer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private InstallSettings installSettings;
		private System.Windows.Forms.Button installButton;
		private InstallConsole installConsole;
	}
}


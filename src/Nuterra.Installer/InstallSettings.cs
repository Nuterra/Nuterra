using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nuterra.Installer
{
	public partial class InstallSettings : UserControl
	{
		public string TerraTechRoot
		{
			get { return rootDirectory.Text; }
			set { rootDirectory.Text = value; }
		}

		public InstallSettings()
		{
			InitializeComponent();
		}

		public event Action<InstallSettings> TerraTechRootSelected;

		private void browseButton_Click(object sender, EventArgs e)
		{
			switch (findGameDialog.ShowDialog())
			{
				case DialogResult.OK:
				case DialogResult.Yes:
					TerraTechRoot = findGameDialog.FileName;
					TerraTechRootSelected?.Invoke(this);
					break;
			}
		}
	}
}

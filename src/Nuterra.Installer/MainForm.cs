using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Nuterra.Build;

namespace Nuterra.Installer
{
	public partial class MainForm : Form
	{
		private Thread _installerThread;

		public MainForm()
		{
			InitializeComponent();
			installButton.Enabled = false;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			installSettings.TerraTechRoot = Properties.Settings.Default.TerraTechRootDir;
			if (installSettings.TerraTechRoot != "")
			{
				installerSettings_TerraTechRootSelected(installSettings);
			}
		}

		private void installerSettings_TerraTechRootSelected(InstallSettings settings)
		{
			UpdateFromInstallSettings(logEvents: true);
		}

		private void UpdateFromInstallSettings(bool logEvents)
		{
			installSettings.Enabled = (_installerThread == null);
			string selectedRoot = installSettings.TerraTechRoot;
			if (IsValidTerraTechRoot(selectedRoot))
			{
				installButton.Enabled = true;
				Properties.Settings.Default.TerraTechRootDir = selectedRoot;
				if (logEvents)
				{
					Console.WriteLine("Selected directory: The selected directory is valid, install can be started");
				}
			}
			else
			{
				installSettings.TerraTechRoot = null;
				installButton.Enabled = false;
				if (logEvents)
				{
					Console.WriteLine("Selected directory: The selected directory is not valid, probably missing the _Data folder");
				}
			}
		}

		private bool IsValidTerraTechRoot(string selectedRoot)
		{
			string realRoot = Path.GetDirectoryName(selectedRoot);
			string extensionless = Path.GetFileNameWithoutExtension(selectedRoot);
			string dataDir = Path.Combine(realRoot, extensionless + "_Data");
			return Directory.Exists(dataDir);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		private void installButton_Click(object sender, EventArgs e)
		{
			if (_installerThread != null)
			{
				MessageBox.Show("This shouldnt happen, but the installer thread is still busy...");
				return;
			}

			installSettings.Enabled = false;
			installButton.Enabled = false;
			_installerThread = new Thread(InstallerWorkerMethod);
			_installerThread.Start();
		}

		private void InstallerWorkerMethod()
		{
			try
			{
				Console.WriteLine();

				string localNuterra = Path.Combine(Directory.GetCurrentDirectory(), "Nuterra_Data");

				var info = new ModificationInfo();
				info.TerraTechRoot = Path.GetDirectoryName(installSettings.TerraTechRoot);
				info.NuterraData = localNuterra;
				info.InitDefaults();

				Console.WriteLine("Modding Assembly-CSharp.dll");
				InstallProgram.PerformInstall(info);
				Console.WriteLine();

				if (!ArePathsEqual(Directory.GetCurrentDirectory(), info.TerraTechRoot))
				{
					CopyNuterraFiles(info, localNuterra);
				}
				else
				{
					Console.WriteLine("You are running the installer from the terratech root directory, copying files is skipped");
				}

				Console.WriteLine();
				Console.WriteLine("Install succesfull");
				Console.WriteLine("Enjoy Nuterra :3");
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine(ex.StackTrace);
				Console.WriteLine();
				Console.WriteLine(ex.Message);
				Console.WriteLine("Install failed");
			}
			finally
			{
				_installerThread = null;
				Invoke(new Action(() => { UpdateFromInstallSettings(logEvents: false); }));
			}
		}

		private static void CopyNuterraFiles(ModificationInfo info, string localNuterra)
		{
			string moddedNuterra = Path.Combine(info.TerraTechRoot, "Nuterra_Data");
			CopyDirectory(localNuterra, moddedNuterra, "Assets", "*");
			CopyDirectory(localNuterra, moddedNuterra, "Mods", "*.dll");
		}

		private static void CopyDirectory(string localNuterra, string moddedNuterra, string dirname, string filePattern)
		{
			Console.WriteLine($"Copying {dirname}");
			string source = Path.Combine(localNuterra, dirname);
			if (!Directory.Exists(source))
			{
				Console.WriteLine("Warning: missing directory from install dir!");
				return;
			}
			string target = Path.Combine(moddedNuterra, dirname);
			if (!Directory.Exists(target))
			{
				Directory.CreateDirectory(target);
			}

			foreach (string file in Directory.EnumerateFiles(source, filePattern))
			{
				string fileName = Path.GetFileName(file);
				Console.WriteLine($"- {fileName}");
				File.Copy(file, Path.Combine(target, fileName), overwrite: true);
			}
			Console.WriteLine();
		}

		private bool ArePathsEqual(string path1, string path2)
		{
			if (path1 == null) throw new ArgumentNullException(nameof(path1));
			if (path2 == null) throw new ArgumentNullException(nameof(path2));
			path1 = Path.GetFullPath(path1);
			path2 = Path.GetFullPath(path2);
			bool isUnix = (Path.DirectorySeparatorChar == '/');
			StringComparison pathComparison = isUnix ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
			return path1.Equals(path2, pathComparison);
		}
	}
}
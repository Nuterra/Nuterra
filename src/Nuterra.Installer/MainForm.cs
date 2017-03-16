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
				string localMods = Path.Combine(localNuterra, "Mods");
				if (!Directory.Exists(localMods))
				{
					throw new Exception("Missing mods from install directory Nuterra_Data/Mods!");
				}

				var info = new ModificationInfo();
				info.TerraTechRoot = Path.GetDirectoryName(installSettings.TerraTechRoot);
				info.NuterraData = localNuterra;
				info.InitDefaults();

				Console.WriteLine("Modding Assembly-CSharp.dll");
				InstallProgram.PerformInstall(info);

				if (!ArePathsEqual(Directory.GetCurrentDirectory(), info.TerraTechRoot))
				{
					CopyNuterraFiles(info, localNuterra, localMods);
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

		private static void CopyNuterraFiles(ModificationInfo info, string localNuterra, string localMods)
		{
			Console.WriteLine("Copying Assetbundles");
			string moddedNuterra = Path.Combine(info.TerraTechRoot, "Nuterra_Data");
			if (!Directory.Exists(moddedNuterra))
			{
				Directory.CreateDirectory(moddedNuterra);
			}

			File.Copy($"{localNuterra}\\mod-nuterra.manifest", $"{moddedNuterra}\\mod-nuterra.manifest", overwrite: true);
			File.Copy($"{localNuterra}\\mod-nuterra", $"{moddedNuterra}\\mod-nuterra", overwrite: true);

			Console.WriteLine("Copying Mods");
			string moddedMods = Path.Combine(moddedNuterra, "Mods");
			if (!Directory.Exists(moddedMods))
			{
				Directory.CreateDirectory(moddedMods);
			}

			foreach (string file in Directory.EnumerateFiles(localMods, "*.dll"))
			{
				string modName = Path.GetFileName(file);
				Console.WriteLine($"- {modName}");
				File.Copy(file, $"{moddedMods}\\{modName}", overwrite: true);
			}
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
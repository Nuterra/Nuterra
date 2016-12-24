using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public class ModConfig
	{
		public KeyCode DrillKey;

		public KeyCode HammerKey;

		public KeyCode MagnetToggleKey;

		public KeyCode ScoopKey;

		public string ConfigFileName;

		public KeyCode SawKey;

		public bool MobileSolarPanels;

		public bool BuyIntoInventory;

		public KeyCode PlasmaKey;

		public float MobileSolarMultiplier;

		public float MobileSolarVelocityThreshold;

		public ModConfig()
		{
			this.ConfigFileName = "mod.maritaria.settings";
			this.BuyIntoInventory = false;
			this.DrillKey = 0;
			this.HammerKey = 0;
			this.MagnetToggleKey = 0;
			this.MobileSolarMultiplier = 0.2f;
			this.MobileSolarPanels = false;
			this.MobileSolarVelocityThreshold = 0.1f;
			this.PlasmaKey = 0;
			this.ScoopKey = 0;
		}

		public void Load()
		{
			this.Clear();
			this.ReadFile();
		}

		private void ReadFile()
		{
			if (!File.Exists(this.ConfigFileName))
			{
				using (StreamWriter streamWriter = File.CreateText(this.ConfigFileName))
				{
					streamWriter.WriteLine("# Lines starting with a # are ignored\n# Some settings are disabled by default, remove the # in front of the line to enable it\n# Settings format: [name] [value]\n# Setting names and values must be split by a single space\n# List of UnityEngine.KeyCode: https://docs.unity3d.com/ScriptReference/KeyCode.html\n\n# The key to trigger all drills on your current tech (UnityEngine.KeyCode)\nDrillKey Alpha1\n\n# The key to trigger all buzzsaws on your current tech (UnityEngine.KeyCode)\nSawKey Alpha2\n\n# The key to trigger all hammer blocks on your current tech (UnityEngine.KeyCode)\nHammerKey Alpha3\n\n# The key to trigger all scoop blocks on your current tech (UnityEngine.KeyCode)\nScoopKey Alpha4\n\n# The key to trigger all \"Plasma Cutter\" blocks on your current tech (UnityEngine.KeyCode)\nPlasmaKey Alpha5\n\n# The key to toggle your magnet blocks on/off; when disabled your magnets will not attract any blocks (UnityEngine.KeyCode)\nMagnetToggleKey M\n\n# Every hit after a block starts to explode lowers the explosion timer by a given amount of seconds (seconds)\n# Vanilla: 0.1\n#ExplodeTimerReductionPerHit 1.0\n\n# Enable solar panels to be used on techs\nMobileSolarPanels 1");
				}
			}
			using (FileStream fileStream = new FileStream(this.ConfigFileName, FileMode.OpenOrCreate))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					while (!streamReader.EndOfStream)
					{
						this.Apply(streamReader.ReadLine());
					}
				}
			}
		}

		private void Clear()
		{
			this.DrillKey = (KeyCode)49;
			this.HammerKey =(KeyCode) 50;
			this.MagnetToggleKey = (KeyCode)109;
			this.ScoopKey = (KeyCode)51;
		}

		private void Apply(string line)
		{
			line = line.Trim();
			if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
			{
				return;
			}
			string[] expr_31 = line.Split(new char[]
			{
				' '
			}, 2, StringSplitOptions.RemoveEmptyEntries);
			string settingName = expr_31[0];
			string settingValue = expr_31[1];
			this.Apply(settingName, settingValue);
		}

		private void Apply(string settingName, string settingValue)
		{
			string a = settingName.ToLower();
			if (a == "drillkey")
			{
				this.DrillKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "sawkey")
			{
				this.SawKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "hammerkey")
			{
				this.HammerKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "scoopkey")
			{
				this.ScoopKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "magnettogglekey")
			{
				this.MagnetToggleKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "plasmakey")
			{
				this.PlasmaKey = ModConfig.ParseKey(settingValue, 0);
				return;
			}
			if (a == "explodetimerreductionperhit")
			{
				Globals.inst.moduleDamageParams.explodeTimerReductionPerAdditionalHit = 1f;
				return;
			}
			if (a == "buyintoinventory")
			{
				this.BuyIntoInventory = ModConfig.ParseBoolean(settingValue);
				return;
			}
			if (a == "mobilesolarpanels")
			{
				this.MobileSolarPanels = ModConfig.ParseBoolean(settingValue);
				return;
			}
		}

		private static KeyCode ParseKey(string keyName, KeyCode fallback)
		{
			KeyCode result;
			try
			{
				result = (KeyCode)Enum.Parse(typeof(KeyCode), keyName, true);
			}
			catch
			{
				result = fallback;
			}
			return result;
		}

		private static float ParseFloat(string input, float fallback)
		{
			float result;
			if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return fallback;
		}

		public static bool ParseBoolean(string input)
		{
			return input.Trim().Equals("1", StringComparison.Ordinal);
		}
	}
}

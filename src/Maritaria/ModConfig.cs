using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public class ModConfig
	{
		public static readonly string DefaultConfigContents = "# Lines starting with a # are ignored\n# Some settings are disabled by default, remove the # in front of the line to enable it\n# Settings format: [name] [value]\n# Setting names and values must be split by a single space\n# List of UnityEngine.KeyCode: https://docs.unity3d.com/ScriptReference/KeyCode.html\n\n# The key to trigger all drills on your current tech (UnityEngine.KeyCode)\nDrillKey Alpha1\n\n# The key to trigger all buzzsaws on your current tech (UnityEngine.KeyCode)\nSawKey Alpha2\n\n# The key to trigger all hammer blocks on your current tech (UnityEngine.KeyCode)\nHammerKey Alpha3\n\n# The key to trigger all scoop blocks on your current tech (UnityEngine.KeyCode)\nScoopKey Alpha4\n\n# The key to trigger all \"Plasma Cutter\" blocks on your current tech (UnityEngine.KeyCode)\nPlasmaKey Alpha5\n\n# The key to toggle your magnet blocks on/off; when disabled your magnets will not attract any blocks (UnityEngine.KeyCode)\nMagnetToggleKey M\n\n# Every hit after a block starts to explode lowers the explosion timer by a given amount of seconds (seconds)\n# Vanilla: 0.1\n#ExplodeTimerReductionPerHit 1.0\n\n# Enable solar panels to be used on techs\nMobileSolarPanels 1\n\n# Enable to allow changing time of day\n#TurnDayKey Alpha9\n#TurnNightKey Alpha0\n\n# This key allows you to switch to first-person mode when controlling a tech. The view is from a smiley cube which has to be present on the tech (blockID 9000).\n#FirstPersonKey F";
		public string ConfigFileName = Mod.DataDirectory + "\\mod.settings";
		
		public KeyCode DrillKey;
		public KeyCode HammerKey;
		public KeyCode MagnetToggleKey;
		public KeyCode ScoopKey;
		public KeyCode PlasmaKey;
		public KeyCode SawKey;
		public KeyCode ProductionToggleKey;

		public bool MobileSolarPanels;
		public float MobileSolarVelocityThreshold;
		public float MobileSolarMultiplier;
		
		public KeyCode TurnNightKey;
		public KeyCode TurnDayKey;
		public KeyCode FirstPersonKey;
		
		public bool BuyIntoInventory;
		
		public ModConfig()
		{
			RestoreDefaultSettings();
		}
		
		public void Load()
		{
			this.RestoreDefaultSettings();
			this.ReadFile();
		}

		private void ReadFile()
		{
			if (!File.Exists(this.ConfigFileName))
			{
				Console.WriteLine("Creating default config file: " + ConfigFileName);
				using (StreamWriter streamWriter = File.CreateText(this.ConfigFileName))
				{
					streamWriter.WriteLine(DefaultConfigContents);
				}
			}
			using (FileStream fileStream = new FileStream(this.ConfigFileName, FileMode.OpenOrCreate))
			using (StreamReader streamReader = new StreamReader(fileStream))
			{
				while (!streamReader.EndOfStream)
				{
					this.Apply(streamReader.ReadLine());
				}
			}
		}
		
		private void RestoreDefaultSettings()
		{
			DrillKey = KeyCode.None;
			HammerKey = KeyCode.None;
			MagnetToggleKey = KeyCode.None;
			ScoopKey = KeyCode.None;
			PlasmaKey = KeyCode.None;
			SawKey = KeyCode.None;

			MobileSolarPanels = false;
			MobileSolarVelocityThreshold = 0.1f;
			MobileSolarMultiplier = 0.2f;

			TurnNightKey = KeyCode.None;
			TurnDayKey = KeyCode.None;
			FirstPersonKey = KeyCode.None;

			BuyIntoInventory = false;
		}

		private void Apply(string line)
		{
			line = line.Trim();
			if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
			{
				string[] parts = line.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
				string settingName = parts[0];
				string settingValue = parts[1];
				this.Apply(settingName, settingValue);
			}
		}

		private void Apply(string settingName, string settingValue)
		{
			string a = settingName.ToLower();
			if (a == "drillkey")
			{
				this.DrillKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "sawkey")
			{
				this.SawKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "hammerkey")
			{
				this.HammerKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "scoopkey")
			{
				this.ScoopKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "magnettogglekey")
			{
				this.MagnetToggleKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "plasmakey")
			{
				this.PlasmaKey = ModConfig.ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "productiontogglekey")
			{
				this.ProductionToggleKey = ModConfig.ParseKey(settingValue, KeyCode.None);
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
			if (a == "turnnightkey")
			{
				this.TurnNightKey = ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "turndaykey")
			{
				this.TurnDayKey = ParseKey(settingValue, KeyCode.None);
				return;
			}
			if (a == "firstpersonkey")
			{
				this.FirstPersonKey = ParseKey(settingValue, KeyCode.None);
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

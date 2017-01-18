using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Maritaria
{
	public class MaritariaConfig
	{
		public static readonly string DefaultConfigContents = "# Lines starting with a # are ignored\n# Some settings are disabled by default, remove the # in front of the line to enable it\n# Settings format: [name] [value]\n# Setting names and values must be split by a single space\n# List of UnityEngine.KeyCode: https://docs.unity3d.com/ScriptReference/KeyCode.html\n\n# The key to trigger all drills on your current tech (UnityEngine.KeyCode)\nDrillKey Alpha1\n\n# The key to trigger all buzzsaws on your current tech (UnityEngine.KeyCode)\nSawKey Alpha2\n\n# The key to trigger all hammer blocks on your current tech (UnityEngine.KeyCode)\nHammerKey Alpha3\n\n# The key to trigger all scoop blocks on your current tech (UnityEngine.KeyCode)\nScoopKey Alpha4\n\n# The key to trigger all \"Plasma Cutter\" blocks on your current tech (UnityEngine.KeyCode)\nPlasmaKey Alpha5\n\n# The key to toggle your magnet blocks on/off; when disabled your magnets will not attract any blocks (UnityEngine.KeyCode)\nMagnetToggleKey M\n\n# Every hit after a block starts to explode lowers the explosion timer by a given amount of seconds (seconds)\n# Vanilla: 0.1\n#ExplodeTimerReductionPerHit 1.0\n\n# Enable solar panels to be used on techs\nMobileSolarPanels 1\n\n# Enable to allow changing time of day\n#TurnDayKey Alpha9\n#TurnNightKey Alpha0\n\n# This key allows you to switch to first-person mode when controlling a tech. The view is from a smiley cube which has to be present on the tech (blockID 9000).\n#FirstPersonKey F";
		public string ConfigFileName = "\\mod.settings";

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
		public KeyCode SuicideKey;

		public MaritariaConfig()
		{
			RestoreDefaultSettings();
		}

		private void RestoreDefaultSettings()
		{
			MobileSolarPanels = false;
			MobileSolarVelocityThreshold = 0.1f;
			MobileSolarMultiplier = 0.2f;

		}

		public void Load(JObject json)
		{
			if (json == null) return;

			//Weapons
			DrillKey = ParseKey(json, nameof(DrillKey), KeyCode.None);
			HammerKey = ParseKey(json, nameof(HammerKey), KeyCode.None);
			MagnetToggleKey = ParseKey(json, nameof(MagnetToggleKey), KeyCode.None);
			ScoopKey = ParseKey(json, nameof(ScoopKey), KeyCode.None);
			PlasmaKey = ParseKey(json, nameof(PlasmaKey), KeyCode.None);
			SawKey = ParseKey(json, nameof(SawKey), KeyCode.None);

			//Cheats
			TurnNightKey = ParseKey(json, nameof(TurnNightKey), KeyCode.None);
			TurnDayKey = ParseKey(json, nameof(TurnDayKey), KeyCode.None);
			FirstPersonKey = ParseKey(json, nameof(FirstPersonKey), KeyCode.None);
			SuicideKey = ParseKey(json, nameof(SuicideKey), KeyCode.None);

			//Mobile solar
			MobileSolarPanels = ParseBoolean(json, nameof(MobileSolarPanels), false);
			MobileSolarVelocityThreshold = ParseFloat(json, nameof(MobileSolarVelocityThreshold), 0.1f);
			MobileSolarMultiplier = ParseFloat(json, nameof(MobileSolarMultiplier), 0.2f);
		}


		private static KeyCode ParseKey(JObject json, string keyName, KeyCode fallback)
		{
			if (json == null)
			{
				return fallback;
			}
			JToken token = json.GetValue(keyName, StringComparison.OrdinalIgnoreCase);
			if (token == null)
			{
				return fallback;
			}
			try
			{
				string value = token.Value<string>();
				return (KeyCode)Enum.Parse(typeof(KeyCode), value, true);
			}
			catch
			{
				return fallback;
			}
		}

		private static float ParseFloat(JObject json, string keyName, float fallback)
		{
			if (json == null)
			{
				return fallback;
			}
			JToken token = json.GetValue(keyName, StringComparison.OrdinalIgnoreCase);
			if (token == null)
			{
				return fallback;
			}
			try
			{
				return token.Value<float>();
			}
			catch
			{
				return fallback;
			}
		}
		private bool ParseBoolean(JObject json, string keyName, bool fallback)
		{
			if (json == null)
			{
				return fallback;
			}
			JToken token = json.GetValue(keyName, StringComparison.OrdinalIgnoreCase);
			if (token == null)
			{
				return fallback;
			}
			try
			{
				return token.Value<bool>();
			}
			catch
			{
				return fallback;
			}
		}

	}
}
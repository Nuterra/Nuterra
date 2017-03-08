using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public sealed class KeyConfig
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode DrillKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode HammerKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode MagnetToggleKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode ScoopKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode PlasmaKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode SawKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode TurnNightKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode TurnDayKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode SuicideKey;

		[JsonConverter(typeof(StringEnumConverter))]
		public KeyCode ProductionToggleKey;
	}
}
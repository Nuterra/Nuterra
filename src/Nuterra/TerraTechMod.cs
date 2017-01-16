using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Nuterra
{
	public abstract class TerraTechMod
	{
		public abstract string Name { get; }//TODO: Create struct for valid string checking
		public abstract string Description { get; }
		public virtual Version Version => new Version(0, 0, 0);
		public JObject Configuration { get; private set; }

		public virtual void Load()
		{
			Console.WriteLine("Loading mod: {Name} ({Version})");
			Configuration = ModConfig.Data[Name] as JObject;
		}

		public virtual void Unload()
		{

		}
	}
}
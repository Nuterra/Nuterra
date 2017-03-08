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
		public JObject Config { get; private set; }
#warning TODO: Improve config model

		public virtual void Load()
		{
			string name = Name;
			if (name == null) throw new InvalidOperationException("TerraTechMod.Name returned null");
			Config = NuterraApi.Configuration.LoadModConfig(this);
		}

		public virtual void Unload()
		{

		}

		public virtual JObject CreateDefaultConfiguration()
		{
			return new JObject();
		}
	}
}
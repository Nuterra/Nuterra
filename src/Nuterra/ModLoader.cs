using System;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Nuterra
{
	public sealed class ModLoader
	{
		public static ModLoader Instance { get; } = new ModLoader();

		private Dictionary<Type, TerraTechMod> _modInstances = new Dictionary<Type, TerraTechMod>();
		private Dictionary<string, Type> _modNames = new Dictionary<string, Type>();

		public void LoadAllMods(Assembly asm)
		{
			foreach (Type modClass in asm.GetExportedTypes().Where(IsModClass))
			{
				LoadMod(modClass);
			}
		}

		private bool IsModClass(Type modType)
		{
			if (modType == null) return false;
			if (!typeof(TerraTechMod).IsAssignableFrom(modType)) return false;
			if (!modType.IsClass) return false;
			if (modType.IsAbstract) return false;

			//Now start displaying warnings for missing requirements
			ConstructorInfo ctor = modType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				Console.WriteLine($"The mod '{modType.Name}' does not have a parameterless constructor and will not be launched");
				return false;
			}
			return true;
		}

		private void LoadMod(Type modClass)
		{
			Console.WriteLine($"Loading mod '{modClass.Name}'");
			TerraTechMod mod = EnsureModInstance(modClass);
			mod.Load();
		}

		private TerraTechMod EnsureModInstance(Type modClass)
		{
			TerraTechMod mod;
			if (!_modInstances.TryGetValue(modClass, out mod))
			{
				mod = (TerraTechMod)Activator.CreateInstance(modClass);
				_modInstances.Add(modClass, mod);
			}
			return mod;
		}

		public TerraTechMod GetMod(string name)
		{
			Type modType = _modNames[name];
			return _modInstances[modType];
		}

		public T GetMod<T>() where T : TerraTechMod
		{
			return (T)_modInstances[typeof(T)];
		}
	}
}
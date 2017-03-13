using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nuterra
{
	public sealed class ModLoader
	{
		public static ModLoader Instance { get; } = new ModLoader();

		private Dictionary<Type, TerraTechMod> _modInstances = new Dictionary<Type, TerraTechMod>();
		private Dictionary<string, Type> _modNames = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		public void LoadAllMods(string directory)
		{
			Console.WriteLine($"Loading debug symbols for mods is {(Debug.isDebugBuild ? "enabled" : "disabled")}");
			foreach (string assemblyFileName in Directory.GetFiles(directory, "*.dll"))
			{
				Console.WriteLine($"Loading mod assembly: {assemblyFileName}");
				string path = Path.Combine(directory, assemblyFileName);
				byte[] assemblyBytes = File.ReadAllBytes(path);
				string symbolFile = path + ".mdb";
				byte[] symbolStore = null;
				if (Debug.isDebugBuild && File.Exists(symbolFile))
				{
					symbolStore = File.ReadAllBytes(symbolFile);
				}
				Assembly asm = Assembly.Load(assemblyBytes, symbolStore);
				LoadAllMods(asm);
			}
		}

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
			TerraTechMod mod = EnsureModInstance(modClass);
			Console.WriteLine($"Loading mod: {mod.Name} (v{mod.Version})");
			mod.Load();
		}

		private TerraTechMod EnsureModInstance(Type modClass)
		{
			TerraTechMod mod;
			if (!_modInstances.TryGetValue(modClass, out mod))
			{
				Console.WriteLine($"Creating mod instance '{modClass.Name}'");
				mod = (TerraTechMod)Activator.CreateInstance(modClass);
				_modInstances.Add(modClass, mod);
				_modNames.Add(mod.Name, modClass);
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
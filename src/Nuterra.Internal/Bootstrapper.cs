using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Nuterra.Internal
{
	internal sealed class Bootstrapper
	{
		public static void Start()
		{
			string managedDir = Path.Combine(Application.dataPath, "Managed");
			string assemblyFile = Path.Combine(managedDir, "Nuterra.dll");
			if (!File.Exists(assemblyFile))
			{
				Console.WriteLine($"Nuterra bootstrapper: Missing Nuterra assembly at {assemblyFile}");
				return;
			}
			byte[] assemblyBytes = File.ReadAllBytes(assemblyFile);

			string symbolFile = assemblyFile + ".mdb";
			byte[] symbolBytes = null;
			if (Debug.isDebugBuild && File.Exists(symbolFile))
			{
				symbolBytes = File.ReadAllBytes(symbolFile);
			}
			Assembly nuterra = Assembly.Load(assemblyBytes, symbolBytes);
			Console.WriteLine("Nuterra assembly loaded");
			Type nuterraMain = nuterra.GetType("Nuterra.NuterraApi");
			MethodInfo start = nuterraMain.GetMethod("Start", BindingFlags.Static | BindingFlags.NonPublic);
			start.Invoke(null, new object[] { });
		}
	}
}
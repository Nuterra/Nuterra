using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nuterra.Internal
{
	internal sealed class Bootstrapper
	{
		public static string TerraTechRootDir { get; } = Directory.GetCurrentDirectory();
		public static string NuterraDataDir { get; } = Path.Combine(TerraTechRootDir, "Nuterra_Data");

		public static void Start()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Assembly nuterra = LoadAssemblyFromModsFolder("Nuterra.dll");
			if (nuterra == null) return;
			Console.WriteLine("Nuterra assembly loaded");
			Type nuterraMain = nuterra.GetType("Nuterra.NuterraApi");
			MethodInfo start = nuterraMain.GetMethod("Start", BindingFlags.Static | BindingFlags.NonPublic);
			start.Invoke(null, new object[] { });

			var test = new MousePointer();
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Got one boss: " + e.ExceptionObject);
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			//Example args.Name: "Nuterra.Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
			string name = args.Name.Split(new char[] { ',' }, 2).First();
			return LoadAssemblyFromModsFolder(name + ".dll");
		}

		private static Assembly LoadAssemblyFromModsFolder(string filename)
		{
			string assemblyFile = Path.Combine(FolderStructure.ModsFolder, filename);
			if (!File.Exists(assemblyFile))
			{
				Console.WriteLine($"Nuterra bootstrapper: Missing Nuterra assembly at {assemblyFile}");
				return null;
			}
			byte[] assemblyBytes = File.ReadAllBytes(assemblyFile);

			string symbolFile = assemblyFile + ".mdb";
			byte[] symbolBytes = null;
			if (Debug.isDebugBuild && File.Exists(symbolFile))
			{
				symbolBytes = File.ReadAllBytes(symbolFile);
			}
			Assembly nuterra = Assembly.Load(assemblyBytes, symbolBytes);
			return nuterra;
		}
	}
}
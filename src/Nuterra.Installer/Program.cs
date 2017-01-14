using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace Nuterra.Installer
{
	public static class Program
	{
		public static readonly string ManagedDir = @"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\Managed";
		public static readonly string AssemblyFile = Path.Combine(ManagedDir, "Assembly-CSharp.dll");
		public static readonly string AccessFile = "access.txt";
		public static readonly string OutputFile = Path.Combine(ManagedDir, "Assembly-CSharp-access.dll");

		internal static void Main(string[] args)
		{
			AssemblyResolver resolver = new AssemblyResolver();
			resolver.PreSearchPaths.Add(ManagedDir);
			ModuleContext context = new ModuleContext();

			ModuleDefMD assembly;
			using (FileStream fs = File.OpenRead(AssemblyFile))
			{
				assembly = ModuleDefMD.Load(fs, context);
			}

			using (FileStream fs = File.OpenRead(AccessFile))
			using (StreamReader reader = new StreamReader(fs))
			{
				while (!reader.EndOfStream)
				{
					ParseLine(assembly, reader.ReadLine());
				}
			}

			ModuleWriterOptions writerOptions = new ModuleWriterOptions(assembly);
			writerOptions.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;
			using (FileStream fs = File.OpenWrite(OutputFile))
			{
				assembly.Write(fs, writerOptions);
			}
		}

		private static void ParseLine(ModuleDefMD assembly, string v)
		{
			string[] parts = v.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

			string subject = parts[0];
			var targetType = assembly.Find(parts[1], isReflectionName: false);
			switch (subject)
			{
				case "field":
					MakeFieldPublic(targetType, parts[2]);
					break;

				case "type":
					MakeTypePublic(targetType);
					break;

				case "method":
					MakeMethodPublic(targetType, parts[2]);
					break;
			}
		}

		private static void MakeFieldPublic(TypeDef type, string fieldName)
		{
			FieldDef field = type.Fields.First(f => f.Name == fieldName);
			field.Attributes &= ~FieldAttributes.FieldAccessMask;
			field.Attributes |= FieldAttributes.Public;
		}

		private static void MakeTypePublic(TypeDef type)
		{
			type.Attributes &= ~TypeAttributes.VisibilityMask;
			if (type.IsNested)
			{
				type.Attributes |= TypeAttributes.NestedPublic;
			}
			else
			{
				type.Attributes |= TypeAttributes.Public;
			}
		}

		private static void MakeMethodPublic(TypeDef targetType, string methodName)
		{
			foreach (MethodDef method in targetType.Methods.Where(m => m.Name == methodName))
			{
				method.Attributes &= ~MethodAttributes.MemberAccessMask;
				method.Attributes |= MethodAttributes.Public;
			}
		}
	}
}
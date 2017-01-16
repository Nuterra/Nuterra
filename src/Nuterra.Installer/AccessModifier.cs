using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace Nuterra.Installer
{
	public static class AccessModifier
	{
		internal static void Main()
		{
			string managedDir = @"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\Managed";
			string assemblyPath = Path.Combine(managedDir, "Assembly-CSharp.dll");
			string accessFile = "access.txt";
			string outputFile = Path.Combine(managedDir, "Assembly-CSharp-access.dll");
			Apply(managedDir, assemblyPath, accessFile, outputFile);
		}

		public static void Apply(string managedDir, string inputAssemblyFile, string accessFile, string outputAssemblyFile)
		{
			AssemblyResolver resolver = new AssemblyResolver();
			resolver.PreSearchPaths.Add(managedDir);
			ModuleContext context = new ModuleContext();
			ModuleDefMD assembly = ModuleDefMD.Load(inputAssemblyFile, context);

			Apply(assembly, accessFile);

			ModuleWriterOptions writerOptions = new ModuleWriterOptions();
			writerOptions.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;
			assembly.Write(outputAssemblyFile, writerOptions);
		}

		public static void Apply(ModuleDefMD assembly, string accessFile)
		{
			using (FileStream fs = File.OpenRead(accessFile))
			using (StreamReader reader = new StreamReader(fs))
			{
				while (!reader.EndOfStream)
				{
					ParseLine(assembly, reader.ReadLine());
				}
			}
		}

		private static void ParseLine(ModuleDefMD assembly, string line)
		{
			string[] parts = line.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

			string subject = parts[0];
			var targetType = assembly.Find(parts[1], isReflectionName: false);
			switch (subject)
			{
				case "field":
					MakeFieldInternal(targetType, parts[2]);
					break;

				case "type":
					MakeTypePublic(targetType);
					break;

				case "method":
					MakeMethodInternal(targetType, parts[2]);
					break;
			}
		}

		private static void MakeFieldInternal(TypeDef type, string fieldName)
		{
			FieldDef field = type.Fields.First(f => f.Name == fieldName);
			bool isFamily = field.IsFamily;
			field.Attributes &= ~FieldAttributes.FieldAccessMask;
			if (isFamily)
			{
				field.Attributes |= FieldAttributes.FamORAssem;
			}
			else
			{
				field.Attributes |= FieldAttributes.Assembly;
			}
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

		private static void MakeMethodInternal(TypeDef targetType, string methodName)
		{
			foreach (MethodDef method in targetType.Methods.Where(m => m.Name == methodName))
			{
				bool isFamily = method.IsFamily;
				method.Attributes &= ~MethodAttributes.MemberAccessMask;
				if (isFamily)
				{
					method.Attributes |= MethodAttributes.FamORAssem;
				}
				else
				{
					method.Attributes |= MethodAttributes.Assembly;
				}
			}
		}
	}
}
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
			ModuleDefMD module = ModuleDefMD.Load(inputAssemblyFile, context);

			Apply(module, accessFile);

			ModuleWriterOptions writerOptions = new ModuleWriterOptions();
			writerOptions.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;
			module.Write(outputAssemblyFile, writerOptions);
		}

		public static void Apply(ModuleDefMD module, string accessFile)
		{
			using (FileStream fs = File.OpenRead(accessFile))
			using (StreamReader reader = new StreamReader(fs))
			{
				while (!reader.EndOfStream)
				{
					ParseLine(module, reader.ReadLine());
				}
			}
			MakeInternalsVisibleToAssembly(module, "Nuterra");
		}

		private static void MakeInternalsVisibleToAssembly(ModuleDefMD module, string assemblyName)
		{
			//Get type and method references / signatures
			AssemblyRef mscorlib = module.CorLibTypes.AssemblyRef;
			TypeRefUser internalsVisibleToAttributeType = new TypeRefUser(module, new UTF8String("System.Runtime.CompilerServices"), new UTF8String("InternalsVisibleToAttribute"), mscorlib);
			TypeSig objectSig = internalsVisibleToAttributeType.ToTypeSig();
			MethodSig ctor = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String);
			MemberRefUser op_EqualityMethod = new MemberRefUser(module, new UTF8String(".ctor"), ctor, internalsVisibleToAttributeType);

			//Create custom attribute declaration
			CAArgument arg = new CAArgument(module.CorLibTypes.String, assemblyName);
			CustomAttribute attr = new CustomAttribute(op_EqualityMethod, new CAArgument[] { arg });

			//Insert into assembly definition
			module.Assembly.CustomAttributes.Add(attr);

		}

		private static void ParseLine(ModuleDefMD module, string line)
		{
			string[] parts = line.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

			string subject = parts[0];
			var targetType = module.Find(parts[1], isReflectionName: false);
			switch (subject)
			{
				case "field":
					MakeFieldVisible(targetType, parts[2]);
					break;

				case "type":
					MakeTypeVisible(targetType);
					break;

				case "method":
					MakeMethodVisible(targetType, parts[2]);
					break;
			}
		}

		private static void MakeFieldVisible(TypeDef type, string fieldName)
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

		private static void MakeTypeVisible(TypeDef type)
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

		private static void MakeMethodVisible(TypeDef targetType, string methodName)
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
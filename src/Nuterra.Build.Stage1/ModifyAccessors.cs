using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nuterra.Build
{
	public sealed class ModifyAccessors : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			Apply(info.AssemblyCSharp, info.AccessFilePath);
		}

		private static void Apply(ModuleDefMD module, string accessFile)
		{
			using (FileStream fs = File.OpenRead(accessFile))
			using (StreamReader reader = new StreamReader(fs))
			{
				while (!reader.EndOfStream)
				{
					ParseLine(module, reader.ReadLine());
				}
			}
			MakeInternalsVisibleToAssembly(module, "Nuterra.Internal");
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
			line = line?.Trim();
			if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
			{
				return;
			}

			Console.WriteLine($"Applying: {line}");
			string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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

				case "method+virtual":
					MakeMethodVirtual(targetType, parts[2]);
					break;

				//wrap_field ManSplashScreen m_SplashScreens SplashScreens read+write
				case "wrap_field":
					WrapField(targetType, parts[2], parts[3], parts[4]);
					break;
			}
		}

		private static void WrapField(TypeDef type, string fieldName, string propertyName, string access)
		{
			FieldDef field = type.Fields.First(f => f.Name == fieldName);
			var propertySignature = PropertySig.CreateInstance(field.FieldType);
			var property = new PropertyDefUser(propertyName, propertySignature);
			type.Properties.Add(property);
			bool propertyValid = false;
			if (access.Contains("read"))
			{
				var getterSignature = MethodSig.CreateInstance(field.FieldType);
				MethodDefUser getter = new MethodDefUser("nuterra_get_" + propertyName, getterSignature, MethodAttributes.Public | MethodAttributes.HideBySig);
				getter.DeclaringType = type;
				getter.Body = new CilBody();
				getter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
				getter.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(field));
				getter.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
				property.GetMethod = getter;
				propertyValid = true;
			}
			if (access.Contains("write"))
			{
				var setterSignature = MethodSig.CreateInstance(type.Module.CorLibTypes.Void, field.FieldType);
				MethodDefUser setter = new MethodDefUser("nuterra_set_" + propertyName, setterSignature, MethodAttributes.Public | MethodAttributes.HideBySig);
				setter.DeclaringType = type;
				setter.Body = new CilBody();
				setter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
				setter.Body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
				setter.Body.Instructions.Add(OpCodes.Stfld.ToInstruction(field));
				setter.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
				property.SetMethod = setter;
				propertyValid = true;
			}
			if (!propertyValid) throw new ArgumentException("wrap_field must define read and/or write accessor", nameof(access));
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

		private static void MakeMethodVirtual(TypeDef targetType, string methodName)
		{
			foreach (MethodDef method in targetType.Methods.Where(m => m.Name == methodName))
			{
				method.IsVirtual = true;
			}
		}
	}
}
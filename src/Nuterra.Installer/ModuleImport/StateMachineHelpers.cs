using dnlib.DotNet;

namespace Nuterra.Installer.ModuleImport
{
	static class StateMachineHelpers
	{
		public static TypeDef GetStateMachineType(MethodDef method)
		{
			var stateMachineType = GetStateMachineTypeCore(method);
			if (stateMachineType == null)
				return null;
			var body = method.Body;
			if (body == null)
				return null;

			foreach (var instr in body.Instructions)
			{
				var def = instr.Operand as IMemberDef;
				if (def?.DeclaringType == stateMachineType)
					return stateMachineType;
			}

			return null;
		}

		static TypeDef GetStateMachineTypeCore(MethodDef method)
		{
			foreach (var ca in method.CustomAttributes)
			{
				if (ca.ConstructorArguments.Count != 1)
					continue;
				if (ca.Constructor?.MethodSig?.Params.Count != 1)
					continue;
				var typeType = (ca.Constructor.MethodSig.Params[0] as ClassOrValueTypeSig)?.TypeDefOrRef;
				if (typeType == null || typeType.FullName != "System.Type")
					continue;
				if (!IsStateMachineTypeAttribute(ca.AttributeType))
					continue;
				var caArg = ca.ConstructorArguments[0];
				var tdr = (caArg.Value as ClassOrValueTypeSig)?.TypeDefOrRef;
				if (tdr == null)
					continue;
				var td = tdr.Module.Find(tdr);
				if (td?.DeclaringType == method.DeclaringType)
					return td;
			}
			return null;
		}

		static bool IsStateMachineTypeAttribute(ITypeDefOrRef tdr)
		{
			var s = tdr.ReflectionFullName;
			return s == "System.Runtime.CompilerServices.AsyncStateMachineAttribute" ||
					s == "System.Runtime.CompilerServices.IteratorStateMachineAttribute";
		}
	}
}
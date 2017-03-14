using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nuterra.Installer.Hooking
{
	internal static class ReflectionExtensions
	{
		internal static string GetCecilFullName(this Type type)
		{
			return type.FullName.Replace('+', '/');
		}

		internal static TypeDef GetNuterraType(this ModuleDefMD moduleMD, Type type)
		{
			return moduleMD.Find(type.FullName, isReflectionName: true);
		}

		internal static TypeDef GetTerraTechType(this ModuleDefMD moduleMD, Type type)
		{
			return moduleMD.Find(type.FullName, isReflectionName: true);
		}

		internal static void Replace(this IList<Instruction> body, int index, Instruction replacement)
		{
			Instruction original = body[index];
			body[index] = replacement;
			foreach (Instruction instr in body)
			{
				if (instr.Operand == original)
				{
					instr.Operand = replacement;
				}
			}
		}
	}
}
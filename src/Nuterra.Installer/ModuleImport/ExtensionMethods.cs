using System;
using System.Collections.Generic;

namespace Nuterra.Installer.ModuleImport
{
	internal static class ExtensionMethods
	{
		public static void AddRange<T>(this ICollection<T> coll, IEnumerable<T> data)
		{
			foreach (var d in data)
				coll.Add(d);
		}
	}
}
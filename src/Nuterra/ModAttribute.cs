using System;

namespace Nuterra
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class ModAttribute : Attribute
	{
	}
}
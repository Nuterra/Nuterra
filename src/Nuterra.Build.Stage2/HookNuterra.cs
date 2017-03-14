using System;
using Nuterra.Build;

namespace Nuterra.Installer
{
	public sealed class HookNuterra : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			Hooking.Hooker.Apply(info.AssemblyCSharp);
		}
	}
}
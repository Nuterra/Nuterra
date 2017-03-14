using System;

namespace Nuterra.Build
{
	public sealed class HookNuterra : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			Hooking.Hooker.Apply(info.AssemblyCSharp);
		}
	}
}
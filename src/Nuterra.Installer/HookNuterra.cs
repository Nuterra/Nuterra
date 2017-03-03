using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

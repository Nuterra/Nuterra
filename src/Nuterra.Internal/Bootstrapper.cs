using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nuterra.Internal
{
	internal sealed class Bootstrapper
	{
		public static void Startup()
		{
			Assembly nuterra = AppDomain.CurrentDomain.Load("Nuterra");
			nuterra.GetType("Nuterra.ModLoader.Load()");
		}
	}
}

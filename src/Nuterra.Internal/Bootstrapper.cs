using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nuterra.Internal
{
	internal sealed class Bootstrapper
	{
		public static void Start()
		{
			Console.WriteLine("Bootstrapper.HeyMayoverse");
			Assembly nuterra = AppDomain.CurrentDomain.Load("Nuterra");
			Type nuterraMain = nuterra.GetType("Nuterra.NuterraMain");
			MethodInfo start = nuterraMain.GetMethod("Start", BindingFlags.Static | BindingFlags.NonPublic);
			start.Invoke(null, new object[] { });
		}
	}
}

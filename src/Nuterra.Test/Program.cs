using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace Nuterra.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			ModuleDefMD module = ModuleDefMD.Load("Test.dll");

			var types = module.Types;
			var terraClass = types[2];
			var method = terraClass.Methods[0];

			var body = method.Body;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuterra.Installer
{
	public struct RedirectSettings
	{
		public bool PassArguments { get; set; }
		public string SourceMethod { get; set; }
		public string TargetMethod { get; set; }
		public bool ReplaceBody { get; set; }
		public int InsertionStart { get; set; }
		public bool AppendToEnd { get; set; }

		public RedirectSettings(string methodName)
		{
			if (methodName == null) throw new ArgumentNullException();
			SourceMethod = methodName;
			TargetMethod = methodName;
			PassArguments = true;
			ReplaceBody = false;
			InsertionStart = 0;
			AppendToEnd = false;
		}
	}
}

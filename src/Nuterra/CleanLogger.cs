using System;
using UnityEngine;

namespace Nuterra
{
	internal sealed class CleanLogger : ILogHandler
	{
		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
		
		public void LogException(Exception ex, UnityEngine.Object context)
		{
			Console.WriteLine("An exception occurred: ");
			while(ex != null)
			{
				Console.WriteLine(ex.ToString());
				ex = ex.InnerException;
			}
		}
		
		public static void Install()
		{
			Debug.logger.logHandler = new CleanLogger();
		}
	}
}
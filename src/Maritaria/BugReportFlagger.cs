using System;
using UnityEngine;

namespace Maritaria
{
	//TODO: Upgrade to nuterra
	public static class BugReportFlagger
	{
		//Hook at UIScreenBugReport.PostIt() and pass WWWForm to this method
		public static void MarkReportForm(WWWForm form)
		{
			form.AddField("mods", "maritaria");
		}

		public static string MarkUserMessage(string userMessage)
		{
			return $"Maritaria.BugReportFlagger({Nuterra.NuterraApi.CurrentVersion})\n\n{userMessage}";
		}
	}
}
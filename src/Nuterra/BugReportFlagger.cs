using System;
using Nuterra.Internal;
using UnityEngine;

namespace Nuterra
{
	internal static class BugReportFlagger
	{
		//Hook at UIScreenBugReport.PostIt() and pass WWWForm to this method
		public static void Init()
		{
			Hooks.BugReports.AlterUserMessage += MarkUserMessage;
			Hooks.BugReports.PreBugReportSubmit += MarkReportForm;
		}

		public static void MarkReportForm(WWWForm form)
		{
			form.AddField("mods", "maritaria");
		}

		public static void MarkUserMessage(BugReportMessageEvent eventInfo)
		{
			eventInfo.UserMessage += $"Maritaria.BugReportFlagger({NuterraApi.CurrentVersion})\n\n{eventInfo.UserMessage}";
		}
	}
}
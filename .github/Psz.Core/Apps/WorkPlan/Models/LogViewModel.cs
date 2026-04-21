using System;

namespace Psz.Core.Apps.WorkPlan.Models.Log
{
	public class LogViewModel
	{
		public string Action { get; set; }
		public DateTime Date { get; set; }
		public LogViewModel() { }
		public LogViewModel(Infrastructure.Data.Entities.Tables.WPL.LogEntity logDb)
		{
			this.Action = logDb.Action;
			this.Date = logDb.CreationTime;
		}
	}
}

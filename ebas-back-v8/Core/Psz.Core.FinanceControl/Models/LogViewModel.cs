using System;

namespace Psz.Core.FinanceControl.Models.Log
{
	public class LogViewModel
	{
		public string Action { get; set; }
		public DateTime Date { get; set; }
		public LogViewModel() { }
		public LogViewModel(Infrastructure.Data.Entities.Tables.FNC.Log_BudgetEntity logDb)
		{
			this.Action = logDb.Action;
			this.Date = logDb.Creation_Date;
		}
	}
}

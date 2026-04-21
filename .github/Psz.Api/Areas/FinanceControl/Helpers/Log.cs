using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.FinanceControl.Helpers
{
	public static class Log
	{
		public static List<Psz.Core.FinanceControl.Models.Log.LogViewModel> GetLog(Psz.Core.Apps.Budget.Enums.LogEnums.LogType type)
		{
			var logDb = Infrastructure.Data.Access.Tables.FNC.Log_BudgetAccess.GetByType((int)type);
			var logVm = new List<Psz.Core.FinanceControl.Models.Log.LogViewModel>();
			logDb.ForEach(l => logVm.Add(new Psz.Core.FinanceControl.Models.Log.LogViewModel(l)));

			return logVm.OrderByDescending(l => l.Date).ToList();
		}

		public static void NewLog(Psz.Core.Apps.Budget.Enums.LogEnums.LogType type, string action, int userId)
		{
			try
			{
				var logDb = new Infrastructure.Data.Entities.Tables.FNC.Log_BudgetEntity()
				{
					Action = action,
					Creation_Date = DateTime.Now,
					Type = (int)type,
					Creation_User_Id = userId



				};

				Infrastructure.Data.Access.Tables.FNC.Log_BudgetAccess.Insert(logDb);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}

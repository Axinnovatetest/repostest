using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.WorkPlan.Helpers
{
	public static class Log
	{
		public static List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel> GetLog(Psz.Core.Apps.WorkPlan.Enums.LogEnums.LogType type)
		{
			var logDb = Infrastructure.Data.Access.Tables.WPL.LogAccess.GetByType((int)type);
			var logVm = new List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel>();
			logDb.ForEach(l => logVm.Add(new Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel(l)));

			return logVm.OrderByDescending(l => l.Date).ToList();
		}

		public static void NewLog(Psz.Core.Apps.WorkPlan.Enums.LogEnums.LogType type, string action, int userId)
		{
			try
			{
				var logDb = new Infrastructure.Data.Entities.Tables.WPL.LogEntity()
				{
					Action = action,
					CreationTime = DateTime.Now,
					Type = (int)type,
					CreationUserId = userId
				};

				Infrastructure.Data.Access.Tables.WPL.LogAccess.Insert(logDb);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.CronJobs
{
	public class Jobs
	{
		public static void UpdateEKForecast()
		{
			var results = new Handlers.Article.Statistics.Sales.GetArticleROHNeed_SyncHandler(new Identity.Models.UserModel
			{
				Id = -2,
				Name = "AdminIT",
				SuperAdministrator = true
			}).Handle();
			Infrastructure.Services.Logging.Logger.LogInfo($"UpdateEKForecast >> {results.Success}");
		}
	}
}

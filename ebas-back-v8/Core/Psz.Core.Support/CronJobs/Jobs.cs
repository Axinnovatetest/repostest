using Hangfire;

namespace Psz.Core.Support.CronJobs
{
	public class Jobs
	{
		[DisableConcurrentExecution(60 * 10)]
		public static async Task SetApisCallsCount()
		{
			try
			{
				await Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.Delete();
				await Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.InsertAsync();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static async Task SetLogsForToday()
		{
			try
			{
				await Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_TodayAccess.DeleteAsync();

				await Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_TodayAccess.InsertAsync();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
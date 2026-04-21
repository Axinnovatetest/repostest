using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using System;
using System.Diagnostics;
using System.Linq;

public class NoMissedRunsAttribute: JobFilterAttribute, IClientFilter
{
	public int MaxDelayMs { get; set; } = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
	public void OnCreating(CreatingContext filterContext)
	{
		if(filterContext.Parameters.TryGetValue("RecurringJobId", out var recurringJobId))
		{
			// the job being created looks like a recurring job instance.

			var recurringJob = filterContext.Connection.GetAllEntriesFromHash($"recurring-job:{recurringJobId}");

			if(recurringJob != null && recurringJob.TryGetValue("NextExecution", out var nextExecution))
			{
				var utcNow = DateTime.UtcNow;

				// the next execution time of a recurring job is updated AFTER the job instance creation,
				// so at the moment it still contains the scheduled execution time from the previous run.
				var scheduledTime = JobHelper.DeserializeDateTime(nextExecution);

				// Check if the job is created way later than expected
				// and if it was created from the scheduler.
				if(utcNow > scheduledTime.AddMilliseconds(MaxDelayMs) && IsCreatedFromRecurringJobScheduler())
				{
					filterContext.Canceled = true;
				}
			}
		}
	}
	private static bool IsCreatedFromRecurringJobScheduler()
	{
		// Get call stack
		var stackTrace = new StackTrace();
		return stackTrace.GetFrames().Any(f => f.GetMethod()?.DeclaringType == typeof(RecurringJobScheduler));
	}
	public void OnCreated(CreatedContext filterContext)
	{
		// Nothing to do.
	}
}
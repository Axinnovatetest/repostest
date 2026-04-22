using System;
using System.Configuration;

namespace Psz.Core.CustomerService.Helpers
{
	public class CronInfo
	{
		public string CronExpression { get; set; }
		public CronInfo(string cronExpression)
		{
			CronExpression = cronExpression;
		}

		public static CronInfo ConvertFromConfig(string jobName)
		{
			try
			{
				if(TimeSpan.TryParse(ConfigurationManager.AppSettings[$"{jobName}.PeriodRecurrence"], out var periodRecurrence))
					return ConvertFromPeriodRecurrence(periodRecurrence);

				if(TimeSpan.TryParse(ConfigurationManager.AppSettings[$"{jobName}.DailyRecurrence"], out var dailyRecurrence))
					return ConvertFromPeriodRecurrence(dailyRecurrence);

				string cronExpression = ConfigurationManager.AppSettings[$"{jobName}.CronExpression"];

				if(!string.IsNullOrEmpty(cronExpression))
					return ConvertFromCronExpression(cronExpression);
			} catch(Exception)
			{
				Console.WriteLine($"The job with name {jobName} don't have recurence specified in the config.");
			}

			return null;
		}

		public static CronInfo ConvertFromPeriodRecurrence(TimeSpan periodRecurrence)
		{
			if(periodRecurrence.Hours >= 1)
			{
				if(periodRecurrence.Seconds != 0)
					throw new ArgumentException("Seconds not allowed when hours are specified.");

				return new CronInfo($"{periodRecurrence.Minutes} */{periodRecurrence.Hours} * * *");
			}

			if(periodRecurrence.Minutes > 1)
			{
				if(periodRecurrence.Seconds != 0)
					throw new ArgumentException("Seconds not allowed when minutes are specified.");

				return new CronInfo($"*/{periodRecurrence.Minutes} * * * *");
			}

			return new CronInfo($"*/1 * * * *");
		}

		public static CronInfo ConvertFromDailyRecurrence(TimeSpan dailyRecurrence)
		{
			if(dailyRecurrence.Seconds != 0)
				throw new ArgumentException("Seconds not allowed for daily recurrence.");

			return new CronInfo($"{dailyRecurrence.Minutes} {dailyRecurrence.Hours} * * *");
		}

		public static CronInfo ConvertFromCronExpression(string cronExpression)
		{
			return new CronInfo(cronExpression);
		}

		public static CronInfo ConvertToTimeSpan(string cronExpression)
		{
			var split = cronExpression.Split(" ");
			var time = new TimeSpan(Convert.ToInt32(split[1]), Convert.ToInt32(split[0]), 0);
			return new CronInfo(time.ToString(@"hh\:mm"));
		}
	}
}

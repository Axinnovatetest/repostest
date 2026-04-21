using System;
using System.Globalization;

namespace Psz.Core.Helpers
{
	public class DateTimeHelper
	{
		public static string GetDayOfWeekName(DateTime date, string culture = "fr-FR")
		{
			return date.ToString("dddd", CultureInfo.CreateSpecificCulture(culture));
		}

		public static DateTime FirstDayOfWeek(DateTime date, DayOfWeek firstDayOfWeek = System.DayOfWeek.Monday)
		{
			int diff = (7 + (date.DayOfWeek - firstDayOfWeek)) % 7;
			return date.AddDays(-1 * diff).Date;
		}
		public static DateTime LastDayOfWeek(DateTime date, DayOfWeek firstDayOfWeek = System.DayOfWeek.Monday)
		{
			return FirstDayOfWeek(date, firstDayOfWeek).AddDays(+6);
		}

		public static string TimeSince(DateTime dateTime, string language = "fr")
		{
			if(string.IsNullOrEmpty(language))
			{
				language = "fr";
			}
			language = language.ToLower();

			const int SECOND = 1;
			const int MINUTE = 60 * SECOND;
			const int HOUR = 60 * MINUTE;
			const int DAY = 24 * HOUR;
			const int MONTH = 30 * DAY;

			var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.ToUniversalTime().Ticks);
			double delta = System.Math.Abs(ts.TotalSeconds);

			if(delta < 1 * MINUTE)
			{
				if(language == "fr")
				{
					return ts.Seconds == 1 ? "Il y a un instant" : ("Il y a " + ts.Seconds + " secondes");
				}
				else
				{
					return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
				}
			}

			if(delta < 2 * MINUTE)
			{
				if(language == "fr")
				{
					return "Il y a une minute";
				}
				else
				{
					return "a minute ago";
				}
			}

			if(delta < 45 * MINUTE)
			{
				if(language == "fr")
				{
					return "Il y a " + ts.Minutes + " minutes";
				}
				else
				{
					return ts.Minutes + " minutes ago";
				}
			}

			if(delta < 90 * MINUTE)
			{
				if(language == "fr")
				{
					return "Il y a une heure";
				}
				else
				{
					return "an hour ago";
				}
			}

			if(delta < 24 * HOUR)
			{
				if(language == "fr")
				{
					return "il y a " + ts.Hours + " heures";
				}
				else
				{
					return ts.Hours + " hours ago";
				}
			}

			if(delta < 48 * HOUR)
			{
				if(language == "fr")
				{
					return "hier";
				}
				else
				{
					return "yesterday";
				}
			}

			if(delta < 30 * DAY)
			{
				if(language == "fr")
				{
					return "Il y a " + ts.Days + " jours";
				}
				else
				{
					return ts.Days + " days ago";
				}
			}

			if(delta < 12 * MONTH)
			{
				int months = Convert.ToInt32(System.Math.Floor((double)ts.Days / 30));
				if(language == "fr")
				{
					return months <= 1 ? "Il y a un mois" : ("Il y a " + months + " mois");
				}
				else
				{
					return months <= 1 ? "one month ago" : months + " months ago";
				}
			}
			else
			{
				int years = Convert.ToInt32(System.Math.Floor((double)ts.Days / 365));
				if(language == "fr")
				{
					return years <= 1 ? "Il y a un an" : "il y a " + years + " ans";
				}
				else
				{
					return years <= 1 ? "one year ago" : years + " years ago";
				}
			}
		}
	}
}

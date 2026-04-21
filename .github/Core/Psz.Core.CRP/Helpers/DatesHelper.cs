using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Helpers
{
	public class DatesHelper
	{
		public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo? ci = null)
		{
			ci = ci ?? new CultureInfo("de-DE");
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
			DateTime firstWeekDay = jan1.AddDays(daysOffset);
			int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
			if((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
			{
				weekOfYear -= 1;
			}
			return firstWeekDay.AddDays(weekOfYear * 7);
		}
	}
}
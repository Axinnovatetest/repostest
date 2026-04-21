using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.Common.Helpers
{
	public class DateHelpers
	{

		public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
		{
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			// Use first Thursday in January to get first week of the year as
			// it will never be in Week 52/53
			DateTime firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear;
			// As we're adding days to a date in Week 1,
			// we need to subtract 1 in order to get the right date for week #1
			if(firstWeek == 1)
			{
				weekNum -= 1;
			}

			// Using the first Thursday as starting week ensures that we are starting in the right year
			// then we add number of weeks multiplied with days
			var result = firstThursday.AddDays(weekNum * 7);

			// Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
			return result.AddDays(-3);
		}
		/// <summary>
		/// extract the week in a year from a given date 
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int ExtractIsoWeek(DateTime time)
		{
			//DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			//if(day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			//{
			//	time = time.AddDays(3);
			//}
			//return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return ISOWeek.GetWeekOfYear(time);
		}
		public static int GetPreviousIsoWeekNumber(DateTime date)
		{
			var previousWeekDate = date.AddDays(-7);
			var calendar = CultureInfo.InvariantCulture.Calendar;
			var weekRule = CalendarWeekRule.FirstFourDayWeek;
			var firstDayOfWeek = DayOfWeek.Monday;

			return calendar.GetWeekOfYear(previousWeekDate, weekRule, firstDayOfWeek);
		}

		public static List<int> GetWeekNumbersForYear(int year)
		{
			List<int> weekNumbers = new List<int>();
			DateTime firstDayOfYear = new DateTime(year, 1, 1);
			DateTime lastDayOfYear = new DateTime(year, 12, 31);
			DateTime currentDate = firstDayOfYear;
			while(currentDate <= lastDayOfYear)
			{
				int weekNumber = ExtractIsoWeek(currentDate);
				if(!weekNumbers.Contains(weekNumber))
				{
					weekNumbers.Add(weekNumber);
				}
				currentDate = currentDate.AddDays(1);
			}
			return weekNumbers.OrderBy(x => x).ToList();
		}

		public static DateTime getFirstDayOfWeekDate(int weekNumber, int year, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			return getDayOfWeekDate(weekNumber - 1, firstDayOfWeek, year, firstDayOfWeek);
		}
		public static DateTime getLastDayOfWeekDate(int weekNumber, int year, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			return getDayOfWeekDate(weekNumber, getLastDayOfWeek(firstDayOfWeek), year, firstDayOfWeek);
		}
		public static DateTime getDayOfWeekDate(int weekNumber, DayOfWeek dayOfWeek, int year, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			#region > input check
			if(weekNumber < 0)
			{
				weekNumber = 0;
			}
			if(weekNumber > 53)
			{
				weekNumber = 52;
			}

			if(dayOfWeek < 0)
			{
				dayOfWeek = 0;
			}

			if(year < 1980)
			{
				year = 1980;
			}
			else if(year > 2080)
			{
				year = 2080;
			}
			#endregion

			var firstDayOfYear = new DateTime(year, 1, 1);

			int daysOffset = (int)firstDayOfWeek - (int)firstDayOfYear.DayOfWeek; //getNextDayOfWeek(firstDayOfWeek) - firstDayOfYear.DayOfWeek;

			var firstFirstDay = firstDayOfYear.AddDays(daysOffset);

			int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(firstDayOfYear, CalendarWeekRule.FirstFullWeek, firstDayOfWeek);

			var weekNum = firstWeek > 1
				? weekNumber
				: (weekNumber - 1);

			return firstFirstDay.AddDays(weekNum * 7 + (int)dayOfWeek - 1);
		}
		public static DayOfWeek getNextDayOfWeek(DayOfWeek dayOfWeek)
		{
			switch(dayOfWeek)
			{
				default:
				case DayOfWeek.Sunday:
					return DayOfWeek.Monday;
				case DayOfWeek.Monday:
					return DayOfWeek.Tuesday;
				case DayOfWeek.Tuesday:
					return DayOfWeek.Wednesday;
				case DayOfWeek.Wednesday:
					return DayOfWeek.Thursday;
				case DayOfWeek.Thursday:
					return DayOfWeek.Friday;
				case DayOfWeek.Friday:
					return DayOfWeek.Saturday;
				case DayOfWeek.Saturday:
					return DayOfWeek.Sunday;
			}
		}
		public static DayOfWeek getLastDayOfWeek(DayOfWeek firstDayOfWeek)
		{
			switch(firstDayOfWeek)
			{
				default:
				case DayOfWeek.Sunday:
					return DayOfWeek.Saturday;
				case DayOfWeek.Monday:
					return DayOfWeek.Sunday;
				case DayOfWeek.Tuesday:
					return DayOfWeek.Monday;
				case DayOfWeek.Wednesday:
					return DayOfWeek.Tuesday;
				case DayOfWeek.Thursday:
					return DayOfWeek.Wednesday;
				case DayOfWeek.Friday:
					return DayOfWeek.Thursday;
				case DayOfWeek.Saturday:
					return DayOfWeek.Friday;
			}
		}

		public static DateTime getFirstDayDateFromMonth(int year, int month)
		{
			return new DateTime(year, month, 1);
		}
		public static DateTime getLastDayDateFromMonth(int year, int month)
		{
			return new DateTime(year, month, 1).AddMonths(+1).AddDays(-1);
		}

		public static DateTime getFirstDayDateFromQuarter(int year, int quarter)
		{
			var firstMonthOfQuarter = 1;
			switch(quarter)
			{
				case 2:
					firstMonthOfQuarter = 4;
					break;
				case 3:
					firstMonthOfQuarter = 7;
					break;
				case 4:
					firstMonthOfQuarter = 10;
					break;
			}

			return getFirstDayDateFromMonth(year, firstMonthOfQuarter);
		}
		public static DateTime getLastDayDateFromQuarter(int year, int quarter)
		{
			var lastMonthOfQuarter = 3;
			switch(quarter)
			{
				case 2:
					lastMonthOfQuarter = 6;
					break;
				case 3:
					lastMonthOfQuarter = 9;
					break;
				case 4:
					lastMonthOfQuarter = 12;
					break;
			}

			return getLastDayDateFromMonth(year, lastMonthOfQuarter);
		}

		public static DateTime getFirstDayDateFromYear(int year)
		{
			return getFirstDayDateFromMonth(year, 1);
		}
		public static DateTime getLastDayDateFromYear(int year)
		{
			return getLastDayDateFromMonth(year, 12);
		}

		public static DateTime GetFirstDateOfWeek(int year, int week)
		{
			DateTime jan4 = new DateTime(year, 1, 4);

			DateTime firstMonday = jan4.AddDays(-(int)jan4.DayOfWeek + (int)DayOfWeek.Monday);

			DateTime result = firstMonday.AddDays((week - 1) * 7);

			if(result.Year < year)
			{
				result = new DateTime(year, 1, 1);
			}

			return result;
		}
		public static bool IsBeforeCurrentWeek(int year, int week)
		{
			var today = DateTime.Today;
			var currentYear = today.Year;
			var currentWeek = ISOWeek.GetWeekOfYear(today);

			// If year is earlier, it's definitely before
			if(year < currentYear)
				return true;

			// If same year, compare week numbers
			if(year == currentYear && week < currentWeek)
				return true;

			return false;
		}

	}
}


using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Psz.Core.ManagementOverview.Helpers
{
	public class SpecialHelper
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
		/// <summary>
		/// return true if week1 equal to week2 otherwise return false
		/// </summary>
		/// <param name="week1"></param>
		/// <param name="week2"></param>
		/// <returns></returns>
		public static bool CompareWeekPattern(string week1, string week2)
		{
			string pattern = @"[0-9]['/'][0-9][0-9][0-9][0-9]";
			Regex rg = new Regex(pattern);
			if(!rg.IsMatch(week1) || !rg.IsMatch(week2))
				return false;
			var temp1 = week1.Split("/");
			var temp2 = week2.Split("/");
			return (int.Parse(temp1[0]) == int.Parse(temp2[0])) && (int.Parse(temp1[1]) == int.Parse(temp2[1]));
		}
		/// <summary>
		/// return true if week 1 is afer week2 otherwise it return false
		/// </summary>
		/// <param name="week1"></param>
		/// <param name="week2"></param>
		/// <returns></returns>
		public static bool CompareWeekPatternDiff(string week1, string week2)
		{
			string pattern = @"[0-9]['/'][0-9][0-9][0-9][0-9]";
			Regex rg = new Regex(pattern);
			if(!rg.IsMatch(week1) || !rg.IsMatch(week2))
				return false;
			var temp1 = week1.Split("/");
			var temp2 = week2.Split("/");

			var x = (int.Parse(temp1[1]) > int.Parse(temp2[1])) || ((int.Parse(temp1[1]) == int.Parse(temp2[1])) && (int.Parse(temp1[0]) > int.Parse(temp2[0])));
			return x;
		}


	}
}

using System;

namespace Psz.Core.ManagementOverview.Helpers
{
	public static class Config
	{
		public static decimal MinAttendance { get; set; } = 1m / 60;
		public static int DecimalPart { get; set; } = 4;
		static int CapacityEditableSpan { get; set; } = 4;

		public static int GetCapacityLastEditableWeek()
		{
			var lastEditWeekDate = DateTime.Today.AddDays(+7 * CapacityEditableSpan);
			return DateTimeHelper.GetIso8601WeekOfYear(lastEditWeekDate);
		}
		public static DateTime GetLastCapacityEditableDay()
		{
			var lastEditWeekDate = DateTime.Today.AddDays(+7 * CapacityEditableSpan);
			return DateTimeHelper.LastDateOfWeekISO8601(lastEditWeekDate.Year, DateTimeHelper.GetIso8601WeekOfYear(lastEditWeekDate));
		}
	}
}

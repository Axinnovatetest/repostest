namespace Psz.Core.MaterialManagement.CRP.Helpers
{
	public static class Config
	{
		public static decimal MinAttendance { get; set; } = 1m / 60;
		public static int DecimalPart { get; set; } = 4;
		static int CapacityEditableSpan { get; set; } = 4;

		public static bool CanEdit(int year, int weekNumber, Enums.Main.CapacityType type)
		{
			var lastEditWeekDate = DateTime.Today.AddDays(+7 * CapacityEditableSpan);
			var lastEditDay = DateTimeHelper.LastDateOfWeekISO8601(lastEditWeekDate.Year, DateTimeHelper.GetIso8601WeekOfYear(lastEditWeekDate));

			switch(type)
			{
				case Enums.Main.CapacityType.Capacity:
					return DateTimeHelper.LastDateOfWeekISO8601(year, weekNumber) <= lastEditDay;
				case Enums.Main.CapacityType.CapacityPlan:
					return DateTimeHelper.LastDateOfWeekISO8601(year, weekNumber) > lastEditDay;
				default:
					return false;
			}
		}
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

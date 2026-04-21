using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting
{
	public class DateHelpers
	{
		public static List<DateTime> getDatesRange(DateTime startDate, DateTime endDate)
		{
			List<DateTime> dates = new List<DateTime>();

			DateTime start = startDate.Date;
			DateTime end = endDate.Date;

			if(start == end)
			{
				dates.Add(start);
			}
			else if(start < end)
			{
				for(DateTime date = start; date <= end; date = date.AddDays(1))
				{
					dates.Add(date);
				}
			}

			return dates;
		}
	}
}

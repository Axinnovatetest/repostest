using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Helpers
{
	public class DatesHelper
	{
		public static (DateTime firstDate, DateTime lastDate) GetDateRange(DateTime currentDate)
		{
			// Get first date of six months prior
			DateTime sixMonthsAgo = currentDate.AddMonths(-6);
			DateTime firstDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1);

			// Get last date of current month
			DateTime firstDayOfNextMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
			DateTime lastDate = firstDayOfNextMonth.AddDays(-1);

			return (firstDate, lastDate);
		}
	}
}
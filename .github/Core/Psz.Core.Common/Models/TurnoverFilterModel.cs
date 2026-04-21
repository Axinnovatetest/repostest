using Psz.Core.Common.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.Common.Models
{
	public class FilterCriteria
	{
		public int Year { get; set; }  // Mandatory
		public List<int>? Quartals { get; set; }
		public List<KeyValuePair<string, int>>? Months { get; set; }
		public List<int>? Weeks { get; set; }
	}

	public class FillMonthsFromQuartalsSpecification: ISpecification<FilterCriteria>
	{
		public bool IsSatisfiedBy(FilterCriteria criteria)
		{
			return criteria.Quartals.Count > 0 && criteria.Months.Count == 0;
		}

		public void Apply(FilterCriteria criteria)
		{
			foreach(var quartal in criteria.Quartals)
			{
				for(int month = (quartal - 1) * 3 + 1; month <= quartal * 3; month++)
				{
					if(!criteria.Months.Select(x => x.Value).Contains(month))
					{
						criteria.Months.Add(new KeyValuePair<string, int>(new DateTime(2010, month, 1).ToString("MMMM", CultureInfo.InvariantCulture), month));
					}
				}
			}
		}
	}
	public class FillWeeksFromMonthsSpecification: ISpecification<FilterCriteria>
	{
		public bool IsSatisfiedBy(FilterCriteria criteria)
		{
			return criteria.Months.Count > 0 && criteria.Weeks.Count == 0;
		}

		public void Apply(FilterCriteria criteria)
		{
			DateTime firstDayOfYear = new DateTime(criteria.Year, 1, 1);
			foreach(KeyValuePair<string, int> month in criteria.Months)
			{
				DateTime firstDay = new DateTime(criteria.Year, month.Value, 1);
				DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

				for(DateTime date = firstDay; date <= lastDay; date = date.AddDays(1))
				{
					int weekNum = GetWeekOfYear(date);
					if(!criteria.Weeks.Contains(weekNum))
					{
						criteria.Weeks.Add(weekNum);
					}
				}
			}
		}

		private int GetWeekOfYear(DateTime date)
		{
			System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
			return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
	}


	public class FillMonthsAndQuartalsFromWeeksSpecification: ISpecification<FilterCriteria>
	{
		public bool IsSatisfiedBy(FilterCriteria criteria)
		{
			return criteria.Weeks.Count > 0 && (criteria.Months.Count == 0 || criteria.Quartals.Count == 0);
		}

		public void Apply(FilterCriteria criteria)
		{
			DateTime firstDayOfYear = new DateTime(criteria.Year, 1, 1);

			foreach(var week in criteria.Weeks)
			{
				DateTime weekStartDate = DateHelpers.GetFirstDateOfWeek(criteria.Year, week);
				int month = weekStartDate.Month;
				int quartal = (month - 1) / 3 + 1;


				if(month > 0)
				{
					criteria.Months.Add(new KeyValuePair<string, int>
						(new DateTime(criteria.Year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture), month));
				}


				if(!criteria.Quartals.Contains(quartal))
				{
					criteria.Quartals.Add(quartal);
				}
			}
		}
	}
	public class FillQuartalsFromMonthsSpecification: ISpecification<FilterCriteria>
	{
		public bool IsSatisfiedBy(FilterCriteria criteria)
		{
			return criteria.Quartals.Count == 0 && criteria.Months.Count > 0;
		}

		public void Apply(FilterCriteria criteria)
		{
			foreach(KeyValuePair<string, int> month in criteria.Months)
			{
				int quartal = (month.Value - 1) / 3 + 1;  // Convert month to quarter
				if(!criteria.Quartals.Contains(quartal))
				{
					criteria.Quartals.Add(quartal);
				}
			}
		}
	}
}

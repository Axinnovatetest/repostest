using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models
{
	public class AnalyseTimePointModel
	{
		public string DataTypeKey { get; set; } // Custom, Week, Month, Quarter, Year
		public string DataType { get; set; } // Custom, Week, Month, Quarter, Year

		public CustomModel Custom { get; set; }
		public WeekModel Week { get; set; }
		public MonthModel Month { get; set; }
		public QuarterModel Quarter { get; set; }
		public YearModel Year { get; set; }

		public class CustomModel
		{
			public DateTime From { get; set; }
			public DateTime To { get; set; }
		}

		public class QuarterModel
		{
			public int Year { get; set; }
			public List<int> Quarters { get; set; } = new List<int>();
		}

		public class MonthModel
		{
			public int Year { get; set; }
			public List<int> Months { get; set; } = new List<int>();
		}

		public class WeekModel
		{
			public int Year { get; set; }
			public List<int> Weeks { get; set; } = new List<int>();
		}

		public class YearModel
		{
			public List<int> Years { get; set; } = new List<int>();
		}
	}
}

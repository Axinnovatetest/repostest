using Psz.Core.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.Common.Models.AnalyseTimePointModel.MonthDataModel;
using static Psz.Core.Common.Models.AnalyseTimePointModel.QuarterDataModel;
using static Psz.Core.Common.Models.AnalyseTimePointModel.WeekDataModel;

namespace Psz.Core.Common.Models
{
	public class AnalyseTimePointModel
	{
		public string DataTypeKey { get; set; }
		public bool Joined { get; set; }
		public Enums.AnalyseTimePointEnums.DataTypes DataType
		{
			get
			{
				return Enums.AnalyseTimePointEnums.DataTypeFromKey(this.DataTypeKey);
			}
		}
		public CustomDataModel CustomData { get; set; }
		public WeekDataModel WeekData { get; set; }
		public MonthDataModel MonthData { get; set; }
		public QuarterDataModel QuarterData { get; set; }
		public YearDataModel YearData { get; set; }

		public CalculatedDataModel GetDates(DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			switch(this.DataType)
			{
				default:
				case Enums.AnalyseTimePointEnums.DataTypes.Custom:
					{
						var response = new CalculatedDataModel();


						if(this.CustomData.Customs.Count == 0 && this.CustomData.Years.Count == 0
							&& this.CustomData.Months.Count == 0 && this.CustomData.Weeks.Count == 0
							&& this.CustomData.Quarters.Count == 0
							)
						{
							return getWrongResponse();
						}

						if(this.CustomData.Customs != null && this.CustomData.Customs.Count > 0)
						{
							response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
							{
								From = this.CustomData.Customs.First().From.Date,
								To = this.CustomData.Customs.First().To.Date.AddDays(+1)
							});
						}
						if(this.CustomData.Years != null && this.CustomData.Years.Count > 0)
						{
							foreach(var yearData in this.CustomData.Years.OrderBy(y => y))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromYear(yearData).Date,
									To = DateHelpers.getLastDayDateFromYear(yearData).Date.AddDays(1),
									RequestUnitType = "year",
									RequestUnitData = yearData.ToString(),
									RequestYear = yearData,
								});
							}
						}
						//filter by quarter
						if(this.CustomData.Quarters != null && this.CustomData.Quarters.Any())
						{
							foreach(var quarterData in this.CustomData.Quarters.OrderBy(q => q.Year).ThenBy(q => q.Quarter))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromQuarter(quarterData.Year, quarterData.Quarter).Date,
									To = DateHelpers.getLastDayDateFromQuarter(quarterData.Year, quarterData.Quarter).Date.AddDays(1),
									RequestUnitType = "quarter",
									RequestUnitData = quarterData.Quarter.ToString(),
									RequestYear = quarterData.Year,
								});
							}
						}

						//filter by month
						if(this.CustomData.Months != null && this.CustomData.Months.Any())
						{
							foreach(var monthData in this.CustomData.Months.OrderBy(m => m.Year).ThenBy(m => m.Month))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromMonth(monthData.Year, monthData.Month).Date,
									To = DateHelpers.getLastDayDateFromMonth(monthData.Year, monthData.Month).Date.AddDays(1),
									RequestUnitType = "month",
									RequestUnitData = monthData.Month.ToString(),
									RequestYear = monthData.Year,
								});
							}
						}


						//filter by week
						if(this.CustomData.Weeks != null && this.CustomData.Weeks.Any())
						{
							foreach(var weekData in this.CustomData.Weeks.OrderBy(w => w.Year).ThenBy(w => w.Week))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayOfWeekDate(weekData.Week, weekData.Year, firstDayOfWeek).Date,
									To = DateHelpers.getLastDayOfWeekDate(weekData.Week, weekData.Year, firstDayOfWeek).Date.AddDays(1),
									RequestUnitType = "week",
									RequestUnitData = weekData.Week.ToString(),
									RequestYear = weekData.Year,
								});
							}
						}
						return response;

					}

				case Enums.AnalyseTimePointEnums.DataTypes.Week:
					{
						if(this.WeekData == null || this.WeekData.Weeks == null || this.WeekData.Weeks.Count == 0)
						{
							return getWrongResponse();
						}

						if(this.Joined)
						{
							var firstYearData = this.WeekData.Weeks.GroupBy(e => e.Year).OrderBy(e => e.Key).First();
							var lastYearData = this.WeekData.Weeks.GroupBy(e => e.Year).OrderBy(e => e.Key).Last();

							var firstYear = firstYearData.Key;
							var lastYear = lastYearData.Key;

							var firstYearFirstWeek = firstYearData.Select(e => e.Week).Min();
							var firstYearLastWeek = lastYearData.Select(e => e.Week).Max();

							return new CalculatedDataModel()
							{
								Customs = new List<CalculatedDataModel.CalculatedDataItemModel>()
								{
									new CalculatedDataModel.CalculatedDataItemModel()
									{
										From = DateHelpers.getFirstDayOfWeekDate(firstYearFirstWeek, firstYear, firstDayOfWeek).Date,
										To = DateHelpers.getLastDayOfWeekDate(firstYearLastWeek, lastYear, firstDayOfWeek).Date.AddDays(+1)
									}
								}
							};
						}
						else
						{
							var response = new CalculatedDataModel();

							foreach(var weekData in this.WeekData.Weeks
								.OrderBy(e => e.Week)
								.OrderBy(e => e.Year))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayOfWeekDate(weekData.Week, weekData.Year, firstDayOfWeek).Date,
									To = DateHelpers.getLastDayOfWeekDate(weekData.Week, weekData.Year, firstDayOfWeek).Date.AddDays(+1),
									RequestUnitType = "week",
									RequestUnitData = weekData.Week.ToString(),
									RequestYear = weekData.Year,
								});
							}

							return response;
						}
					}

				case Enums.AnalyseTimePointEnums.DataTypes.Month:
					{
						if(this.MonthData == null || this.MonthData.Months == null || this.MonthData.Months.Count == 0)
						{
							return getWrongResponse();
						}

						if(this.Joined)
						{
							var firstYearData = this.MonthData.Months.GroupBy(e => e.Year).OrderBy(e => e.Key).First();
							var lastYearData = this.MonthData.Months.GroupBy(e => e.Year).OrderBy(e => e.Key).Last();

							var firstYear = firstYearData.Key;
							var lastYear = lastYearData.Key;

							var firstYearFirstMonth = firstYearData.Select(e => e.Month).Min();
							var firstYearLastMonth = lastYearData.Select(e => e.Month).Max();

							return new CalculatedDataModel()
							{
								Customs = new List<CalculatedDataModel.CalculatedDataItemModel>()
								{
									new CalculatedDataModel.CalculatedDataItemModel()
									{
										From = DateHelpers.getFirstDayDateFromMonth(firstYear, firstYearFirstMonth).Date,
										To = DateHelpers.getLastDayDateFromMonth(lastYear, firstYearLastMonth).Date.AddDays(+1)
									}
								}
							};
						}
						else
						{
							var response = new CalculatedDataModel();

							foreach(var monthData in this.MonthData.Months
								.OrderBy(e => e.Month)
								.OrderBy(e => e.Year))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromMonth(monthData.Year, monthData.Month).Date,
									To = DateHelpers.getLastDayDateFromMonth(monthData.Year, monthData.Month).Date.AddDays(+1),
									RequestUnitType = "month",
									RequestUnitData = monthData.Month.ToString(),
									RequestYear = monthData.Year,
								});
							}

							return response;
						}
					}

				case Enums.AnalyseTimePointEnums.DataTypes.Quarter:
					{
						if(this.QuarterData == null || this.QuarterData.Quarters == null || this.QuarterData.Quarters.Count == 0)
						{
							return getWrongResponse();
						}

						if(this.Joined)
						{
							var firstYearData = this.QuarterData.Quarters.GroupBy(e => e.Year).OrderBy(e => e.Key).First();
							var lastYearData = this.QuarterData.Quarters.GroupBy(e => e.Year).OrderBy(e => e.Key).Last();

							var firstYear = firstYearData.Key;
							var lastYear = lastYearData.Key;

							var firstYearFirstQuarter = firstYearData.Select(e => e.Quarter).Min();
							var firstYearLastQuarter = lastYearData.Select(e => e.Quarter).Max();

							return new CalculatedDataModel()
							{
								Customs = new List<CalculatedDataModel.CalculatedDataItemModel>()
								{
									new CalculatedDataModel.CalculatedDataItemModel()
									{
										From = DateHelpers.getFirstDayDateFromQuarter(firstYear, firstYearFirstQuarter).Date,
										To = DateHelpers.getLastDayDateFromQuarter(lastYear, firstYearLastQuarter).Date.AddDays(+1)
									}
								}
							};
						}
						else
						{
							var response = new CalculatedDataModel();

							foreach(var quarterData in this.QuarterData.Quarters
								.OrderBy(e => e.Quarter)
								.OrderBy(e => e.Year))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromQuarter(quarterData.Year, quarterData.Quarter).Date,
									To = DateHelpers.getLastDayDateFromQuarter(quarterData.Year, quarterData.Quarter).Date.AddDays(+1),
									RequestUnitType = "quarter",
									RequestUnitData = quarterData.Quarter.ToString(),
									RequestYear = quarterData.Year,
								});
							}

							return response;
						}
					}

				case Enums.AnalyseTimePointEnums.DataTypes.Year:
					{
						if(this.YearData == null || this.YearData.Years == null || this.YearData.Years.Count == 0)
						{
							return getWrongResponse();
						}

						if(this.Joined)
						{
							var firstYear = this.YearData.Years.OrderBy(e => e).First();
							var lastYear = this.YearData.Years.OrderBy(e => e).Last();

							return new CalculatedDataModel()
							{
								Customs = new List<CalculatedDataModel.CalculatedDataItemModel>()
								{
									new CalculatedDataModel.CalculatedDataItemModel()
									{
										From = DateHelpers.getFirstDayDateFromYear(firstYear).Date,
										To = DateHelpers.getLastDayDateFromYear(lastYear).Date.AddDays(+1)
									}
								}
							};
						}
						else
						{
							var response = new CalculatedDataModel();

							foreach(var year in this.YearData.Years
								.OrderBy(e => e))
							{
								response.Customs.Add(new CalculatedDataModel.CalculatedDataItemModel()
								{
									From = DateHelpers.getFirstDayDateFromYear(year).Date,
									To = DateHelpers.getLastDayDateFromYear(year).Date.AddDays(+1),
									RequestUnitType = "year",
									RequestUnitData = year.ToString(),
									RequestYear = year,
								});
							}

							return response;
						}
					}
			}
		}

		private CalculatedDataModel getWrongResponse()
		{
			return new CalculatedDataModel()
			{ };
		}


		public class CalculatedDataModel
		{
			public List<CalculatedDataItemModel> Customs { get; set; } = new List<CalculatedDataItemModel>();

			public class CalculatedDataItemModel
			{
				public string RequestUnitType { get; set; }
				public int RequestYear { get; set; }
				public string RequestUnitData { get; set; }
				public DateTime From { get; set; }
				public DateTime To { get; set; }
			}
		}

		public class CustomDataModel
		{
			public List<QuarterModel>? Quarters { get; set; } = new List<QuarterModel>();
			public List<MonthModel>? Months { get; set; } = new List<MonthModel>();
			public List<WeekModel>? Weeks { get; set; } = new List<WeekModel>();
			public List<int>? Years { get; set; } = new List<int>();

			public List<CustomDataItem> Customs { get; set; } = new List<CustomDataItem>();

			public class CustomDataItem
			{
				public DateTime From { get; set; }
				public DateTime To { get; set; }
			}
		}

		public class QuarterDataModel
		{
			public List<QuarterModel> Quarters { get; set; } = new List<QuarterModel>();

			public class QuarterModel
			{
				public int Year { get; set; }
				public int Quarter { get; set; }
			}
		}

		public class MonthDataModel
		{
			public List<MonthModel> Months { get; set; } = new List<MonthModel>();

			public class MonthModel
			{
				public int Year { get; set; }
				public int Month { get; set; }
			}
		}

		public class WeekDataModel
		{
			public List<WeekModel> Weeks { get; set; } = new List<WeekModel>();

			public class WeekModel
			{
				public int Year { get; set; }
				public int Week { get; set; }
			}
		}

		public class YearDataModel
		{
			public List<int> Years { get; set; } = new List<int>();
		}
	}
}

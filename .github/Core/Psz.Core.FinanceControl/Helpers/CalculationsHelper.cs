using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.FinanceControl.Helpers
{
	public class CalculationsHelper
	{
		public static Dictionary<string, int> monthMapping = new Dictionary<string, int>
		{
			{ "Januar", 1 },
			{ "Februar", 2 },
			{ "März", 3 },
			{ "April", 4 },
			{ "Mai", 5 },
			{ "Juni", 6 },
			{ "Juli", 7 },
			{ "August", 8 },
			{ "September", 9 },
			{ "Oktober", 10 },
			{ "November", 11 },
			{ "Dezember", 12 }
		};
		public static string ConvertDaysToYearsMonthsDays(int totalDays)
		{
			int years = totalDays / 365;
			int remainingDays = totalDays % 365;
			int months = remainingDays / 30;
			int days = remainingDays % 30;

			string result = "";

			if(years > 0)
			{
				result += $"{years} year(s)";
			}

			if(months > 0)
			{
				if(result != "")
				{
					result += ", ";
				}
				result += $"{months} month(s)";
			}

			if(days > 0 || result == "")
			{
				if(result != "")
				{
					result += ", ";
				}
				result += $"{days} day(s)";
			}

			return result;
		}
		public static List<OrdersMonthlyViewByType> FillEmptyMonthsV1(List<OrdersMonthlyViewByType> data)
		{
			if(data != null && data.Count > 0)
			{
				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new OrdersMonthlyViewByType
						{
							Month = month,
							Count = 0,
							Type = data[0].Type,
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}
		public static List<OrdersMonthlyViewByPoPaymentType> FillEmptyMonthsV2(List<OrdersMonthlyViewByPoPaymentType> data)
		{
			if(data != null && data.Count > 0)
			{
				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new OrdersMonthlyViewByPoPaymentType
						{
							Month = month,
							Count = 0,
							PoPaymentType = data[0].PoPaymentType,
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}
		public static List<PorjectsMonthlyStats> FillEmptyMonthsV3(List<PorjectsMonthlyStats> data)
		{
			if(data != null && data.Count > 0)
			{
				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new PorjectsMonthlyStats
						{
							Month = month,
							Count = 0,
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}
		public static List<PorjectsMonthlyStatsByStatus> FillEmptyMonthsV4(List<PorjectsMonthlyStatsByStatus> data)
		{
			if(data != null && data.Count > 0)
			{
				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new PorjectsMonthlyStatsByStatus
						{
							Month = month,
							Count = 0,
							Status = data[0].Status
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}
		public static List<PorjectsMonthlyStatsByApprovalStatus> FillEmptyMonthsV5(List<PorjectsMonthlyStatsByApprovalStatus> data)
		{
			if(data != null && data.Count > 0)
			{

				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new PorjectsMonthlyStatsByApprovalStatus
						{
							Month = month,
							Count = 0,
							ApprovalStatus = data[0].ApprovalStatus
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}
		public static List<PorjectsMonthlyStatsByType> FillEmptyMonthsV6(List<PorjectsMonthlyStatsByType> data)
		{
			if(data != null && data.Count > 0)
			{

				CultureInfo germanCulture = new CultureInfo("de-DE");
				string[] monthNames = germanCulture.DateTimeFormat.MonthNames;
				var monthNamesList = monthNames.Take(12).ToList();

				var dataMonthNames = data?.Select(m => m.Month).ToList();
				foreach(var month in monthNamesList)
				{
					if(!dataMonthNames.Contains(month))
					{
						data.Add(new PorjectsMonthlyStatsByType
						{
							Month = month,
							Count = 0,
							Type = data[0].Type
						});
					}
				}
				return data.OrderBy(x => monthMapping[x.Month]).ToList();
			}

			return null;
		}

		public static List<OrdersMonthlyViewByType> GetAmountsForOrdersByType(int? year, int? companyId, int? departmentId, int? employeeId, List<Tuple<string, int, string, int>> data, string type)
		{
			var result = new List<OrdersMonthlyViewByType>();
			if(data == null || data.Count == 0)
				return new List<OrdersMonthlyViewByType>();
			data.ForEach(x =>
			{
				result.Add(new OrdersMonthlyViewByType(x, Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.SupplementForMonthlyOrders(year, companyId, departmentId, employeeId, 1, type, null, x.Item2)));
			});
			return result;
		}
		public static List<OrdersMonthlyViewByPoPaymentType> GetAmountsForOrdersByPoPayement(int? year, int? companyId, int? departmentId, int? employeeId, List<Tuple<string, int, string, int>> data, string poPayement)
		{
			var result = new List<OrdersMonthlyViewByPoPaymentType>();
			if(data == null || data.Count == 0)
				return new List<OrdersMonthlyViewByPoPaymentType>();
			data.ForEach(x =>
			{
				result.Add(new OrdersMonthlyViewByPoPaymentType(x, Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.SupplementForMonthlyOrders(year, companyId, departmentId, employeeId, 2, null, poPayement, x.Item2)));
			});
			return result;
		}
	}
}
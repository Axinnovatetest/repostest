using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<List<DateValueOrderModel>> Get_INSOverviewUmsatz_Aktuelle_WocheChart(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<DateValueOrderModel>>.AccessDeniedResponse();

			var Umsatz_Aktuelle_Woche = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Umsatz_Aktuelle_Woche() ?? new List<KeyValuePair<decimal, DateTime>> { };
			var currentWeekDates = GetDatesInIsoWeek(DateTime.Now.Year, ISOWeek.GetWeekOfYear(DateTime.Now));
			var sumGutshrift = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.GetSUMGutschrift(currentWeekDates.Select(d => d.ToString("dd/MM/yyyy"))
			.ToList());
			if(Umsatz_Aktuelle_Woche != null && Umsatz_Aktuelle_Woche.Count > 0)
			{
				foreach(var date in currentWeekDates)
				{
					var value = Umsatz_Aktuelle_Woche?.FirstOrDefault(x => x.Value.Date == date.Date);
					if(value is null || value?.Key == 0)
						Umsatz_Aktuelle_Woche.Add(new KeyValuePair<decimal, DateTime>(0, date));
				}
				Umsatz_Aktuelle_Woche = Umsatz_Aktuelle_Woche?.OrderBy(x => x.Value).ToList();
			}
			var response = Umsatz_Aktuelle_Woche?.Select(x => new DateValueOrderModel
			{
				Date = x.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
				Value = x.Key,
			}).ToList();
			ApplyCutGS(response, sumGutshrift);

			return ResponseModel<List<DateValueOrderModel>>.SuccessResponse(response);
		}
		public static List<DateTime> GetDatesInIsoWeek(int year, int isoWeek)
		{
			// Get the first day of the ISO week
			DateTime firstDay = ISOWeek.ToDateTime(year, isoWeek, DayOfWeek.Monday);

			// Create a list to store all dates in the week
			List<DateTime> datesInWeek = new List<DateTime>();

			// Add all 7 days of the week to the list
			for(int i = 0; i < 7; i++)
			{
				datesInWeek.Add(firstDay.AddDays(i));
			}

			return datesInWeek;
		}
		public static List<DateValueOrderModel> ApplyCutGS(List<DateValueOrderModel> data, List<KeyValuePair<int, DateTime>> gsValues)
		{
			var result = new List<DateValueOrderModel>();
			if(gsValues?.Count > 0)
			{
				foreach(var d in data)
				{
					var gs = gsValues.FirstOrDefault(x => x.Value.ToString("dd/MM/yyyy") == d.Date);
					if(gs.Key > 0)
					{
						d.Value = d.Value - gs.Key;
					}
				}
			}
			return result;
		}
	}
}
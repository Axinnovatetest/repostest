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
		public ResponseModel<ChartsAndOlderDatesModel> Get_INSOverviewRückständige_BestellungenChart(UserModel user, Ruckstandige_BestellungenRequestModel data)
		{
			if(user == null)
				return ResponseModel<ChartsAndOlderDatesModel>.AccessDeniedResponse();
			try
			{

				var kwsExtension = "";
				if(data.Kws != null && data.Kws.Count > 0)
					kwsExtension = GetKwsExtension(data.Kws);
				int? userId = user.IsAdministrator || user.IsCorporateDirector || user.IsGlobalDirector || user.Access.Purchase.AllCustomers
						? null
						: user.Id;
				var Ruckstandige_Bestellungen = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Rückständige_Bestellungen(data.CustomerNumber, data.MitarbeiterId, kwsExtension, data.Dates, userId);
				var response = new ChartsAndOlderDatesModel();
				response.ChartValues = new List<DateValueOrderModel>();
				if(Ruckstandige_Bestellungen != null && Ruckstandige_Bestellungen.Count > 0)
				{
					if(Ruckstandige_Bestellungen.Count > 10 && (data.Kws == null || data.Kws.Count == 0) && (data.Dates == null || data.Dates.Count == 0))
					{
						response.ChartValues = Ruckstandige_Bestellungen.Take(10).Select(x => new DateValueOrderModel
						{
							Date = x.Value,
							Value = x.Key,
						}).ToList();
						var _plus10 = Ruckstandige_Bestellungen.Skip(10).ToList();
						response.OlderDatesButtonValue = _plus10.Sum(x => x.Key);
					}
					else
						response.ChartValues = Ruckstandige_Bestellungen.Select(x => new DateValueOrderModel
						{
							Date = x.Value,
							Value = x.Key,
						}).ToList();
				}
				//fill empty values
				if(data.Kws != null && data.Kws.Count > 0)
					foreach(var item in data.Kws)
					{
						var dateValue = response.ChartValues.FirstOrDefault(x => x.Date == item);
						if(dateValue == null)
							response.ChartValues.Add(new DateValueOrderModel
							{
								Date = item,
								Value = 0,
							});
					}
				if(data.Dates != null && data.Dates.Count > 0)
				{
					foreach(var item in data.Dates)
					{
						var dateValue = response.ChartValues.FirstOrDefault(x => Convert.ToDateTime(x.Date) == item);
						if(dateValue == null)
							response.ChartValues.Add(new DateValueOrderModel
							{
								Date = item.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
								Value = 0,
							});
					}
				}
				if(data.Kws != null && data.Kws.Count > 0)
					response.ChartValues = response.ChartValues.OrderBy(x => x.Value).ToList();
				if((data.Dates != null && data.Dates.Count > 0) || ((data.Dates == null || data.Dates.Count == 0) && (data.Kws == null || data.Kws.Count == 0)))
					response.ChartValues = response.ChartValues.OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();

				return ResponseModel<ChartsAndOlderDatesModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal string GetKwsExtension(List<string> kws)
		{
			var result = "";
			result += " AND (";
			foreach(var kw in kws)
			{
				var week = Convert.ToInt32(kw.Split("/")[0]);
				var year = Convert.ToInt32(kw.Split("/")[1]);

				result += $" (DATEPART(ISO_WEEK,aa.Liefertermin)={week} AND YEAR(aa.Liefertermin)={year}) ";
				if(kws.IndexOf(kw) != kws.Count - 1)
					result += " OR ";
			}
			result += " )";
			return result;
		}
	}
}
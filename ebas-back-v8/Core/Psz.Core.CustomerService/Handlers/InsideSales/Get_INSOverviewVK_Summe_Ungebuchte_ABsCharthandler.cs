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
		public ResponseModel<ChartsAndOlderDatesModel> Get_INSOverviewVK_Summe_Ungebuchte_ABsChart(UserModel user, Ruckstandige_BestellungenRequestModel data)
		{
			if(user == null)
				return ResponseModel<ChartsAndOlderDatesModel>.AccessDeniedResponse();

			var kwsExtension = "";
			if(data.Kws != null && data.Kws.Count > 0)
				kwsExtension = GetKwsExtension(data.Kws);
			int? userId = user.IsAdministrator || user.IsCorporateDirector || user.IsGlobalDirector || user.Access.Purchase.AllCustomers
					? null
					: user.Id;
			var VK_Summe_Ungebuchte_ABs = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_VK_Summe_ungebuchte_ABs(data.CustomerNumber, data.MitarbeiterId, kwsExtension, data.Dates, userId);
			var response = new ChartsAndOlderDatesModel();
			response.ChartValues = new List<DateValueOrderModel>();
			if(VK_Summe_Ungebuchte_ABs != null && VK_Summe_Ungebuchte_ABs.Count > 0)
			{
				if(VK_Summe_Ungebuchte_ABs.Count > 10 && (data.Kws == null || data.Kws.Count == 0) && (data.Dates == null || data.Dates.Count == 0))
				{
					response.ChartValues = VK_Summe_Ungebuchte_ABs.Take(10).Select(x => new DateValueOrderModel
					{
						Date = x.Value,
						Value = x.Key,
					}).ToList();
					var _plus10 = VK_Summe_Ungebuchte_ABs.Skip(10).ToList();
					response.OlderDatesButtonValue = _plus10.Sum(x => x.Key);
				}
				else
					response.ChartValues = VK_Summe_Ungebuchte_ABs.Select(x => new DateValueOrderModel
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
		}
	}
}
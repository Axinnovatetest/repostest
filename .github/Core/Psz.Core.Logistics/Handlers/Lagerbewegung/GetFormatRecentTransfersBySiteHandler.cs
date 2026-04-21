using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	using Infrastructure.Data.Access.Joins.Logistics;
	public class GetFormatRecentTransfersBySiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<FormatRecentTransfersBySiteResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data {  get; set; }
		public GetFormatRecentTransfersBySiteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<FormatRecentTransfersBySiteResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var response = new FormatRecentTransfersBySiteResponseModel();
				var site = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(_data);
				var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(site.IdCompany ?? 0);
				var country = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(company?.CountryId ?? 0);
				int prevYear = DateTime.Today.Year;
				int prevMonth = DateTime.Today.Month - 1;
				if(DateTime.Today.Month == 1)
				{
					prevYear = DateTime.Today.Year - 1;
					prevMonth = 12;
				}

				response = new FormatRecentTransfersBySiteResponseModel
				{
					SiteId = site.Id,
					SiteName = site.Name1,
					CountryId = country?.Id ?? 0,
					CountryName = country?.Name,
					SentTransfers = new FormatRecentTransfersBySiteResponseModel.TransferModel
					{
						CurrentMonth = new FormatRecentTransfersBySiteResponseModel.MonthTransferModel
						{
							MonthDate = DateTime.Today,
							MonthNumber = DateTime.Today.Month,
							TransferDates = new List<FormatRecentTransfersBySiteResponseModel.TransferDateModel>()
						},
						PreviousMonth = new FormatRecentTransfersBySiteResponseModel.MonthTransferModel
						{
							MonthDate = new DateTime(prevYear, prevMonth, 1),
							MonthNumber = prevMonth,
							TransferDates = new List<FormatRecentTransfersBySiteResponseModel.TransferDateModel>()
						}
					},
					ReceivedTransfers = new FormatRecentTransfersBySiteResponseModel.TransferModel
					{
						CurrentMonth = new FormatRecentTransfersBySiteResponseModel.MonthTransferModel
						{
							MonthDate = DateTime.Today,
							MonthNumber = DateTime.Today.Month,
							TransferDates = new List<FormatRecentTransfersBySiteResponseModel.TransferDateModel>()
						},
						PreviousMonth = new FormatRecentTransfersBySiteResponseModel.MonthTransferModel
						{
							MonthDate = new DateTime(prevYear, prevMonth, 1),
							MonthNumber = prevMonth,
							TransferDates = new List<FormatRecentTransfersBySiteResponseModel.TransferDateModel>()
						}
					}
				};
				var prevMonthSends = StatisticsAccess.GetFormatDataHeaderByMonth(new DateTime(prevYear, prevMonth, 1), site.SiteName, true);
				var prevMonthReceives = StatisticsAccess.GetFormatDataHeaderByMonth(new DateTime(prevYear, prevMonth, 1), site.SiteName, false);
				var currMonthSends = StatisticsAccess.GetFormatDataHeaderByMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), site.SiteName, true);
				var currMonthReceives = StatisticsAccess.GetFormatDataHeaderByMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), site.SiteName, false);
				
				// - sends
				if (prevMonthSends != null && prevMonthSends.Count > 0)
				{
					response.SentTransfers.PreviousMonth.TransferDates = prevMonthSends.Select(x=> new FormatRecentTransfersBySiteResponseModel.TransferDateModel { Key = x.LogUserId, Value = x.datum ?? DateTime.MinValue, LagerId = x.LagerId }).Distinct().ToList();
				}
				if(currMonthSends != null && currMonthSends.Count > 0)
				{
					response.SentTransfers.CurrentMonth.TransferDates = currMonthSends.Select(x => new FormatRecentTransfersBySiteResponseModel.TransferDateModel { Key = x.LogUserId, Value = x.datum ?? DateTime.MinValue, LagerId = x.LagerId }).Distinct().ToList();
				}

				// - receives
				if(prevMonthReceives != null && prevMonthReceives.Count > 0)
				{
					response.ReceivedTransfers.PreviousMonth.TransferDates = prevMonthReceives.Select(x => new FormatRecentTransfersBySiteResponseModel.TransferDateModel { Key = x.LogUserId, Value = x.datum ?? DateTime.MinValue, LagerId = x.LagerId }).Distinct().ToList();
				}
				if(currMonthReceives != null && currMonthReceives.Count > 0)
				{
					response.ReceivedTransfers.CurrentMonth.TransferDates = currMonthReceives.Select(x => new FormatRecentTransfersBySiteResponseModel.TransferDateModel { Key = x.LogUserId, Value = x.datum ?? DateTime.MinValue, LagerId = x.LagerId }).Distinct().ToList();
				}

				return ResponseModel<FormatRecentTransfersBySiteResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<FormatRecentTransfersBySiteResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<FormatRecentTransfersBySiteResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get(_data) == null)
			{
				return ResponseModel<FormatRecentTransfersBySiteResponseModel>.FailureResponse($"Site [{_data}] not found");
			}

			return ResponseModel<FormatRecentTransfersBySiteResponseModel>.SuccessResponse();
		}
	}
}

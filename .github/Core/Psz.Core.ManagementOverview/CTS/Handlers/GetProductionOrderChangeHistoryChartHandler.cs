using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetProductionOrderChangeHistoryChartHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private GetProductionOrderChangeHistoryChartRequestModel _data { get; set; }
		public GetProductionOrderChangeHistoryChartHandler(Identity.Models.UserModel user, GetProductionOrderChangeHistoryChartRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				if(!this._user.IsGlobalDirector && !this._user.IsAdministrator && this._user.Access.ManagementOverview.CtsPlanning == false)
				{
					return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.SuccessResponse(null);
				}

				var DateFrom = DateTime.Now.AddMonths(-6);
				var DateTo = DateTime.Now.AddMonths(6);
				var responseBody = new List<FaChangesWeekYearHoursLeftResponseModel>();
				// -  
				int horizon = Module.AppSettingsCTS.FAHorizons.H1LengthInDays;
				var entities = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetProductionOrderHistoryPerWarehouseYearWeekFull(DateFrom, DateTo, this._data.ProductionOrderWarehouseId ,horizon, this._data.InFrozenZone, this._data.OutFrozenZone);
				if(entities != null && entities.Count > 0)
				{
					responseBody.AddRange(entities.Select(x => new FaChangesWeekYearHoursLeftResponseModel(x)));
				}

				// -
				return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.SuccessResponse();
		}
	}
}

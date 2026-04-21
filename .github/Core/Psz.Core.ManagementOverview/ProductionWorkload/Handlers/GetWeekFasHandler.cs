using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Data;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class GetWeekFasHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<WeekFaResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private WorkloadHistoryRequestModel _data { get; set; }

		public GetWeekFasHandler(Identity.Models.UserModel user, WorkloadHistoryRequestModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<List<WeekFaResponseModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var results = Infrastructure.Data.Access.Joins.MGO.Statistics.GetProductionWorkload_WeekFas(_data.WarehouseId, _data.Week, _data.Year, _data.IsBacklog ?? false)
					?.Select(x => new WeekFaResponseModel(x))?.OrderBy(x => x.FaProductionTime)?.ToList();
				return ResponseModel<List<WeekFaResponseModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<WeekFaResponseModel>> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<WeekFaResponseModel>>.AccessDeniedResponse();
			}

			// -
			if(_data.WarehouseId <= 0)
			{
				return ResponseModel<List<WeekFaResponseModel>>.FailureResponse($"Warehouse ID invalid");
			}
				if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.WarehouseId) == null)
				{
					return ResponseModel<List<WeekFaResponseModel>>.FailureResponse($"Warehouse not found");
				}
			if(!_data.IsBacklog.HasValue && !_data.IsBacklog.Value)
			{
				if(_data.Week <= 0 || _data.Week > 53)
				{
					return ResponseModel<List<WeekFaResponseModel>>.FailureResponse($"Week [{_data.Week}] invalid");
				}
				if(_data.Year <= 0)
				{
					return ResponseModel<List<WeekFaResponseModel>>.FailureResponse($"Year [{_data.Year}] invalid");
				}
			}
			// - 
			return ResponseModel<List<WeekFaResponseModel>>.SuccessResponse();
		}
	}
}
